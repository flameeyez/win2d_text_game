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

namespace win2d_text_game
{
    class Textbox
    {
        private static int PaddingX = 10;
        private static int PaddingY = 2;

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

        public Textbox(CanvasDevice device, Vector2 position, int width, int height)
        {
            Position = position;
            Width = width;
            Height = height;

            Border = new Rect(position.X, position.Y, width, height);
            Color = Colors.White;

            TextPosition = new Vector2(position.X + PaddingX, position.Y + PaddingY);

            Text = "test!";
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
            args.DrawingSession.DrawText(Text, TextPosition, Colors.White, Statics.DefaultFont);
        }

        private void DrawCursor(CanvasAnimatedDrawEventArgs args)
        {
            Cursor.Position = CalculateCursorPosition(args.DrawingSession);
            Cursor.Draw(args);
        }

        private Vector2 CalculateCursorPosition(ICanvasResourceCreator resourceCreator)
        {
            CanvasTextLayout layout = new CanvasTextLayout(resourceCreator, Text, Statics.DefaultFont, 0, 0);
            return new Vector2(Position.X + (float)layout.LayoutBounds.Width + PaddingX, Position.Y + PaddingY);
        }

        public void Update(CanvasAnimatedUpdateEventArgs args)
        {
            Cursor.Update(args);
        }
    }
}
