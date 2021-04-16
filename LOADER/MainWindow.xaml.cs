using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace LOADER
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //Путь к файлам плаксиса
        string PATH_FOR_LOAD = @"\\192.168.0.1\files\Прикладные программы\TestFolder\";
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Создаём массив из папок
            DirectoryInfo dir = new DirectoryInfo(PATH_FOR_LOAD);
            foreach (var item in dir.GetDirectories())
            {
                Combo.Items.Add(item.Name);
            }
            Combo.SelectedIndex = 0;
        }
        //Класс копирования папок
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }
            DirectoryInfo[] dirs = dir.GetDirectories();      
            Directory.CreateDirectory(destDirName);
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = System.IO.Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = System.IO.Path.Combine(destDirName, subdir.Name);

                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
        // тут спрашивается существования на раб столе папки ЛОАД и туда грузит все файлы
        private void start_loading_TextInput()
        {
            Dispatcher.Invoke(() => Status.Value = 5);
            string select_folder = PATH_FOR_LOAD + Dispatcher.Invoke(() => Combo.SelectedItem.ToString()) + @"\";
            Dispatcher.Invoke(() => Status.Value = 15);
            string select_ico_project = PATH_FOR_LOAD + Dispatcher.Invoke(() => Combo.SelectedItem.ToString().Remove(Combo.SelectedItem.ToString().Length - 3));
            Dispatcher.Invoke(() => Status.Value = 20);
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\LOAD"))
            {

                System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\LOAD");
            } 
            else
            {
                Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\LOAD", true);
                System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\LOAD");
            }
            var word = select_ico_project.Split('\\')[6];
            Console.WriteLine(word);
            Dispatcher.Invoke(() => Status.Value = 50);
            DirectoryCopy(select_folder, Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\LOAD\", true);
            try
            {
                File.Copy(select_ico_project, Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\LOAD\" + word);
            }
            catch
            {
                MessageBox.Show("я не нашёл отдельный файл запуска, но всё равно продолжу установку!");
            }
            Dispatcher.Invoke(() => Status.Value = 100);
            Thread.Sleep(1000);
        }
        //Мултипоточность, чтоб работал нормально интерфейс
        private void start_loading_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Thread myThread = new Thread(new ThreadStart(strah));
            Thread myThread2 = new Thread(new ThreadStart(krah));
            myThread2.Start();
            myThread.Start();
        }
        //поток 1
        public void strah()
        {
            Dispatcher.Invoke(() => STOP_LAUNCHER.Visibility = Visibility.Visible);
        }
        //поток 2
        public void krah()
        {
            start_loading_TextInput();
            Dispatcher.Invoke(() => STOP_LAUNCHER.Visibility = Visibility.Hidden);
            Dispatcher.Invoke(() => Status.Value = 0);
        }
    }
}
