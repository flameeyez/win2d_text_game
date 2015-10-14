using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.System;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace win2d_text_game
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        bool bRight = false;
        int nRectX = 100;
        int nRectY = 100;

        Textbox textbox;

        HashSet<VirtualKey> KeyboardState = new HashSet<VirtualKey>();

        public MainPage()
        {
            this.InitializeComponent();

            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
        }

        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (args.VirtualKey == Windows.System.VirtualKey.Right) { bRight = true; }

            if (!KeyboardState.Contains(args.VirtualKey))
            {
                KeyboardState.Add(args.VirtualKey);
            }

            if(args.VirtualKey == VirtualKey.Back && textbox.Text.Length > 0)
            {
                textbox.Text = textbox.Text.Substring(0, textbox.Text.Length - 1);
            }
        }

        private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (args.VirtualKey == Windows.System.VirtualKey.Right) { bRight = false; }

            if (KeyboardState.Contains(args.VirtualKey))
            {
                KeyboardState.Remove(args.VirtualKey);

                //if (args.VirtualKey == VirtualKey.Back && textbox.Text.Length > 0)
                //{
                //    textbox.Text = textbox.Text.Substring(0, textbox.Text.Length - 1);
                //}
                if (args.VirtualKey >= VirtualKey.Number0 && args.VirtualKey <= VirtualKey.Number9)
                {
                    textbox.Text += args.VirtualKey;
                }
                else if (args.VirtualKey == VirtualKey.Space)
                {
                    textbox.Text += " ";
                }
                else if (args.VirtualKey >= VirtualKey.A && args.VirtualKey <= VirtualKey.Z)
                {
                    if (KeyboardState.Contains(VirtualKey.Shift))
                    {
                        textbox.Text += args.VirtualKey;
                    }
                    else
                    {
                        textbox.Text += args.VirtualKey.ToString().ToLower();
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
        }

        private void gridMain_PointerReleased(object sender, PointerRoutedEventArgs e)
        {

        }

        private void gridMain_PointerMoved(object sender, PointerRoutedEventArgs e)
        {

        }

        private void canvasMain_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.FillRectangle(new Rect(nRectX, nRectY, 100, 100), Colors.Red);

            textbox.Draw(args);
        }

        private void canvasMain_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            if (bRight) { nRectX++; }

            textbox.Update(args);
        }

        private void canvasMain_CreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            textbox = new Textbox(sender.Device, new System.Numerics.Vector2(200, 200), 300, 32);
        }
    }
}
