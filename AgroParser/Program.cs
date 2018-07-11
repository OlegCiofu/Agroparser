using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgroParser
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DateTime timeStamp = DateTime.Now;
            FileStream filestream = new FileStream($"{System.Windows.Forms.Application.StartupPath}\\log.txt", FileMode.OpenOrCreate); //C:\Agroparser\
            var streamwriter = new StreamWriter(filestream);
            streamwriter.AutoFlush = true;
            streamwriter.WriteLine($"{timeStamp.ToString()} Program has started");
            streamwriter.NewLine = $"\n{timeStamp.ToString()}  ";
            Console.SetOut(streamwriter);
            Console.SetError(streamwriter);
            Application.Run(new Visual());
        }
    }
}
