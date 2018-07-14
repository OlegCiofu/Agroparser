using System;
using System.IO;
using System.Windows.Forms;

namespace AgroParser
{
    class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DateTime timeStamp = DateTime.Now;
            FileStream filestream = new FileStream($"{Application.StartupPath}\\log.txt", FileMode.Create); //C:\Agroparser\
            var log = new StreamWriter(filestream);
            log.AutoFlush = true;
            log.WriteLine($"{timeStamp.ToString()} Program has started");
            log.NewLine = $"\n{timeStamp.ToString()}  ";
            Console.SetOut(log);
            Console.SetError(log);
            Application.Run(new Visual());
        }
    }
}
