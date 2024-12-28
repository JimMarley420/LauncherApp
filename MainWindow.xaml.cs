using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            LoadAppButtons();
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


                var appData = new AppData
                {
                    AppName = appName,
                    AppPath = appPath,
                    IconPath = iconPath,
                    RunAsAdmin = runAsAdmin
                };


                var appDataList = AppDataManager.LoadAppData();
                appDataList.Add(appData);
                AppDataManager.SaveAppData(appDataList);

                var appButton = new Button
                {
                    Content = appName,
                    Tag = appData, 
                    Style = (Style)FindResource("AppButtonStyle"),
                    Width = 180,
                    Height = 180,
                    Margin = new Thickness(15),
                    VerticalAlignment = VerticalAlignment.Top
                };


                if (!string.IsNullOrEmpty(iconPath))
                {
                    try
                    {
                        var iconImage = new BitmapImage(new Uri(iconPath));
                        appButton.Background = new ImageBrush(iconImage);
                        appButton.Content = null; 
                    }
                    catch
                    {
                        MessageBox.Show("Failed to load icon. Defaulting to text-only.");
                    }
                }


                AppWrapPanel.Children.Add(appButton);

  
                appButton.Click += (s, args) =>
                {
                    var appInfo = (AppData)appButton.Tag;
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = appInfo.AppPath,
                        UseShellExecute = true,
                        Verb = appInfo.RunAsAdmin ? "runas" : ""
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


                var contextMenu = new ContextMenu();

                var deleteMenuItem = new MenuItem
                {
                    Header = "Delete",
                    Background = Brushes.Red,
                    Foreground = Brushes.White
                };
                deleteMenuItem.Click += (s, args) =>
                {
                    AppWrapPanel.Children.Remove(appButton); // Supprimer le bouton de l'application
                    appDataList.Remove(appData); // Supprimer l'application de la liste
                    AppDataManager.SaveAppData(appDataList); // Sauvegarder les changements
                };

                contextMenu.Items.Add(deleteMenuItem);
                appButton.ContextMenu = contextMenu;
            }
        }

        private void LoadAppButtons()
        {
            var appDataList = AppDataManager.LoadAppData();

            foreach (var appData in appDataList)
            {

                var appButton = new Button
                {
                    Content = appData.AppName,
                    Tag = appData,
                    Style = (Style)FindResource("AppButtonStyle"),
                    Width = 180,
                    Height = 180,
                    Margin = new Thickness(15),
                    VerticalAlignment = VerticalAlignment.Top
                };


                if (!string.IsNullOrEmpty(appData.IconPath))
                {
                    try
                    {
                        var iconImage = new BitmapImage(new Uri(appData.IconPath));
                        appButton.Background = new ImageBrush(iconImage);
                        appButton.Content = null; // Retirer le texte si une image est définie
                    }
                    catch
                    {
                        MessageBox.Show("Failed to load icon. Defaulting to text-only.");
                    }
                }

                AppWrapPanel.Children.Add(appButton);


                appButton.Click += (s, args) =>
                {
                    var appInfo = (AppData)appButton.Tag;
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = appInfo.AppPath,
                        UseShellExecute = true,
                        Verb = appInfo.RunAsAdmin ? "runas" : ""
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


                var contextMenu = new ContextMenu();

                var deleteMenuItem = new MenuItem
                {
                    Header = "Delete",
                    Background = Brushes.Red,
                    Foreground = Brushes.White
                };
                deleteMenuItem.Click += (s, args) =>
                {
                    AppWrapPanel.Children.Remove(appButton);
                    appDataList.Remove(appData); 
                    AppDataManager.SaveAppData(appDataList); 
                };

                contextMenu.Items.Add(deleteMenuItem);
                appButton.ContextMenu = contextMenu;
            }
        }
    }

    public class AppData
    {
        public string AppName { get; set; }
        public string AppPath { get; set; }
        public string IconPath { get; set; }
        public bool RunAsAdmin { get; set; }
    }

    public static class AppDataManager
    {
        private static string dataFile = "appData.json"; 

        public static void SaveAppData(List<AppData> appDataList)
        {
            var json = JsonConvert.SerializeObject(appDataList, Formatting.Indented);
            File.WriteAllText(dataFile, json);
        }

        public static List<AppData> LoadAppData()
        {
            if (File.Exists(dataFile))
            {
                var json = File.ReadAllText(dataFile);
                return JsonConvert.DeserializeObject<List<AppData>>(json);
            }
            return new List<AppData>();
        }
    }
}
