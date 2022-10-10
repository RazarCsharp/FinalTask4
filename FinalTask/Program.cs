using System.Runtime;
using System.Runtime.Serialization.Formatters.Binary;

namespace FinalTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SelectTask();
        }

        /// <summary>
        /// получает путь к файлу и проверяет его наличие
        /// </summary>
        /// <returns></returns>
        static string GetFilePath()
        {
            Console.WriteLine("Enter path to file\nExemple  C:/folderName/fileName");
            try
            {
                var path = new FileInfo(Console.ReadLine());
                if (path.Exists)
                {
                    return path.FullName;
                }
                else
                {
                    Console.WriteLine("\nNot correct try again");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return GetFilePath();
        }
        /// <summary>
        /// плучает путь к папке и проверяет его наличие
        /// </summary>
        /// <returns></returns>
        static string GetFolderPath()
        {
            Console.WriteLine("Enter path to file\nExemple  C:/folderName");
            try
            {
                var path = new DirectoryInfo(Console.ReadLine());
                if (path.Exists)
                {
                    return path.FullName;
                }
                else
                {
                    Console.WriteLine("\nNot correct try again");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return GetFilePath();
        }
        /// <summary>
        /// принимает путь к папке и возвращяет размер вложенных файлов
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static long GetSise(DirectoryInfo dir)
        {
            long folderSise = 0;
            try
            {
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    try
                    {
                        folderSise += file.Length;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                DirectoryInfo[] directories = dir.GetDirectories();
                foreach (DirectoryInfo directory in directories)
                {
                    try
                    {
                        folderSise += GetSise(directory);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return folderSise;
        }
        /// <summary>
        /// принимает путь к папке и удаляет вложенные файлы к кторым не обращялись более 30мин,возвращяет колличество удаленных файлов
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        static int DelFiles(DirectoryInfo directory)
        {
            int filesdeleted = 0;
            try
            {
                FileInfo[] files = directory.GetFiles();
                foreach (FileInfo file in files)
                {
                    if (file.Exists && DateTime.Now - file.LastAccessTime >= TimeSpan.FromMinutes(30))
                    {
                        try
                        {
                            file.Delete();
                            filesdeleted++;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }

                DirectoryInfo[] dirs = directory.GetDirectories();
                foreach (DirectoryInfo dir in dirs)
                {
                    if (dir.Exists)
                    {
                        try
                        {
                            filesdeleted += DelFiles(dir);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return filesdeleted;
        }
        /// <summary>
        /// принимает путь к папке и удаляет вложенные папки к кторым не обращялись более 30мин,возвращяет колличество удаленных папок
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        static int DelFolders(DirectoryInfo directory)
        {
            int foldersdeleted = 0;
            try
            {
                DirectoryInfo[] dirs = directory.GetDirectories();
                foreach (DirectoryInfo dir in dirs)
                {
                    if (dir.Exists && DateTime.Now - dir.LastAccessTime >= TimeSpan.FromMinutes(30))
                    {
                        try
                        {
                            dir.Delete(true);
                            foldersdeleted++;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            foldersdeleted += DelFolders(dir);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return foldersdeleted;
        }
        /// <summary>
        /// выполняет методы DelFiles и DelFolders
        /// </summary>
        static void Task1()
        {
            var dir = new DirectoryInfo(GetFolderPath());
            DelFiles(dir);
            DelFolders(dir);
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
        /// <summary>
        /// выполняет метод GetSise и выводит в консоль размер файлов
        /// </summary>
        static void Task2()
        {
            var dir = new DirectoryInfo(GetFolderPath());
            var sise = GetSise(dir);
            Console.WriteLine("\nDirectory sise : {0} byte", sise);
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
        /// <summary>
        /// выполняет методы GetSise, DelFiles, DelFolders и выводит в консоль сведения об освобожденном пространстве
        /// </summary>
        static void Task3()
        {
            var dir = new DirectoryInfo(GetFolderPath());
            var initialSise = GetSise(dir);
            var filesDeleted = DelFiles(dir);
            var foldersDeleted = DelFolders(dir);
            var currentSise = GetSise(dir);
            var freedSpace = initialSise - currentSise;
            Console.WriteLine("Исходный размер папки : {0} byte \nФайлов удалено : {1}\tПапок удалено : {3}", initialSise, filesDeleted, foldersDeleted);
            Console.WriteLine("Освобождено : {0} byte\tТекущий размер папки : {1} byte", freedSpace, currentSise);
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
        /// <summary>
        /// Принимает бинарную базу данных студентов, сортирует ее и сохраняет в новую папку на рабочем столе
        /// </summary>
        static void Task4()
        {
            string path = GetFilePath();
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            try
            {
                var studentsFolder = new DirectoryInfo(desktopPath + "/Students/");
                if (!studentsFolder.Exists)
                {
                    studentsFolder.Create();
                }
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                Student[] student = (Student[])formatter.Deserialize(fs);
                foreach (var s in student)
                {
                    string filePath = studentsFolder.FullName + s.Group + ".txt";
                    string write = s.Name + "  " + s.Date.ToString();
                    if (!File.Exists(filePath))
                    {
                        using (StreamWriter sw = File.CreateText(filePath)) ;
                    }
                    FileInfo fileInfo = new FileInfo(filePath);
                    using (StreamWriter sw = fileInfo.AppendText())
                    {
                        sw.WriteLine(write);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("\nTask complete!");
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
        /// <summary>
        /// предоставляет пользователю выбор задачи,описание выводится в консоль
        /// </summary>
        static void SelectTask()
        {
            var description1 = "Удаляет все файлы и папки к которым не обращялись в течение 30минут  по указанному пути";
            var description2 = "Выводит в консоль размер папки по указанному пути";
            var description3 = "Удаляет все файлы и папки к которым не обращялись в течение 30минут  по указанному пути\n\t\tи выводит в консоль данные об освобожденном пространстве";
            var description4 = "Принимает бинарную базу данных студентов, сортирует ее и сохраняет в новую папку на рабочем столе";

            Console.WriteLine("Выберете задачу\n1 - Задача #1 {0}\n2 - Задача #2 {1}\n3 - Задача #3 {2}\n4 - Задача #4 {3}", description1, description2, description3, description4);
            switch (Console.ReadLine())
            {
                case "1":
                    Task1();
                    SelectTask();
                    break;

                case "2":
                    Task2();
                    SelectTask();
                    break;

                case "3":
                    Task3();
                    SelectTask();
                    break;

                case "4":
                    Task4();
                    SelectTask();
                    break;

                default:
                    SelectTask();
                    break;
            }
        }
    }
}