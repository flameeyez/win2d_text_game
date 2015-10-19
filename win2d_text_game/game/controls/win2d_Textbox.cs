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
using System.Runtime.InteropServices;

namespace win2d_text_game
{
    class win2d_Textbox : win2d_Control
    {
        private static int PaddingX = 10;
        private static int PaddingY = 10;

        // CanvasTextLayout?
        public string Text { get; set; }
        // public int MaxTextLength { get; set; }

        private win2d_TextboxCursor Cursor { get; set; }
        private int CursorStringIndex { get; set; }
        private bool bUpdateCursorPosition { get; set; }
        private Rect Border { get; set; }
        private Vector2 TextPosition { get; set; }

        private HashSet<VirtualKey> KeyboardState = new HashSet<VirtualKey>();

        public win2d_Textbox(CanvasDevice device, Vector2 position, int width) : base(position, width, -1)
        {
            CanvasTextLayout layout = new CanvasTextLayout(device, "TEST!", Statics.DefaultFont, 0, 0);

            Position = position;
            Width = width;
            Height = (int)layout.LayoutBounds.Height + PaddingY * 2;
            Color = Colors.White;

            TextPosition = new Vector2(position.X + PaddingX, position.Y + PaddingY);
            Border = new Rect(position.X, position.Y, width, Height);

            Cursor = new win2d_TextboxCursor(device, Colors.White);
            CursorStringIndex = 0;
            bUpdateCursorPosition = true;
        }

        #region Draw
        public override void Draw(CanvasAnimatedDrawEventArgs args)
        {
            DrawBorder(args);
            DrawText(args);

            if (HasFocus)
            {
                DrawCursor(args);
            }
        }

        private void DrawBorder(CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.DrawRectangle(Border, Color);
        }

        private void DrawText(CanvasAnimatedDrawEventArgs args)
        {
            if (Text == null) { return; }
            args.DrawingSession.DrawText(Text, TextPosition, Colors.White, Statics.DefaultFont);
        }

        private void DrawCursor(CanvasAnimatedDrawEventArgs args)
        {
            if (bUpdateCursorPosition)
            {
                Cursor.Position = CalculateCursorPosition(args.DrawingSession);
            }

            Cursor.Draw(args);
        }
        #endregion

        #region Event Handling
        public override bool KeyDown(VirtualKey virtualKey)
        {
            if (virtualKey == VirtualKey.Back)
            {
                if (Text != null && Text.Length > 0)
                {
                    Text = Text.Substring(0, Text.Length - 1);
                    --CursorStringIndex;
                    bUpdateCursorPosition = true;
                }
            }
            else
            {
                KeyboardState.Add(virtualKey);
            }

            // consider if textbox has focus?
            return true;
        }
        public override bool KeyUp(VirtualKey virtualKey)
        {
            if (!KeyboardState.Contains(virtualKey)) { return true; }
            KeyboardState.Remove(virtualKey);

            if (virtualKey == VirtualKey.Left)
            {
                if (CursorStringIndex < Text.Length)
                {
                    ++CursorStringIndex;
                }
            }
            else if (virtualKey == VirtualKey.Right)
            {
                if(CursorStringIndex > 0)
                {
                    --CursorStringIndex;
                }
            }
            else
            {
                string s = Statics.VirtualKeyToString(virtualKey, KeyboardState.Contains(VirtualKey.Shift));
                if (s.Length > 1) { throw new Exception(); }
                Text += s;

                ++CursorStringIndex;
                bUpdateCursorPosition = true;
            }

            return true;
        }
        public override bool MouseDown(Point point)
        {
            return HitTest(point);
        }
        #endregion

        private Vector2 CalculateCursorPosition(ICanvasResourceCreator resourceCreator)
        {
            Statics.CalculateCount++;
            bUpdateCursorPosition = false;

            if (Text == null || Text.Length == 0)
            {
                return new Vector2(Position.X + PaddingX, Position.Y + PaddingY);
            }
            else
            {
                CanvasTextLayout layout = new CanvasTextLayout(resourceCreator, Text.Replace(' ', '.'), Statics.DefaultFont, 0, 0);
                return new Vector2(Position.X + (float)layout.LayoutBounds.Width + PaddingX, Position.Y + PaddingY);
            }
        }

        public override void Update(CanvasAnimatedUpdateEventArgs args)
        {
            Cursor.Update(args);
        }
    }
}
