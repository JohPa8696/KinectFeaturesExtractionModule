using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FallDetectionSystemDataProcessor
{
    class FeatureExtractor11 : IRawDataExtractor
    {
        private string columns = "Head_Y_Vel,Class";
        public static int ID = 11;

        public string getColumns()
        {
            return this.columns;
        }

        public string[] process(List<double[]> data)
        {
            // Contains a list of strings
            List<String> extractedData = new List<String>();
            double[] previousRow = data[0];

            for (int i = 1; i < data.Count; i++)
            {
                // This list contains features values of type double for final traiing dataset
                ArrayList featureValues = new ArrayList();
                double[] currentRow = data[i];
                double timeDiff = (previousRow[64] - currentRow[64]);

                featureValues.Add(previousRow[1] - currentRow[1]); // HEAD Y Pre 
                featureValues.Add(currentRow[66]); // class label
                // convert the double array to string
                string s = "";
                foreach (double fl in featureValues)
                {
                    s += fl.ToString() + ",";
                }
                // remove the last comma
                s.Remove(s.Length - 1, 1);
                extractedData.Add(s);
            }

            // Depend on difference instances get the middle instances
            // 5, 10,15,20 and 25 - windows sizes 
            // taking the middle rows

            StringBuilder builder5 = new StringBuilder();
            StringBuilder builder10 = new StringBuilder();
            StringBuilder builder15 = new StringBuilder();
            StringBuilder builder20 = new StringBuilder();
            StringBuilder builder25 = new StringBuilder();
            for (int i = 2; i < extractedData.Count; i++)
            {
                if (i >= 2 && i <= 27)
                {
                    builder25.AppendLine(extractedData[i]);
                }
                if (i >= 4 && i <= 24)
                {
                    builder20.AppendLine(extractedData[i]);
                }
                if (i >= 6 && i <= 21)
                {
                    builder15.AppendLine(extractedData[i]);
                }
                if (i >= 9 && i <= 19)
                {
                    builder10.AppendLine(extractedData[i]);
                }
                if (i >= 12 && i <= 17)
                {
                    builder5.AppendLine(extractedData[i]);
                }
            }
            string[] res = { builder5.ToString(), builder10.ToString(), builder15.ToString(), builder20.ToString(), builder25.ToString() };
            return res;
        }

        public void makeModel()
        {

        }

        public int getID()
        {
            return 11;
        }

        public string getColumns(int windowSize)
        {
            return columns;
        }
    }
}
