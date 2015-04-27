using System;
using SharpDX;
using SharpDX.Direct3D9;

namespace LeagueSharp.CommonEx.Core.Render.RenderObjects
{
    /// <summary>
    /// 
    /// </summary>
    public class Rectangle : Render.RenderObject
    {
        /// <summary>
        /// 
        /// </summary>
        public delegate Vector2 PositionDelegate();

        private readonly SharpDX.Direct3D9.Line _line;
        private int _x;
        private int _y;

        /// <summary>
        /// 
        /// </summary>
        public ColorBGRA Color;

        /// <summary>
        /// </summary>
        public static Device Device
        {
            get { return Drawing.Direct3DDevice; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        public Rectangle(int x, int y, int width, int height, ColorBGRA color)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Color = color;
            _line = new SharpDX.Direct3D9.Line(Device) { Width = height };
        }

        /// <summary>
        /// 
        /// </summary>
        public int X
        {
            get
            {
                if (PositionUpdate != null)
                {
                    return (int)PositionUpdate().X;
                }
                return _x;
            }
            set { _x = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Y
        {
            get
            {
                if (PositionUpdate != null)
                {
                    return (int)PositionUpdate().Y;
                }
                return _y;
            }
            set { _y = value; }
        }

        /// <summary>
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// </summary>
        public PositionDelegate PositionUpdate { get; set; }

        public override void OnEndScene()
        {
            try
            {
                if (_line.IsDisposed)
                {
                    return;
                }

                _line.Begin();
                _line.Draw(new[] { new Vector2(X, Y + Height / 2), new Vector2(X + Width, Y + Height / 2) }, Color);
                _line.End();
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Common.Render.Rectangle.OnEndScene: " + e);
            }
        }

        public override void OnPreReset()
        {
            _line.OnLostDevice();
        }

        public override void OnPostReset()
        {
            _line.OnResetDevice();
        }

        public override void Dispose()
        {
            if (!_line.IsDisposed)
            {
                _line.Dispose();
            }
        }
    }
}
