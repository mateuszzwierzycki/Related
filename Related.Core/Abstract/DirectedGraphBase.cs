using System.Collections.Generic;
using System.Linq;

namespace Related.Abstract {
    abstract public class DirectedGraphBase : Graphs.GraphBase {

        public static List<List<int>> FindAllWalks(int Source, List<int>[] AdjacencyMatrix) {
            List<HashSet<int>> hsl = new List<HashSet<int>>() { new HashSet<int>() { Source } };

            bool run = true;
            List<List<int>> walks = new List<List<int>>();

            while (run) {
                run = false;
                List<HashSet<int>> nhsl = new List<HashSet<int>>();

                foreach (HashSet<int> hs in hsl) {
                    int lastvert = hs.Last();
                    List<int> thisconn = AdjacencyMatrix[lastvert];

                    if (thisconn.Count == 0) { walks.Add(hs.ToList()); continue; }

                    for (int i = 0; i < thisconn.Count; i++) {
                        int thisnei = thisconn[i];
                        if (hs.Contains(thisnei)) {
                            //terminate
                            walks.Add(hs.ToList());
                        }
                        else {
                            run = true;
                            HashSet<int> newone = new HashSet<int>(hs);
                            newone.Add(thisnei);
                            nhsl.Add(newone);
                        }
                    }
                }
                hsl.Clear();
                hsl = nhsl;
            }

            return walks;
        }

        /// <summary>
        /// Finds vertices without "parents" in a directed graph.
        /// </summary>
        /// <param name="AdjacencyMatrix"></param>
        /// <returns></returns>
        public static List<int> FindSources(List<int>[] AdjacencyMatrix) {
            List<int> l = new List<int>();
            bool[] isnt = new bool[AdjacencyMatrix.Length];

            for (int i = 0; i < AdjacencyMatrix.Length; i++) {
                for (int j = 0; j < AdjacencyMatrix[i].Count; j++) {
                    isnt[AdjacencyMatrix[i][j]] = true;
                }
            }

            for (int i = 0; i < AdjacencyMatrix.Length; i++) {
                if (!isnt[i]) { l.Add(i); }
            }

            return l;
        }

        /// <summary>
        /// Finds vertices without "kids" in a directed graph.
        /// </summary>
        /// <param name="AdjacencyMatrix"></param>
        /// <returns></returns>
        public static List<int> FindTargets(List<int>[] AdjacencyMatrix) {
            List<int> l = new List<int>();

            for (int i = 0; i < AdjacencyMatrix.Length; i++) {
                if (AdjacencyMatrix[i].Count == 0) { l.Add(i); }
            }

            return l;
        }
    }
}
