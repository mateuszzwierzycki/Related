using Related.Edges;
using Related.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Related.Graphs {
    
    [Serializable]
    public class UndirectedGraph<T> : GraphBase where T : struct {

        private GraphEdgeList<UndirectedEdge> _edges = null;
        private GraphVertexList<T> _vertices = null;

        public GraphEdgeList<UndirectedEdge> Edges { get => _edges; set => _edges = value; }
        public GraphVertexList<T> Vertices { get => _vertices; set => _vertices = value; }

        public UndirectedGraph() : base() {
            _edges = new GraphEdgeList<UndirectedEdge>(this);
            _vertices = new GraphVertexList<T>(this);
        }

        public UndirectedGraph(IEnumerable<T> Vertices) {
            _edges = new GraphEdgeList<UndirectedEdge>(this);
            _vertices = new GraphVertexList<T>(this);
            if (Vertices != null) { _vertices.AddRange(Vertices); }
        }

        public UndirectedGraph<T> Duplicate() {
            UndirectedGraph<T> ng = new UndirectedGraph<T>(this.Vertices);
            ng.Edges = this._edges.Duplicate(ng);
            return ng;
        }

        public override int VertexCount => Vertices.Count();

        public int EdgeCount => Edges.Count();

        public override List<int>[] GetAdjacencyMatrix() {
            List<int>[] adj = new List<int>[this.VertexCount - 1];

            int cnt = 0;
            foreach (T item in this.Vertices) {
                adj[cnt] = new List<int>();
                cnt += 1;
            }

            foreach (UndirectedEdge ed in this.Edges) {
                if (ed.IsValid()) {
                    adj[ed.PointA].Add(ed.PointB);
                    adj[ed.PointB].Add(ed.PointA);
                }
            }

            return adj;
        }

        public override List<int> GetAdjacent(int index) {
            List<int> nl = new List<int>();

            foreach (UndirectedEdge ed in Edges) {
                if (!ed.IsValid()) { continue; }

                if (ed.IsCycle() & ed.PointA == index) { nl.Add(ed.PointA); continue; }

                if (ed.PointA == index) { nl.Add(ed.PointB); }
                if (ed.PointB == index) { nl.Add(ed.PointA); }
            }

            return nl;
        }

        public override void OnRemove(int Vertex) {
            foreach (UndirectedEdge item in Edges) {
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

        public DirectedGraph<T> GetDirected() {
            DirectedGraph<T> g = new DirectedGraph<T>(this.Vertices);

            foreach (UndirectedEdge item in Edges) {
                g.Edges.Add(new DirectedEdge(item.PointA, item.PointB));
            }

            return g;
        }
    }

    [Serializable]
    public class UndirectedGraphEdgeT<T> : GraphBase where T : struct, IComparable<T> {

        private GraphEdgeList<UndirectedEdge<T>> _edges = null; // new GraphEdgeList<UndirectedEdge<T>>(this); 
        private GraphVertexList<bool> _vertices = null;

        public UndirectedGraphEdgeT() : base() {
            this._edges = new GraphEdgeList<UndirectedEdge<T>>(this);
            this._vertices = new GraphVertexList<bool>(this);
        }

        public UndirectedGraphEdgeT(IEnumerable<bool> Vertices) {
            this._edges = new GraphEdgeList<UndirectedEdge<T>>(this);
            this._vertices = new GraphVertexList<bool>(this);

            if (this._vertices != null) {
                this._vertices.AddRange(Vertices);
            }
        }

        public UndirectedGraphEdgeT<T> Duplicate() {
            UndirectedGraphEdgeT<T> ng = new UndirectedGraphEdgeT<T>(this.Vertices);
            ng.Edges = this.Edges.Duplicate(ng);
            return ng;
        }

        public override int VertexCount => this.Vertices.Count();
        public int EdgeCount => Edges.Count();

        public GraphEdgeList<UndirectedEdge<T>> Edges { get => _edges; set => _edges = value; }
        public GraphVertexList<bool> Vertices { get => _vertices; set => _vertices = value; }

        public override List<int>[] GetAdjacencyMatrix() {
            List<int>[] adj = new List<int>[this.VertexCount - 1];

            int cnt = 0;
            foreach (bool item in this.Vertices) {
                adj[cnt] = new List<int>();
                cnt += 1;
            }

            foreach (UndirectedEdge<T> ed in this.Edges) {
                if (ed.IsValid()) {
                    adj[ed.PointA].Add(ed.PointB);
                    adj[ed.PointB].Add(ed.PointA);
                }
            }

            return adj;
        }

        public override List<int> GetAdjacent(int index) {
            List<int> nl = new List<int>();

            foreach (UndirectedEdge<T> ed in Edges) {
                if (!ed.IsValid()) { continue; }

                if (ed.IsCycle() & ed.PointA == index) { nl.Add(ed.PointA); continue; }

                if (ed.PointA == index) { nl.Add(ed.PointB); }
                if (ed.PointB == index) { nl.Add(ed.PointA); }
            }

            return nl;
        }

        public override void OnRemove(int Vertex) {
            foreach (UndirectedEdge<T> item in this.Edges) {
                item.OnVertexRemove(Vertex); 
            }
        }
    }
    
}
