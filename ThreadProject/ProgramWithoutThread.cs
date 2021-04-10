using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//  Перший приклад, а саме розбір даних про домен і контекст активного потока
//namespace ThreadProject
//{
//    class Program
//    {
//        // Делегат C#, обєкт якого може зсилатися на метод,
//        // який приймає 2 параметри (int) і повертає int-елемент
//        // При компіляції формується sealed-клас
//        public delegate int BinaryOp(int x, int y);
//        static void Main(string[] args)
//        {

//        }

//        // <summary>
//        ///     Метод, який повертає домен потоку який запущений
//        /// </summary>
//        /// <returns>Домен активного потоку</returns>
//        public static AppDomain GetDomainThread()
//        {
//            return Thread.GetDomain();
//        }

//        /// <summary>
//        ///     Метод, який повертає контекст, у якому працює текущий потік
//        /// </summary>
//        /// <returns>Контекст</returns>
//        public static Context ExtractCurrentThreadContext()
//        {
//            return Thread.CurrentContext;
//        }
//    }
//}


//  Другий приклад, а саме тестування синхронної роботи делегата, 
//  який перериває метод доти поки методи на які вказує делегат не завершать роботу
//namespace SyncDelegateReview
//{
//    public delegate int BinaryOp(int x, int y);
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            Console.WriteLine("*****Synch Delegate Review *****");
//            //  Виведення ідентифікатора потоку, у якому запускається програма
//            Console.WriteLine("Main() invoked on thread {0}.",
//            Thread.CurrentThread.ManagedThreadId);
//            // Створення обєкту типу делегата BinaryOp, який зсилається на метод Add
//            BinaryOp b = new BinaryOp(Add);
//            //  Викликання методів обєкта делегата і присвоєння результату
//            int answer = b(10, 10);
//            // Цей код не буде виконаним поки делегат не завершить роботу
//            Console.WriteLine("Doing more work m Main()'");
//            Console.WriteLine("10 + 10 is {0}.", answer);
//            Console.ReadLine();
//        }
//        static int Add(int x, int y)
//        {
//            // Вивоиться ідентифікатор потоку
//            Console.WriteLine("Add () invoked on thread {0}.",
//            Thread.CurrentThread.ManagedThreadId);
//            // Пауза
//            Thread.Sleep(5000);
//            return x + y;
//        }
//    }
//}
