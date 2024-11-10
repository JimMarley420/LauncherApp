using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;

namespace Luncher
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void OpenSettingsWindow_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            if (settingsWindow.ShowDialog() == true)
            {
                string appName = settingsWindow.AppName;
                string appPath = settingsWindow.AppPath;
                string iconPath = settingsWindow.IconPath;
                bool runAsAdmin = settingsWindow.RunAsAdmin;

                // Create the button for the app
                var appButton = new Button
                {
                    Content = appName,
                    Tag = new { appPath, runAsAdmin },
                    Style = (Style)FindResource("AppButtonStyle"),
                    Width = 180,
                    Height = 180,
                    Margin = new Thickness(15),
                    VerticalAlignment = VerticalAlignment.Top
                };

                // Set the icon image if available
                if (!string.IsNullOrEmpty(iconPath))
                {
                    try
                    {
                        var iconImage = new BitmapImage(new Uri(iconPath));
                        appButton.Background = new ImageBrush(iconImage);
                        appButton.Content = null; // Remove text when an image is set
                    }
                    catch
                    {
                        MessageBox.Show("Failed to load icon. Defaulting to text-only.");
                    }
                }

                // Create a context menu for the app button
                var contextMenu = new ContextMenu();

                var deleteMenuItem = new MenuItem
                {
                    Header = "Delete",
                    Background = Brushes.Red,
                    Foreground = Brushes.White
                };
                deleteMenuItem.Click += (s, args) =>
                {
                    AppWrapPanel.Children.Remove(appButton); // Remove the app button
                };

                contextMenu.Items.Add(deleteMenuItem);
                appButton.ContextMenu = contextMenu;

                // Add the app button to the WrapPanel
                AppWrapPanel.Children.Add(appButton);

                // Define what happens when the app button is clicked
                appButton.Click += (s, args) =>
                {
                    var appInfo = (dynamic)appButton.Tag;
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = appInfo.appPath,
                        UseShellExecute = true,
                        Verb = appInfo.runAsAdmin ? "runas" : ""
                    };

                    try
                    {
                        Process.Start(startInfo);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to start application: {ex.Message}");
                    }
                };
            }
        }
    }
}
