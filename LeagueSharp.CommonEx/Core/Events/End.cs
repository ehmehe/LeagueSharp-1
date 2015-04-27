using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LeagueSharp.CommonEx.Core.Events
{
    /// <summary>
    /// 
    /// </summary>
    public static class End
    {
        /// <summary>
        ///  OnEnd Delegate.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">EventArgs</param>
        public delegate void OnEndDelegate(object sender, EventArgs e);

        private static readonly List<Obj_HQ> NexusList = new List<Obj_HQ>();
        private static bool _endGameCalled;

        /// <summary>
        ///     Static constructor.
        /// </summary>
        static End()
        {
            foreach (var hq in ObjectManager.Get<Obj_HQ>().Where(hq => hq.IsValid))
            {
                NexusList.Add(hq);
            }

            Load.OnLoad += Load_OnLoad;
        }

        private static void Load_OnLoad(object sender, EventArgs e)
        {
            Game.OnUpdate += Game_OnGameUpdate;
        }

        /// <summary>
        ///     OnEnd is getting called when the Game ends.
        /// </summary>
        public static event OnEndDelegate OnEnd;

        /// <summary>
        ///     Internal event that is called when the game ends.
        /// </summary>
        /// <param name="args">System.EventArgs</param>
        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (NexusList.Count == 0 || _endGameCalled)
            {
                return;
            }

            foreach (var nexus in NexusList)
            {
                if (nexus != null && nexus.IsValid && nexus.Health <= 0)
                {
                    if (OnEnd != null)
                    {
                        OnEnd(MethodBase.GetCurrentMethod().DeclaringType, new EventArgs());
                        _endGameCalled = true;
                    }
                }
            }
        }
    }
}