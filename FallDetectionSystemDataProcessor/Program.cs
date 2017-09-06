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
        public static int AMOUNT_OF_WINDOWS = 5;

        static void Main(string[] args)
        {
            // Directory contians all raw data files FOR TRAINING

            string baseDir = "C:/Users/johnn/Desktop/";
            string trainDir = baseDir + "TrainingData/Train";
            string testDir = baseDir + "TrainingData/TestDataSet";
            //string resDir = baseDir + "TrainingData/TestDataSet/ResultDatasets/";
            // Get files in the directory
            string[] trainingFileEntries = Directory.GetFiles(trainDir);
            string[] testFileEntries = Directory.GetFiles(testDir);

            ArrayList featuresExtractors = new ArrayList();
            featuresExtractors.Add(new FeatureExtractor1());
            featuresExtractors.Add(new FeatureExtractor2());
            featuresExtractors.Add(new FeatureExtractor3());



            foreach (IRawDataExtractor extractor in featuresExtractors)
            {

                List<StringBuilder> trainingBuilders = new List<StringBuilder>();
                List<StringBuilder> testBuilders = new List<StringBuilder>();
                // Need 5 stringbuilders for both training and test because of 5 differnt window size 5,10,15,20,25.
                for (int i = 0; i < 5; i++)
                {
                    trainingBuilders.Add(new StringBuilder());
                    trainingBuilders[i].AppendLine(extractor.getColumns(i));
                    testBuilders.Add(new StringBuilder());
                    testBuilders[i].AppendLine(extractor.getColumns(i));
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

                //create folder for files
                int folderIndex = 1;
                string newFolder = Directory.GetCurrentDirectory() + "/FeatureExtractor_" + extractor.getID();
                while (Directory.Exists(newFolder))
                {
                    newFolder = Directory.GetCurrentDirectory() + "/FeatureExtractor_" + extractor.getID() + "_" + folderIndex;
                    folderIndex++;
                }

                Directory.CreateDirectory(newFolder);
                Directory.CreateDirectory(newFolder + "/5");
                Directory.CreateDirectory(newFolder + "/10");
                Directory.CreateDirectory(newFolder + "/15");
                Directory.CreateDirectory(newFolder + "/20");
                Directory.CreateDirectory(newFolder + "/25");

                // Write the result to a csv file
                for (int i = 5; i < 30; i += 5)
                {
                    string trainingsetPath = newFolder + "/" + i + "/trainingset" + i + ".csv";

                    // write data to file
                    File.AppendAllText(trainingsetPath, trainingBuilders[i / 5 - 1].ToString());
                    trainingBuilders[i / 5 - 1].Clear();
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
                    string testsetPath = newFolder + "/" + i + "/testset" + i + ".csv";
                    File.AppendAllText(testsetPath, testBuilders[i / 5 - 1].ToString());
                    testBuilders[i / 5 - 1].Clear();
                }

                for (int i = 5; i < 30; i += 5)
                {
                    string folder = newFolder + "/" + i;
                    StringBuilder summary = new StringBuilder();
                    summary.AppendLine("Summary results");
                    summary.AppendLine("Iteration,Correctly Classified,Incorrectly Classified,Fall Percision,Fall Recall,Fall F1 Score,No Fall Percision,No Fall Recall,No Fall F1 Score");

                    StringBuilder matrix = new StringBuilder();

                    for (int l = 0; l < 10; l++)
                    {
                        SVMEvaluator svm = null;
                        SVMEvaluator test = null;

                        foreach (string filename in Directory.GetFiles(folder))
                        {
                            if (filename.Contains("train"))
                            {
                                svm = new SVMEvaluator(filename);
                                svm.buildModel();
                            }
                            else if(filename.Contains("test"))
                            {
                                test = new SVMEvaluator(filename);
                                //Console.Write(test.outputs);
                            }
                        }
                        bool[] results = svm.classify(test.inputs);

                        StringBuilder output = new StringBuilder();
                        output.AppendLine("Actual,Classified As");

                        int noFallTP = 0;
                        int noFallFP = 0;
                        int fallTP = 0;
                        int fallFP = 0;
                        int count = 0;
                        foreach (int j in test.outputs)
                        {
                            if (results[count])
                            {
                                if (j == ToInt(results[count])) // fall TP
                                {
                                    fallTP++;
                                }
                                else // fall FP
                                {
                                    fallFP++;
                                }
                            }
                            else
                            {
                                if (j == ToInt(results[count])) // nofall TP
                                {
                                    noFallTP++;
                                }
                                else // nofall FP
                                {
                                    noFallFP++;
                                }
                            }
                            output.AppendLine(j + "," + ToInt(results[count]));
                            count++;
                        }
                        string outputPath = newFolder + "/" + i + "/output" + l + ".csv";
                        File.AppendAllText(outputPath, output.ToString());

                        double correctly_classified = (double)(fallTP + noFallTP) / (double)(noFallTP + noFallFP + fallTP + fallFP);
                        double incorrectly_classified = (double)(fallFP + noFallFP) / (double)(noFallTP + noFallFP + fallTP + fallFP);

                        double percisionFall = (double)fallTP / (double)(fallTP + fallFP);
                        double recallFall = (double)fallTP / (double)(fallTP + noFallTP);
                        double f1_score_fall = 2 * (percisionFall * recallFall) / (percisionFall + recallFall);

                        double percisionNoFall = (double)noFallTP / (double)(noFallTP + noFallFP);
                        double recallNoFall = (double)noFallTP / (double)(noFallTP + fallFP);
                        double f1_score_noFall = 2 * (percisionNoFall * recallNoFall) / (percisionNoFall + recallNoFall);

                        summary.AppendLine(l + "," + correctly_classified + "," + incorrectly_classified + "," 
                            + percisionFall + "," + recallFall + "," + f1_score_fall + ","
                            + percisionNoFall + "," + recallNoFall + "," + f1_score_noFall);

                        matrix.AppendLine("Confusion Matrix Iteration" + i);
                        matrix.AppendLine("Fall, No Fall,<-Classified As");
                        matrix.AppendLine(fallTP + "," + noFallFP + ",Fall=Fall");
                        matrix.AppendLine(fallFP + "," + noFallTP + ",No Fall=No Fall");
                        matrix.AppendLine();
                    }
                    string summaryPath = newFolder + "/" + i + "/summary" + i + ".csv";
                    File.AppendAllText(summaryPath, summary.ToString());

                    string matrixPath = newFolder + "/" + i + "/confusion_matrix" + i + ".csv";
                    File.AppendAllText(matrixPath, matrix.ToString());
                }
            }



            if (System.Diagnostics.Debugger.IsAttached) Console.ReadLine();
        }


        // Return an array of datarow in a file
        private static List<double[]> getData(string filePath)
        {
            Console.WriteLine(filePath);
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
                        if (numbers[i].Contains("0-") || numbers[i].Contains("1-"))
                        {
                            numbers[i] = numbers[i].Remove(0, 1);

                        }
                        else if (Convert.ToDouble(numbers[i]) >= 10 && i < 64 && numbers[66] == "1")
                        {
                            numbers[i] = (Convert.ToDouble(numbers[i]) - 10.0).ToString();
                        }
                    }
                    // Convert string to an array of double 
                    data.Add(Array.ConvertAll(numbers, Double.Parse));

                }
            }
            return data;
        }

        public static int ToInt(bool value)
        {
            return value ? 1 : 0;
        }

    }
}
