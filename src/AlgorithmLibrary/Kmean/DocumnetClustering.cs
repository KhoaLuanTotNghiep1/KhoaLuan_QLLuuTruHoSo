using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAlgorithmLibrary.Kmeans;

namespace AlgorithmLibrary.Kmeans
{
    public static class DocumnetClustering
    {
        private static Centroid centroid;
        private static DocumentVector documentVector;
        /// <summary>
        /// Hàm phân nhóm document bằng độ tương tự giữa document với center
        /// </summary>
        /// <param name="k">số lượng nhóm</param>
        /// <param name="documentCollection">Danh sách document</param>
        /// <returns>Danh sách centroid</returns>
        public static List<Centroid> DocumentCluster(int k, List<DocumentVector> documentCollection, string textSearch)
        {
            // Danh sách centroid ban đầu
            List<Centroid> centroidCollection = new List<Centroid>();

            bool stoppingCriteria; // biến dừng thuật toán
            List<Centroid> resultSet; // danh sách centroid trả về
            List<Centroid> prevClusterCenter; // danh sách centroid cũ

            //Danh sách số ngẫu nhiên dùng để chọn document ngẫu nghiên làm centroid
            HashSet<int> uniqRand = new HashSet<int>();
            uniqRand = GenerateRandomNumber(k,documentCollection.Count);
            
            foreach(int pos in uniqRand) 
            {
                var c = new Centroid
                {
                    GroupedDocument = new List<DocumentVector>
                    {
                        documentCollection[pos]
                    }   
                };

                centroidCollection.Add(c);                
            }

            resultSet = InitializeClusterCentroid(centroidCollection.Count); // khởi tạo cluster

            do
            {
                prevClusterCenter = centroidCollection;
                centroid = new Centroid();
                documentVector = new DocumentVector();

                foreach (DocumentVector obj in documentCollection)
                {
                    int index = FindClosestClusterCenter(centroidCollection, obj);
                    resultSet[index].GroupedDocument.Add(obj);

                    if (obj.Content == textSearch)
                    { 
                        centroid = resultSet[index];
                        documentVector = obj;
                    }
                }
                centroidCollection = InitializeClusterCentroid(centroidCollection.Count());
                centroidCollection = CalculateMeanPoints(resultSet);
                stoppingCriteria = CheckStoppingCriteria(prevClusterCenter, centroidCollection);

                if (!stoppingCriteria)
                {
                    //initialize the result set for next iteration
                    resultSet = InitializeClusterCentroid(centroidCollection.Count);
                }
            } while (stoppingCriteria == false);

            return resultSet;
        }

        /// <summary>
        /// Hàm Tạo số ngẫu nhiên
        /// </summary>
        /// <param name="uniqRand"></param>
        /// <param name="k">số lượng nhóm</param>
        /// <param name="docCount">số lượng document</param>
        /// <returns></returns>
        private static HashSet<int> GenerateRandomNumber(int k, int docCount)
        {
            //Random r = new Random();
            HashSet<int> uniqRand = new HashSet<int>();
            int count = 0;
            if (k < docCount)
                count = k;
            else
                count = docCount;

            for (int i = 1; i <= count; i++)
                uniqRand.Add(i);

            //if (k > docCount)
            //{
            //    do
            //    {
            //        int pos = r.Next(0, docCount);
            //        uniqRand.Add(pos);

            //    } while (uniqRand.Count != docCount);
            //}            
            //else
            //{
            //    do
            //    {
            //        int pos = r.Next(0, docCount);
            //        uniqRand.Add(pos);

            //    } while (uniqRand.Count != k);
            //}

            return uniqRand;
        }

        /// <summary>
        /// Khởi tạo centroid cho các cluster
        /// </summary>
        /// <param name="centroid">trả lại danh sách centroid</param>
        /// <param name="count">số lượng cluster</param>\
        /// <returns></returns>
        private static List<Centroid> InitializeClusterCentroid(int count)
        {
            List<Centroid> centroids = new List<Centroid>();
            for (int i = 0; i < count; i++)
            {
                var c = new Centroid
                {
                    GroupedDocument = new List<DocumentVector>()
                };
                centroids.Add(c);
            }

            return centroids;
        }

