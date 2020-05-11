using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace AlgorithmLibrary.Kmeans
{
    public class VectorSpaceModel
    {
        private static HashSet<string> distinctTerms;
        private static List<string> documentCollection;
        private static Regex r = new Regex("([ \\t{}()\",:;. \n])");

      /// <summary>
      /// Xử lý documnet thành dạng vector
      /// </summary>
      /// <param name="collection">Danh sách document</param>
      /// <returns>Danh sách document với vectorSpace tương ứng</returns>
        public static List<DocumentVector> ProcessDocumentCollection(DocumentCollection collection)
        {
            
            distinctTerms = new HashSet<string>(); // túi từ
            documentCollection = collection.DocumentList;

            #region Bag of word
            foreach (string documentContent in documentCollection)
            {
                foreach (string term in r.Split(documentContent))
                {
                    distinctTerms.Add(term);
                }
            }

            List<string> removeList = new List<string>(){"\"","\r","\n","(",")","[","]","{","}","","."," ",","};
            foreach (string s in removeList)
            {
                distinctTerms.Remove(s);
            }
            #endregion

            List<DocumentVector> documentVectorSpace = new List<DocumentVector>();
            float[] vectorSpaceArray;
            foreach (string document in documentCollection)
            {
                int count = 0;
                vectorSpaceArray = new float[distinctTerms.Count];

                foreach (string term in distinctTerms)
                {
                    vectorSpaceArray[count] = FindTFIDF(document,term); // tính Tf_Idf
                    count++;
                }

                var documentVector = new DocumentVector();
                documentVector.Content = document;
                documentVector.VectorSpace = vectorSpaceArray;
                documentVectorSpace.Add(documentVector);
            }
            
            return documentVectorSpace;

        }
        #region Tính Tf_Idf

        // Hàm tính Tf_Idf
        private static float FindTFIDF(string document, string term)
        {
            float tf = FindTermFrequency(document, term);
            float idf = FindInverseDocumentFrequency(term);
            return tf * idf;
        }

        // Hàm tính Tf
        private static float FindTermFrequency(string document, string term)
        {
            int count = r.Split(document).Where(s => s.ToUpper() == term.ToUpper()).Count();
            return (float)((float)count / (float)(r.Split(document).Count()));
        }

        // Hàm tính Idf
        private static float FindInverseDocumentFrequency(string term)
        {
            int count = documentCollection.ToArray().Where(s => r.Split(s.ToUpper()).ToArray().Contains(term.ToUpper())).Count();
            return (float)Math.Log((float)documentCollection.Count() / (float)count);

        }
        #endregion
    }
}
