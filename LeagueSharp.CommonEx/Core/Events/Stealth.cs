﻿using System;
using System.Reflection;

namespace LeagueSharp.CommonEx.Core.Events
{
    /// <summary>
    ///     Provides events for OnStealth
    /// </summary>
    public class Stealth
    {
        /// <summary>
        ///     OnStealth Delegate.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">OnStealth Event Arguments Container</param>
        public delegate void OnStealthDelegate(object sender, OnStealthEventArgs e);

        /// <summary>
        ///     Static constructor.
        /// </summary>
        static Stealth()
        {
            GameObject.OnIntegerPropertyChange += GameObject_OnIntegerPropertyChange;
        }

        /// <summary>
        ///     Function is called when a <see cref="GameObject" /> gets an integer property change and is called by an event.
        /// </summary>
        /// <param name="sender">GameObject</param>
        /// <param name="args">Integer Property Change Data</param>
        private static void GameObject_OnIntegerPropertyChange(GameObject sender,
            GameObjectIntegerPropertyChangeEventArgs args)
        {
            if (!args.Property.Equals("ActionState"))
            {
                return;
            }

            var oldState = (GameObjectCharacterState) args.OldValue;
            var newState = (GameObjectCharacterState) args.NewValue;

            if (!oldState.HasFlag(GameObjectCharacterState.IsStealth) &&
                newState.HasFlag(GameObjectCharacterState.IsStealth))
            {
                FireOnStealth(
                    new OnStealthEventArgs {Sender = (Obj_AI_Hero) sender, Time = Game.Time, IsStealthed = true});
            }
            else if (oldState.HasFlag(GameObjectCharacterState.IsStealth) &&
                     !newState.HasFlag(GameObjectCharacterState.IsStealth))
            {
                FireOnStealth(new OnStealthEventArgs {Sender = (Obj_AI_Hero) sender, IsStealthed = false});
            }
        }

        /// <summary>
        ///     Gets fired when any hero is invisible.
        /// </summary>
        public static event OnStealthDelegate OnStealth;

        /// <summary>
        /// </summary>
        /// <param name="args">OnStealthEventArgs <see cref="OnStealthEventArgs" /></param>
        private static void FireOnStealth(OnStealthEventArgs args)
        {
            if (OnStealth != null)
            {
                OnStealth(MethodBase.GetCurrentMethod().DeclaringType, args);
            }
        }

        /// <summary>
        ///     On Stealth Event Data, contains useful information that is passed with OnStealth
        ///     <seealso cref="OnStealth" />
        /// </summary>
        public class OnStealthEventArgs : EventArgs
        {
            /// <summary>
            ///     Returns if the unit is stealthed or not.
            /// </summary>
            public bool IsStealthed;

            /// <summary>
            ///     Stealth Sender
            /// </summary>
            public Obj_AI_Hero Sender;

            /// <summary>
            ///     Spell Start Time
            /// </summary>
            public float Time;
        }
    }
}