using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FallDetectionSystemDataProcessor
{

    // This is the same as extractor, but scaled up adn added head to floor distance
    // scale by converting to cm
    class FeatureExtractor2 : IRawDataExtractor
    {

        private string columns = "Head_Y,Head_Vel_Y,HipCenter_Y,HipCenter_Vel_Y_,Spine_X,Spine_Y,Spine_Z,Box_W, Box_H,Box_D,Box_Delta_W,Box_Delta_H,Box_Delta_D, Head_to_Floor_Distance, Class";

        public string[] process(List<double[]> data)
        {
            // Contains a list of strings
            List<String> extractedData = new List<String>();
            double[] previousRow = data[0];
            double prevBoxW = 0.0, prevBoxH = 0.0, prevBoxD = 0.0;
            for (int i = 0; i < data.Count; i++)
            {
                // This list contains features values of type double for final traiing dataset
                ArrayList featureValues = new ArrayList();
                double[] currentRow = data[i];
                // Find the smallest and largest X value
                double minX = 1000;
                double maxX = -1000;
                // 7 because the last 7 values are for floor abcd, timestmap framecounter and class
                for (int j = 0; j < currentRow.Length - 7; j += 3)
                {
                    if (currentRow[j] > maxX)
                    {
                        maxX = currentRow[j];
                    }
                    else if (currentRow[j] < minX)
                    {
                        minX = currentRow[j];
                    }
                }
                // Height
                double minY = 1000;
                double maxY = -1000;
                for (int j = 0; j < currentRow.Length - 7; j += 3)
                {
                    if (currentRow[j] > maxY)
                    {
                        maxY = currentRow[j];
                    }
                    else if (currentRow[j] < minY)
                    {
                        minY = currentRow[j];
                    }
                }
                // Depth
                double minZ = 1000;
                double maxZ = -1000;
                for (int j = 0; j < currentRow.Length - 7; j += 3)
                {
                    if (currentRow[j] > maxZ)
                    {
                        maxZ = currentRow[j];
                    }
                    else if (currentRow[j] < minZ)
                    {
                        minZ = currentRow[j];
                    }
                }
                double boxW = Math.Abs(maxX - minX);
                double boxH = Math.Abs(maxY - minY);
                double boxD = Math.Abs(maxZ - minZ);

                // if this is the first row aka first frame skip
                if (i == 0)
                {
                    prevBoxW = boxW;
                    prevBoxH = boxH;
                    prevBoxD = boxD;
                    continue;

                }
                //Calculate the distance from head to floor
                double headToFloorDistance = 1.0 ;
                if (!(currentRow[60] ==0 && currentRow[61] == 0 && currentRow[62] == 0 && currentRow[63] == 0))
                {
                    //Calculate the distance between Head and Floor
                    headToFloorDistance = currentRow[0] * currentRow[60] + currentRow[1] * currentRow[61] + currentRow[2] * currentRow[62] + currentRow[63];
                    // Scale distance 
                    headToFloorDistance *= 100;
                }

                double timeDiff = (currentRow[64] - previousRow[64]);

                featureValues.Add(currentRow[1]*100); // HEAD Y

                featureValues.Add((currentRow[1] - previousRow[1])*100 *1000 / timeDiff); // HEAD Vel Y

                // Calculate the hipcenter veldelta
                featureValues.Add(currentRow[28]*100); // hipcenter Y
                double hcvel = (currentRow[28] - previousRow[28])*100 *1000 / timeDiff;
                featureValues.Add(hcvel); // hip center vel y

                featureValues.Add(currentRow[51]* 100); // Spine X 
                featureValues.Add(currentRow[52]* 100); // Spine Y
                featureValues.Add(currentRow[53]* 100); // Spine Z

                featureValues.Add(boxW * 100); // box width
                featureValues.Add(boxH * 100); // box height
                featureValues.Add(boxD * 100); // box depth

                //Calculate box delta
                double box_delta_w = (boxW - prevBoxW) * 100 *1000/ timeDiff;
                double box_delta_h = (boxH - prevBoxH) * 100 *1000 / timeDiff;
                double box_delta_d = (boxD - prevBoxD) * 100 *1000/ timeDiff;
                featureValues.Add(box_delta_w);  // box delta w
                featureValues.Add(box_delta_h);  // box delta h
                featureValues.Add(box_delta_d);  // box delta d
                featureValues.Add(headToFloorDistance); // head to floor distance
                featureValues.Add(currentRow[66]); // class label

                // convert the double array to string
                string s = "";
                foreach (double fl in featureValues)
                {
                    s += fl.ToString() + ",";
                }
                // remove the last comma
                s.Remove(s.Length - 1, 1);
                previousRow = currentRow;
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

        public string getColumns(int windowSize)
        {
            return this.columns;
        }
    }
}
