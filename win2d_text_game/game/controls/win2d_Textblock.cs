using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Windows.UI.Input;

namespace win2d_text_game
{
    public class win2d_Textblock : win2d_Control
    {
        private static int PaddingX = 10;
        private static int PaddingY = 10;

        private Rect Border { get; set; }

        private win2d_TextblockStringCollection Strings;
        private win2d_ScrollBar ScrollBar { get; set; }

        public win2d_Textblock(Vector2 position, int width, int height) : base(position, width, height)
        {
            Border = new Rect(Position.X, Position.Y, Width, Height);
            Strings = new win2d_TextblockStringCollection(new Vector2(Position.X + PaddingX, Position.Y + PaddingY));
            Click += Win2d_Textblock_Click;

            // calculate scrollbar layout
            ScrollBar = new win2d_ScrollBar(Vector2.Zero, 0, 0);
            ScrollBar.ScrollUp += ScrollBar_ScrollUp;
            ScrollBar.ScrollDown += ScrollBar_ScrollDown;
        }

        private void ScrollBar_ScrollDown()
        {
            throw new NotImplementedException();
        }

        private void ScrollBar_ScrollUp()
        {
            throw new NotImplementedException();
        }

        private void Win2d_Textblock_Click(PointerPoint point)
        {
            if(ScrollBar != null && ScrollBar.HitTest(point.Position))
            {
                // ScrollBar.OnClick(point);
            }
        }

        public override void Draw(CanvasAnimatedDrawEventArgs args)
        {
            DrawBorder(args);
            DrawStrings(args);
            DrawScrollBar(args);
        }

        private void DrawBorder(CanvasAnimatedDrawEventArgs args)
        {
            if (Border != null)
            {
                args.DrawingSession.DrawRectangle(Border, Colors.White);
            }
        }

        private void DrawStrings(CanvasAnimatedDrawEventArgs args)
        {
            Strings.Draw(args);
        }

        private void DrawScrollBar(CanvasAnimatedDrawEventArgs args)
        {
            if (ScrollBar != null)
            {
                ScrollBar.Draw(args);
            }
        }

        public void Append(CanvasDevice device, string str)
        {
            Strings.Add(device, str);
        }
    }
}
