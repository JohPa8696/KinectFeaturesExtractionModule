using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FallDetectionSystemDataProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            // Directory contians all raw data files FOR TRAINING
            string baseDir = "C:/Users/johnn/Desktop/";
            string trainDir =baseDir + "TrainingData/Train";
            string testDir = baseDir + "TrainingData/TestDataSet";
            //string resDir = baseDir + "TrainingData/TestDataSet/ResultDatasets/";
            // Get files in the directory
            string[] trainingFileEntries  = Directory.GetFiles(trainDir);
            string[] testFileEntries = Directory.GetFiles(testDir);

            ArrayList featuresExtractors = new ArrayList();
            featuresExtractors.Add(new FeatureExtractor1());
            //featuresExtractors.Add(new FeatureExtractor2());
            //featuresExtractors.Add(new FeatureExtractor3());

            foreach (IRawDataExtractor extractor in featuresExtractors)
            {

                List<StringBuilder> trainingBuilders = new List<StringBuilder>();
                List<StringBuilder> testBuilders = new List<StringBuilder>();
                // Need 5 stringbuilders for both training and test because of 5 differnt window size 5,10,15,20,25.
                for(int i =0; i<5; i++)
                {
                    trainingBuilders.Add(new StringBuilder());
                    trainingBuilders[i].AppendLine(extractor.getColumns());
                    testBuilders.Add(new StringBuilder());
                    testBuilders[i].AppendLine(extractor.getColumns());
                }
                // Loop thru all training datasets
                foreach (string filename in trainingFileEntries)
                {
                    List<double[]> data = getData(filename);
                    // Call formatter processes the datarow and return a string 
                    string[] res = extractor.process(data);
                    for (int i = 0; i < 5; i++)
                    {
                        trainingBuilders[i].Append(res[i]);
                    }
                }
                // Write the result to a csv file
                for(int i =5; i<30; i += 5)
                {
                    string trainingsetPath = Directory.GetCurrentDirectory() + "/trainingset"+i+".csv";
                    int counter = 0;
                    while (File.Exists(trainingsetPath))
                    {
                        trainingsetPath = Directory.GetCurrentDirectory() + "/trainingset" + i + "_" + counter + ".csv";
                        counter++;
                    }
                    // write data to file
                    File.AppendAllText(trainingsetPath, trainingBuilders[i/5 -1].ToString());
                    trainingBuilders[i/5 -1].Clear();
                }

                // Loop thru all test datasets
                foreach (string filename in testFileEntries)
                {
                    List<double[]> data = getData(filename);
                    // Call formatter processes the datarow and return a string 
                    string[] res = extractor.process(data);
                    for (int i = 0; i < 5; i++)
                    {
                        testBuilders[i].Append(res[i]);
                    }
                }

                // test set
                for (int i = 5; i < 30; i += 5)
                {
                    string testsetPath = Directory.GetCurrentDirectory() + "/testset" + i + ".csv";
                    int counter = 0;
                    while (File.Exists(testsetPath))
                    {
                        testsetPath = Directory.GetCurrentDirectory() + "/testset" + i + "_" + counter + ".csv";
                        counter++;
                    }
                    // write data to file
                    File.AppendAllText(testsetPath, testBuilders[i/5 -1].ToString());
                    testBuilders[i/5 -1 ].Clear();
                }

            }
        }


        // Return an array of datarow in a file
        private static List<double[]> getData(string filePath)
        {
            // ArrayList contains all 30 rows of data in the file
            List<double[]> data = new List<double[]>();
            foreach (string line in File.ReadLines(filePath, Encoding.UTF8))
            {
                if (line.Contains("Head"))
                {
                    // This is the column do nothing
                }
                else
                {
                    string[] numbers = line.Split(',');

                    // Some negative values are in the format (0-0.23) for some reason,
                    // which coses wrong format exception when being converted
                    for (int i = 0; i < numbers.Length; i++)
                    {
                        if (numbers[i].Contains("0-"))
                        {
                            numbers[i] = numbers[i].Remove(0, 1);
                        }else if (numbers[i].Contains("1-"))
                        {
                            numbers[i] = (Convert.ToDouble(numbers[i].Remove(0, 1)) +1.0).ToString();
                        }
                    }
                    // Convert string to an array of double 
                    data.Add(Array.ConvertAll(numbers, Double.Parse));

                }
            }
            return data;
        }

    }
}
