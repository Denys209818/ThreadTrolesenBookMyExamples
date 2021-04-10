using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TimerProject
{
    class Program
    {
        static void Main(string[] args)
        {
            TimerCallback callback = new TimerCallback(PrintTime);
            //  Створення обєкту Timer, який приймає делегат
            //  метод якого буде виконуватись на протязі інтервалу часу
            //  другий параметр це дані, які передається делегату
            //  Третій параметр це період очікування перед першим запуском
            //  Четвертий параметр це інтервал
            Timer timer = new Timer(callback, "", 0, 1000);
            Console.WriteLine("Hit to terminal!");
            Console.ReadLine();
        }
        // сигнатура метода, яка підходить для делегата Callback класа Timer
        static void PrintTime(object state) 
        {
            Console.WriteLine("Час: {0}", DateTime.Now.ToLongTimeString());
            //  Використання даних, які передаються таймером
            Console.WriteLine(state.ToString());
        }
    }
}
