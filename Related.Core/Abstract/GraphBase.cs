using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Related.Graphs {
    [Serializable]
    public abstract class GraphBase {

        public abstract List<int>[] GetAdjacencyMatrix();
        public abstract List<int> GetAdjacent(int index);

        public abstract int VertexCount { get; }

        /// <summary>
        /// Can be used by the vertex collection (whatever that is) to inform 
        /// the GraphBase about the Remove called.
        /// </summary>
        /// <param name="Vertex"></param>
        public virtual void OnRemove(int Vertex) { }

        /// <summary>
        /// Can be used by the vertex collection (whatever that is) to inform 
        /// the GraphBase about the Add called.
        /// </summary>
        /// <param name="Vertex"></param>
        public virtual void OnAdd(int Vertex) { }

        public static List<int> BreadthFirstSearch(IEnumerable<int> Sources, int LookFor, List<int>[] AdjacencyMatrix) {
            int vcount = AdjacencyMatrix.Length;
            List<int>[] adj = AdjacencyMatrix;
            List<List<int>> paths = new List<List<int>>();
            bool[] vis = new bool[vcount - 1];

            for (int i = 0; i < Sources.Count(); i++) {
                List<int> npth = new List<int> { Sources.ElementAt(i) };
                vis[Sources.ElementAt(i)] = true;
                paths.Add(npth);
            }

            int left = vis.Length - 1;

            for (;;) {
                List<List<int>> nextpaths = new List<List<int>>();

                foreach (List<int> pth in paths) {
                    int thistip = pth[pth.Count - 1];
                    List<int> thisadj = adj[thistip];

                    bool wasempty = true;

                    foreach (int vert in thisadj) {
                        if (vis[vert]) continue;
                        vis[vert] = true;
                        wasempty = false;
                        List<int> newsplit = new List<int>(pth);
                        newsplit.Add(vert);
                        if (vert == LookFor) { return newsplit; }
                        nextpaths.Add(newsplit);
                    }

                    if (wasempty) { nextpaths.Add(pth); }
                }

                paths.Clear();
                paths.AddRange(nextpaths);

                int thisleft = 0;

                foreach (bool val in vis) {
                    if (!val) { thisleft += 1; }
                }

                if (thisleft == left) { break; }
                left = thisleft;
            }

            return null;
        }

        public static List<List<int>> BreadthFirstTree(IEnumerable<int> Sources, List<int>[] AdjacencyMatrix, out bool[] Visited) {
            int vcount = AdjacencyMatrix.Length;
            List<int>[] adj = AdjacencyMatrix;
            List<List<int>> paths = new List<List<int>>();
            bool[] vis = new bool[vcount - 1];

            for (int i = 0; i < Sources.Count(); i++) {
                List<int> pth = new List<int> { Sources.ElementAt(i) };
                vis[Sources.ElementAt(i)] = true;
                paths.Add(pth);
            }

            int left = vis.Length - 1;

            for (;;) {
                List<List<int>> npath = new List<List<int>>();
                foreach (List<int> pth in paths) {
                    int thistip = pth[pth.Count() - 1];
                    List<int> thisadj = adj[thistip];

                    bool wasempty = true;

                    foreach (int vert in thisadj) {
                        if (vis[vert]) { continue; }
                        vis[vert] = true;
                        wasempty = false;
                        List<int> newsplit = new List<int>(pth);
                        newsplit.Add(vert);
                        npath.Add(newsplit);
                    }

                    if (wasempty) { npath.Add(pth); }
                }

                paths.Clear();
                paths.AddRange(npath);

                int thisleft = 0;

                foreach (bool item in vis) {
                    if (!item) { thisleft += 1; }
                }

                if (thisleft == left) { break; }
                left = thisleft;
            }

            Visited = vis;
            return paths;
        }

        /// <summary>
        /// Finds disconnected groups.
        /// </summary>
        /// <param name="AdjacencyMatrix"></param>
        /// <returns></returns>
        public static List<List<int>> FindDisconnected(List<int>[] AdjacencyMatrix) {
            List<int>[] adj = AdjacencyMatrix;
            List<List<int>> ll = new List<List<int>>();
            int vcount = AdjacencyMatrix.Length;

            bool[] done = new bool[vcount - 1];

            bool run = true;

            while (run) {
                run = false;
                List<int> l = new List<int>();
                List<int> dump = new List<int>();
                l.Clear();

                for (int i = 0; i < vcount; i++) {
                    if (!done[i]) {
                        l.Add(i);
                        dump.Add(i);
                        done[i] = true;
                        break;
                    }
                }

                bool rrun = true;

                while (rrun) {
                    rrun = false;
                    List<int> nl = new List<int>();

                    for (int i = 0; i < l.Count(); i++) {
                        int thisv = l[i];

                        for (int j = 0; j < adj[thisv].Count(); j++) {
                            int thisn = adj[thisv][j];
                            if (!done[thisn]) {
                                done[thisn] = true;
                                rrun = true;
                                run = true;
                                nl.Add(thisn);
                            }
                        }
                    }

                    dump.AddRange(nl);
                    l = nl;
                }

                if (dump.Count() > 0) { ll.Add(dump); }
            }

            return ll;
        }

        /// <summary>
        /// Finds vertices without "parents" in a directed graph.
        /// Doesn't make sense in an undirected graph.
        /// </summary>
        /// <param name="AdjacencyMatrix"></param>
        /// <returns></returns>
        public List<int> FindSources(List<int>[] AdjacencyMatrix) {
            List<int> l = new List<int>();
            bool[] isnt = new bool[AdjacencyMatrix.Length - 1];

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
        /// Doesn't make sense in an undirected graph.
        /// </summary>
        /// <param name="AdjacencyMatrix"></param>
        /// <returns></returns>
        public List<int> FindTargets(List<int>[] AdjacencyMatrix) {
            List<int> l = new List<int>();

            for (int i = 0; i < AdjacencyMatrix.Length; i++) {
                if (AdjacencyMatrix[i].Count == 0) { l.Add(i); }
            }

            return l;
        }

        /// <summary>
        /// As the Related library uses an array of lists as an adjacency matrix, 
        /// it might be a bit trickier to transpose it.
        /// </summary>
        /// <param name="AdjacencyMatrix"></param>
        /// <returns></returns>
        public static List<int>[] TransposeMatrix(List<int>[] AdjacencyMatrix) {
            List<int>[] nm = new List<int>[AdjacencyMatrix.Length - 1];
            for (int i = 0; i < nm.Length; i++) {
                nm[i] = new List<int>();
            }

            for (int i = 0; i < AdjacencyMatrix.Length; i++) {
                List<int> tl = AdjacencyMatrix[i];

                for (int j = 0; j < tl.Count; j++) {
                    nm[tl[j]].Add(i);
                }
            }

            return nm;
        }

    }
}