using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.CommonEx.Core.Enumerations;
using SharpDX;

namespace LeagueSharp.CommonEx.Core.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class Mouse
    {
        internal static class CursorPosT
        {
            private static int _posX;
            private static int _posY;

            static CursorPosT()
            {
                Game.OnWndProc += Game_OnWndProc;
            }

            private static void Game_OnWndProc(WndEventArgs args)
            {
                if (args.Msg == (uint) WindowsMessages.MOUSEMOVE)
                {
                    _posX = unchecked((short) args.LParam);
                    _posY = unchecked((short) ((long) args.LParam >> 16));
                }
            }

            internal static Vector2 GetCursorPos()
            {
                return new Vector2(_posX, _posY);
            }
        }
    }
}