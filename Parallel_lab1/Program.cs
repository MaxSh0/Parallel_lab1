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
            var numThread = 5;
            

            //int[] arr = new int[100000000];
            int[] arr = new int[] { 1, 2, 0, 0, 0, 0, 0, 2, -3, 1, 4, 5, 6, 0, 0, 0, 5, 4,0,0 };
            int chunks_array = (int)Math.Ceiling((decimal)arr.Length / numThread);
            int sum = 0;
            int max_count_parallel = 0, max_count = 0, count = 0;

            //Random rnd = new Random();
            //for (int j = 0; j < 100000000; j++)
            //{
            //    arr[j] = rnd.Next();
            //}

          



            #region not_parallel
            for (int i = 0; i < arr.Length; i++)
            {
                sum += arr[i];
                if (sum == 0)
                {
                    count++;
                    if (count > max_count)
                    {
                        max_count = count;
                    }
                }
                else
                {
                    count = 0;
                    sum = 0;
                }
            }

            Console.WriteLine("Не параллельный результат - "+ max_count);

            #endregion




            DateTime dold = DateTime.Now;
            var locker = new Object();
            int index = 0;
            var result = Parallel.For(1, numThread, (i, state) =>
            {

                    Console.WriteLine("Поток " + i.ToString() + " запущен");
                    for (int j = 0; j < chunks_array; j++)
                    {
                        if(i * chunks_array < arr.Length) 
                        {
                            index = i * chunks_array-1;
                        }
                        else 
                        {
                        index = arr.Length - (arr.Length - i * chunks_array);
                        }
                    Console.WriteLine(index);
                        sum += arr[index];
                    lock(locker)
                    index++;

                        if (sum == 0)
                        {
                            count++;
                        
                            if (count > max_count_parallel)
                            {

                                max_count_parallel = count;
                            }
                        }
                        else
                        {
                            count = 0;
                            sum = 0;
                        }

                    
                }
            });
            Console.WriteLine( DateTime.Now - dold );

            Console.WriteLine("Параллельный результат - " + max_count_parallel);
            Console.ReadLine();








        }
    }
}
