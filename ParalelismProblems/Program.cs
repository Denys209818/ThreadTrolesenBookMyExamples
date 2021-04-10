using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParalelismProblems
{
    /// <summary>
    ///     Проблеми паралелізма - це коли декілька потоків
    ///     взаємодіють з одними данними,  а CLR розприділяє роботу потоків.
    ///     Може виникнути проблема, що один з потоків може працювати над даними і разом другий потік
    ///     який запускається пізніше використовуватиме дані які були змінені (частково) першим потоком
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Default;
            Console.WriteLine("Ідентифікатор текущого потока: {0}", 
                Thread.CurrentThread.ManagedThreadId);
            Thread.CurrentThread.Name = "ParallelismProblems";
            Console.WriteLine("Назва проєкту: {0}", 
                Thread.CurrentThread.Name);
            Printer p = new Printer();
            Thread[] threads = new Thread[10];
            for (int i = 0; i < 10; i++) 
            {
                threads[i] = new Thread(new ThreadStart(p.Print));
                threads[i].Start();
            }
        }
        /// <summary>
        ///     Атрибут, який робить безпесним кожний метод класа
        ///     для роботи в потоках. щоб дані одного обєкта змінвались атомарно
        /// </summary>
        [Synchronization]
        class Printer
        {
            public int element;
            private object obj = new object();
            public void Print() 
            {
                //  Ключове слово lock - маркер, який закриває цей метод для інших потоків, коли
                //  текущий поток виконує цей метод, коли поток завершив виконання роботи цей маркер
                //  передається іншому потоку
                lock (obj) 
                {
                    int x = 0;
                    Console.WriteLine("Thread ID: {0}", Thread.CurrentThread.ManagedThreadId);
                    Console.Write("Printing: ");
                    for (int i = 0; i < 10; i++) 
                    {
                        Console.Write("{0}, ", i.ToString());
                        Thread.Sleep(200);
                        //  Використання статичного класу Interlocked, який гарантує атомарну зміну данних
                        //  прикладом може бути метод Incremet(ref el)
                        int newVal = Interlocked.Increment(ref x);
                    }


                    Console.WriteLine();
                }
            }

            /// <summary>
            ///     Метод дозволяє атомарно змінити дані коли обєкт даного класа використовується в потоці
            ///     без використання ключового слова lock
            /// </summary>
            public void ExchangeWithoutLock()
            {
                Interlocked.Exchange(ref element, 100);
            }
            /// <summary>
            ///     Метод, який дозволяє атомарно перевірити і змінити дані,
            ///     коли обєкт даного класу опрацьовується в багатьох потоках
            /// </summary>
            public void CompareWithouLock() 
            {
                Interlocked.CompareExchange(ref element, 100, 101);
            }
        }
    }
}
