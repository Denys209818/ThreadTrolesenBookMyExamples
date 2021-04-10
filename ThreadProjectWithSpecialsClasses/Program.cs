using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//  Робота з властивостями і методами потока
//namespace ThreadProjectWithSpecialsClasses
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            Console.WriteLine("***** Primary Thread stats *****\n");
//            // Отримання текущого потока для використання данних потока.
//            Thread primaryThread = Thread.CurrentThread;
//            //  Встановлення назви для потока
//            primaryThread.Name = "ThePrimaryThread";
//            // Вивід назви домена, в якому запущено додаток
//            Console.WriteLine("Name of current AppDomain: {0}",
//            Thread.GetDomain().FriendlyName); 
//            //  Вивід ідентифікатора контекста
//            Console.WriteLine("ID of current Context: {0}",
//            Thread.CurrentContext.ContextID); 
//            //  Вивід назви потока
//            Console.WriteLine("Thread Name: {0}",
//            primaryThread.Name); 
//            //  Вивід перевірки чи поток запущений
//            Console.WriteLine("Has thread started?: {0}",
//            primaryThread.IsAlive);
//            //  Вивід пріоритета потоку (ThreadPriority-enum)
//            Console.WriteLine("Priority Level: {0}",
//            primaryThread.Priority);
//            //  Вивід стану потоку (активний, перерваний і т.д. ThreadState-enum)
//            Console.WriteLine("Thread State: {0}",
//            primaryThread.ThreadState);

//            Console.ReadLine();
//        }
//    }
//}


namespace ThreadProjectWithSpecialsClasses 
{
    class Program 
    {
        /// <summary>
        ///     Властивість типу AutoResetEvent зупиняє роботу потока
        ///     методом WaitOne(), який чекає поки вторичний поток не закінчить свою роботу
        ///     і не викличе метод Set() цього ж потоку
        /// </summary>
        public static AutoResetEvent resetEvent { get; set; } = new AutoResetEvent(false);
        static void Main(string[] args) 
        {
            Console.OutputEncoding = Encoding.Default;
            int currThreadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("Current thread ID: {0}", currThreadId);

            Thread.CurrentThread.Name = "Head Thread!";
            Console.WriteLine("Name of thread: {0}", Thread.CurrentThread.Name);

            //  Робота з типом ThreadStart, який являє собою делегат, 
            //  який не приймає параметрів і не повертає їх. В конструкторі приймає метод,
            //  який буде виконувати. Якщо його запустити методом Invoke(), то він виконуватиметься не 
            //  в окремому потоці

            //Console.WriteLine("Введіть кількість потоків 1 або 2:");
            //string count = Console.ReadLine();

            //Printer printer = new Printer();

            //switch (count) 
            //{
            //    case "1": 
            //        {
            //            printer.PrintText();
            //            break; 
            //        }
            //    case "2": 
            //        {
            //            ThreadStart thread = new ThreadStart(printer.PrintText);
            //            Thread backgroundThread = new Thread(thread);
            //            backgroundThread.Name = "backgroundThread";
            //            backgroundThread.Start();
            //            break; 
            //        }
            //}

            //Console.WriteLine("Робота програми після виконання методу!");

            //  Робота з типом ParametrizedThreadStart, який є делегатом, 
            //  що нічого не повертає, але приймає тип object

            ParameterizedThreadStart pThread = new ParameterizedThreadStart(Sum);
            Thread thread = new Thread(pThread);
            thread.Start(new AddParams(10, 10));
            //  Метод, який призупиняє роботу потоку і жде виклику метода Set(),
            //  який здебільшого викликається у вторичному потоці
            resetEvent.WaitOne();
            Console.WriteLine("Подальша робота метода!");
            Thread.Sleep(5000);
            //  Створення нового потоку і встановлення його як фоновим потоком
            //  Тобто при завершенні роботи програми усі потоки, які є фоновими автоматично знищуються
            //  Це може бути корисно, коли процес, який виконує потік при закриванні вікна уже є непотрібнии
            Thread backgroundThread = new Thread(pThread);
            backgroundThread.IsBackground = true;
            backgroundThread.Start(new AddParams(95, 3));

        }
        //  Для другого приклада (ParametrizedThreadStart)
        static void Sum(object obj) 
        {
            if (obj is AddParams) 
            {
                AddParams c = obj as AddParams;
                Console.WriteLine("Результат {0} + {1} = {2}", 
                    c.a, c.b, c.a + c.b);
            }

            resetEvent.Set();
        }
    }

    //  Для першого приклада (ThreadStart)
    class Printer 
    {
        public void PrintText() 
        {
            Console.Write("Printing: ");
            for (int i = 0; i < 10; i++) 
            {
                Console.Write("{0}, ", i.ToString());
                Thread.Sleep(1000);
            }
        }
    }
    //  Для другого приклада (ParametrizedThreadStart)
    class AddParams 
    {
        public int a { get; set; }
        public int b { get; set; }
        public AddParams(int a, int b)
        {
            this.a = a;
            this.b = b;
        }
    }
}