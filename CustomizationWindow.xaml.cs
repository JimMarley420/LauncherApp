using System.Windows;
using System.Windows.Media;
using System.Windows.Forms;
using System.Windows.Controls;

namespace Luncher
{
    public partial class CustomizationWindow : Window
    {
        public CustomizationWindow()
        {
            InitializeComponent();
        }

        // Open color dialog to choose background color
        private void ChooseBackgroundColor_Click(object sender, RoutedEventArgs e)
        {
            var colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Change the background color of the window
                this.Background = new SolidColorBrush(Color.FromRgb(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
            }
        }

        // Save the customization settings
        private void SaveCustomization_Click(object sender, RoutedEventArgs e)
        {
            string selectedSize = (ButtonSizeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            // Logic to apply button size customization (implement size logic as needed)
            System.Windows.MessageBox.Show($"Customization saved! Button size: {selectedSize}", "Customization", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }

        // Close the customization window
        private void CloseCustomizationWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
