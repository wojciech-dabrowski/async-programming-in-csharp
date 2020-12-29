using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AsyncProgramming.DesktopApp
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Random _rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SyncButton_Click(object sender, RoutedEventArgs e)
        {
            var text = GetText();
            TextBox.Text = text;
        }

        private async void AsyncButton_Click(object sender, RoutedEventArgs e)
        {
            var text = await GetTextAsync();
            TextBox.Text = text;
        }

        private static async Task<string> GetTextAsync()
        {
            // Simulate getting some value asynchronously with delay
            await Task.Delay(3000);
            return "Returned string using async method";
        }

        private static string GetText()
        {
            // Simulate getting some value synchronously with delay
            Thread.Sleep(3000);
            return "Returned string using sync method";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
            => ((Button) sender).Background =
               new SolidColorBrush(Color.FromArgb(GetRandomColorComponent(), GetRandomColorComponent(), GetRandomColorComponent(), GetRandomColorComponent()));

        private byte GetRandomColorComponent() => (byte) _rnd.Next(0, 256);
    }
}
