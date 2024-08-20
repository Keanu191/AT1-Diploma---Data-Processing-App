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
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Buffers;
using System.Text.RegularExpressions; // import for 4.14
using System.Linq;

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

        #region Global Methods
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
                sensorA.AddLast(read.SensorA(double.Parse(mu1.Text), double.Parse(sigma1.Text))); // Convert text to double for sigma and mu
                sensorB.AddLast(read.SensorB(double.Parse(mu1.Text), double.Parse(sigma1.Text)));
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

            // get the first node of both sensors
            var node1 = sensorA.First;
            var node2 = sensorB.First;

            for (int i = 0; i < sensorA.Count; i++)
            {
                // create anonymous type for the sensor data
                var sensorData = new { A = node1.Value, B = node2.Value };
                listView1.Items.Add(sensorData); // add data from sensorA & B to the listview
                // go to the next node for both sensors
                node1 = node1.Next;
                node2 = node2.Next;
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
            DisplayListBoxData(listBoxIdentifier: sensorB, listBox: listBoxB);
        }
        #endregion

        #region Utility
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
            listBox.Items.Clear(); // clear both listboxes

            foreach (double data in listBoxIdentifier)
            {
                listBox.Items.Add(data);
            }
        }
        #endregion

        #region Sort & Search
        /*
         * 4.7 Create a method called “SelectionSort” which has a single input parameter of type LinkedList, while the calling code argument is the linkedlist name.
         * The method code must follow the pseudo code supplied below in the Appendix. 
         * The return type is Boolean. 
         */
        private bool SelectionSort(LinkedList<double> list)
        {
            int min = 0;
            int max = NumberOfNodes(list);

            for (int i = 0; i < max - 1; i++)
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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectionSort(list: sensorB); // call selectionSort method
            DisplayListBoxData(listBoxIdentifier: sensorB, listBox: listBoxB); // call Display List Box Data method
        }

        /*
         * 4.8 Create a method called "InsertionSort" which has a single parameter of type LinkedList,
         * while the calling code argument is the linkedlist name. The method code must follow the psuedo code
         * supplied below in the Appendix. The return type is Boolean.
         */
        private bool InsertionSort(LinkedList<double> list)
        {
            int max = NumberOfNodes(list);
            for (int i = 0; i < max - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    if (list.ElementAt(j - 1) > list.ElementAt(j))
                    {
                        //swap logic

                        LinkedListNode<double> current = list.Find(list.ElementAt(j));
                        var temp = list.ElementAt(j - 1);
                        current.Previous.Value = list.ElementAt(j);
                        current.Value = temp;
                    }
                }
            }
            return true; // return type boolean
        }

        private void btnInserationSort_Click(object sender, RoutedEventArgs e)
        {
            InsertionSort(list: sensorB);
            DisplayListBoxData(listBoxIdentifier: sensorB, listBox: listBoxB); // call Display List Box Data method
        }

        /*
         * 4.9
         * Create a method called “BinarySearchIterative” which has the following four parameters: LinkedList, SearchValue, Minimum and Maximum.
         * This method will return an integer of the linkedlist element from a successful search or the nearest neighbour value.
         * The calling code argument is the linkedlist name, search value, minimum list size and the number of nodes in the list. 
         * The method code must follow the pseudo code supplied below in the Appendix. 
         */
        private static int BinarySearchIterative(LinkedList<double> searchList, int searchValue, int minimum, int maximum)
        {

            while (minimum <= maximum - 1) {
                int middle = (minimum + maximum) / 2;

                if (searchValue == searchList.ElementAt(middle))
                {
                    return ++middle;
                }
                else if (searchValue < searchList.ElementAt(middle)) {
                    maximum = middle - 1;
                }
                else
                {
                    minimum = middle + 1;
                }
            }
            return minimum - 1;
        }

        /*
         * 4.10 Create a method called “BinarySearchRecursive” which has the following four parameters: LinkedList, SearchValue, Minimum and Maximum.
         * This method will return an integer of the linkedlist element from a successful search or the nearest neighbour value. The calling code argument is the linkedlist name, search value, minimum list size and the number of nodes in the list.
         * The method code must follow the pseudo code supplied below in the Appendix. 
         */

        private static int BinarySearchRecursive(LinkedList<double> searchList, int searchValue, int minimum, int maximum)
        {
            if (minimum <= maximum - 1)
            {
                int middle = (minimum + maximum) / 2;

                if (searchValue == searchList.ElementAt(middle))
                {
                    return minimum;
                } else if (searchValue < searchList.ElementAt(middle)) {
                    return BinarySearchRecursive(searchList, searchValue, minimum, middle - 1);
                } else
                {
                    return BinarySearchRecursive(searchList, searchValue, middle + 1, maximum);
                }
            }
            return minimum;
        }
        #endregion

        #region UI Buttons

        private void btnBSIA_Click(object sender, RoutedEventArgs e)
        {
            SelectionSort(sensorA);
            int searchDetail = int.Parse(searchValA.Text);
            int highest = sensorA.Count;
            int lowest = 0;
            // start the stopwatch before calling the search method
            Stopwatch stopwatch = new Stopwatch(); // create stopwatch object
            stopwatch.Start(); // start stopwatch
            BinarySearchIterative(searchList: sensorA, searchValue: searchDetail, minimum: lowest, maximum: highest);

            stopwatch.Stop(); // stop stopwatch
            TimeSpan timeTaken = stopwatch.Elapsed;
            txtBoxBSIA.Text = $"{timeTaken.Milliseconds} MS";

            DisplayListBoxData(listBoxIdentifier: sensorA, listBox: listBoxA);
            listBoxA.SelectedItem = searchValA.Text; 

        }
        private void btnBSIB_Click(object sender, RoutedEventArgs e)
        {
            SelectionSort(sensorB);
            int searchDetail = int.Parse(searchValB.Text);
            int highest = sensorB.Count;
            int lowest = 0;
            // start the stopwatch before calling the search method
            Stopwatch stopwatch = new Stopwatch(); // create stopwatch object
            stopwatch.Start(); // start stopwatch
            BinarySearchIterative(searchList: sensorB, searchValue: searchDetail, minimum: lowest, maximum: highest);

            stopwatch.Stop(); // stop stopwatch
            TimeSpan timeTaken = stopwatch.Elapsed;
            txtBoxBSIB.Text = $"{timeTaken.Milliseconds} MS";

            DisplayListBoxData(listBoxIdentifier: sensorB, listBox: listBoxB);
            listBoxB.SelectedItems.ToString();

        }

        private void btnBSRA_Click(object sender, RoutedEventArgs e)
        {
            SelectionSort(sensorA);
            int searchDetail = int.Parse(searchValA.Text);
            int highest = sensorA.Count;
            int lowest = 0;
            // start the stopwatch before calling the search method
            Stopwatch stopwatch = new Stopwatch(); // create stopwatch object
            stopwatch.Start(); // start stopwatch
            BinarySearchRecursive(searchList: sensorA, searchValue: searchDetail, minimum: lowest, maximum: highest);

            stopwatch.Stop(); // stop stopwatch
            TimeSpan timeTaken = stopwatch.Elapsed;
            txtBoxBSRA.Text = $"{timeTaken.Milliseconds} MS";

            DisplayListBoxData(listBoxIdentifier: sensorA, listBox: listBoxA);
            listBoxA.SelectedIndex.ToString();
        }

        private void btnBSRB_Click(object sender, RoutedEventArgs e)
        {
            SelectionSort(sensorB);
            int searchDetail = int.Parse(searchValB.Text);
            int highest = sensorB.Count;
            int lowest = 0;
            // start the stopwatch before calling the search method
            Stopwatch stopwatch = new Stopwatch(); // create stopwatch object
            stopwatch.Start(); // start stopwatch
            BinarySearchRecursive(searchList: sensorB, searchValue: searchDetail, minimum: lowest, maximum: highest);

            stopwatch.Stop(); // stop stopwatch
            TimeSpan timeTaken = stopwatch.Elapsed;
            txtBoxBSRB.Text = $"{timeTaken.Milliseconds} MS";

            DisplayListBoxData(listBoxIdentifier: sensorB, listBox: listBoxB);
            listBoxB.SelectedItems.ToString();
        }

        private void btnSelectionSortA_Click(object sender, RoutedEventArgs e)
        {
            SelectionSort(list: sensorA);
            DisplayListBoxData(listBoxIdentifier: sensorA, listBox: listBoxA); // call Display List Box Data method
        }

        private void btnInserationSortA_Click(object sender, RoutedEventArgs e)
        {
            InsertionSort(list: sensorA);
            DisplayListBoxData(listBoxIdentifier: sensorA, listBox: listBoxA); // call Display List Box Data method
        }
        #endregion

        #region 4.14
        /*
         * Source for finding this code:
         * https://stackoverflow.com/questions/1268552/how-do-i-get-a-textbox-to-only-accept-numeric-input-in-wpf
         */
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion

        

        }
    }

