using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Parallel_lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            int max_count_parallel = 0;
            var numThread = 4;
            var locker = new Object();

            Console.Write("Введите количество потоков: ");
            numThread = Convert.ToInt32(Console.ReadLine());

            int[] arr = new int[500000];

            //int chunks_array = (int)Math.Ceiling((decimal)arr.Length / numThread); 
            int chunks_array = arr.Length / numThread;
            int sum = 0;
            int max_count = 0;

            Random rnd = new Random();
            for (int j = 0; j < 500000; j++)
            {
                arr[j] = rnd.Next();
            }


            for (int h = 0; h < 4; h++) { 

            DateTime dold_not_par = DateTime.Now;
            #region not_parallel
            for (int i = 0; i < arr.Length; i++)
            {
                sum = arr[i];
                for(int j = i+1; j < arr.Length; j++)
                {
                    sum += arr[j];
                    if (sum == 0)
                    {
                        if (j - i + 1 > max_count)
                        {
                            max_count = j - i + 1;
                        }
                    }
                }
                sum = 0;
            }
            Console.WriteLine(DateTime.Now - dold_not_par);
            Console.WriteLine("Не параллельный результат - "+ max_count);

            #endregion




            DateTime dold = DateTime.Now;
            



            var result = Parallel.For(0, numThread, i =>
            {
                int local_max_count_parallel = 0;
                int local_sum = 0;
                int startIndex = getStartIndex(i);
                

                for (int k = 0; k < chunks_array; k++)
                {
                    local_sum = arr[startIndex+k];
                    for (int j = startIndex +k+ 1; j < arr.Length; j++)
                    {

                        local_sum += arr[j];
                        if (local_sum == 0)
                        {

                            if (j - (startIndex +k)+ 1 > local_max_count_parallel)
                            {
                                local_max_count_parallel = j - (startIndex+k) + 1;
                            }

                        }
                    }
                }
                lock (locker)
                {
                    if (local_max_count_parallel > max_count_parallel)
                    {
                        max_count_parallel = local_max_count_parallel;
                    }
                }

            });
            Console.WriteLine( DateTime.Now - dold );

            Console.WriteLine("Параллельный результат - " + max_count_parallel);
            Console.ReadLine();

            }



            int getStartIndex(int iTread)
            {
                int index = 0;
                if (iTread < chunks_array)
                {
                    index = iTread * chunks_array;
                }
                else
                {
                    index = arr.Length - (arr.Length - iTread * chunks_array) - 1;
                }
                return index;
            }



        }
    }
}
