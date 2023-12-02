using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace NumberJuggle
{
    public class Program
    {
        public static void Main() 
        {
            var program = new Program();

            program.Start();
        }

        public void Start()
        {
            var numberList = new NumberList();
            var sorter = new Sorter();

            Console.WriteLine("Welcome to NumberJuggle!\n\n");
            string strLength;
            int intLength;

            do
            {
                Console.WriteLine("Enter a length for the number list:");
                strLength = Console.ReadLine() ?? string.Empty; ;

            } while (!int.TryParse(strLength, out intLength));

            List<int> randomList = numberList.CreateList(intLength);

            Console.WriteLine($"Random list generated: [{string.Join(", ", randomList.Take(10))}, ..., {randomList.Last()}]");
            Console.WriteLine("Choose a sorting type from 1 to 5 (BubbleSort/SelectionSort/InsertionSort/QuickSort/MergeSort):\n");
            string choice = Console.ReadLine() ?? string.Empty; ;
            DateTime startTime = DateTime.Now;
            Console.WriteLine("\nSorting...");
            SortChoosed(randomList, choice);
            DateTime endTime = DateTime.Now;
            TimeSpan timeSpend = endTime - startTime;
            string sortType = "";
            switch (choice)
            {
                case "1":
                    sortType = "BubbleSort";
                    break;
                case "2":
                    sortType = "SelectionSort";
                    break;
                case "3":
                    sortType = "InsertionSort";
                    break;
                case "4":
                    sortType = "QuickSort";
                    break;
                case "5":
                    sortType = "MergeSort";
                    break;
                default:
                    choice = "0";
                    System.Threading.Thread.Sleep(1000);
                    Console.Clear();
                    Start();
                    break;
            }

            Console.WriteLine($"Random list sorted with {sortType}: [{string.Join(", ", randomList.Take(10))}, ..., {randomList.Last()}]");
            Console.WriteLine("Time span for sorting : " + timeSpend.TotalMilliseconds + " ms.");
            AllSortedList(randomList, sortType);
            Restart();
    }
        
        public void AllSortedList(List<int> randomList, string sT)
        {
            Console.WriteLine("\nDo you want to see the all sorted list? (Y/N)");
            string answer = Console.ReadLine() ?? string.Empty; ;
            string answerUp  = answer.ToUpper();
            if (answerUp == "Y")
            {
                ViewAllSortedList(randomList, sT);
            }
            else if (answerUp == "N")
            {
                Restart();
            }
        }

        public void ViewAllSortedList(List<int> randomList, string sT)
        {
            Console.WriteLine("\nRandom list sorted by {0}: : [" + string.Join(", ", randomList) + "]\n", sT);
            Restart();
        }

        public void Restart()
        {
            Console.WriteLine("\nDo you want to try with a new list? (Y/N)");
            string answer = Console.ReadLine()?.Trim().ToUpper() ?? string.Empty; 

            if (answer == "Y")
            {
                Console.Clear();
                Start();
            }
            else if (answer == "N")
            {
                Console.WriteLine("Press any key to quit.");
                Console.ReadKey();
            }
            else
            {
                Restart();
            }
        }

        static void SortChoosed(List<int> list, string ch)
        {   
            var sorter = new Sorter();
            switch (ch)
            {
                case "1":
                    sorter.BubbleSort(list);
                    break;
                case "2":
                    sorter.SelectionSort(list);
                    break;
                case "3":
                    sorter.InsertionSort(list);
                    break;
                case "4":
                    sorter.QuickSort(list, 0, list.Count - 1);
                    break;
                case "5":
                    sorter.MergeSort(list);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }


    }
    public class NumberList()
    {
        public List<int> CreateList(int l)
        {
            Random random = new Random();
            List<int> randomList = new List<int>();

            for (int i = 0; i < l; i++)
            {
                randomList.Add(random.Next(1, 100001));
            }

            return randomList;
        }
    }

    public class Sorter()
    {
        public void BubbleSort(List<int> list)
        {
            int length = list.Count;

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length - i - 1; j++)
                {
                    if (list[j] > list[j + 1])
                    {
                        int temp = list[j];
                        list[j] = list[j+1];
                        list[j+1] = temp;
                    }
                }
            }
        }

        public void SelectionSort(List<int> list)
        {
            int n = list.Count;

            for (int i = 0; i < n - 1; i++)
            {
                // Trouver l'index du plus petit élément dans la partie non triée
                int indexMin = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (list[j] < list[indexMin])
                    {
                        indexMin = j;
                    }
                }

                // Échanger l'élément actuel avec le plus petit élément trouvé
                int temp = list[i];
                list[i] = list[indexMin];
                list[indexMin] = temp;
            }
        }

        public void InsertionSort(List<int> list)
        {
            int n = list.Count;

            for (int i = 1; i < n; i++)
            {
                int currentValue = list[i];
                int j = i - 1;

                while (j >= 0 && list[j] > currentValue)
                {
                    list[j + 1] = list[j];
                    j--;
                }

                list[j + 1] = currentValue;
            }

        }

        public void QuickSort(List<int> list, int first, int last)
        {
            if (first < last)
            {
                int pivotIndex = Partitioning(list, first, last);

                // Récursivement trier les sous-listes gauche et droite
                QuickSort(list, first, pivotIndex - 1);
                QuickSort(list, pivotIndex + 1, last);
            }
        }

        static int Partitioning(List<int> list, int first, int last)
        {
            int pivotIndex = MedianOfThree(list, first, first + (last - first) / 2, last);
            int pivot = list[pivotIndex];
            int i = first - 1;

            // Échanger le pivot avec le dernier élément de la sous-liste
            (list[pivotIndex], list[last]) = (list[last], list[pivotIndex]);

            for (int j = first; j < last; j++)
            {
                if (list[j] < pivot)
                {
                    i++;
                    // Échanger liste[i] et liste[j]
                    (list[j], list[i]) = (list[i], list[j]);
                }
            }

            // Échanger liste[i+1] et le pivot
            (list[i + 1], list[last]) = (list[last], list[i + 1]);
            return i + 1;
        }

        static int MedianOfThree(List<int> list, int a, int b, int c)
        {
            // Retourner l'index du milieu des trois éléments
            if (list[a] < list[b])
            {
                if (list[b] < list[c])
                    return b;
                else if (list[a] < list[c])
                    return c;
                else
                    return a;
            }
            else
            {
                if (list[a] < list[c])
                    return a;
                else if (list[b] < list[c])
                    return c;
                else
                    return b;
            }
        }

        public void MergeSort(List<int> list)
        {
            MergeSort(list, 0, list.Count - 1);
        }

        private void MergeSort(List<int> list, int left, int right)
        {
            if (left < right)
            {
                // Trouver le milieu
                int middle = (left + right) / 2;

                // Trier la moitié gauche
                MergeSort(list, left, middle);

                // Trier la moitié droite
                MergeSort(list, middle + 1, right);

                // Fusionner les deux moitiés triées
                Merge(list, left, middle, right);
            }
        }

        private void Merge(List<int> list, int left, int middle, int right)
        {
            int n1 = middle - left + 1;
            int n2 = right - middle;

            // Créer des tableaux temporaires
            int[] leftArray = new int[n1];
            int[] rightArray = new int[n2];

            // Copier les données vers les tableaux temporaires
            for (int i = 0; i < n1; ++i)
                leftArray[i] = list[left + i];

            for (int j = 0; j < n2; ++j)
                rightArray[j] = list[middle + 1 + j];

            // Fusionner les tableaux temporaires

            // Indices initiaux des sous-tableaux
            int iLeft = 0, iRight = 0;

            // Indice initial du tableau fusionné
            int k = left;

            while (iLeft < n1 && iRight < n2)
            {
                if (leftArray[iLeft] <= rightArray[iRight])
                {
                    list[k] = leftArray[iLeft];
                    iLeft++;
                }
                else
                {
                    list[k] = rightArray[iRight];
                    iRight++;
                }
                k++;
            }

            // Copier les éléments restants de leftArray[], s'il y en a
            while (iLeft < n1)
            {
                list[k] = leftArray[iLeft];
                iLeft++;
                k++;
            }

            // Copier les éléments restants de rightArray[], s'il y en a
            while (iRight < n2)
            {
                list[k] = rightArray[iRight];
                iRight++;
                k++;
            }
        }
    }
}