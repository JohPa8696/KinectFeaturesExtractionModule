using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FallDetectionSystemDataProcessor
{
    class FeatureExtractor12 : IRawDataExtractor
    {
        private string columns = "Head_Y_Vel,Spine_X_Delta,Spine_Y_Delta,Spine_Z_Delta";

        public string getColumns()
        {
            return this.columns;
        }

        public string getColumns(int windowSize)
        {
            throw new NotImplementedException();
        }

        public int getID()
        {
            throw new NotImplementedException();
        }

        public string[] process(List<double[]> data)
        {

            throw new NotImplementedException();
        }
    }
}
