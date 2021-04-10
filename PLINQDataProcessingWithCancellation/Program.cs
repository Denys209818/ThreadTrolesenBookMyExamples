using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PLINQDataProcessingWithCancellation
{
    /// <summary>
    ///    Тестування PLINQ, а саме скасування LINQ-запита завдяки типу CancellationTokenSource
    /// </summary>
    class Program
    {
        //  Властивість, яка дозволяє скасовувати пулові потоки, якщо їх передавати в налаштування
        //  потока Parallel
        public static CancellationTokenSource tokenSource { get; set; } = new CancellationTokenSource();
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Default;
            //  Вічний цикл
            do
            {
                Console.WriteLine("Натисніть любу клавішу для початку роботи");
                // Потрібно нажати любу клавішу для початку обробки
                Console.ReadKey();
                Console.WriteLine("Процес...");
                Task.Factory.StartNew(() => ProcessIntData());
                Console.Write("Нажміть Q, щоб вийти: ");
                // Ввід тексту від користувача:
                string answer = Console.ReadLine();
                // Перевірка чи бажає користувач вийти
                if (answer.Equals("Q", StringComparison.OrdinalIgnoreCase))
                {
                    tokenSource.Cancel();
                    break;
                }
            } while (true);
            Console.ReadLine();

        }
        static void ProcessIntData()
        {
            // Отримання великого масиву чисел.
            int[] source = Enumerable.Range(1, 10_000_000).ToArray();
            // Пошук чисел, які діляться на 3
            int[] modThreelsZero = null;
            try
            {
                //  Запит, який робить виконання (якщо це необхідно) в іншому потоці
                //  Завдяки методу AsParallel, а метод WithCancellation дозволяє передати
                //  в парметрах налаштування
                modThreelsZero =
                (from num in source.AsParallel().WithCancellation(tokenSource.Token)
                 where num % 3 == 0
                 orderby num descending
                 select num).ToArray();
                Console.WriteLine();
                // Вивід кількості знайдених чисел
                Console.WriteLine($"Found {modThreelsZero.Count()} numbers that match query!");
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message); }
        }
    } 
}
