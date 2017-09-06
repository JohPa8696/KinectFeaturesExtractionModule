using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FallDetectionSystemDataProcessor
{
    interface IRawDataExtractor
    {

        string[] process(List<double[]> data);
        string getColumns(int windowSize);
    }
}
