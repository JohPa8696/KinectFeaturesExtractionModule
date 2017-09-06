using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Math;
using Accord.Statistics.Kernels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FallDetectionSystemDataProcessor
{
    class SVMEvaluator
    {
        public string fileLocation;
        public Accord.MachineLearning.VectorMachines.SupportVectorMachine<Gaussian> svmModel { get; set; }
        public double[][] inputs { get; set; }
        public int[] outputs { get; set; }
        List<double> data;

        public SVMEvaluator(string fileLocation)
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
                        vectorLength = featureVector.Length;
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
            //ScatterplotBox.Show("Fall non fall", inputs, outputs).Hold();
        }


        public Accord.MachineLearning.VectorMachines.SupportVectorMachine<Gaussian> buildModel()
        {
            // Create a new Sequential Minimal Optimization (SMO) learning 
            // algorithm and estimate the complexity parameter C from data
            var teacher = new SequentialMinimalOptimization<Gaussian>()
            {
                UseComplexityHeuristic = true,
                UseKernelEstimation = true // estimate the kernel from the data
            };

            this.svmModel = teacher.Learn(inputs, outputs);
            return svmModel;
        }

        public bool[] classify(double[][] inputs)
        {
            bool[] answers = null;
            if (svmModel != null)
            {
                answers = svmModel.Decide(inputs);
            }
            return answers;
        }

    }
}
