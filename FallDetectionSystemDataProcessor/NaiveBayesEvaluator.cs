using Accord.MachineLearning.Bayes;
using Accord.Math;
using Accord.Statistics.Distributions.Univariate;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FallDetectionSystemDataProcessor
{
    class NaiveBayesEvaluator
    {
        NaiveBayes<NormalDistribution> nb;
        public double[][] inputs { get; set; }
        public int[] outputs { get; set; }

        public NaiveBayesEvaluator(string fileLocation)
        {
            // Read the Excel worksheet into a DataTable    
            //DataTable table = new ExcelReader(fileLocation).GetWorksheet("Sheet1");

            StreamReader oStreamReader = new StreamReader(fileLocation);
            DataTable table = new DataTable();
            int rowCount = 0;
            int vectorLength = 0;
            string[] columnNames = null;
            string[] featureVector = null;

            List<int[]> tmpFeatures = new List<int[]>();
            List<int> tmpClass = new List<int>();

            while (!oStreamReader.EndOfStream)
            {
                String row = oStreamReader.ReadLine().Trim();

                if (row.Length > 0)
                {
                    featureVector = row.Split(',');
                    if (rowCount == 0)
                    {
                        columnNames = featureVector;
                        vectorLength = featureVector.Length - 1;
                        foreach (string columnHeader in columnNames)
                        {
                            DataColumn column = new DataColumn(columnHeader.ToUpper(), typeof(string));
                            column.DefaultValue = (float)0;
                            table.Columns.Add(column);
                        }
                    }
                    else
                    {
                        DataRow tableRow = table.NewRow();
                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            if (featureVector[i] == null)
                            {
                                tableRow[columnNames[i]] = 0;
                            }
                            else
                            {
                                tableRow[columnNames[i]] = featureVector[i];
                            }
                        }
                        table.Rows.Add(tableRow);
                    }
                }
                rowCount++;
            }
            oStreamReader.Close();
            oStreamReader.Dispose();

            string[] features = new string[vectorLength];
            Array.Copy(columnNames, 0, features, 0, vectorLength);

            this.inputs = table.ToJagged<double>(features);
            this.outputs = table.Columns["Class"].ToArray<int>();

            /*
            for(int i = 0; i < outputs.Length; i++)
            {
                if (outputs[i] == 0)
                {
                    outputs[i] = -1;
                }
            }*/
        }

        public void buildModel()
        {
            // Now, let's create the forest learning algorithm
            var teacher = new NaiveBayesLearning<NormalDistribution>();

            // Finally, learn a random forest from data
            this.nb = teacher.Learn(inputs, outputs);

        }

        public bool[] classify(double[][] oinputs)
        {
            // We can estimate class labels using
            int[] predicted = nb.Decide(oinputs);
            bool[] answers = new bool[predicted.Length];

            for (int i = 0; i < predicted.Length; i++)
            {
                if (predicted[i] == 0)
                {
                    answers[i] = false;
                }
                else
                {
                    answers[i] = true;
                }
            }
            return answers;
        }
    }
}
