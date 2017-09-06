using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FallDetectionSystemDataProcessor
{
    class FeatureExtractor4 : IRawDataExtractor
    {

        private string columns5 = "Head_Vel_Y_1,HipCenter_Vel_Y_1,Head2FloorDist_1," +
                        "Head_Vel_Y_2,HipCenter_Vel_Y_2,Head2FloorDist_2," +
                        "Head_Vel_Y_3,HipCenter_Vel_Y_3,Head2FloorDist_3," +
                        "Head_Vel_Y_4,HipCenter_Vel_Y_4,Head2FloorDist_4," +
                        "Head_Vel_Y_5,HipCenter_Vel_Y_5,Head2FloorDist_5," +
                        "Head_Vel_Y_6,HipCenter_Vel_Y_6,Head2FloorDist_6,Class";
        private string columns10 = "Head_Vel_Y_1,HipCenter_Vel_Y_1,Head2FloorDist_1," +
                "Head_Vel_Y_2,HipCenter_Vel_Y_2,Head2FloorDist_2," +
                "Head_Vel_Y_3,HipCenter_Vel_Y_3,Head2FloorDist_3,Class";
        private string columns15 = "Head_Vel_Y_1,HipCenter_Vel_Y_1,Head2FloorDist_1," +
        "Head_Vel_Y_2,HipCenter_Vel_Y_2,Head2FloorDist_2,Class";
        private string columns20 = "Head_Vel_Y_1,HipCenter_Vel_Y_1,Head2FloorDist_1,Class";

        public string getColumns(int windowSize)
        {
            if (windowSize == 0)
            {
                return this.columns5;
            }
            else if (windowSize == 1)
            {
                return this.columns10;
            }
            else if (windowSize == 2)
            {
                return this.columns15;
            }
            else
            {
                return this.columns20;
            }
        }

        public string[] process(List<double[]> data)
        {
            // Contains a list of strings
            List<Double> extractedData = new List<Double>();

            string win5 = "";
            string win10 = "";
            string win15 = "";
            string win20 = "";

            // Window size 5 , 10, 15, 20, dont have enough for 25
            for (int step = 5; step <= 20; step += 5)
            {
                double[] previousRow = new double[67];
                for (int i = 0; i <= 30; i += step)
                {

                    if (i == 0)
                    {
                        previousRow = data[0];
                    }
                    else
                    {
                        // This list contains features values of type double for final traiing dataset
                        double[] currentRow = data[i - 1];
                        double timeDiff = (currentRow[64] - previousRow[64]);

                        double hcvel = (currentRow[28] - previousRow[28]) * 100 * 1000 / timeDiff; // hip center vel
                        // Calculate the head difference, 5 frame apart
                        double headToFloorDistance = 1.0;
                        if (!(currentRow[60] == 0 && currentRow[61] == 0 && currentRow[62] == 0 && currentRow[63] == 0))
                        {
                            //Calculate the distance between Head and Floor
                            headToFloorDistance = currentRow[0] * currentRow[60] + currentRow[1] * currentRow[61] + currentRow[2] * currentRow[62] + currentRow[63];
                            // Scale distance 
                            headToFloorDistance *= 100;
                        }


                        if (step == 5)
                        {
                            win5 += (currentRow[1] - previousRow[1]) * 100 * 1000 / timeDiff + ","; // Head Vel 
                            win5 += hcvel + ",";
                            win5 += headToFloorDistance + ",";
                        }
                        else if (step == 10)
                        {
                            win10 += (currentRow[1] - previousRow[1]) * 100 * 1000 / timeDiff + ","; // Head Vel 
                            win10 += hcvel + ",";
                            win10 += headToFloorDistance + ",";
                        }
                        else if (step == 15)
                        {
                            win15 += (currentRow[1] - previousRow[1]) * 100 * 1000 / timeDiff + ","; // Head Vel 
                            win15 += hcvel + ",";
                            win15 += headToFloorDistance + ",";
                        }
                        else if (step == 20)
                        {
                            win20 += (currentRow[1] - previousRow[1]) * 100 * 1000 / timeDiff + ","; // Head Vel 
                            win20 += hcvel + ",";
                            win20 += headToFloorDistance + ",";
                        }
                        previousRow = currentRow;
                    }
                }
            }



            win5 += data[0][66]; // Class value
            win10 += data[0][66]; // Class value
            win15 += data[0][66]; // Class value
            win20 += data[0][66]; // Class value
            StringBuilder builder5 = new StringBuilder();
            builder5.AppendLine(win5);
            StringBuilder builder10 = new StringBuilder();
            builder10.AppendLine(win10);
            StringBuilder builder15 = new StringBuilder();
            builder15.AppendLine(win15);
            StringBuilder builder20 = new StringBuilder();
            builder20.AppendLine(win20);


            string[] res = new string[5];
            res[0] = builder5.ToString();
            res[1] = builder10.ToString();
            res[2] = builder15.ToString();
            res[3] = builder20.ToString();
            res[4] = builder20.ToString();
            return res;
        }

        public int getID()
        {
            return 4;
        }
    }
}