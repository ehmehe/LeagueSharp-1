using System;
using SharpDX;
using SharpDX.Direct3D9;

namespace LeagueSharp.CommonEx.Core.Render.RenderObjects
{
    /// <summary>
    /// 
    /// </summary>
    public class Line : Render.RenderObject
    {
        /// <summary>
        /// 
        /// </summary>
        public delegate Vector2 PositionDelegate();

        private readonly SharpDX.Direct3D9.Line _line;
        private Vector2 _end;
        private Vector2 _start;
        private int _width;

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
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="width"></param>
        /// <param name="color"></param>
        public Line(Vector2 start, Vector2 end, int width, ColorBGRA color)
        {
            _line = new SharpDX.Direct3D9.Line(Device);
            Width = width;
            Color = color;
            _start = start;
            _end = end;
        }

        /// <summary>
        /// 
        /// </summary>
        public Vector2 Start
        {
            get { return StartPositionUpdate != null ? StartPositionUpdate() : _start; }
            set { _start = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Vector2 End
        {
            get { return EndPositionUpdate != null ? EndPositionUpdate() : _end; }
            set { _end = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public PositionDelegate StartPositionUpdate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public PositionDelegate EndPositionUpdate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Width
        {
            get { return _width; }
            set
            {
                _line.Width = value;
                _width = value;
            }
        }

        public override void OnEndScene()
        {
            try
            {
                if (_line.IsDisposed)
                {
                    return;
                }

                _line.Begin();
                _line.Draw(new[] { Start, End }, Color);
                _line.End();
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Common.Render.Line.OnEndScene: " + e);
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
