using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MyEBookReader
{
    class Program
    {
        //  Властивість, у яку зчитується книжка
        public static string theEBook { get; set; }
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Default;
            GetBook();
        }

        static void GetBook()
        {
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += (s, eArgs) =>
            {
                theEBook = eArgs.Result;
                Console.WriteLine("Завантежено!.");
                GetStats();
            };
            //  Загрузка тектсу з сайта, на якому розміщується твір
            wc.DownloadStringAsync(new Uri("http://www.gutenberg.org/files/65035/65035-0.txt"));

            Console.ReadLine();
        }

        static void GetStats()
        {
            // Масив символів, по яким розділятиметься текст
            string[] words = theEBook.Split(new char[] { '\u000A', '?', '/', ' ', '-', '.' },
             StringSplitOptions.RemoveEmptyEntries);
            // Метод, який знаходить 10 найвживаніших слів
            string[] tenMostCommon = FindTenMostCommon(words);
            // Метод, який отримує найдовше слово.
            string longestWord = FindLongestWord(words);
            // За допомогою класу StringBuilder будується фінальний
            // варіант слова
            StringBuilder bookStats = new StringBuilder("Найбільш вживані слова:\n");
            foreach (string s in tenMostCommon)
            {
                //  Додає нову строку
                bookStats.AppendLine(s);
            }
            //  Додає нову строку, яка відображає найдовше слово
            bookStats.AppendFormat("Longest word is: {0} ", longestWord); 
            bookStats.AppendLine();
            //  Виведення інформації по книжці
            Console.WriteLine(bookStats.ToString(), "Book info"); 
        }

        private static string[] FindTenMostCommon(string[] words)
        {
            // Linq-запит, який групує слова і сортує їх по кількості вживаності їх у тексті
            // Повертає ключ(ключ є власне словом)
            var frequencyOrder = from word in words
                                 where word.Length > 6
                                 group word by word into g
                                 orderby g.Count() descending
                                 select g.Key;
            //  Вибірка 10 перших елементів
            string[] commonWords = (frequencyOrder.Take(10)).ToArray();
            return commonWords;
        }
        private static string FindLongestWord(string[] words)
        {
            //  Linq-запит, який шукає найдовше слово (тобто впорядковує спершу по довжині і 
            //  повертає перше слово)
            return (from w in words
                    orderby w.Length descending
                    select w).FirstOrDefault();
        }
    }
}
