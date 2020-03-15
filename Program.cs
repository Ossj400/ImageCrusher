using System;
using System.Windows.Forms;
namespace ImageCrusher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }

    class Auto
    {
        public Opony rodzaj { get; set; }
    }

    class Opony
    {
       public int rozmiar = 6;
    }
}
