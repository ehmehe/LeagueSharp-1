using System;
using SharpDX;
using SharpDX.Direct3D9;

namespace LeagueSharp.CommonEx.Core.Render.RenderObjects
{
    public class Text : Render.RenderObject
    {
        public delegate Vector2 PositionDelegate();

        public delegate string TextDelegate();

        private string _text;
        private Font _textFont;
        private int _x;
        private int _y;
        public bool Centered = false;
        public Vector2 Offset;
        public bool OutLined = false;
        public PositionDelegate PositionUpdate;
        public TextDelegate TextUpdate;
        public Obj_AI_Base Unit;

        /// <summary>
        /// 
        /// </summary>
        public static Device Device
        {
            get { return Drawing.Direct3DDevice; }
        }

        public Text(string text, int x, int y, int size, ColorBGRA color, string fontName = "Calibri")
        {
            Color = color;
            this.text = text;

            _x = x;
            _y = y;

            _textFont = new Font(
                Device,
                new FontDescription
                {
                    FaceName = fontName,
                    Height = size,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.Default
                });
        }

        public Text(string text, Vector2 position, int size, ColorBGRA color, string fontName = "Calibri")
        {
            Color = color;
            this.text = text;

            _x = (int)position.X;
            _y = (int)position.Y;

            _textFont = new Font(
                Device,
                new FontDescription
                {
                    FaceName = fontName,
                    Height = size,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.Default
                });
        }

        public Text(string text,
            Obj_AI_Base unit,
            Vector2 offset,
            int size,
            ColorBGRA color,
            string fontName = "Calibri")
        {
            Unit = unit;
            Color = color;
            this.text = text;
            Offset = offset;

            var pos = unit.HPBarPosition + offset;

            _x = (int)pos.X;
            _y = (int)pos.Y;

            _textFont = new Font(
                Device,
                new FontDescription
                {
                    FaceName = fontName,
                    Height = size,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.Default
                });
        }

        public Text(int x, int y, string text, int size, ColorBGRA color, string fontName = "Calibri")
        {
            Color = color;
            this.text = text;

            _x = x;
            _y = y;

            _textFont = new Font(
                Device,
                new FontDescription
                {
                    FaceName = fontName,
                    Height = size,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.Default
                });
        }

        public Text(Vector2 position, string text, int size, ColorBGRA color, string fontName = "Calibri")
        {
            Color = color;
            this.text = text;
            _x = (int)position.X;
            _y = (int)position.Y;
            _textFont = new Font(
                Device,
                new FontDescription
                {
                    FaceName = fontName,
                    Height = size,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.Default
                });
        }

        public FontDescription TextFontDescription
        {
            get { return _textFont.Description; }

            set
            {
                _textFont.Dispose();
                _textFont = new Font(Device, value);
            }
        }

        public int X
        {
            get
            {
                var dx = Centered ? -_textFont.MeasureText(null, text, FontDrawFlags.Center).Width / 2 : 0;

                if (PositionUpdate != null)
                {
                    return (int)PositionUpdate().X + dx;
                }

                return _x + dx;
            }
            set { _x = value; }
        }

        public int Y
        {
            get
            {
                var dy = Centered ? -_textFont.MeasureText(null, text, FontDrawFlags.Center).Height / 2 : 0;

                if (PositionUpdate != null)
                {
                    return (int)PositionUpdate().Y + dy;
                }
                return _y + dy;
            }
            set { _y = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ColorBGRA Color { get; set; }

        public string text
        {
            get
            {
                if (TextUpdate != null)
                {
                    return TextUpdate();
                }
                return _text;
            }
            set { _text = value; }
        }

        public override void OnEndScene()
        {
            try
            {
                if (_textFont.IsDisposed || text == "")
                {
                    return;
                }

                if (Unit != null && Unit.IsValid)
                {
                    var pos = Unit.HPBarPosition + Offset;
                    X = (int)pos.X;
                    Y = (int)pos.Y;
                }

                var xP = X;
                var yP = Y;
                if (OutLined)
                {
                    var outlineColor = new ColorBGRA(0, 0, 0, 255);
                    _textFont.DrawText(null, text, xP - 1, yP - 1, outlineColor);
                    _textFont.DrawText(null, text, xP + 1, yP + 1, outlineColor);
                    _textFont.DrawText(null, text, xP - 1, yP, outlineColor);
                    _textFont.DrawText(null, text, xP + 1, yP, outlineColor);
                }
                _textFont.DrawText(null, text, xP, yP, Color);
            }
            catch (Exception e)
            {
                Console.WriteLine(@"Common.Render.Text.OnEndScene: " + e);
            }
        }

        public override void OnPreReset()
        {
            _textFont.OnLostDevice();
        }

        public override void OnPostReset()
        {
            _textFont.OnResetDevice();
        }

        public override void Dispose()
        {
            if (!_textFont.IsDisposed)
            {
                _textFont.Dispose();
            }
        }
    }
}
