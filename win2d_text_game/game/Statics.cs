﻿using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.Foundation;
using Microsoft.Graphics.Canvas.Text;
using System.Numerics;
using Microsoft.Graphics.Canvas;

namespace win2d_text_game
{
    public static class Statics
    {
        public static int MaxStringWidth = 0;

        public static int MouseX = 0;
        public static int MouseY = 0;
        public static int FrameCount = 0;

        // layout dimensions
        public static int RightColumnWidth = 800;
        public static int RightColumnPadding = 10;
        public static int LeftColumnPadding = 10;
        public static int LeftColumnWidth;
        public static Vector2 ColumnDividerTop;
        public static Vector2 ColumnDividerBottom;

        public static int CanvasHeight;

        public static Vector2 MapPosition = new Vector2(0, 0);

        public static CanvasBitmap CrownImage;

        // probability that region will continue to try to expand past minimum size
        // calculated once for each tile added
        // e.g. a tile that has just met minimum size requirements has an n% chance of trying to add an additional tile (will fail if attempted add is already occupied),
        //  then an n% chance of attempting to add another tile after that, and so on
        public static int ProbabilityOfExpansion = 0;
        public static int MinimumRegionSize = 100;
        public static int MergeThreshold = 500;

        // timing
        public static int MapUpdateThreshold = 0; //500;
        public static int PauseBetweenBattlesMilliseconds = 500;

        public static Random Random = new Random(DateTime.Now.Millisecond);
        public static CanvasTextFormat FontSmall = new CanvasTextFormat();
        public static CanvasTextFormat FontMedium = new CanvasTextFormat();
        public static CanvasTextFormat FontLarge = new CanvasTextFormat();
        public static CanvasTextFormat FontExtraLarge = new CanvasTextFormat();

        static Statics()
        {
            FontSmall.FontFamily = "Old English Text MT";
            FontSmall.FontSize = 18;
            FontSmall.WordWrapping = CanvasWordWrapping.NoWrap;

            FontMedium.FontFamily = "Old English Text MT";
            FontMedium.FontSize = 24;
            FontMedium.WordWrapping = CanvasWordWrapping.NoWrap;

            FontLarge.FontFamily = "Old English Text MT";
            FontLarge.FontSize = 32;
            FontLarge.WordWrapping = CanvasWordWrapping.NoWrap;

            FontExtraLarge.FontFamily = "Old English Text MT";
            FontExtraLarge.FontSize = 48;
            FontExtraLarge.WordWrapping = CanvasWordWrapping.NoWrap;
        }

        public static Color RandomColor()
        {
            int red = 20 + Statics.Random.Next(235);
            int green = 20 + Statics.Random.Next(235);
            int blue = 20 + Statics.Random.Next(235);

            return Color.FromArgb(255, (byte)red, (byte)green, (byte)blue);
        }

        public static string RandomString(this string[] array)
        {
            return array[Statics.Random.Next(array.Length)];
        }
    }
}