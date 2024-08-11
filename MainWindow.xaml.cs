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
using Galileo6; // import the galileo library (4.2)
using System.Runtime.InteropServices;
using System.Numerics;

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

        private void LoadData()
        {
            // declare an instance of the galileo library
            ReadData read = new ReadData();

            // putting the total into a const int as its more efficient than writing it in the for loop
            const int total = 400; // 400 is the max that is required

            for (int i = 0; i < total; i++)
            {
                sensorA.AddFirst(read.SensorA(double.Parse(mu1.Text), double.Parse(sigma1.Text))); // Convert text to double for sigma and mu
                sensorB.AddFirst(read.SensorB(double.Parse(mu1.Text), double.Parse(sigma1.Text)));
            }
        }


        /*
         * 4.3 Create a custom method called "ShowAllSensorData" which will display both
         * linkedLists in a ListView. Add column titles "Sensor A" and "Sensor B" to the ListView.
         * The input parameters are empty, and the return type is void.
         */
        private void ShowAllSensorData()
        {
            listView1.Items.Clear(); // clear items in the listview

            // display linked list for sensor A
            foreach (double data in sensorA)
            {
                listView1.Items.Add(data);
            }

            // display linked list for sensor B
            foreach (double data in sensorB)
            {
                listView1.Items.Add(data);
            }
        }

        /*
         *  4.4 Create a button and associated click method that will call the LoadData and ShowAllSensorData methods.
         *  The input parameters are empty, and the return type is void. 
         */
        private void btnLoadAB_Click(object sender, RoutedEventArgs e)
        {
            LoadData(); // call load data method
            ShowAllSensorData(); // call show all sensor method
            DisplayListBoxData(listBoxIdentifier: sensorA, listBox: listBoxA); // call Display List Box Data method
        }


        /*
         * 4.5 Create a method called “NumberOfNodes” that will return an integer which is the number of nodes(elements) in a LinkedList.
         * The method signature will have an input parameter of type LinkedList, and the calling code argument is the linkedlist name. 
         */
        private int NumberOfNodes(LinkedList<double> nodes)
        {
            return nodes.Count;
        }

        /*
         * 4.6 Create a method called “DisplayListboxData” that will display the content of a LinkedList inside the appropriate ListBox.
         * The method signature will have two input parameters; a LinkedList, and the ListBox name. 
         * The calling code argument is the linkedlist name and the listbox name. 
         */
        private void DisplayListBoxData(LinkedList<double> listBoxIdentifier, ListBox listBox)
        {
            listBoxA.Items.Clear();
            listBoxB.Items.Clear();

            foreach (double data in listBoxIdentifier)
            {
                listBoxA.Items.Add(data);
                listBoxB.Items.Add(data);
            }
        }

        /*
         * 4.7 Create a method called “SelectionSort” which has a single input parameter of type LinkedList, while the calling code argument is the linkedlist name.
         * The method code must follow the pseudo code supplied below in the Appendix. 
         * The return type is Boolean. 
         */
        private bool SelectionSort(LinkedList<double> list)
        {
            int min = 0;
            int max = NumberOfNodes(list);

            for (int i = 0; i < max -1; i++)
            {
                min = i;
                for (int j = i + 1; j < max; j++)
                {
                    if (list.ElementAt(j) < list.ElementAt(min))
                    {
                        min = j;
                    }   
                }
                LinkedListNode<double> currentMin = list.Find(list.ElementAt(min));
                LinkedListNode<double> currentI = list.Find(list.ElementAt(i));

                var temp = currentMin.Value;
                currentMin.Value = currentI.Value;
                currentI.Value = temp;
            }
            return true;

            /*
             * integer min => 0 

               integer max => numberOfNodes(list) 

               for ( i = 0 to max - 1 ) 

               min => i 

               for ( j = i + 1 to max ) 

               if (list element(j) < list element(min)) 

               min => j 

               END for 

               // Supplied C# code 

               LinkedListNode<double> currentMin = list.Find(list.ElementAt(min)) 

               LinkedListNode<double> currentI = list.Find(list.ElementAt(i)) 

               // End of supplied C# code 

               var temp = currentMin.Value 

               currentMin.Value = currentI.Value 

               currentI.Value = temp 

               END for 
             */
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectionSort(list: sensorA); // call selectionSort method
            DisplayListBoxData(listBoxIdentifier: sensorA, listBox: listBoxA); // call Display List Box Data method
        }

        /*
         * 4.8 Create a method called "InsertionSort" which has a single parameter of type LinkedList,
         * while the calling code argument is the linkedlist name. The method code must follow the psuedo code
         * supplied below in the Appendix. The return type is Boolean.
         */
        private bool InsertionSort(LinkedList<double> list)
        {
            int max = NumberOfNodes(list);
            for (int i = 0; i < max -1; i++)
            {
                for (int j = i + 1; j < max; j--)
                {
                    if (list.ElementAt(j - 1) > list.ElementAt(j))
                    {
                        // swap code
                        /*
                         * I made the placeholder variables because when i wanted to do list.ElementAt(j - 1) for instance
                         * I would get compiler error CS0131.
                         * Janky logic i know :(
                         */
                      LinkedListNode<double> current = list.Find(list.ElementAt(j));
                      var temp = list.ElementAt(j - 1);
                      var placeHolder1 = list.ElementAt(j);
                      var placeHolder2 = list.ElementAt(j - 1);
                      placeHolder2 = list.ElementAt(j);
                      placeHolder1 = temp;
                    }
                }
            }
            return true; // return type boolean

            /*
             * integer max = numberOfNodes(list) 

                for ( i = 0 to max – 1 ) 

                for ( j = i + 1 to j > 0, j-- ) 

                if (list element(j - 1) > list element(j)) 

                // Supplied C# code 

                LinkedListNode<double> current = list.Find(list.ElementAt(j)) 

                // End of supplied C# code 

                // Add Swap code here by swapping 

                // previous value with current value. 

                END if 

                END for 

                END for 
             */
        }

        private void btnInserationSort_Click(object sender, RoutedEventArgs e)
        {
            InsertionSort(list: sensorA);
            DisplayListBoxData(listBoxIdentifier: sensorA, listBox: listBoxA); // call Display List Box Data method
        }
    }
}