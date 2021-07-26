using System;
using System.Diagnostics;

namespace monitor
{

    //Написать на C# утилиту, которая мониторит процессы Windows и "убивает" те процессы, которые работают слишком долго.
    //- На входе три параметра: название процесса, допустимое время жизни(в минутах) и частота проверки(в минутах).
    //- Утилита запускается из командной строки.При старте она считывает три входных параметра и начинает мониторить процессы с указанной частотой.Если процесс живет слишком долго – завершает его и выдает сообщение в лог.
    //Пример запуска:
    //monitor.exe notepad 5 1
    //С такими параметрами утилита раз в минуту проверяет, не живет ли процесс notepad больше пяти минут, и "убивает" его, если живет.


    class Program
    {
        static void Main(string[] args)
        {
            foreach (var arg in args)
            {
                Console.WriteLine(arg);
            }
            MonitorProcess();
            Console.ReadLine();
        }

        private static void MonitorProcess()
        {
            var allProcesses = Process.GetProcesses();
            foreach (var proc in allProcesses)
            {
                //Console.WriteLine("Process name: {0}, id: {1}", 
                //proc.ProcessName, proc.Id);
            }
        }
    }
}
