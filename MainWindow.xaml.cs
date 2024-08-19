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
using System.Linq; // import linq for 4.11

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
                for (int i = 0; i < max - 1; i++)
                {
                    for (int j = i + 1; j > 0; j--)
                    {
                        if (list.ElementAt(j - 1) > list.ElementAt(j))
                        {
                            /* swap logic
                             * current.Previous.Value essentially means list.ElementAt(i) or list.ElementAt(j - 1) 
                             */
                            LinkedListNode<double> current = list.Find(list.ElementAt(j));
                            var temp = list.ElementAt(i);
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

            #region 4.11

            static bool IsOrdered<T>(LinkedList<T> listBox, IComparer<T>? comparer = default)
            {
                /*
                 * found this snippet of code from:
                 * https://code-maze.com/csharp-fastest-way-to-check-if-a-list-is-in-order/#:~:text=We%20iterate%20the%20ordered%20enumerable,the%20list%20is%20not%20ordered.
                 */

                // This method involves checking the linked list order using an Enumerator
                if (listBox.Count <= 1)
                    return true;

                comparer ??= Comparer<T>.Default;
                var previous = listBox.ElementAt(0); // define previous varaible to be the first element of linked list

                /* create enumerator via a call to GetEnumerator()
                 and since we want to start our comparison at the second element of the linkedlist, we perform an MoveNext() before we iterate our collection with the enumerator
                */
                using var enumerator = listBox.GetEnumerator();
                enumerator.MoveNext();
                while (enumerator.MoveNext())
                {
                    // move to the next element in the collection and store it as the current element
                    var current = enumerator.Current;
                    // compare current to the previous to make sre that they are in the proper order
                    if (comparer.Compare(previous, current) > 0)
                        return false;

                    // before beginning the next iteration, set previous to current
                    previous = current;
                }

                return true;
            }

            private void btnBSIA_Click(object sender, RoutedEventArgs e)
            {
                // check if an integer is typed in to the textbox or not
                int parsedValue;
                if (!int.TryParse(searchValA.Text, out parsedValue))
                {
                    MessageBox.Show("ERROR: Only enter numbers into this textbox!");
                    return;
                }

                // start the stopwatch before calling the IsOrdered method
                Stopwatch stopwatch = new Stopwatch(); // create stopwatch object
                stopwatch.Start(); // start stopwatch

                IsOrdered(sensorA); // call IsOrdered method with sensorA as the parameter

                int searchDetail = int.Parse(searchValA.Text);
                /* https://stackoverflow.com/questions/44055511/how-do-i-get-the-average-highest-and-lowest-of-values-from-a-listbox-and-then-d
                 using linq, i used the extension methods to give me the minimum and maximum values of the listbox after it has been populated
                */
                // this line will take all the items from sensorA's listbox and cast them to integers then convert them to doubles
                IEnumerable<int> listBoxDoubleItems = listBoxA.Items.Cast<string>().Select(item => Convert.ToInt32(item)); /* i previously had CS0121 compiler error so to fix this i used a lambda expression called "anonymous function" to specify the string to int conversion
                                                                                                          * https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions
                                                                                                          */
                // now that i have an IEnumerable<int> to work with, i have used the linq extension methods below
                int highest = listBoxDoubleItems.Max();
                int lowest = listBoxDoubleItems.Min();
                BinarySearchIterative(searchList: sensorA, searchValue: searchDetail, minimum: lowest, maximum: highest);

                stopwatch.Stop(); // stop stopwatch

                TimeSpan timeTaken = stopwatch.Elapsed;
                txtBoxBSIA.Text = $"{timeTaken.Milliseconds} MS";
                DisplayListBoxData(listBoxIdentifier: sensorA, listBox: listBoxA);
            }

            private void btnBSIB_Click(object sender, RoutedEventArgs e)
            {
                // check if an integer is typed in to the textbox or not
                int parsedValue;
                if (!int.TryParse(txtBoxBSIB.Text, out parsedValue))
                {
                    MessageBox.Show("ERROR: Only enter numbers into this textbox!");
                    return;
                }
                Stopwatch stopwatch = new Stopwatch(); // create stopwatch object
                stopwatch.Start(); // start stopwatch
                IsOrdered(sensorB); // call IsOrdered method with sensorB as the parameter

                int searchDetail = int.Parse(searchValB.Text);
                IEnumerable<int> listBoxDoubleItems = listBoxB.Items.Cast<string>().Select(item => Convert.ToInt32(item));

                int highest = listBoxDoubleItems.Max();
                int lowest = listBoxDoubleItems.Min();
                BinarySearchIterative(searchList: sensorB, searchValue: searchDetail, minimum: lowest, maximum: highest);

                stopwatch.Stop(); // stop stopwatch

                TimeSpan timeTaken = stopwatch.Elapsed;
                txtBoxBSIA.Text = $"{timeTaken.Milliseconds} MS";
                DisplayListBoxData(listBoxIdentifier: sensorA, listBox: listBoxA);
            }

            private void btnBSRA_Click(object sender, RoutedEventArgs e)
            {
                // check if an integer is typed in to the textbox or not
                int parsedValue;
                if (!int.TryParse(txtBoxBSRA.Text, out parsedValue))
                {
                    MessageBox.Show("ERROR: Only enter numbers into this textbox!");
                    return;
                }
                IsOrdered(sensorA); // call IsOrdered method with sensorA as the parameter
            }

            private void btnBSRB_Click(object sender, RoutedEventArgs e)
            {
                // check if an integer is typed in to the textbox or not
                int parsedValue;
                if (!int.TryParse(txtBoxBSRB.Text, out parsedValue))
                {
                    MessageBox.Show("ERROR: Only enter numbers into this textbox!");
                    return;
                }
                IsOrdered(sensorB); // call IsOrdered method with sensorB as the parameter
            }
            #endregion

            private void btnInserationSortA_Click(object sender, RoutedEventArgs e)
            {
                InsertionSort(list: sensorA);
                DisplayListBoxData(listBoxIdentifier: sensorA, listBox: listBoxA); // call Display List Box Data method
            }

            private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {

            }

        private void searchValA_TextChanged(object sender, TextChangedEventArgs e)
        {
            // 4.14 check if an integer is typed in to the textbox or not
            int parsedValue;
            if (!int.TryParse(searchValA.Text, out parsedValue))
            {
                MessageBox.Show("ERROR: Only enter numbers into this textbox!");
                return;
            }
        }

        private void searchValB_TextChanged(object sender, TextChangedEventArgs e)
        {
            // 4.14 check if an integer is typed in to the textbox or not
            int parsedValue;
            if (!int.TryParse(searchValA.Text, out parsedValue))
            {
                MessageBox.Show("ERROR: Only enter numbers into this textbox!");
                return;
            }
        }
    }
    }
