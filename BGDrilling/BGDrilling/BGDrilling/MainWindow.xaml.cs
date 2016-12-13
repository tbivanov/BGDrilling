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
using System.IO;

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




        private void buttonCalibrate_Click(object sender, RoutedEventArgs e)
        {
            //Load data from file and initialize the acc array
            Sensor[] sensors;
            String path = textBoxInput.Text;
            try
            {
                StreamReader sr = new StreamReader(path);
                labelResults.Content = "";
                string line;
                string[] lineDiv = new string[] { "" };
                line = sr.ReadLine();
                int N = Int32.Parse(line.Split(',')[0]);
                int M = Int32.Parse(line.Split(',')[1]);
                int L = Int32.Parse(line.Split(',')[2]);
                sensors = new Sensor[N + M + L];

                for (int i = 0; i < N; i++)
                {
                    sensors[i] = new Accelerometer();
                }

                for (int i = N; i < N + M; i++)
                {
                    //CREATE GYROS
                }

                for (int i = N + M; i < N + M + L; i++)
                {
                    //CREATE MAGNETOMETERS
                }

                while ((line = sr.ReadLine()) != null)
                {
                    lineDiv = line.Split(',');
                    for (int i = 0; i < N; i++)
                    {
                        Measurement meas = new Measurement
                            (new decimal[] { Decimal.Parse(lineDiv[5 + i * 3]), Decimal.Parse(lineDiv[6 + i * 3]), Decimal.Parse(lineDiv[7 + i * 3]) },
                            Decimal.Parse(lineDiv[2]), Decimal.Parse(lineDiv[4]), Decimal.Parse(lineDiv[3]));
                        sensors[i].data.Add(meas);
                    }
                }
                //MessageBox.Show(sensors[0].data[3].data[0].ToString()+" "+ sensors[1].data[3].data[0].ToString() + " "+ sensors[2].data[3].data[0].ToString() + " ");


                //TODO: Foreach i in sensors, compute calibration parameters and save them in the respective fields of the accelerometer objects
                //decimal[] res = LinearAlgebra.BackwardSubstitutionLow(new decimal[,] { { 65, 0, 0, 0, 0 }, { 10, 2, 0, 0, 0 }, { 3, 1, 1, 0, 0 }, { 23, 1, 1, 20, 0 }, { 23, 4, 10, 20, 40 } }, new decimal[] { 1, 2, 3, 4, 5 });
                 
             
                decimal[] p = { 1, 0.5M, 7M };
                decimal[,] A = { { 21, 10, 40 }, { 50, 31, 60}, { 70, 80, 31 }, {90,130,5460} };
                List<decimal[,]> LU = LinearAlgebra.LUDecomposition(A);
                decimal[,] J1 = MathDecimal.Prod(MathDecimal.Prod(LU[2], A), LU[3]);
                decimal[,] J2 = MathDecimal.Prod(LU[0], LU[1]);

                for (int i = 0; i < LU[1].GetLength(0); i++)
                {
                    for (int j = 0; j < LU[1].GetLength(1); j++)
                        labelResults.Content += LU[1][i, j].ToString() + " ";
                    labelResults.Content += "\n";
                }

                //decimal[] pAdd = { 0.1M, 0.5M, 0.7M };

                //decimal[] res =Optimization.GaussNewton(pAdd);

                //Measurement mеas1 = new Measurement(new decimal[] { 1, 2, 3 }, 0);
                //Accelerometer acc1 = new BGDrilling.Accelerometer();
                //decimal[] res = sensors[0].calibrate();//sensors[0].computeJ(new decimal[] {1,2,3,4,5,6,6,7,5,6,5,6});//test.calibrate();
                /*decimal[,] res = sensors[0].computeJ(new decimal[12] { 1.1010140221357660322585672123M, 1.1010140226772653231732328849M,
1.1010140226577987745426498384M,
71.010269789341302933970247202M,
71.010269804278793653568671568M,
71.010269803736848793938931208M,
0.9999999999999996303434170532M,
1.0000000000000000112202919374M,
0.9999999999999999977944535153M,
0.9290575790068339843251263346M,
66.856235107462547424100808752M,
0.1077311408480793898213008661M});*/

                /*decimal[,] B = MathDecimal.Prod(MathDecimal.Transpose(res), res);
                String [] lines=new string[res.GetLength(0)];
                for (int i = 0; i < B.GetLength(0); i++)
                {
                    lines[i] += "{";
                    for (int j = 0; j < B.GetLength(1); j++)
                    {
                        labelResults.Content += res[i, j].ToString() + "\n";
                        lines[i] += res[i, j].ToString() + ", ";
                    }
                    //labelResults.Content += "\n";
                    lines[i] += "},";
                }*/
                    //bool res1 = MathDecimal.Pow2(MathDecimal.Norm2(pAdd)) - MathDecimal.Pow2(MathDecimal.Norm2(MathDecimal.Sum(p, MathDecimal.Prod(a, pAdd)))) <
                    //       1M / 2M * a * MathDecimal.Pow2(MathDecimal.Norm2(MathDecimal.Prod(J, p))) && a >= 0.00001M;

                    

                

                //labelResults.Content += res.ToString() + "\n";
                //System.Console.WriteLine(lines);

                /*for (int i = 0; i < res.GetLength(0); i++)
                {
                    labelResults.Content += res[i].ToString() + " ";
                    //lines[i] += res[i].ToString() + " ";
                    //for (int j=0; j<res.GetLength(1); j++)
                    {
                    //    labelResults.Content += res[i, j].ToString()+" ";
                    //    
                    }

                    //labelResults.Content += "\n";
                    //
                    /*for (int i = 0; i < res.Length; i++)
                    {
                        labelResults.Content += res[i].ToString() + " ";
                    }
                }*/
            //System.IO.File.WriteAllLines(@"C:\Users\Gali\Desktop\writeLines.txt", lines);

            }
            catch (Exception exc)
            {
                MessageBox.Show("Invalid path to the input file.\n");
            }

            //TODO: Save in archive

            //TODO: Print results
            labelResults.Visibility = Visibility.Visible;
            buttonSave.Visibility = Visibility.Visible;

        }


    }
}
