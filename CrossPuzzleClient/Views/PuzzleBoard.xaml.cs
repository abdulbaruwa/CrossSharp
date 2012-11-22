using System;
using System.Collections.Generic;
using CrossPuzzleClient.Common;
using CrossPuzzleClient.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace CrossPuzzleClient.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class PuzzleBoard : ViewBase
    {
        public PuzzleBoard()
        {
            InitializeComponent();
        }

        protected override void OnKeyUp(KeyRoutedEventArgs e)
        {
            var keyint = (int) e.Key;
            if (keyint >= 60 && keyint <= 90)
            {
                string keyname = Enum.GetName(typeof (VirtualKey), e.Key);
                Messenger.Default.Send(new KeyReceivedMessage {KeyChar = keyname});
            }
            else if (keyint == 46)
            {
                Messenger.Default.Send(new KeyReceivedMessage {KeyCharType = KeyCharType.Delete});
            }
            else if (keyint == 8)
            {
                Messenger.Default.Send(new KeyReceivedMessage {KeyCharType = KeyCharType.BackSpace});
            }

            base.OnKeyUp(e);
        }
    }
}