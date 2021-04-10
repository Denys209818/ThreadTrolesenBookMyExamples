using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPoolProject
{
    class Program
    {
        //  ThreadPool - статичний клас, який дозволяє маніпулювати потоками з пула
        //  Щоб запросити потік із пула потрібно викликати метод QueueUserWorkItem(),
        //  який приймає першим параметром тип WaitCallback, а другий (по перегрузці) тип object
        //  Цей метод ставить виконання методу в чергу для одного з таких потоків
        static void Main(string[] args)
        {
            //  Створення делегата, який буде добавлятись у чергу пулів і буде виконуватися
            WaitCallback wait = new WaitCallback(ShowPrinting);
            Printer p = new Printer();
            //  Добавлення в чергу 10 делегатів, які будуть виконуватися
            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(wait, p);
            }
            //  Зупинка проєкту, оскільки усі ці потоки є фоновими і при завершені роботи програми вони
            //  удаляються
            Console.ReadLine();
        }

        static void ShowPrinting(object state) 
        {
            Printer p = state as Printer;
            p.Print();
        }
    }

    class Printer 
    {
        public void Print() 
        {
            Console.Write("Printing: ");
            for (int i = 0; i < 10; i++)
            {
                Console.Write("{0}, ", i.ToString());
            }

            Console.WriteLine();
        }
    }
}
