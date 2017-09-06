using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FallDetectionSystemDataProcessor
{

    // Head velocity all in 1 feature vectors
    class FeatureExtractor3 : IRawDataExtractor
    {
        private string columns5 = "Head_Vel_Y_1,Head_Vel_Y_2,Head_Vel_Y_3,Head_Vel_Y_4,Head_Vel_Y_5,Class";
        private string columns10 = "Head_Vel_Y_1,Head_Vel_Y_2,Head_Vel_Y_3,Head_Vel_Y_4,Head_Vel_Y_5,"+
                                   "Head_Vel_Y_6,Head_Vel_Y_7,Head_Vel_Y_8,Head_Vel_Y_9,Head_Vel_Y_10,Class";
        private string columns15 = "Head_Vel_Y_1,Head_Vel_Y_2,Head_Vel_Y_3,Head_Vel_Y_4,Head_Vel_Y_5," +
                                   "Head_Vel_Y_6,Head_Vel_Y_7,Head_Vel_Y_8,Head_Vel_Y_9,Head_Vel_Y_10,"+
                                    "Head_Vel_Y_11,Head_Vel_Y_12,Head_Vel_Y_13,Head_Vel_Y_14,Head_Vel_Y_15,Class";
        private string columns20 = "Head_Vel_Y_1,Head_Vel_Y_2,Head_Vel_Y_3,Head_Vel_Y_4,Head_Vel_Y_5," +
                                   "Head_Vel_Y_6,Head_Vel_Y_7,Head_Vel_Y_8,Head_Vel_Y_9,Head_Vel_Y_10," +
                                   "Head_Vel_Y_11,Head_Vel_Y_12,Head_Vel_Y_13,Head_Vel_Y_14,Head_Vel_Y_15,"+
                                   "Head_Vel_Y_16,Head_Vel_Y_17,Head_Vel_Y_18,Head_Vel_Y_19,Head_Vel_Y_20,Class";
        private string columns25 = "Head_Vel_Y_1,Head_Vel_Y_2,Head_Vel_Y_3,Head_Vel_Y_4,Head_Vel_Y_5," +
                                   "Head_Vel_Y_6,Head_Vel_Y_7,Head_Vel_Y_8,Head_Vel_Y_9,Head_Vel_Y_10," +
                                   "Head_Vel_Y_11,Head_Vel_Y_12,Head_Vel_Y_13,Head_Vel_Y_14,Head_Vel_Y_15," +
                                   "Head_Vel_Y_16,Head_Vel_Y_17,Head_Vel_Y_18,Head_Vel_Y_19,Head_Vel_Y_20,"+
                                   "Head_Vel_Y_21,Head_Vel_Y_22,Head_Vel_Y_23,Head_Vel_Y_24,Head_Vel_Y_25,Class";
        public string[] process(List<double[]> data)
        {
            // Contains a list of strings
            List<Double> extractedData = new List<Double>();
            double[] previousRow = data[0];
            for (int i = 1; i < data.Count; i++)
            {
                // This list contains features values of type double for final traiing dataset
                double[] currentRow = data[i];
                
                double timeDiff = (currentRow[64] - previousRow[64]);
                
                extractedData.Add((currentRow[1] - previousRow[1]) *100 *1000/ timeDiff); // HEAD Vel 
                previousRow = currentRow;
            }


            // Depend on difference instances get the middle instances
            // 5, 10,15,20 and 25 - windows sizes 
            // taking the middle rows
            string s5 = "";
            string s10 = "";
            string s15 = "";
            string s20 = "";
            string s25 = "";

            for (int i = 2; i < extractedData.Count; i++)
            {
                if (i >= 2 && i < 27)
                {
                    s25 += extractedData[i] + ",";
                }
                if (i >= 4 && i < 24)
                {
                    s20 += extractedData[i] + ",";
                }
                if (i >= 6 && i < 21)
                {
                    s15 += extractedData[i] + ",";
                }
                if (i >= 9 && i < 19)
                {
                    s10 += extractedData[i] + ",";
                }
                if (i >= 12 && i < 17)
                {
                    s5 += extractedData[i] + ",";
                }
            }

            s25 += previousRow[66]; // Class value
            s20 += previousRow[66]; // Class value
            s15 += previousRow[66]; // Class value
            s10 += previousRow[66]; // Class value
            s5 += previousRow[66]; // Class value
            StringBuilder builder5 = new StringBuilder();
            builder5.AppendLine(s5);
            StringBuilder builder10 = new StringBuilder();
            builder10.AppendLine(s10);
            StringBuilder builder15 = new StringBuilder();
            builder15.AppendLine(s15);
            StringBuilder builder20 = new StringBuilder();
            builder20.AppendLine(s20);
            StringBuilder builder25 = new StringBuilder();
            builder25.AppendLine(s25);

            string[] res = new string[5];
            res[0] = builder5.ToString();
            res[1] = builder10.ToString();
            res[2] = builder15.ToString();
            res[3] = builder20.ToString();
            res[4] = builder25.ToString();
            return res;
        }

        public string getColumns(int windowSize)
        {
            if (windowSize == 0)
            {
                return this.columns5;
            }else if(windowSize == 1){
                return this.columns10;
            }
            else if (windowSize == 2)
            {
                return this.columns15;
            }
            else if (windowSize == 3)
            {
                return this.columns20;
            }else
            {
                return this.columns25;
            }
        }

        public int getID()
        {
            return 3;
        }
    }
}
