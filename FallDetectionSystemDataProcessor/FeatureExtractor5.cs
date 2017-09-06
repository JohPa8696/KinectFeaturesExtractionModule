using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FallDetectionSystemDataProcessor
{
    class FeatureExtractor5 : IRawDataExtractor
    {
        private string columns5 = "Head_Y_1, HeadDist_1,HipCenter_Y_1,HipCenterDist_1,Spine_Y_1,SpineDist_1,HeadToFloorDist_1," +
                                  "Head_Y_2, HeadDist_2,HipCenter_Y_2,HipCenterDist_2,Spine_Y_2,SpineDist_2,HeadToFloorDist_2," +
                                  "Head_Y_3, HeadDist_3,HipCenter_Y_3,HipCenterDist_3,Spine_Y_3,SpineDist_3,HeadToFloorDist_3," +
                                  "Head_Y_4, HeadDist_4,HipCenter_Y_4,HipCenterDist_4,Spine_Y_4,SpineDist_4,HeadToFloorDist_4," +
                                  "Head_Y_5, HeadDist_5,HipCenter_Y_5,HipCenterDist_5,Spine_Y_5,SpineDist_5,HeadToFloorDist_5," +
                                  "Head_Y_6, HeadDist_6,HipCenter_Y_6,HipCenterDist_6,Spine_Y_6,SpineDist_6,HeadToFloorDist_6,Class";
        private string columns10 = "Head_Y_1, HeadDist_1,HipCenter_Y_1,HipCenterDist_1,Spine_Y_1,SpineDist_1,HeadToFloorDist_1," +
                          "Head_Y_2, HeadDist_2,HipCenter_Y_2,HipCenterDist_2,Spine_Y_2,SpineDist_2,HeadToFloorDist_2," +
                          "Head_Y_3, HeadDist_3,HipCenter_Y_3,HipCenterDist_3,Spine_Y_3,SpineDist_3,HeadToFloorDist_3,Class";
        private string columns15 = "Head_Y_1, HeadDist_1,HipCenter_Y_1,HipCenterDist_1,Spine_Y_1,SpineDist_1,HeadToFloorDist_1," +
                          "Head_Y_2, HeadDist_2,HipCenter_Y_2,HipCenterDist_2,Spine_Y_2,SpineDist_2,HeadToFloorDist_2,Class";
        private string columns20 = "Head_Y_1, HeadDist_1,HipCenter_Y_1,HipCenterDist_1,Spine_Y_1,SpineDist_1,HeadToFloorDist_1,Class";

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
            return 5;
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
                            win5 += currentRow[1] + ","; // head y
                            win5 += headDistance + ","; // Head Vel 
                            win5 += currentRow[28] + ","; // hipcenter y
                            win5 += hipcenterDistance + ","; // hipcenter distance 
                            win5 += currentRow[52] + ",";  // spine y
                            win5 += spineDistance + ",";
                            win5 += headToFloorDistance + ",";
                        }
                        else if (step == 10)
                        {

                            win10 += currentRow[1] + ","; // head y
                            win10 += headDistance + ","; // Head Vel 
                            win10 += currentRow[28] + ","; // hipcenter y
                            win10 += hipcenterDistance + ","; // hipcenter distance 
                            win10 += currentRow[52] + ",";  // spine y
                            win10 += spineDistance + ",";
                            win10 += headToFloorDistance + ",";
                        }
                        else if (step == 15)
                        {

                            win15 += currentRow[1] + ","; // head y
                            win15 += headDistance + ","; // Head Vel 
                            win15 += currentRow[28] + ","; // hipcenter y
                            win15 += hipcenterDistance + ","; // hipcenter distance 
                            win15 += currentRow[52] + ",";  // spine y
                            win15 += spineDistance + ",";
                            win15 += headToFloorDistance + ",";
                        }
                        else if (step == 20)
                        {

                            win20 += currentRow[1] + ","; // head y
                            win20 += headDistance + ","; // Head Vel 
                            win20 += currentRow[28] + ","; // hipcenter y
                            win20 += hipcenterDistance + ","; // hipcenter distance 
                            win20 += currentRow[52] + ",";  // spine y
                            win20 += spineDistance + ",";
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
