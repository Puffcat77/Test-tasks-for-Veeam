using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace monitor
{

    //Написать на C# утилиту, которая мониторит процессы Windows и "убивает" те процессы, которые работают слишком долго.
    //- На входе три параметра: название процесса, допустимое время жизни(в минутах) и частота проверки(в минутах).
    //- Утилита запускается из командной строки. При старте она считывает три входных параметра и начинает мониторить процессы с указанной частотой. Если процесс живет слишком долго – завершает его и выдает сообщение в лог.
    //Пример запуска:
    //monitor.exe notepad 5 1
    //С такими параметрами утилита раз в минуту проверяет, не живет ли процесс notepad больше пяти минут, и "убивает" его, если живет.


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            if (!IsInputValid(args))
            {
                Console.WriteLine("Parameters should be: [proccess name] [number of minutes of life time] " +
                    "[number of minutes of check frequency]\n");
                return;
            }
            string procName = args[0];
            double lifeTime = double.Parse(args[1]);
            double checkTime = double.Parse(args[2]);
            MonitorProcess(procName, lifeTime, checkTime);
        }

        private static bool IsInputValid(string[] args)
        {
            if (args.Length != 3) { Console.WriteLine("There must be three parameters"); return false; }
            double frequency;
            double lifeTime;
            if (!double.TryParse(args[1], out lifeTime) || lifeTime < 0 || 
                !double.TryParse(args[2], out frequency) || frequency <= 0) 
            { 
                Console.WriteLine("Minutes must be an integer or a fractional positive number with a comma separator\n" +
                    "Life time should not be negative, while check frequency must be positive\n");
                return false; 
            }
            return true;
        }

        private static void MonitorProcess(string procName, double lifeTime, double checkTime)
        {
            Process[] allProcesses = Process.GetProcesses();
            List<Process> processes = allProcesses.Where((p) => p.ProcessName == procName).ToList();
            if (processes == null || processes.Count == 0)
            {
                Console.WriteLine("There is no processes with such name");
                return;
            }
            // Console.WriteLine("There are {0} processes running with such name", processes.Count);
            double checkAmount = lifeTime / checkTime;
            for (int i = 0; i < checkAmount && !AreAllExited(processes); i++)
            {
                processes = processes.Where(p => !p.HasExited).ToList();
                // Console.WriteLine("{0} processes are still active, {1:0.0} minutes of life time is left", 
                //    processes.Count, lifeTime - i * checkTime);
                Thread.Sleep(TimeSpan.FromMinutes(checkTime));
            }
            if (!AreAllExited(processes))
            {
                foreach (Process p in processes) p.Kill();
                Console.WriteLine("{0} processes has been terminated", procName);
            }
            //else
            //    Console.WriteLine("{0} processes has exited before terminating", procName);
        }

        private static bool AreAllExited(List<Process> processes) => processes.Select(p => p.HasExited).All(ex => ex);
    }
}
