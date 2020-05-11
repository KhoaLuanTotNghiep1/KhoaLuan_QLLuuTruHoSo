using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgorithmLibrary.Kmeans
{
    public class DocumentVector
    {
        public string Content { get; set; } // nội dung document
        
        public float[] VectorSpace { get; set; } // Tf_Idf
    }
}
