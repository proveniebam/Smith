using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeSort
{
    class Program
    {
        
        static void Main(string[] args)
        {
            int[] numbersToSort = new int[] { 5, 1, 4, 2, 3 };

            int[] results = MainMergeSort(numbersToSort, 0, 4).Result;

            foreach (int x in results)
                Console.WriteLine("{0} ", x);

            Console.ReadLine();
        }

        public async static Task<int[]> MainMergeSort(int[] numbers, int left, int right)
        {
            //Console.WriteLine("Left:{0}, Right:{1}", left, right);

            int[] results = numbers;
            int midPointOfArray = (right + left) / 2;

            if (right > left)
            {

                //we can do something really clever here like create n tasks and split the array into smaller chunks
                //but for simplicity we'll simply split it into two arrays.
                

                Task<int[]> SortLeftTask = MainMergeSort(numbers, left, midPointOfArray);
                Task<int[]> SortRightTask = MainMergeSort(numbers, midPointOfArray + 1, right);

                Task<int[][]> Results = Task.WhenAll(SortLeftTask, SortRightTask);

                if (SortLeftTask.Status != TaskStatus.RanToCompletion || SortRightTask.Status != TaskStatus.RanToCompletion)
                    throw new Exception("Sort tasks failed.");

                DoMergeSort(numbers, 0, midPointOfArray, right);
            }
            return results;
        }

        private async static Task<int[]> DoMergeSort(int[] numbers, int left, int mid, int right) //potentially a long running task, so async.
        {
            //Sort code from: http://www.softwareandfinance.com/CSharp/MergeSort_Recursive.html
            int[] temp = new int[numbers.Count()];
            int i, left_end, num_elements, tmp_pos;

            left_end = (mid - 1);
            tmp_pos = left;
            num_elements = (right - left + 1);

            while ((left <= left_end) && (mid <= right))
            {
                if (numbers[left] <= numbers[mid])
                    temp[tmp_pos++] = numbers[left++];
                else
                    temp[tmp_pos++] = numbers[mid++];
            }

            while (left <= left_end)
                temp[tmp_pos++] = numbers[left++];

            while (mid <= right)
                temp[tmp_pos++] = numbers[mid++];

            for (i = 0; i < num_elements; i++)
            {
                numbers[right] = temp[right];
                right--;

            }

            return numbers;
        }
    }

    //BDD (hard to compile without specflow :-)
    public class MergeSortScenario
    {
        private int[] actualResult = new int[] {0};
        

        //[When(@"When The System Needs To Sort An Array")]
        public void WhenTheSystemNeedsToSortAnArray()
        {
            int[] numbersToSort = new int[] { 5, 1, 4, 2, 3 };

            int[] results = Program.MainMergeSort(numbersToSort, 0, 4).Result;

            foreach (int x in results)
                Console.WriteLine("{0} ", x);

            Console.ReadLine();

            actualResult = results;
        }
        //[Then(@"the results will be sorted")]
        public void ThenTheResultsWillBeSorted()
        {
            //Assert.AreEqual(actualResult[0] = 1); etc
        }
    }
}
