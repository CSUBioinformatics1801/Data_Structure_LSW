using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace exp4
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        public static readonly string[] SMtag = new string[] { "Direct insert sort", "Shell sort", "Bubble sort", "Quick sort", "Simple selection sort", "Heap sort", "Merge sort", "Binary insert sort" };
        public static readonly string[] SDtag = new string[] { "Binary", "Sequential" };
        [STAThread]

        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new main());
        }
    }
}
