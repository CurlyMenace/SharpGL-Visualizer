using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLGraph
{
    public class FileProperties
    {
        public double Columns { get; set; }
        public double Rows { get; set; }
        public double Last { get; set; }
        public double Highest { get; set; }
        public List<double> Data {get; set;}

       public double getHighest(List<double> tempList)
        {
            tempList.Sort();
            return tempList.Last();
        }
    }
}
