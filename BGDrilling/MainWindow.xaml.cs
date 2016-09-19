using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BGDrilling
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Image imageBrowse = new Image();
            imageBrowse.Source = new BitmapImage(new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location + @"\..\Images\browse.jpg"));
            imageBrowse.Height = 23;
            imageBrowse.Width = 22;
            buttonLoadOut.Content = imageBrowse;
            Image imageBrowse2 = new Image();
            imageBrowse2.Source = new BitmapImage(new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location + @"\..\Images\browse.jpg"));
            imageBrowse2.Height = 23;
            imageBrowse2.Width = 22;
            buttonLoadIn.Content = imageBrowse2;
        }



        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name
            dlg.DefaultExt = ".csv"; // Default file extension
            dlg.Filter = "CSV Files|*.csv"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                textBoxInput.Text = filename;
            }

        }


        private void textBoxInput_MouseEnter(object sender, MouseEventArgs e)
        {
            textBoxInput.Focus();
            textBoxInput.Foreground = Brushes.Gray;
        }

        private void textBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text=="Path to the input .csv file" || textBox.Text == "Input g value (m/s^2)" || textBox.Text == "Path to the output .csv file")
               textBox.Text = "";
        }

        private void textBoxInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxInput.Text == "")
                textBoxInput.Text = "Path to the input .csv file";
        }

        private void textBoxG_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxG.Text=="")
                textBoxG.Text = "Input g value (m/s^2)";
        }


        private void textBoxOutput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxOutput.Text == "")
                textBoxOutput.Text = "Path to the output .csv file";
        }

        private void buttonCalibrate_Click(object sender, RoutedEventArgs e)
        {
            labelResults.Content = "mxx=1.1, .....................";
            labelResults.Visibility = System.Windows.Visibility.Visible;
            buttonSave.Visibility = System.Windows.Visibility.Visible;
        }

        private void buttonLoadOut_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = ""; // Default file name
            dlg.DefaultExt = ".csv"; // Default file extension
            dlg.Filter = "CSV Files|*.csv"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                textBoxOutput.Text = filename;
            }
        }

        private void textBoxG_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if ((!char.IsDigit(e.Text, e.Text.Length - 1) && e.Text[e.Text.Length - 1]!='.') 
                || (e.Text[e.Text.Length - 1] == '.' && tb.Text.Contains('.')))
            {
                e.Handled = true;
            }
        }

      
    }
}
