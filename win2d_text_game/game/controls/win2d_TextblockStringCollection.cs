using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace win2d_text_game
{
    class win2d_TextblockStringCollection
    {
        // keep track of total height
        private List<win2d_TextblockString> Strings = new List<win2d_TextblockString>();

        private int StringPositionX { get; set; }
        private int NextStringPositionY { get; set; }
        private static int StringPaddingY = 5;

        public win2d_TextblockStringCollection(Vector2 position)
        {
            StringPositionX = (int)position.X;
            NextStringPositionY = (int)position.Y;
        }

        public void Draw(CanvasAnimatedDrawEventArgs args)
        {
            foreach(win2d_TextblockString str in Strings)
            {
                args.DrawingSession.DrawTextLayout(str.Text, str.Position, Colors.White);
            }
        }

        public void Add(CanvasDevice device, string str)
        {
            win2d_TextblockString s = new win2d_TextblockString(device, str, new Vector2(StringPositionX, NextStringPositionY));
            Strings.Add(s);
            NextStringPositionY = NextStringPositionY + (int)s.Text.LayoutBounds.Height + StringPaddingY;
        }
    }
}
