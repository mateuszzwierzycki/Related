using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Related.Abstract {
    abstract public class UndirectedGraphBase : Abstract.GraphBase {

        private static void BronKerbosch(HashSet<int> R, HashSet<int> P, HashSet<int> X, ref List<int>[] AdjacencyMatrix, ref HashSet<HashSet<int>> Cliques, int MaxCount = -1) {
            if (Cliques.Count >= MaxCount) { return; }

            if (P.Count == 0 & X.Count == 0 & R.Count > 2) { Cliques.Add(R); }

            Debug.WriteLine(Cliques.Count); 

            while (P.Count > 0) {
                int vertex = P.First();

                HashSet<int> nR = new HashSet<int>(R);
                HashSet<int> nP = new HashSet<int>(P);
                HashSet<int> nX = new HashSet<int>(X);

                nR.Add(vertex);

                List<int> adj = AdjacencyMatrix[vertex];
                adj.Remove(vertex);

                nP.IntersectWith(adj);
                nX.IntersectWith(adj);

                BronKerbosch(nR, nP, nX, ref AdjacencyMatrix, ref Cliques, MaxCount);

                P.Remove(vertex);
                X.Add(vertex);
            }
        }

        /// <summary>
        /// Won't return edges and vertices as cliques. Will stop when the clique count == MaxCount.
        /// </summary>
        /// <param name="MaxCount"></param>
        /// <param name="AdjacencyMatrix"></param>
        /// <returns></returns>
        public static HashSet<HashSet<int>> FindMaximalCliques(List<int>[] AdjacencyMatrix, int MaxCount =-1 ) {
            List<int> VertIndices = new List<int>(AdjacencyMatrix.Length);
            for (int i = 0; i < AdjacencyMatrix.Length; i++) { VertIndices.Add(i); }

            HashSet<int> nR = new HashSet<int>();
            HashSet<int> nP = new HashSet<int>(VertIndices);
            HashSet<int> nX = new HashSet<int>();

            HashSet<HashSet<int>> cliques = new HashSet<HashSet<int>>();
            BronKerbosch(nR, nP, nX, ref AdjacencyMatrix, ref cliques, MaxCount);

            return cliques;
        }

    }
}
