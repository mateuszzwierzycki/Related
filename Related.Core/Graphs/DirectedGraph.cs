using Related.Edges;
using Related.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Related.Graphs {

    [Serializable]
    public class DirectedGraph<T> : GraphBase where T : struct {

        private GraphEdgeList<DirectedEdge> _edges = null;
        private GraphVertexList<T> _vertices = null;

        public GraphEdgeList<DirectedEdge> Edges { get => _edges; set => _edges = value; }
        public GraphVertexList<T> Vertices { get => _vertices; set => _vertices = value; }

        public DirectedGraph() : base() {
            Edges = new GraphEdgeList<DirectedEdge>(this);
            Vertices = new GraphVertexList<T>(this);
        }

        public DirectedGraph(IEnumerable<T> Vertices) {
            Edges = new GraphEdgeList<DirectedEdge>(this);
            Vertices = new GraphVertexList<T>(this);
            if (Vertices != null) { this.Vertices.AddRange(Vertices); }
        }

        public DirectedGraph<T> Duplicate() {
            DirectedGraph<T> ng = new DirectedGraph<T>(this.Vertices);
            ng.Edges = this._edges.Duplicate(ng);
            return ng;
        }

        public UndirectedGraph<T> GetUndirected()
        {
            UndirectedGraph<T> ng = new UndirectedGraph<T>(this.Vertices);

            foreach (EdgeBase item in this.Edges) {
                ng.Edges.Add(new UndirectedEdge(item));
            }

            return ng; 
        }

        public override int VertexCount => Vertices.Count();

        public int EdgeCount => Edges.Count();

        public List<int>[] GetAdjacencyMatrixUndirected() {
            List<int>[] adj = new List<int>[this.VertexCount - 1];

            int cnt = 0;
            foreach (T item in this.Vertices) {
                adj[cnt] = new List<int>();
                cnt += 1;
            }

            foreach (DirectedEdge item in Edges) {
                if (item.IsValid()) {
                    adj[item.From].Add(item.To);
                    adj[item.To].Add(item.From);
                }
            }

            return adj;
        }

        public override List<int>[] GetAdjacencyMatrix() {
            List<int>[] adj = new List<int>[this.VertexCount - 1];

            int cnt = 0;
            foreach (T item in this.Vertices) {
                adj[cnt] = new List<int>();
                cnt += 1;
            }

            foreach (DirectedEdge ed in this.Edges) {
                if (ed.IsValid()) { adj[ed.From].Add(ed.To); }
            }

            return adj;
        }

        public override List<int> GetAdjacent(int index) {
            List<int> nl = new List<int>();

            foreach (DirectedEdge ed in Edges) {
                if (ed.From != index) { continue; }
                if (ed.IsValid()) { nl.Add(ed.To); }
            }

            return nl;
        }

        public override void OnRemove(int Vertex) {
            foreach (DirectedEdge item in Edges) {
                item.OnVertexRemove(Vertex);
            }
        }

        public override string ToString() {
            return "Directed Graph (V:" + VertexCount.ToString() + " E:" + EdgeCount.ToString() + ")";
        }

        public List<List<int>> FindAllWalks(int Source, List<int>[] AdjacencyMatrix) {

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
                        } else {
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
    }
}
