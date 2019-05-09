using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renamer
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string htmlFile = "";
            string cssFile = "";
            Console.WriteLine("---Select HTML and CSS files---");
            Console.WriteLine("Press any button to select HTML");
            Console.ReadKey();
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine("HTML file pass: " + ofd.FileName);
                htmlFile = ofd.FileName;
            }
            Console.WriteLine("Select CSS");
            Console.ReadKey();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine("CSS file pass: " + ofd.FileName);
                cssFile = ofd.FileName;
            }
            Console.WriteLine("Your files are ready for penetration! Press Enter!");
            Console.ReadLine();
            Console.WriteLine("Write new names:");
            string name = Console.ReadLine();
            Console.WriteLine("New id's name is '" + name + "'");
            Console.WriteLine("Write 'start' to start!");
            if (Console.ReadLine() == "start")
            {
                RenamerMachine rm = new RenamerMachine(name, htmlFile, cssFile);
                rm.CheckDefaults();
                Console.ReadLine();
                rm.UpdateCSS();
                Console.ReadLine();
            }
        }
    }
}
