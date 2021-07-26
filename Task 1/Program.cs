using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

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
            if (!IsInputValid(args))
            {
                Console.WriteLine("Parameters should be: [proccess name] [number of minutes of life time] [number of minutes of frequency check]");
                return;
            }
            var procName = args[0];
            var lifeTime = int.Parse(args[1]);
            var checkTime = int.Parse(args[2]);
            MonitorProcess(procName, lifeTime, checkTime);
        }

        private static bool IsInputValid(string[] args)
        {
            if (args.Length != 3) return false;
            if (!int.TryParse(args[1], out _) || !int.TryParse(args[2], out _)) return false;
            return true;
        }

        private static void MonitorProcess(string procName, int lifeTime, int checkTime)
        {
            var allProcesses = Process.GetProcesses();
            var proc = allProcesses.FirstOrDefault((p) => p.ProcessName == procName);
            if (proc == null)
            {
                Console.WriteLine("There is no process with such name");
                return;
            }
            var checkAmount = lifeTime / checkTime;
            for (int i = 0; i < checkAmount && !proc.HasExited; i++)
            {
                Console.WriteLine("{0} is still active", procName);
                Thread.Sleep(TimeSpan.FromMinutes(checkTime));
            }
            if (!proc.HasExited)
            {
                proc.Kill();
                Console.WriteLine("{0} has been terminated", procName);
            }
            else
                Console.WriteLine("{0} has exited before terminating", procName);
        }
    }
}
