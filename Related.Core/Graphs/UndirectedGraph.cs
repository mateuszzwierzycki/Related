using Related.Abstract;
using Related.Edges;
using Related.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Related.Graphs {

    [Serializable]
    public class UndirectedGraph<VertexValue> : UndirectedGraphBase where VertexValue : struct {

        private GraphEdgeList<UndirectedEdge> _edges = null;
        private GraphVertexList<VertexValue> _vertices = null;

        public GraphEdgeList<UndirectedEdge> Edges { get => _edges; set => _edges = value; }
        public GraphVertexList<VertexValue> Vertices { get => _vertices; set => _vertices = value; }

        public override int VertexCount => Vertices.Count();
        public int EdgeCount => Edges.Count();

        public UndirectedGraph() : base() {
            _edges = new GraphEdgeList<UndirectedEdge>(this);
            _vertices = new GraphVertexList<VertexValue>(this);
        }

        public UndirectedGraph(IEnumerable<VertexValue> Vertices) {
            _edges = new GraphEdgeList<UndirectedEdge>(this);
            _vertices = new GraphVertexList<VertexValue>(this);
            if (Vertices != null) { _vertices.AddRange(Vertices); }
        }

        public UndirectedGraph<VertexValue> Duplicate() {
            UndirectedGraph<VertexValue> ng = new UndirectedGraph<VertexValue>(this.Vertices);
            ng.Edges = this._edges.Duplicate(ng);
            return ng;
        }

        public override List<int>[] GetAdjacencyMatrix() {
            List<int>[] adj = new List<int>[this.VertexCount];

            int cnt = 0;
            foreach (VertexValue item in this.Vertices) {
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
            return "Directed Graph(V:" + VertexCount.ToString() + " E:" + EdgeCount.ToString() + ")";
        }

        //public List<List<int>> FindAllWalks(int Source, List<int>[] AdjacencyMatrix) {

        //    List<HashSet<int>> hsl = new List<HashSet<int>>() { new HashSet<int>() { Source } };

        //    bool run = true;
        //    List<List<int>> walks = new List<List<int>>();

        //    while (run) {
        //        run = false;
        //        List<HashSet<int>> nhsl = new List<HashSet<int>>();

        //        foreach (HashSet<int> hs in hsl) {
        //            int lastvert = hs.Last();
        //            List<int> thisconn = AdjacencyMatrix[lastvert];

        //            if (thisconn.Count == 0) { walks.Add(hs.ToList()); continue; }

        //            for (int i = 0; i < thisconn.Count; i++) {
        //                int thisnei = thisconn[i];
        //                if (hs.Contains(thisnei)) {
        //                    //terminate
        //                    walks.Add(hs.ToList());
        //                }
        //                else {
        //                    run = true;
        //                    HashSet<int> newone = new HashSet<int>(hs);
        //                    newone.Add(thisnei);
        //                    nhsl.Add(newone);
        //                }
        //            }
        //        }

        //        hsl.Clear();
        //        hsl = nhsl;
        //    }

        //    return walks;
        //}

        public DirectedGraph<VertexValue> GetDirected() {
            DirectedGraph<VertexValue> g = new DirectedGraph<VertexValue>(this.Vertices);
            foreach (UndirectedEdge item in Edges) { g.Edges.Add(new DirectedEdge(item.PointA, item.PointB)); }
            return g;
        }

    }


    /// <summary>
    /// An undirected graph which can hold values in it's edges.
    /// </summary>
    /// <typeparam name="EdgeValue"></typeparam>
    [Serializable]
    public class UndirectedGraphEdgeT<EdgeValue> : UndirectedGraphBase where EdgeValue : struct, IComparable<EdgeValue> {

        private GraphEdgeList<UndirectedEdge<EdgeValue>> _edges = null; // new GraphEdgeList<UndirectedEdge<T>>(this); 
        private GraphVertexList<bool> _vertices = null;

        public override int VertexCount => Vertices.Count();
        public int EdgeCount => Edges.Count();

        public GraphEdgeList<UndirectedEdge<EdgeValue>> Edges { get => _edges; set => _edges = value; }
        public GraphVertexList<bool> Vertices { get => _vertices; set => _vertices = value; }

        public UndirectedGraphEdgeT() : base() {
            this._edges = new GraphEdgeList<UndirectedEdge<EdgeValue>>(this);
            this._vertices = new GraphVertexList<bool>(this);
        }

        public UndirectedGraphEdgeT(IEnumerable<bool> Vertices) {
            this._edges = new GraphEdgeList<UndirectedEdge<EdgeValue>>(this);
            this._vertices = new GraphVertexList<bool>(this);

            if (this._vertices != null) {
                this._vertices.AddRange(Vertices);
            }
        }

        public UndirectedGraphEdgeT<EdgeValue> Duplicate() {
            UndirectedGraphEdgeT<EdgeValue> ng = new UndirectedGraphEdgeT<EdgeValue>(this.Vertices);
            ng.Edges = this.Edges.Duplicate(ng);
            return ng;
        }

        public override List<int>[] GetAdjacencyMatrix() {
            List<int>[] adj = new List<int>[this.VertexCount];

            int cnt = 0;
            foreach (bool item in this.Vertices) {
                adj[cnt] = new List<int>();
                cnt += 1;
            }

            foreach (UndirectedEdge<EdgeValue> ed in this.Edges) {
                if (ed.IsValid()) {
                    adj[ed.PointA].Add(ed.PointB);
                    adj[ed.PointB].Add(ed.PointA);
                }
            }

            return adj;
        }

        public override List<int> GetAdjacent(int index) {
            List<int> nl = new List<int>();

            foreach (UndirectedEdge<EdgeValue> ed in Edges) {
                if (!ed.IsValid()) { continue; }

                if (ed.IsCycle() & ed.PointA == index) { nl.Add(ed.PointA); continue; }

                if (ed.PointA == index) { nl.Add(ed.PointB); }
                if (ed.PointB == index) { nl.Add(ed.PointA); }
            }

            return nl;
        }

        public override void OnRemove(int Vertex) {
            foreach (UndirectedEdge<EdgeValue> item in this.Edges) {
                item.OnVertexRemove(Vertex);
            }
        }

    }

}



