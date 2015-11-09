using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.System;
using Windows.UI.Input;
using System.Text;
using Microsoft.Graphics.Canvas;

namespace win2d_text_game
{
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();

            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
        }

        #region Initialization
        private void canvasMain_CreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            Statics.Initialize(sender.Device);
            UI.Initialize(sender);
            Game.Initialize();

            Game.OnGameCommand += Game_OnGameCommand;
        }

        private void Game_OnGameCommand(string strAppend)
        {
            UI.DisplayText(strAppend);
        }
        #endregion

        #region UI Event Handling
        private void CoreWindow_KeyDown(CoreWindow sender, Windows.UI.Core.KeyEventArgs args) { UI.KeyDown(args.VirtualKey); }
        private void CoreWindow_KeyUp(CoreWindow sender, Windows.UI.Core.KeyEventArgs args) { UI.KeyUp(args.VirtualKey); }
        private void canvasMain_PointerPressed(object sender, PointerRoutedEventArgs e) { UI.PointerPressed(e); }
        private void canvasMain_PointerMoved(object sender, PointerRoutedEventArgs e) { UI.PointerMoved(e); }
        private void canvasMain_PointerReleased(object sender, PointerRoutedEventArgs e) { UI.PointerReleased(e); }
        private void canvasMain_PointerWheelChanged(object sender, PointerRoutedEventArgs e) { UI.PointerWheelChanged(e); }
        private void canvasMain_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args) { UI.Draw(args); }
        private void canvasMain_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args) { UI.Update(args); }
        #endregion
    }
}
