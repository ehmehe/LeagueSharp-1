﻿#region

using System.Collections.Generic;
using System.Linq;
using LeagueSharp.CommonEx.Core.Enumerations;
using LeagueSharp.CommonEx.Core.Extensions;
using LeagueSharp.CommonEx.Core.Extensions.SharpDX;
using LeagueSharp.CommonEx.Core.Managers;
using LeagueSharp.CommonEx.Core.Math.Prediction;
using LeagueSharp.CommonEx.Core.Utils;
using SharpDX;

#endregion

namespace LeagueSharp.CommonEx.Core.Wrappers
{
    /// <summary>
    ///     Spell Container
    /// </summary>
    public class Spell
    {
        #region Private Data Members

        /// <summary>
        ///     Charged Cast Tick
        /// </summary>
        private int _chargedCastedT;

        /// <summary>
        ///     Charged Request Sent Tick
        /// </summary>
        private int _chargedReqSentT;

        /// <summary>
        ///     From Vector3 Source
        /// </summary>
        private Vector3 _from;

        /// <summary>
        ///     Range
        /// </summary>
        private float _range;

        /// <summary>
        ///     Range Check From Vector3 Source
        /// </summary>
        private Vector3 _rangeCheckFrom;

        /// <summary>
        ///     Width
        /// </summary>
        private float _width;

        #endregion

        #region Public Data Members

        /// <summary>
        ///     Charged Buff Name
        /// </summary>
        public string ChargedBuffName { get; set; }

        /// <summary>
        ///     Charged Max Range
        /// </summary>
        public int ChargedMaxRange { get; set; }

        /// <summary>
        ///     Charged Min Range
        /// </summary>
        public int ChargedMinRange { get; set; }

        /// <summary>
        ///     Charged Spell Name
        /// </summary>
        public string ChargedSpellName { get; set; }

        /// <summary>
        ///     Charge Duration
        /// </summary>
        public int ChargeDuration { get; set; }

        /// <summary>
        ///     Collision Flag
        /// </summary>
        public bool Collision { get; set; }

        /// <summary>
        ///     Delay
        /// </summary>
        public float Delay { get; set; }

        /// <summary>
        ///     Is Charged Spell
        /// </summary>
        public bool IsChargedSpell { get; set; }

        /// <summary>
        ///     Is Skillshot
        /// </summary>
        public bool IsSkillshot { get; set; }

        /// <summary>
        ///     Last Casted Attempt Tick
        /// </summary>
        public int LastCastAttemptT { get; set; }

        /// <summary>
        ///     Minimum Hit Chance
        /// </summary>
        public HitChance MinHitChance { get; set; }

        /// <summary>
        ///     Slot
        /// </summary>
        public SpellSlot Slot { get; set; }

        /// <summary>
        ///     Speed
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        ///     Type
        /// </summary>
        public SkillshotType Type { get; set; }

        /// <summary>
        ///     Damage Type <see cref="DamageType" />
        /// </summary>
        public DamageType DamageType { get; set; }

        /// <summary>
        ///     Width
        /// </summary>
        public float Width
        {
            get { return _width; }
            set
            {
                _width = value;
                WidthSqr = value*value;
            }
        }

        /// <summary>
        ///     Width Squared
        /// </summary>
        public float WidthSqr { get; private set; }

        /// <summary>
        ///     Instance
        /// </summary>
        public SpellDataInst Instance
        {
            get { return ObjectManager.Player.Spellbook.GetSpell(Slot); }
        }

        /// <summary>
        ///     Range
        /// </summary>
        public float Range
        {
            get
            {
                if (!IsChargedSpell)
                {
                    return _range;
                }

                if (IsCharging)
                {
                    return ChargedMinRange +
                           System.Math.Min(
                               ChargedMaxRange - ChargedMinRange,
                               (Variables.TickCount - _chargedCastedT)*(ChargedMaxRange - ChargedMinRange)/
                               ChargeDuration - 150);
                }

                return ChargedMaxRange;
            }
            set { _range = value; }
        }

