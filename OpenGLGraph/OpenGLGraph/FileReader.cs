using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenGLGraph
{
    public class FileReader
    {
        public List<double> Reader()
        {
            ///<summary>
            ///This method reads a .DAT file containing a lot of numeric values.
            ///Returns a list of those values.
            /// </summary>
            List<double> readList = new List<double>();

            OpenFileDialog readFile = new OpenFileDialog();
            readFile.Filter = "DAT(*.DAT)|*.dat";
            int index = 0;
            if (readFile.ShowDialog() == DialogResult.OK)
            {
                string fileName = readFile.FileName;
                string[] fileLines = File.ReadAllLines(fileName);


                bool isReadingData = false;

                foreach (string line in fileLines)
                {
                    if (line.Contains("[Columns]"))
                    {
                        char splitChar = ':';
                        string[] result = line.Split(splitChar);

                        readList.Add(double.Parse(result[1]));
                        continue;
                    }
                    if (line.Contains("[Rows]"))
                    {
                        char splitChar = ':';
                        string[] result = line.Split(splitChar);

                        readList.Add(double.Parse(result[1]));
                        continue;
                    }
                    if (line.Contains("[Data]"))
                    {
                        isReadingData = true;
                        continue;
                    }

                    if (isReadingData)
                    {
                        char splitChar = ';';
                        string[] result = line.Split(splitChar);
                        foreach (string val in result)
                        {
                            if(val != "")
                            {
                                double valToSave = double.Parse(val);
                                if(valToSave < 0)
                                {
                                    valToSave = 0;
                                }
                                readList.Add(valToSave);
                            }
                        }
                    }

                }
            }

            return readList;
        }
    }
}
