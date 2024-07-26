using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Generic; // import (4.1)
using System.IO;

namespace AT1Dip
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /* 4.1 Create two data structures using the LinkedList<T> class from the C# Systems.Collections.Generic
         * namespace. The data must be of type "double". The two LinkedLists of type double are to be declared as global
         * within the "public partial class".
         */
        LinkedList<double> sensorA = new LinkedList<double>();
        LinkedList<double> sensorB = new LinkedList<double>();

        /*
         * 4.2 Copy the Galileo.DLL file into the root directory of your solution folder and add the appropiate
         * reference in the solution explorer. Create a method called "LoadData" which will populate both 
         * LinkedLists. Declare an instance of the Galileo library in the method and create the appropirate
         * loop construct to populate the two LinkedList; the data from Sensor A will populate the first LinkedList,
         * while the data from Sensor B will populate the second LinkedList. The LinkedList size will be hardcoded
         * inside the method and must be equal to 400. The input parameters are empty, and the return type is void.
         */

        public void LoadData()
        {
            string galileoPath = "D:\\TAFE DIPLOMA\\Monday C# - Milan\\Projects\\AT1Dip\\Galileo6.dll";

            if (File.Exists(galileoPath))
            {
                using (var stream = File.Open(galileoPath, FileMode.Open))
                {
                    foreach (double data in sensorA)
                    {
                        sensorA.AddFirst(400); // Sensor A linked listsize 400
                    }
                    
                    foreach (double data in sensorB)
                    {
                        sensorB.AddFirst(400); //Sensor B linked listsize 400
                    } 
                }
            }
            else
            {
                MessageBox.Show("ERROR: Cannot find the DLL file!");
            }
            
        }

        private void btnLoadAB_Click(object sender, RoutedEventArgs e)
        {
            LoadData(); // call load data method
        }
    }
}