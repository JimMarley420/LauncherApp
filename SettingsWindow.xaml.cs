using System.Windows;
using Microsoft.Win32;

namespace Luncher
{
    public partial class SettingsWindow : Window
    {
        // Properties to hold user input
        public string AppName => AppNameTextBox.Text;
        public string AppPath { get; private set; }
        public string IconPath { get; private set; }
        public bool RunAsAdmin => AdminCheckBox.IsChecked ?? false;

        public SettingsWindow()
        {
            InitializeComponent();
            // Clear default text when focused
            AppNameTextBox.GotFocus += (s, e) => {
                if (AppNameTextBox.Text == "Enter App Name") AppNameTextBox.Text = "";
            };
        }

        private void SelectAppButton_Click(object sender, RoutedEventArgs e)
        {
            // Open file dialog to select executable file
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Executable Files (*.exe)|*.exe",
                Title = "Select Application Executable"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                AppPath = openFileDialog.FileName;
                SelectAppButton.Content = "Executable Selected";
            }
        }

        private void SelectIconButton_Click(object sender, RoutedEventArgs e)
        {
            // Open file dialog to select icon file
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.png;*.jpg;*.ico)|*.png;*.jpg;*.ico",
                Title = "Select Icon"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                IconPath = openFileDialog.FileName;
                SelectIconButton.Content = "Icon Selected";
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(AppName) || string.IsNullOrEmpty(AppPath))
            {
                MessageBox.Show("Please provide both an application name and an executable path.", "Incomplete Details", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true; // Close dialog with success
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Close dialog without saving
        }
    }
}