        /// <summary>
        ///     Range Squared
        /// </summary>
        public float RangeSqr
        {
            get { return Range*Range; }
        }

        /// <summary>
        ///     Is Charging
        /// </summary>
        public bool IsCharging
        {
            get
            {
                return ObjectManager.Player.HasBuff(ChargedBuffName) ||
                       Variables.TickCount - _chargedCastedT < 300 + Game.Ping;
            }
        }

        /// <summary>
        ///     Spell Level
        /// </summary>
        public int Level
        {
            get { return ObjectManager.Player.Spellbook.GetSpell(Slot).Level; }
        }

        /// <summary>
        ///     From Vector3 Source
        /// </summary>
        public Vector3 From
        {
            get { return !_from.ToVector2().IsValid() ? ObjectManager.Player.ServerPosition : _from; }
            set { _from = value; }
        }

        /// <summary>
        ///     Range Check From Vector3 Source
        /// </summary>
        public Vector3 RangeCheckFrom
        {
            get
            {
                return !_rangeCheckFrom.ToVector2().IsValid() ? ObjectManager.Player.ServerPosition : _rangeCheckFrom;
            }
            set { _rangeCheckFrom = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Instances a new Spell
        /// </summary>
        /// <param name="spellDataWrapper">SpellSlot Wrapper</param>
        /// <param name="hitChance">Minimum Hit Chance</param>
        public Spell(SpellDataWrapper spellDataWrapper, HitChance hitChance = HitChance.Medium)
        {
            Slot = spellDataWrapper.Slot;
            Range = spellDataWrapper.Range;
            Width = spellDataWrapper.Width;
            Speed = spellDataWrapper.Speed;
            Delay = spellDataWrapper.Delay;

            MinHitChance = hitChance;
        }

        /// <summary>
        ///     Instances a new Spell
        /// </summary>
        /// <param name="slot">SpellSlot</param>
        /// <param name="loadFromGame">Load SpellData From Game</param>
        /// <param name="hitChance">Minimum Hit Chance</param>
        public Spell(SpellSlot slot, bool loadFromGame, HitChance hitChance = HitChance.Medium)
        {
            Slot = slot;

            if (!loadFromGame)
            {
                return;
            }

            var spellData = ObjectManager.Player.Spellbook.GetSpell(slot).SData;

            Range = spellData.CastRange;
            Width = spellData.LineWidth.Equals(0) ? spellData.CastRadius : spellData.LineWidth;
            Speed = spellData.MissileSpeed;
            Delay = spellData.DelayTotalTimePercent;

            MinHitChance = hitChance;
        }

        /// <summary>
        ///     Instance a new Spell.
        /// </summary>
        /// <param name="slot">Slot</param>
        /// <param name="range">Spell Range</param>
        public Spell(SpellSlot slot, float range = float.MaxValue)
        {
            Slot = slot;
            Range = range;
        }

        #endregion

        #region Prediction

        /// <summary>
        ///     Sets the Spell Data to targetted data.
        /// </summary>
        /// <param name="delay">Spell Delay</param>
        /// <param name="speed">Spell Speed</param>
        /// <param name="from">From Vector3 Source</param>
        /// <param name="rangeCheckFrom">Range Check From Vector3 Source</param>
        public void SetTargetted(float delay,
            float speed,
            Vector3 from = new Vector3(),
            Vector3 rangeCheckFrom = new Vector3())
        {
            Delay = delay;
            Speed = speed;
            From = from;
            RangeCheckFrom = rangeCheckFrom;
            IsSkillshot = false;
        }

        /// <summary>
        ///     Sets the Spell Data to targetted data.
        /// </summary>
        /// <param name="from">From Vector3 Source</param>
        /// <param name="rangeCheckFrom">Range Check From Vector3 Source</param>
        public void SetTargetted(Vector3 from = new Vector3(), Vector3 rangeCheckFrom = new Vector3())
        {
            From = from;
            RangeCheckFrom = rangeCheckFrom;
            IsSkillshot = false;
        }

        /// <summary>
        ///     Sets the Spell Data to Skillshot data.
        /// </summary>
        /// <param name="delay">Spell Delay</param>
        /// <param name="width">Spell Width</param>
        /// <param name="speed">Spell Speed</param>
        /// <param name="collision">Spell Collision Flag</param>
        /// <param name="type">Skillshot Type</param>
        /// <param name="from">From Vector3 Source</param>
        /// <param name="rangeCheckFrom">Range Check From Vecto3 Source</param>
        public void SetSkillshot(float delay,
            float width,
            float speed,
            bool collision,
            SkillshotType type,
            Vector3 from = new Vector3(),
            Vector3 rangeCheckFrom = new Vector3())
        {
            Delay = delay;
            Width = width;
            Speed = speed;
            From = from;
            Collision = collision;
            Type = type;
            RangeCheckFrom = rangeCheckFrom;
            IsSkillshot = true;
        }

        /// <summary>
        ///     Sets the Spell Data to Skillshot data.
        /// </summary>
        /// <param name="collision">Spell Collision Flag</param>
        /// <param name="type">Skillshot Type</param>
        /// <param name="from">From Vector3 Source</param>
        /// <param name="rangeCheckFrom">Range Check From Vecto3 Source</param>
        public void SetSkillshot(bool collision,
            SkillshotType type,
            Vector3 from = new Vector3(),
            Vector3 rangeCheckFrom = new Vector3())
        {
            From = from;
            Collision = collision;
            Type = type;
            RangeCheckFrom = rangeCheckFrom;
            IsSkillshot = true;
        }

        /// <summary>
        ///     Sets the Spell Data to Charged data.
        /// </summary>
        /// <param name="spellName">Spell Name</param>
        /// <param name="buffName">Spell Buff Name</param>
        /// <param name="minRange">Spell Minimum Range</param>
        /// <param name="maxRange">Spell Maximum Range</param>
        /// <param name="deltaT">Charge Duration</param>
        public void SetCharged(string spellName, string buffName, int minRange, int maxRange, float deltaT)
        {
            IsChargedSpell = true;
            ChargedSpellName = spellName;
            ChargedBuffName = buffName;
            ChargedMinRange = minRange;
            ChargedMaxRange = maxRange;
            ChargeDuration = (int) (deltaT*1000);
            _chargedCastedT = 0;

            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
            Spellbook.OnUpdateChargedSpell += Spellbook_OnUpdateChargedSpell;
            Spellbook.OnCastSpell += SpellbookOnCastSpell;
        }

        /// <summary>
        ///     Start charging the spell if its not charging.
        /// </summary>
        public void StartCharging()
        {
            if (IsCharging || Variables.TickCount - _chargedReqSentT <= 400 + Game.Ping)
            {
                return;
            }

            ObjectManager.Player.Spellbook.CastSpell(Slot);
            _chargedReqSentT = Variables.TickCount;
        }

        /// <summary>
        ///     Start charging the spell if its not charging.
        /// </summary>
        public void StartCharging(Vector3 position)
        {
            if (IsCharging || Variables.TickCount - _chargedReqSentT <= 400 + Game.Ping)
            {
                return;
            }

            ObjectManager.Player.Spellbook.CastSpell(Slot, position);
            _chargedReqSentT = Variables.TickCount;
        }

        /// <summary>
        ///     On Charged Spell Update subscribed event function.
        /// </summary>
        /// <param name="sender"><see cref="Spellbook" /> sender</param>
        /// <param name="args">Spellbok Update Charged Spell Data</param>
        private void Spellbook_OnUpdateChargedSpell(Spellbook sender, SpellbookUpdateChargedSpellEventArgs args)
        {
            if (sender.Owner.IsMe && Variables.TickCount - _chargedReqSentT < 3000)
            {
                args.Process = false;
            }
        }

        /// <summary>
        ///     Spellbook On Cast Spell subscribed event function.
        /// </summary>
        /// <param name="spellbook"><see cref="Spellbook" /> sender</param>
        /// <param name="args">Spellbook Cast Spell Data</param>
        private void SpellbookOnCastSpell(Spellbook spellbook, SpellbookCastSpellEventArgs args)
        {
            if (args.Slot != Slot)
            {
                return;
            }

            if ((Variables.TickCount - _chargedReqSentT > 500))
            {
                if (IsCharging)
                {
                    Cast(new Vector2(args.EndPosition.X, args.EndPosition.Y));
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"><see cref="Obj_AI_Base" /> sender</param>
        /// <param name="args">Process Spell Cast Data</param>
        private void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name == ChargedSpellName)
            {
                _chargedCastedT = Variables.TickCount;
            }
        }

        /// <summary>
        ///     Update Source Position
        /// </summary>
        /// <param name="from">From Vector3 Source</param>
        /// <param name="rangeCheckFrom">Range Check From Vector3 Source</param>
        public void UpdateSourcePosition(Vector3 from = new Vector3(), Vector3 rangeCheckFrom = new Vector3())
        {
            From = from;
            RangeCheckFrom = rangeCheckFrom;
        }

        /// <summary>
        ///     Returns Spell Prediction
        /// </summary>
        /// <param name="unit">Predicted Unit</param>
        /// <param name="aoe">Is Area-of-effect</param>
        /// <param name="overrideRange">Overriden Range</param>
        /// <param name="collisionable">Collisionable Flags</param>
        /// <returns>
        ///     <see cref="PredictionOutput" />
        /// </returns>
        public PredictionOutput GetPrediction(Obj_AI_Base unit,
            bool aoe = false,
            float overrideRange = -1,
            CollisionableObjects collisionable = CollisionableObjects.Heroes | CollisionableObjects.Minions)
        {
            return
                Movement.GetPrediction(
                    new PredictionInput
                    {
                        Unit = unit,
                        Delay = Delay,
                        Radius = Width,
                        Speed = Speed,
                        From = From,
                        Range = (overrideRange > 0) ? overrideRange : Range,
                        Collision = Collision,
                        Type = Type,
                        RangeCheckFrom = RangeCheckFrom,
                        Aoe = aoe,
                        CollisionObjects = collisionable
                    });
        }

        /// <summary>
        ///     Returns Collision List
        /// </summary>
        /// <param name="from">From Vector3 Source</param>
        /// <param name="to">To Vector3 Source</param>
        /// <param name="delayOverride">Delay Override</param>
        /// <returns>Collision List</returns>
        public List<Obj_AI_Base> GetCollision(Vector2 from, List<Vector2> to, float delayOverride = -1)
        {
            return Math.Collision.GetCollision(
                to.Select(h => h.ToVector3()).ToList(),
                new PredictionInput
                {
                    From = from.ToVector3(),
                    Type = Type,
                    Radius = Width,
                    Delay = delayOverride > 0 ? delayOverride : Delay,
                    Speed = Speed
                });
        }

        /// <summary>
        ///     Returns Hit Count
        /// </summary>
        /// <param name="hitChance">HitChance</param>
        /// <returns>Hit Count</returns>
        public float GetHitCount(HitChance hitChance = HitChance.High)
        {
            return ObjectHandler.EnemyHeroes.Select(e => GetPrediction(e)).Count(p => p.Hitchance >= hitChance);
        }

        #endregion

        #region Cast

        /// <summary>
        ///     Casts the spell.
        /// </summary>
        /// <param name="unit">Unit to cast on</param>
        /// <param name="exactHitChance">Is exact hitchance</param>
        /// <param name="areaOfEffect">Is Area of Effect</param>
        /// <param name="minTargets">Minimum of Targets</param>
        /// <param name="tempHitChance">Temporary HitChance Override</param>
        /// <returns></returns>
        public CastStates Cast(Obj_AI_Base unit,
            bool exactHitChance = false,
            bool areaOfEffect = false,
            int minTargets = -1,
            HitChance tempHitChance = HitChance.None)
        {
            if (!unit.IsValid())
            {
                return CastStates.InvalidTarget;
            }
            if (!Slot.IsReady())
            {
                return CastStates.NotReady;
            }

            if (!areaOfEffect && minTargets != -1)
            {
                areaOfEffect = true;
            }

            if (!IsSkillshot)
            {
                if (RangeCheckFrom.DistanceSquared(unit.ServerPosition) > RangeSqr)
                {
                    return CastStates.OutOfRange;
                }

                LastCastAttemptT = Variables.TickCount;

                return !ObjectManager.Player.Spellbook.CastSpell(Slot, unit)
                    ? CastStates.NotCasted
                    : CastStates.SuccessfullyCasted;
            }

            var prediction = GetPrediction(unit, areaOfEffect);

            if (minTargets != -1 && prediction.AoeTargetsHitCount <= minTargets)
            {
                return CastStates.NotEnoughTargets;
            }

            if (prediction.CollisionObjects.Count > 0)
            {
                return CastStates.Collision;
            }

            if (RangeCheckFrom.DistanceSquared(prediction.CastPosition) > RangeSqr)
            {
                return CastStates.OutOfRange;
            }

            if (prediction.Hitchance < ((tempHitChance == HitChance.None) ? (MinHitChance) : (tempHitChance)) ||
                (exactHitChance &&
                 prediction.Hitchance != ((tempHitChance == HitChance.None) ? (MinHitChance) : (tempHitChance))))
            {
                return CastStates.LowHitChance;
            }

            LastCastAttemptT = Variables.TickCount;

            if (IsChargedSpell)
            {
                if (IsCharging)
                {
                    ShootChargedSpell(Slot, prediction.CastPosition);
                }
                else
                {
                    StartCharging();
                }
            }
            else
            {
                if (!ObjectManager.Player.Spellbook.CastSpell(Slot, prediction.CastPosition))
                {
                    return CastStates.NotCasted;
                }
            }

            return CastStates.SuccessfullyCasted;
        }

        /// <summary>
        ///     Cast Spell directly onto a unit
        /// </summary>
        /// <param name="unit">Target</param>
        /// <returns>Was Spell Casted</returns>
        public bool CastOnUnit(Obj_AI_Base unit)
        {
            if (!Slot.IsReady() || From.DistanceSquared(unit.ServerPosition) > RangeSqr)
            {
                return false;
            }

            LastCastAttemptT = Variables.TickCount;

            return ObjectManager.Player.Spellbook.CastSpell(Slot, unit);
        }

        /// <summary>
        ///     Cast Spell onto self
        /// </summary>
        /// <returns>Was Spell Casted</returns>
        public bool Cast()
        {
            return CastOnUnit(ObjectManager.Player);
        }

        /// <summary>
        ///     Cast Spell from a Vector2 to another Vector2 boundaries
        /// </summary>
        /// <param name="fromPosition">From Position</param>
        /// <param name="toPosition">To Position</param>
        /// <returns>Was Spell Casted</returns>
        public bool Cast(Vector2 fromPosition, Vector2 toPosition)
        {
            return Cast(fromPosition.ToVector3(), toPosition.ToVector3());
        }

        /// <summary>
        ///     Cast Spell from a Vector3 to another Vector3 boundaries
        /// </summary>
        /// <param name="fromPosition">From Position</param>
        /// <param name="toPosition">To Position</param>
        /// <returns>Was Spell Casted</returns>
        public bool Cast(Vector3 fromPosition, Vector3 toPosition)
        {
            return Slot.IsReady() && ObjectManager.Player.Spellbook.CastSpell(Slot, fromPosition, toPosition);
        }

        /// <summary>
        ///     Cast Spell to a Vector2
        /// </summary>
        /// <param name="position">Position</param>
        /// <returns>Was Spell Casted</returns>
        public bool Cast(Vector2 position)
        {
            return Cast(position.ToVector3());
        }

        /// <summary>
        ///     Cast Spell to a Vector3
        /// </summary>
        /// <param name="position">Position</param>
        /// <returns>Was Spell Casted</returns>
        public bool Cast(Vector3 position)
        {
            if (!Slot.IsReady())
            {
                return false;
            }

            LastCastAttemptT = Variables.TickCount;

            if (IsChargedSpell)
            {
                if (IsCharging)
                {
                    ShootChargedSpell(Slot, position);
                }
                else
                {
                    StartCharging();
                }
            }
            else
            {
                return ObjectManager.Player.Spellbook.CastSpell(Slot, position);
            }
            return false;
        }

        /// <summary>
        ///     Shoot Charged Spell
        /// </summary>
        /// <param name="slot">SpellSlot</param>
        /// <param name="position">Vector3 Position</param>
        /// <param name="releaseCast">Release Cast</param>
        private static void ShootChargedSpell(SpellSlot slot, Vector3 position, bool releaseCast = true)
        {
            ObjectManager.Player.Spellbook.CastSpell(slot, position, false);
            ObjectManager.Player.Spellbook.UpdateChargedSpell(slot, position, releaseCast, false);
        }

        /// <summary>
        ///     Cast Spell if HitChance equals to input HitChance
        /// </summary>
        /// <param name="unit">Target</param>
        /// <param name="hitChance">HitChance</param>
        /// <returns>Was Spell Casted</returns>
        public CastStates CastIfHitchanceEquals(Obj_AI_Base unit, HitChance hitChance)
        {
            return Cast(unit, true, false, -1, hitChance);
        }

        /// <summary>
        ///     Cast Spell if HitChance is more than the minimum to input HitChance
        /// </summary>
        /// <param name="unit">Target</param>
        /// <param name="hitChance">HitChance</param>
        /// <returns>Was Spell Casted</returns>
        public CastStates CastIfHitchanceMinimum(Obj_AI_Base unit, HitChance hitChance)
        {
            return Cast(unit, false, false, -1, hitChance);
        }

        /// <summary>
        ///     Cast Spell if will hit Minimum input targets counts.
        /// </summary>
        /// <param name="unit">Main Target</param>
        /// <param name="minTargets">Minimum Targets</param>
        /// <returns>Was Spell Casted</returns>
        public CastStates CastIfWillHit(Obj_AI_Base unit, int minTargets = 5)
        {
            return Cast(unit, false, true, minTargets);
        }

        /// <summary>
        ///     Cast Spell on best Target.
        /// </summary>
        /// <param name="extraRange">Extra Range</param>
        /// <param name="areaOfEffect">Area-of-Effect</param>
        /// <param name="minTargets">Minimum Area-of-Effect targets</param>
        /// <returns>CastState. <seealso cref="CastStates" /></returns>
        public CastStates CastOnBestTarget(float extraRange = 0, bool areaOfEffect = false, int minTargets = -1)
        {
            return Cast(GetTarget(extraRange), false, areaOfEffect, minTargets);
        }

        #endregion

        #region Other

        /// <summary>
        ///     Returns health prediction on a unit.
        /// </summary>
        /// <param name="unit">Unit</param>
        /// <returns>Unit's predicted health</returns>
        public float GetHealthPrediction(Obj_AI_Base unit)
        {
            var time = (int) (Delay*1000 + From.Distance(unit.ServerPosition)/Speed - 100);
            return Health.GetHealthPrediction(unit, time);
        }

        /// <summary>
        ///     Get Circular Farm Location
        /// </summary>
        /// <param name="minionPositions">Minions</param>
        /// <param name="overrideWidth">Override Width</param>
        /// <returns>Farm Location. <seealso cref="MinionManager.FarmLocation" /></returns>
        public MinionManager.FarmLocation GetCircularFarmLocation(List<Obj_AI_Base> minionPositions,
            float overrideWidth = -1)
        {
            var positions = MinionManager.GetMinionsPredictedPositions(
                minionPositions, Delay, Width, Speed, From, Range, false, SkillshotType.SkillshotCircle);

            return GetCircularFarmLocation(positions, overrideWidth);
        }

        /// <summary>
        ///     Get Circular Farm Location
        /// </summary>
        /// <param name="minionPositions">Minion Positions</param>
        /// <param name="overrideWidth">Override Width</param>
        /// <returns>Farm Location. <seealso cref="MinionManager.FarmLocation" /></returns>
        public MinionManager.FarmLocation GetCircularFarmLocation(List<Vector2> minionPositions,
            float overrideWidth = -1)
        {
            return MinionManager.GetBestCircularFarmLocation(
                minionPositions, overrideWidth >= 0 ? overrideWidth : Width, Range);
        }

        /// <summary>
        ///     Get Line Farm Location
        /// </summary>
        /// <param name="minionPositions">Minions</param>
        /// <param name="overrideWidth">Override Width</param>
        /// <returns>Farm Location. <seealso cref="MinionManager.FarmLocation" /></returns>
        public MinionManager.FarmLocation GetLineFarmLocation(List<Obj_AI_Base> minionPositions,
            float overrideWidth = -1)
        {
            var positions = MinionManager.GetMinionsPredictedPositions(
                minionPositions, Delay, Width, Speed, From, Range, false, SkillshotType.SkillshotLine);

            return GetLineFarmLocation(positions, overrideWidth >= 0 ? overrideWidth : Width);
        }

        /// <summary>
        ///     Get Line Farm Location
        /// </summary>
        /// <param name="minionPositions">Minion Positions</param>
        /// <param name="overrideWidth">Override Width</param>
        /// <returns>Farm Location. <seealso cref="MinionManager.FarmLocation" /></returns>
        public MinionManager.FarmLocation GetLineFarmLocation(List<Vector2> minionPositions, float overrideWidth = -1)
        {
            return MinionManager.GetBestLineFarmLocation(
                minionPositions, overrideWidth >= 0 ? overrideWidth : Width, Range);
        }

        /// <summary>
        ///     Returns the spell counted hits.
        /// </summary>
        /// <param name="units">Minions</param>
        /// <param name="castPosition">Cast Position Vector3 Source</param>
        /// <returns></returns>
        public int CountHits(List<Obj_AI_Base> units, Vector3 castPosition)
        {
            var points = units.Select(unit => GetPrediction(unit).UnitPosition).ToList();
            return CountHits(points, castPosition);
        }

        /// <summary>
        ///     Returns the spell counted hits.
        /// </summary>
        /// <param name="points">Minion Positions</param>
        /// <param name="castPosition">Cast Position Vector3 Source</param>
        /// <returns></returns>
        public int CountHits(List<Vector3> points, Vector3 castPosition)
        {
            return points.Count(point => WillHit(point, castPosition));
        }

        /// <summary>
        ///     Returns Spell Damage.
        /// </summary>
        /// <param name="target">Target</param>
        /// <param name="stage">The "stage" of the spell.</param>
        /// <returns></returns>
        public float GetDamage(Obj_AI_Base target, int stage = 0)
        {
            return (float) ObjectManager.Player.GetSpellDamage(target, Slot, stage);
        }

        /// <summary>
        ///     Gets the damage that the spell will deal to the target using the damage lib and returns if the target is
        ///     killable or not.
        /// </summary>
        /// <param name="target">Target</param>
        /// <param name="stage">Spell Stage</param>
        public bool IsKillable(Obj_AI_Base target, int stage = 0)
        {
            return ObjectManager.Player.GetSpellDamage(target, Slot) > target.Health;
        }

        /// <summary>
        ///     Returns if the spell will hit the unit when casted.
        /// </summary>
        /// <param name="unit">Target</param>
        /// <param name="castPosition">Cast Position</param>
        /// <param name="extraWidth">Extra Width</param>
        /// <param name="minHitChance">Minimum Hit Chance</param>
        /// <returns>Will Spell Hit</returns>
        public bool WillHit(Obj_AI_Base unit,
            Vector3 castPosition,
            int extraWidth = 0,
            HitChance minHitChance = HitChance.High)
        {
            var unitPosition = GetPrediction(unit);
            return unitPosition.Hitchance >= minHitChance &&
                   WillHit(unitPosition.UnitPosition, castPosition, extraWidth);
        }

        /// <summary>
        ///     Returns if the spell will hit the point when casted
        /// </summary>
        /// <param name="point">Vector3 Target</param>
        /// <param name="castPosition">Cast Position</param>
        /// <param name="extraWidth">Extra Width</param>
        /// <returns>Will Spell Hit</returns>
        public bool WillHit(Vector3 point, Vector3 castPosition, int extraWidth = 0)
        {
            switch (Type)
            {
                case SkillshotType.SkillshotCircle:
                    if (point.ToVector2().DistanceSquared(castPosition) < WidthSqr)
                    {
                        return true;
                    }
                    break;

                case SkillshotType.SkillshotLine:
                    if (point.ToVector2().DistanceSquared(castPosition.ToVector2(), From.ToVector2(), true) <
                        System.Math.Pow(Width + extraWidth, 2))
                    {
                        return true;
                    }
                    break;
                case SkillshotType.SkillshotCone:
                    var edge1 = (castPosition.ToVector2() - From.ToVector2()).Rotated(-Width/2);
                    var edge2 = edge1.Rotated(Width);
                    var v = point.ToVector2() - From.ToVector2();
                    if (point.ToVector2().DistanceSquared(From) < RangeSqr && edge1.CrossProduct(v) > 0 &&
                        v.CrossProduct(edge2) > 0)
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }

        /// <summary>
        ///     Returns if a spell can be cast and the target is in range.
        /// </summary>
        /// <param name="unit">Target</param>
        /// <returns>Can spell be casted and target is in range</returns>
        public bool CanCast(Obj_AI_Base unit)
        {
            return Slot.IsReady() && unit.IsValidTarget(Range);
        }

        /// <summary>
        ///     Returns if the GameObject is in range of the spell.
        /// </summary>
        /// <param name="obj">
        ///     <see cref="GameObject" />
        /// </param>
        /// <param name="range">Range</param>
        /// <returns>Is GameObject in range of spell</returns>
        public bool IsInRange(GameObject obj, float range = -1)
        {
            return
                IsInRange(
                    obj is Obj_AI_Base ? ((Obj_AI_Base) obj).ServerPosition.ToVector2() : obj.Position.ToVector2(),
                    range);
        }

        /// <summary>
        ///     Returns if the Vector3 is in range of the spell.
        /// </summary>
        /// <param name="point">Vector3 point</param>
        /// <param name="range">Range</param>
        public bool IsInRange(Vector3 point, float range = -1)
        {
            return IsInRange(point.ToVector2(), range);
        }

        /// <summary>
        ///     Returns if the Vector2 is in range of the spell.
        /// </summary>
        /// <param name="point">Vector2 point</param>
        /// <param name="range">Range</param>
        public bool IsInRange(Vector2 point, float range = -1)
        {
            return RangeCheckFrom.ToVector2().DistanceSquared(point) < (range < 0 ? RangeSqr : range*range);
        }

        /// <summary>
        ///     Returns the best target found using the current TargetSelector mode.
        ///     Please make sure to set the Spell.DamageType Property to the type of damage this spell does (if not done on
        ///     initialization).
        /// </summary>
        /// <param name="extraRange">Extra Range</param>
        /// <param name="acccountForCollision">If true, will get a target that can be hit by the spell.</param>
        /// <param name="champsToIgnore">Champions to Ignore</param>
        public Obj_AI_Hero GetTarget(float extraRange = 0,
            bool acccountForCollision = false,
            IEnumerable<Obj_AI_Hero> champsToIgnore = null)
        {
            return acccountForCollision
                ? TargetSelector.GetTargetNoCollision(this, champsToIgnore)
                : TargetSelector.GetTarget(Range + extraRange, DamageType, From);
        }

        /// <summary>
        ///     Gets all of the units that this spell can hit that is greater then or equal to the <see cref="HitChance"/> provided.
        /// </summary>
        /// <param name="minimumHitChance">Minimum HitChance</param>
        /// <returns>All of the units that this spell can hit that is greater then or equal to the <see cref="HitChance"/> provided.</returns>
        public IEnumerable<Obj_AI_Base> GetUnitsByHitChance(HitChance minimumHitChance = HitChance.High)
        {
            return
                ObjectHandler.AllEnemies.Where(
                    unit => WillHit(unit, ObjectManager.Player.ServerPosition, 0, minimumHitChance));
        }

        #endregion
    }
}