using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FallDetectionSystemDataProcessor
{
    class FeatureExtractor6 : IRawDataExtractor
    {
        private string columns5 = "HeadDist_1,Head_Vel_1,HipCenterDist_1,HipCenter_Vel_1,SpineDist_1,Spine_Vel_1,HeadToFloorDist_1," +
                          "HeadDist_2,Head_Vel_2,HipCenterDist_2,HipCenter_Vel_2,SpineDist_2,Spine_Vel_2,HeadToFloorDist_2," +
                          "HeadDist_3,Head_Vel_3,HipCenterDist_3,HipCenter_Vel_3,SpineDist_3,Spine_Vel_3,HeadToFloorDist_3," +
                          "HeadDist_4,Head_Vel_4,HipCenterDist_4,HipCenter_Vel_4,SpineDist_4,Spine_Vel_4,HeadToFloorDist_4," +
                          "HeadDist_5,Head_Vel_5,HipCenterDist_5,HipCenter_Vel_5,SpineDist_5,Spine_Vel_5,HeadToFloorDist_5," +
                          "HeadDist_6,Head_Vel_6,HipCenterDist_6,HipCenter_Vel_6,SpineDist_6,Spine_Vel_6,HeadToFloorDist_6,Class";
        private string columns10 = "HeadDist_1,Head_Vel_1,HipCenterDist_1,HipCenter_Vel_1,SpineDist_1,Spine_Vel_1,HeadToFloorDist_1," +
                          "HeadDist_2,Head_Vel_2,HipCenterDist_2,HipCenter_Vel_2,SpineDist_2,Spine_Vel_2,HeadToFloorDist_2," +
                          "HeadDist_3,Head_Vel_3,HipCenterDist_3,HipCenter_Vel_3,SpineDist_3,Spine_Vel_3,HeadToFloorDist_3,Class";
        private string columns15 = "HeadDist_1,Head_Vel_1,HipCenterDist_1,HipCenter_Vel_1,SpineDist_1,Spine_Vel_1,HeadToFloorDist_1," +
                          "HeadDist_2,Head_Vel_2,HipCenterDist_2,HipCenter_Vel_2,SpineDist_2,Spine_Vel_2,HeadToFloorDist_2,Class";
        private string columns20 = "HeadDist_1,Head_Vel_1,HipCenterDist_1,HipCenter_Vel_1,SpineDist_1,Spine_Vel_1,HeadToFloorDist_1,Class";
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

        public int getID()
        {
           return 6;
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
                        // Calculate the head difference, 5 frame apart
                        double headToFloorDistance = 1.0;
                        if (!(currentRow[60] == 0 && currentRow[61] == 0 && currentRow[62] == 0 && currentRow[63] == 0))
                        {
                            //Calculate the distance between Head and Floor
                            headToFloorDistance = currentRow[0] * currentRow[60] + currentRow[1] * currentRow[61] + currentRow[2] * currentRow[62] + currentRow[63];
                            // Scale distance 
                            headToFloorDistance *= 100;
                        }

                        // 3D distances
                        double headDistance = Math.Sqrt(Math.Pow(currentRow[0] - previousRow[0], 2) + Math.Pow(currentRow[1] - previousRow[1], 2) + Math.Pow(currentRow[2] - previousRow[2], 2));
                        double hipcenterDistance = Math.Sqrt(Math.Pow(currentRow[27] - previousRow[27], 2) + Math.Pow(currentRow[28] - previousRow[28], 2) + Math.Pow(currentRow[29] - previousRow[29], 2));
                        double spineDistance = Math.Sqrt(Math.Pow(currentRow[51] - previousRow[51], 2) + Math.Pow(currentRow[52] - previousRow[52], 2) + Math.Pow(currentRow[53] - previousRow[53], 2));
                        if (step == 5)
                        {
                            win5 += headDistance *100 + ","; // Head DIST 
                            win5 += headDistance *100 *1000/timeDiff + ","; // head vel
                            win5 += hipcenterDistance + ","; // hipcenter distance 
                            win5 += hipcenterDistance *100000/ timeDiff + ",";  // hc vel
                            win5 += spineDistance + ",";
                            win5 += spineDistance *100000/ timeDiff + ",";
                            win5 += headToFloorDistance + ",";
                        }
                        else if (step == 10)
                        {

                            win10 += headDistance * 100 + ","; // Head DIST 
                            win10 += headDistance * 100 * 1000 / timeDiff + ","; // head vel
                            win10 += hipcenterDistance + ","; // hipcenter distance 
                            win10 += hipcenterDistance * 100000 / timeDiff + ",";  // hc vel
                            win10 += spineDistance + ",";
                            win10 += spineDistance * 100000 / timeDiff + ",";
                            win10 += headToFloorDistance + ",";
                        }
                        else if (step == 15)
                        {

                            win15 += headDistance * 100 + ","; // Head DIST 
                            win15 += headDistance * 100 * 1000 / timeDiff + ","; // head vel
                            win15 += hipcenterDistance + ","; // hipcenter distance 
                            win15 += hipcenterDistance * 100000 / timeDiff + ",";  // hc vel
                            win15 += spineDistance + ",";
                            win15 += spineDistance * 100000 / timeDiff + ",";
                            win15 += headToFloorDistance + ",";
                        }
                        else if (step == 20)
                        {

                            win20 += headDistance * 100 + ","; // Head DIST 
                            win20 += headDistance * 100 * 1000 / timeDiff + ","; // head vel
                            win20 += hipcenterDistance + ","; // hipcenter distance 
                            win20 += hipcenterDistance * 100000 / timeDiff + ",";  // hc vel
                            win20 += spineDistance + ",";
                            win20 += spineDistance * 100000 / timeDiff + ",";
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
    }
}
