using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAlgorithmLibrary.Kmeans
{
    public class SimilarityMatrics
    {
        // Tính độ tương tự giữa 2 vector bằng công thức cosine
        // Cosine Similarity (d1, d2) =  Dot product(d1, d2) / ||d1|| * ||d2||
        public static float FindCosineSimilarity(float[] vecA, float[] vecB)
        {
            var dotProduct = DotProduct(vecA, vecB);
            var magnitudeOfA = Magnitude(vecA);
            var magnitudeOfB = Magnitude(vecB);
            float result = dotProduct / (magnitudeOfA * magnitudeOfB);
            
            if (float.IsNaN(result))
                return 0;
            else
                return (float)result;
        }

        // Dot product(d1, d2) = d1[0]*d2[0] + d1[1]*d2[1] + ....d1[n]*d2[n]
        public static float DotProduct(float[] vecA, float[] vecB)
        {
            float dotProduct = 0;
            for (var i = 0; i < vecA.Length; i++)
            {
                dotProduct += (vecA[i] * vecB[i]);
            }

            return dotProduct;
        }

        // Hàm tính căn bậc 2 vector
        public static float Magnitude(float[] vector)
        {
            return (float)Math.Sqrt(DotProduct(vector, vector));
        }

        #region Extended Jaccard
        //Combines properties of both cosine similarity and Euclidean distance
        public static float FindExtendedJaccard(float[] vecA, float[] vecB)
        {
            var dotProduct = DotProduct(vecA, vecB);
            var magnitudeOfA = Magnitude(vecA);
            var magnitudeOfB = Magnitude(vecB);

            return dotProduct / (magnitudeOfA + magnitudeOfB - dotProduct);

        }
        #endregion


        public static float FindEuclideanDistance(float[] vecA, float[] vecB)
        {
            float euclideanDistance = 0;
            for (var i = 0; i < vecA.Length; i++)
            {
                euclideanDistance += (float)Math.Pow((vecA[i] - vecB[i]), 2);
            }

            return (float)Math.Sqrt(euclideanDistance);

        }


    }
}
