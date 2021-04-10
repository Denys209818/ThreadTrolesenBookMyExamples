using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Threading;
using System.IO;
using Path = System.IO.Path;

namespace DataParallelismWithForEach
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource cancelationTokenSource { get; set; } = new CancellationTokenSource();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            cancelationTokenSource.Cancel();
        }
        private void cmdProcess_Click(object sender, EventArgs e)
        {
            //  Запускає метод в новому потоці завдяки класу Tast
            //  і його властивості Factory, який містить метод StartNew(),
            //  який викликає метод в новому потоці, приймаючи в себе делегат типу Action<>
            Task.Factory.StartNew(() => ProcessFiles());
        }
        private void ProcessFiles()
        {
            ParallelOptions parOpt = new ParallelOptions();
            parOpt.CancellationToken = cancelationTokenSource.Token;
            parOpt.MaxDegreeOfParallelism = System.Environment.ProcessorCount;
            // Загружає усі назви фотографій з папки
            string[] files = Directory.GetFiles(@"C:\Users\Denis\Desktop\Зображення", "*.jpg",
            SearchOption.AllDirectories);
            //  Створення нової папки яка розміщена в проєкті
            string newDir = "ModifledPictures";
            Directory.CreateDirectory(newDir);
            // Обробка зображень і добавлення їх у нову папку
            try
            {
                //  Запущення циклу, який запускається в потоці
                Parallel.ForEach(files,parOpt, currentFile => {
                    string filename = System.IO.Path.GetFileName(currentFile);
                    parOpt.CancellationToken.ThrowIfCancellationRequested();
                    using (Bitmap bitmap = new Bitmap(currentFile))
                    {
                        //  Перевертання зображення, яке передається параметром до типа Bitmap
                        bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        //  Збереження нового зображення у нову папку
                        bitmap.Save(Path.Combine(newDir, filename));
                        // Виводиться ідентифікатор потока який виконує цю операцію
                        Application.Current.Dispatcher.Invoke(new Action(() => {
                            this.Title = $"Processing { filename} on thread { Thread.CurrentThread.ManagedThreadId}";
                        }));

                    }
                });
            }
            catch (Exception ex) 
            {
                this.Title = ex.Message;
            }
        }
    }
}
