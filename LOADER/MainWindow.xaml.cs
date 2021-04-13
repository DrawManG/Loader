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
        string PATH_FOR_LOAD = @"\\192.168.0.1\files\Прикладные программы\TestFolder\";
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            DirectoryInfo dir = new DirectoryInfo(PATH_FOR_LOAD);
            foreach (var item in dir.GetDirectories())
            {
                Combo.Items.Add(item.Name);
            }
            Combo.SelectedIndex = 0;
            
        }

        private void start_loading_Click(object sender, RoutedEventArgs e)
        {

            start_loading_TextInput();
            Setka.Visibility = Visibility.Visible;
        }

        
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

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = System.IO.Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = System.IO.Path.Combine(destDirName, subdir.Name);

                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }

        private void start_loading_TextInput()
        {
            
            string select_folder = PATH_FOR_LOAD + Dispatcher.Invoke(() => Combo.SelectedItem.ToString()) + @"\";
            string select_ico_project = PATH_FOR_LOAD + Dispatcher.Invoke(() => Combo.SelectedItem.ToString().Remove(Combo.SelectedItem.ToString().Length - 3));
            
            //DirectoryCopy(select_folder, Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\go\", true);

            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\LOAD"))
            {

                System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\LOAD");
            }

            else
            {
                Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\LOAD", true);
                System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\LOAD");
            }


            DirectoryCopy(select_folder, Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\LOAD\", true);
        }

        private void start_loading_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Thread myThread = new Thread(new ThreadStart(strah));
            Thread myThread2 = new Thread(new ThreadStart(krah));
            myThread2.Start();
            myThread.Start();
            


        }

        public void strah()
        {
            Dispatcher.Invoke(() => Setka.Visibility = Visibility.Hidden);
        }

        public void krah()
        {
            start_loading_TextInput();
        }



    }
}
