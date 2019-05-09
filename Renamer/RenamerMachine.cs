using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renamer
{
    public class RenamerMachine
    {
        public string name;
        public string htmlPass;
        public string cssPass;
        private int counter = 1;
        private List<string> CSSNewTagsCollection = new List<string>();
        private List<string> CSSOldTagsCollection = new List<string>();
        public RenamerMachine(string Name, string HTML, string CSS)
        {
            name = Name;
            htmlPass = HTML;
            cssPass = CSS;
        }

        public void CheckDefaults()
        {
            Console.WriteLine("Name - " + name);
            Console.WriteLine("HTML - " + htmlPass);
            Console.WriteLine("CSS - " + cssPass);
        }

        public void UpdateCSS()
        {
            try
            {
                File.Delete(@"NewCSS.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            CSSReader();
        }

        private void CSSReader()
        {
            using (StreamReader sr = new StreamReader(cssPass, Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains("#pu") || line.Contains("#u"))
                    {
                        int lastIndex = line.IndexOf("{");
                        int firstIndex = line.IndexOf("#");
                        string targetToReplace = line.Substring(firstIndex, lastIndex-firstIndex);
                        string finalLine = line.Remove(firstIndex, lastIndex-firstIndex).Insert(firstIndex, "#" + name + counter.ToString());
                        Console.WriteLine(finalLine);
                        Console.WriteLine(targetToReplace);
                        CSSWriter(finalLine + " /*OLD_" + targetToReplace + "*/");
                        CSSOldTagsCollection.Add(targetToReplace.Replace("#", string.Empty).Replace(" ", string.Empty));
                        CSSNewTagsCollection.Add(name + counter.ToString());
                        counter++;
                    }
                    else
                    {
                        Console.WriteLine(line);
                        CSSWriter(line);
                    }
                }
            }

            UpdateHTML();
            Console.WriteLine("Completed!");
        }

        private void CSSWriter(string inputLine)
        {
            using (StreamWriter sw = new StreamWriter(@"NewCSS.txt", true, Encoding.UTF8))
            {
                sw.WriteLine(inputLine);
            }
        }

        public void UpdateHTML()
        {
            try
            {
                File.Delete(@"NewHTML.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            HTMLReader();
        }

        private void HTMLReader()
        {
            using (StreamReader sr = new StreamReader(htmlPass, Encoding.UTF8))
            {
                string line;
                int firstIndexOfID = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    foreach (string i in CSSOldTagsCollection)
                    {
                        if (line.Contains(i) && !line.Contains("img"))
                        {
                            firstIndexOfID = line.IndexOf("id=");
                            string sub = line.Substring(firstIndexOfID + 4, i.Length + 1);
                            if (sub.Contains("\""))
                            {
                                line = line.Replace(sub.Replace("\"", string.Empty), CSSNewTagsCollection[CSSOldTagsCollection.IndexOf(i)]);
                                break;
                            }
                        }
                    }

                    if (line.Contains("img"))
                        HTMLWriter(line + "<!-- " + "img" + " -->");
                    else
                        HTMLWriter(line);
                }
            }
        }

        private void HTMLWriter(string inputLine)
        {
            using (StreamWriter sw = new StreamWriter(@"NewHTML.txt", true, Encoding.UTF8))
            {
                sw.WriteLine(inputLine);
            }
        }
    }
}
