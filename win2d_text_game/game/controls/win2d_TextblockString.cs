using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace win2d_text_game
{
    class win2d_TextblockString
    {
        public CanvasTextLayout Text { get; set; }
        public Vector2 Position { get; set; }

        public win2d_TextblockString(CanvasDevice device, string text, Vector2 position)
        {
            Text = new CanvasTextLayout(device, text, Statics.DefaultFont, 0, 0);
            Position = position;
        }
    }
}