        /// <summary>
        /// Dừng thuật toán
        /// Nếu centroid không di chuyển vị trí
        /// </summary>
        /// <param name="prevClusterCenter">Danh sách centroid cũ</param>
        /// <param name="newClusterCenter">Danh sách centroid mới</param>
        /// <returns>true/false</returns>
        private static bool CheckStoppingCriteria(List<Centroid> prevClusterCenter, List<Centroid> newClusterCenter)
        {
            bool stoppingCriteria;
            int[] changeIndex = new int[newClusterCenter.Count()];

            int index = 0; // nếu index lớn hơn hoặc bằng số lượng center thì dừng vòng lặp 
            do
            {
                int count = 0;
                if (newClusterCenter[index].GroupedDocument.Count == 0 && prevClusterCenter[index].GroupedDocument.Count == 0)
                {
                    index++;
                }
                else
                {
                    if (newClusterCenter[index].GroupedDocument.Count != 0 && prevClusterCenter[index].GroupedDocument.Count != 0)
                    {
                        for (int j = 0; j < newClusterCenter[index].GroupedDocument[0].VectorSpace.Count(); j++)
                        {
                            // Đếm số lần vectorSpace bằng nhau của 2 DS mới và cũ
                            if (newClusterCenter[index].GroupedDocument[0].VectorSpace[j] == prevClusterCenter[index].GroupedDocument[0].VectorSpace[j])
                            {
                                count++;
                            }
                        }

                        // nếu số lần vectorSpace bằng với số vectorSpace của nhóm document
                        // 1 là di chuyển 0 là không di chuyển
                        if (count == newClusterCenter[index].GroupedDocument[0].VectorSpace.Count())
                        {
                            changeIndex[index] = 0;
                        }
                        else
                        {
                            changeIndex[index] = 1;
                        }
                        index++;
                    }
                    else
                    {
                        index++;
                        continue;
                    }
                }
            } while (index < newClusterCenter.Count());

            // Nếu changeIndex có chứa thuộc tính có giá trị 1 thì false ngược lại true
            if (changeIndex.Where(s => s != 0).Select(r => r).Any())
                stoppingCriteria = false;
            else
                stoppingCriteria = true;

            return stoppingCriteria;
        }

        /// <summary>
        /// Tìm center có độ tương tự cao nhất với document thông qua công thức cosine
        /// </summary>
        /// <param name="clusterCenter">danh sách center</param>
        /// <param name="obj">document</param>
        /// <returns>center có độ tương tự cao nhất với document</returns>
        private static int FindClosestClusterCenter(List<Centroid> clusterCenters,DocumentVector obj)
        {
            int countCenter = clusterCenters.Count(); // số lượng center
            float[] similarityMeasure = new float[countCenter];

            // Tính độ tương tự của document với các center rồi add vào mảng similarityMeasure
            for (int i = 0; i < countCenter; i++)
            {
                similarityMeasure[i] = SimilarityMatrics.FindExtendedJaccard(clusterCenters[i].GroupedDocument[0].VectorSpace, obj.VectorSpace);
            }

            // Tìm max trong mảng similarityMeasure
            int index = 0;
            float maxValue = similarityMeasure[0];
            for (int i = 0; i < similarityMeasure.Count(); i++)
            {
                if (similarityMeasure[i] >maxValue)
                {
                    maxValue = similarityMeasure[i];
                    index = i;
                }
            }
            return index;    
        }

        public static string FindClosestDocument()
        {
            if (centroid != null)
            {
                //int countCenter = centroid.GroupedDocument.Count() - 1;
                float max = 0;
                string doc = "";
                // Tính độ tương tự của document với các center rồi add vào mảng similarityMeasure
                foreach (var ce in centroid.GroupedDocument)
                {
                    if (ce.Content != documentVector.Content)
                    {
                        float current = SimilarityMatrics.FindCosineSimilarity(ce.VectorSpace, documentVector.VectorSpace);
                        if (max < current)
                        {
                            max = current;
                            doc = ce.Content;
                        }
                    }
                }
                return doc;
            }
            return null;
        }

        // Tính lại center cho các Cluster
        private static List<Centroid> CalculateMeanPoints(List<Centroid> clusterCenters)
        {
            for (int i = 0; i < clusterCenters.Count(); i++)
            {
                if (clusterCenters[i].GroupedDocument.Count() > 0)
                {
                    for (int j = 0; j < clusterCenters[i].GroupedDocument[0].VectorSpace.Count(); j++)
                    {
                        float total = 0;

                        // Tính tổng VectorSpace của cluster
                        foreach (DocumentVector document in clusterCenters[i].GroupedDocument)
                        {
                            var a = document.VectorSpace[j];
                            total += document.VectorSpace[j];
                        }

                        // Tính trung bình cluster để cho ra center mới cho cluster
                        clusterCenters[i].GroupedDocument[0].VectorSpace[j] = total / clusterCenters[i].GroupedDocument.Count();
                    }
                }
            }
            return clusterCenters;
        } 

    }  
}
