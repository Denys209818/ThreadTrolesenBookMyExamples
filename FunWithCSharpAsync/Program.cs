using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FunWithCSharpAsync
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Fun with Async ---->");
            List<int> i = default;
            Console.WriteLine(DoWork());
            string message = await AsyncDoWork();
            Console.WriteLine(message);
            await MethodReturningVoidValue();
            await MultiAwaits();
            Console.WriteLine("Completed!");


            //  Контрукція кода використовується коли не використовується ключове слово await,
            //  але потрібно почекати поки завершиться метод
            //  --------------------------------------------------
            //  Очікується поки метод не поверне результат (призупиняє роботу метода)
            Console.WriteLine(AsyncDoWork().Result);
            //  Якщо метод нічого не повертає, то можна виклкати метод класа Task, 
            //  який очікує завершення метода
            MethodReturningVoidValue().Wait();


            //  В C# 6.0 можна запихувати конструкцію await у блок try...catch
            try
            {
                //  Код....
            }
            catch 
            {
                await MethodReturningVoidValue();
            }

            Console.ReadLine();
        }

        static string DoWork() 
        {
            Thread.Sleep(5000);
            return "Done with work!";
        }
        static async Task<string> AsyncDoWork() 
        {
            return await Task.Run(() => {
                Thread.Sleep(5000);
                return "Done work!";
            });
        }

        /// <summary>
        ///     Метод асинхронний який нічого не повертає
        /// </summary>
        /// <returns>Нічого не повертає</returns>
        static async Task MethodReturningVoidValue() 
        {
            await Task.Run(() => { //   Виконання роботи
            });
            Console.WriteLine("Return void");
        }
        /// <summary>
        ///     Асинхронний метод дозволяє викликати в собі кілька конструкцій await
        /// </summary>
        /// <returns>Нічого не повертає</returns>
        static async Task MultiAwaits() 
        {
            Thread.Sleep(2000);
            await Task.Run(() => {
                Console.WriteLine("Done 1");
            });
            await Task.Run(() => {
                Console.WriteLine("Done 2");
            });
            await Task.Run(() => {
                Console.WriteLine("Done 3");
            });
        }

        /// <summary>
        ///     Метод повертає елемент напряму
        ///     тобто елемент не потребує подальшого діставання з heap
        /// </summary>
        /// <returns>Повертає інтове число</returns>
        static async ValueTask<int> GetInt() 
        {
            return await Task.Run(() => {
                return new Random().Next(0, 100);
            });
        }

        static async Task LocalFunc() 
        {
            await localmethod();
            async Task localmethod() 
            {
                await Task.Run(new Action(()=> {
                    Console.WriteLine("Локальний метод");
                }));
            }
        }
    }
}
