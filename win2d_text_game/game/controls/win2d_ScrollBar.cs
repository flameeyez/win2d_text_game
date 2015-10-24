using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Input;
using Microsoft.Graphics.Canvas;

namespace win2d_text_game
{
    public delegate void ScrollEventHandler();

    public class win2d_ScrollBar : win2d_Control
    {
        private Rect TopArrowRect { get; set; }
        private Rect MiddleRect { get; set; }
        private Rect BottomArrowRect { get; set; }
        private Rect BorderRect { get; set; }

        public event ScrollEventHandler ScrollUp;
        public event ScrollEventHandler ScrollDown;

        public win2d_ScrollBar(Vector2 position, int width, int height) : base(position, width, height)
        {
            Position = position;
            Width = width;
            Height = height;

            TopArrowRect = new Rect(Position.X, Position.Y, Width, Width);
            MiddleRect = new Rect(Position.X, Position.Y + Width, Width, Height - Width * 2);
            BottomArrowRect = new Rect(Position.X, Position.Y + Height - Width, Width, Width);
            BorderRect = new Rect(Position.X, Position.Y, Width, Height);

            Click += Win2d_ScrollBar_Click;
        }

        private void Win2d_ScrollBar_Click(PointerPoint point)
        {
            if(HitTestTopArrow(point.Position))
            {
                if(ScrollUp != null) { ScrollUp(); }
            }
            else if(HitTestBottomArrow(point.Position))
            {
                if (ScrollDown != null) { ScrollDown(); }
            }
        }

        private bool HitTestTopArrow(Point point)
        {
            if(point.X < TopArrowRect.X) { return false; }
            if(point.X >= TopArrowRect.X + TopArrowRect.Width) { return false; }
            if(point.Y < TopArrowRect.Y) { return false; }
            if(point.Y >= TopArrowRect.Y + TopArrowRect.Height) { return false; }

            return true;
        }

        private bool HitTestBottomArrow(Point point)
        {
            if (point.X < BottomArrowRect.X) { return false; }
            if (point.X >= BottomArrowRect.X + BottomArrowRect.Width) { return false; }
            if (point.Y < BottomArrowRect.Y) { return false; }
            if (point.Y >= BottomArrowRect.Y + BottomArrowRect.Height) { return false; }

            return true;
        }

        #region Draw
        public override void Draw(CanvasAnimatedDrawEventArgs args)
        {
            DrawTopArrow(args);
            DrawMiddle(args);
            DrawBottomArrow(args);
            DrawBorder(args);
        }

        private void DrawTopArrow(CanvasAnimatedDrawEventArgs args)
        {
            if(TopArrowRect != null)
            {
                args.DrawingSession.FillRectangle(TopArrowRect, Colors.LightGray);
                
                // draw arrow
            }
        }

        private void DrawMiddle(CanvasAnimatedDrawEventArgs args)
        {
            if (MiddleRect != null)
            {
                args.DrawingSession.FillRectangle(MiddleRect, Colors.DarkGray);
            }
        }

        private void DrawBottomArrow(CanvasAnimatedDrawEventArgs args)
        {
            if (BottomArrowRect != null)
            {
                args.DrawingSession.FillRectangle(BottomArrowRect, Colors.LightGray);

                // draw arrow
            }
        }

        private void DrawBorder(CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.DrawRectangle(BorderRect, Colors.White);
        }
        #endregion
    }
}
