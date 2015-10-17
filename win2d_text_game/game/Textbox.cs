using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.System;

namespace win2d_text_game
{
    class Textbox
    {
        private static int PaddingX = 10;
        private static int PaddingY = 10;

        private Vector2 Position { get; set; }
        private int Width { get; set; }
        private int Height { get; set; }

        // CanvasTextLayout?
        public string Text { get; set; }
        public int MaxTextLength { get; set; }

        private TextboxCursor Cursor { get; set; }

        private Rect Border { get; set; }
        private Color Color { get; set; }

        private Vector2 TextPosition { get; set; }

        private HashSet<VirtualKey> KeyboardState = new HashSet<VirtualKey>();

        public Textbox(CanvasDevice device, Vector2 position, int width)
        {
            CanvasTextLayout layout = new CanvasTextLayout(device, "TEST!", Statics.DefaultFont, 0, 0);

            Position = position;
            Width = width;
            Height = (int)layout.LayoutBounds.Height + PaddingY * 2;
            Color = Colors.White;

            TextPosition = new Vector2(position.X + PaddingX, position.Y + PaddingY);
            Border = new Rect(position.X, position.Y, width, Height);
            Cursor = new TextboxCursor(CalculateCursorPosition(device), Colors.White);
        }

        public void Draw(CanvasAnimatedDrawEventArgs args)
        {
            DrawBorder(args);
            DrawText(args);
            DrawCursor(args);
        }

        private void DrawBorder(CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.DrawRectangle(Border, Color);
        }

        private void DrawText(CanvasAnimatedDrawEventArgs args)
        {
            if(Text != null)
            {
                args.DrawingSession.DrawText(Text, TextPosition, Colors.White, Statics.DefaultFont);
            }            
        }

        private void DrawCursor(CanvasAnimatedDrawEventArgs args)
        {
            Cursor.Position = CalculateCursorPosition(args.DrawingSession);
            Cursor.Draw(args);
        }

        public void KeyDown(VirtualKey virtualKey)
        {
            if (!KeyboardState.Contains(virtualKey))
            {
                KeyboardState.Add(virtualKey);
            }

            if (virtualKey == VirtualKey.Back && Text.Length > 0)
            {
                Text = Text.Substring(0, Text.Length - 1);
            }
        }

        public void KeyUp(VirtualKey virtualKey)
        {
            if (!KeyboardState.Contains(virtualKey)) { return; }

            if (virtualKey >= VirtualKey.Number0 && virtualKey <= VirtualKey.Number9)
            {
                Text += virtualKey;
            }
            else if (virtualKey == VirtualKey.Space)
            {
                Text += " ";
            }
            else if (virtualKey >= VirtualKey.A && virtualKey <= VirtualKey.Z)
            {
                if (KeyboardState.Contains(VirtualKey.Shift))
                {
                    Text += virtualKey;
                }
                else
                {
                    Text += virtualKey.ToString().ToLower();
                }
            }
            //else
            //{
            //    int keyValue = (int)args.VirtualKey;
            //    if ((keyValue >= 0x30 && keyValue <= 0x39) // numbers
            //     || (keyValue >= 0x41 && keyValue <= 0x5A) // letters
            //     || (keyValue >= 0x60 && keyValue <= 0x69)) // numpad
            //    {
            //        textbox.Text += args.VirtualKey.ToString().ToLower();
            //    }
            //}
        }

        private Vector2 CalculateCursorPosition(ICanvasResourceCreator resourceCreator)
        {
            if (Text == null)
            {
                return new Vector2(Position.X + PaddingX, Position.Y);
            }
            else
            {
                CanvasTextLayout layout = new CanvasTextLayout(resourceCreator, Text, Statics.DefaultFont, 0, 0);
                return new Vector2(Position.X + (float)layout.LayoutBounds.Width + PaddingX, Position.Y);
            }
        }

        public void Update(CanvasAnimatedUpdateEventArgs args)
        {
            Cursor.Update(args);
        }

        public void KeyPress()
        {
            // update Text
            // update TextLayout
            // update cursor position
        }
    }
}
