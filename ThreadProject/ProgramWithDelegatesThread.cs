using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

//  Приклад асинхронної роботи делегата
//namespace ThreadProject
//{
//    class Program
//    {
//        public delegate int BinaryOp(int x, int y);
//        public static bool isDone { get; set; } = false;
//        static void Main(string[] args)
//        {
//            Console.OutputEncoding = Encoding.Default;

//            Console.WriteLine("***** Async Delegate Invocation *****");
//            // Виводиться ідентифікатор головного потоку
//            Console.WriteLine("Main() invoked on thread {0}.",
//            Thread.CurrentThread.ManagedThreadId);
//            // Створення обєкта делегата і присвоєння йому метода Add
//            BinaryOp b = new BinaryOp(Add);
//            //  Викликання метода Add асинхронно тобто в потоці 
//            //  (перші параметри - це параметри, які має приймати обєкт делегата)
//            //  Два останніх параметра - це AsyncCallBack i Object
//            //  AsyncCallback - делегат, який приймає тип IAsyncResult і нічого не повертає
//            //  Він викликається при закінченні роботи делегата
//            IAsyncResult ar = b.BeginInvoke(10, 10, new AsyncCallback(AddComplete), null);
//            // Виведення для того щоб протестувати, що код делегата не перериватиме головний потік
//            // Цикл відпрацьовуватиме доти, доки метод Add не завершить роботу і не поверне результата
//            //while (!ar.AsyncWaitHandle.WaitOne(1000, true))
//            //{
//            //    Console.WriteLine("Doing more work in Main()!");
//            //    Thread.Sleep(1000);
//            //}


//            // Якщо метод Add виконав роботу то отримати результат
//            // Метод EndInvoke блокує основний потік, поки не буде результата роботи метода
//            //int answer = b.EndInvoke(ar);
//            //  Виведення результату
//            // Console.WriteLine("10 + 10 is {0}.", answer);


//            while (!isDone) 
//            {
//                Console.WriteLine("Process...");
//                Thread.Sleep(1000);
//            }
//            Console.ReadLine();
//        }
//        public static int Add(int x, int y)
//        {
//            // Виводиться ідентифікатор потоку
//            Console.WriteLine("Add () invoked on thread {0}.",
//            Thread.CurrentThread.ManagedThreadId);
//            // Пауза
//            Thread.Sleep(5000);
//            return x + y;
//        }
//        /// <summary>
//        ///     Метод, який викликається, коли делегат закінчує роботу.
//        ///     Викликається лише в вторичному потоці. Це можна побачити запустивши програму
//        /// </summary>
//        /// <param name="res">Приймає результат роботи делегата</param>
//        public static void AddComplete(IAsyncResult res) 
//        {
//            //  Виводить потік, у якому працює метод, який викликається, коли делегат завершив роботу
//            Console.WriteLine("Потік в якому запускаєтсься метод AddComplete: {0}", 
//                Thread.CurrentThread.ManagedThreadId);
//            Console.WriteLine("Operation is completed!");
//            //  Інтерфейс IAsyncResult представляє собою клас типу AsyncResult, який находиться
//            //  в просторі імен System.Runtime.Remote.Messaging. Цей клас має властивість AsyncDelegate,
//            //  яка повертає делегат, який запущений асинхронно
//            AsyncResult ar = (AsyncResult)res;
//            BinaryOp deleg = ar.AsyncDelegate as BinaryOp;
//            Console.WriteLine("Результат роботи метода: {0}", deleg.EndInvoke(res));
//            //  Присвоєння прапорцю isDone значення true, що означає що делегат закінчив роботу
//            isDone = true;
//        }
//    }
//}



//  Головне про асинхронну роботу делегатів
namespace ThreadProject 
{
    class Program 
    {
        public delegate int MyNewDelegate(int a1, int a2);
        static void Main(string [] args) 
        {
            Console.OutputEncoding = Encoding.Default;
            Console.WriteLine("Thread Main: {0}", 
                Thread.CurrentThread.ManagedThreadId);
            MyNewDelegate newDel = Sum;
            ///  Запускання делегата в потоці
            ///  Приймає параметри: 
            ///  1) Параметри які приймає сама структура делегата
            ///  2) Параметр типу AsyncCallback - це делегат, який викликається коли метод в потоці завершив роботу
            ///  3) Параметр типу object, може приймати любі дані
            IAsyncResult res = newDel.BeginInvoke(10, 10, new AsyncCallback((IAsyncResult r)=> {
                //  Кол-бек делегат запускається в окремому потоці
                Console.WriteLine("Thread id callbackDelegate: {0}", Thread.CurrentThread.ManagedThreadId);
                AsyncResult ar = (AsyncResult)r;
                MyNewDelegate del = ar.AsyncDelegate as MyNewDelegate;
                MessageBox.Show($"Метод Sum, запущений в потоці завершив виконання Результат: {del.EndInvoke(r)}", 
                    "Програма");
            }), null);

            //  Очікування результата роботи метода в потоці властивістю IsCompleted, властивість постійно
            //  запитує в метода чи він завершив виуонання роботи
            //while (!res.IsCompleted) 
            //{
            //    Console.WriteLine("Чекайте... (IsCompleted)");
            //    Thread.Sleep(1000);
            //}


            //  Очікування завершення роботи метода властивістю AsyncWaitHandle, який повідомляє
            //  про закінчення роботи і методом WaitOne через певний проміжок часу перевіряє чи метод
            //  в потоці закінчив сіою роботу
            //while (!res.AsyncWaitHandle.WaitOne(1000, true)) 
            //{
            //    Console.WriteLine("Чекайте... (AsyncWaitHandle)");
            //    Thread.Sleep(1000);
            //}
            //int answer = newDel.EndInvoke(res);
            Console.ReadLine();
        }

        static int Sum(int a1, int a2) 
        {
            Console.WriteLine("Thread Sum: {0}", 
                Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("result is: {0}", a1 + a2);
            return a1 + a2;
        }
    }
}