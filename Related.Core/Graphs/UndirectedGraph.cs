using Related.Abstract;
using Related.Edges;
using Related.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Related.Graphs {

    [Serializable]
    public class UGraph<VertexValue> : UGraphBase where VertexValue : struct {

        private EdgeList<UEdge> _edges = null;
        private VertexList<VertexValue> _vertices = null;

        public EdgeList<UEdge> Edges { get => _edges; set => _edges = value; }
        public VertexList<VertexValue> Vertices { get => _vertices; set => _vertices = value; }

        public override int VertexCount => Vertices.Count();
        public int EdgeCount => Edges.Count();

        public UGraph() : base() {
            _edges = new EdgeList<UEdge>(this);
            _vertices = new VertexList<VertexValue>(this);
        }

        public UGraph(IEnumerable<VertexValue> Vertices) {
            _edges = new EdgeList<UEdge>(this);
            _vertices = new VertexList<VertexValue>(this);
            if (Vertices != null) { _vertices.AddRange(Vertices); }
        }

        public UGraph<VertexValue> Duplicate() {
            UGraph<VertexValue> ng = new UGraph<VertexValue>(this.Vertices);
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

            foreach (UEdge ed in this.Edges) {
                if (ed.IsValid()) {
                    adj[ed.PointA].Add(ed.PointB);
                    adj[ed.PointB].Add(ed.PointA);
                }
            }

            return adj;
        }

        public override List<int> GetAdjacent(int index) {
            List<int> nl = new List<int>();

            foreach (UEdge ed in Edges) {
                if (!ed.IsValid()) { continue; }

                if (ed.IsCycle() & ed.PointA == index) { nl.Add(ed.PointA); continue; }

                if (ed.PointA == index) { nl.Add(ed.PointB); }
                if (ed.PointB == index) { nl.Add(ed.PointA); }
            }

            return nl;
        }

        public override List<int> GetAdjacent(int index, List<int>[] CachedAMatrix) {
            return CachedAMatrix[index];
        }

        public override void OnRemove(int Vertex) {
            foreach (UEdge item in Edges) {
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

        public DGraph<VertexValue> GetDirected() {
            DGraph<VertexValue> g = new DGraph<VertexValue>(this.Vertices);
            foreach (UEdge item in Edges) { g.Edges.Add(new DEdge(item.PointA, item.PointB)); }
            return g;
        }

    }
    
    /// <summary>
    /// An undirected graph which can hold values in it's edges.
    /// </summary>
    /// <typeparam name="EdgeValue"></typeparam>
    [Serializable]
    public class UGraphEdge<EdgeValue> : UGraphBase where EdgeValue : struct, IComparable<EdgeValue> {

        private EdgeList<UEdge<EdgeValue>> _edges = null; // new GraphEdgeList<UndirectedEdge<T>>(this); 
        private VertexList<bool> _vertices = null;

        public override int VertexCount => Vertices.Count();
        public int EdgeCount => Edges.Count();

        public EdgeList<UEdge<EdgeValue>> Edges { get => _edges; set => _edges = value; }
        public VertexList<bool> Vertices { get => _vertices; set => _vertices = value; }

        public UGraphEdge() : base() {
            this._edges = new EdgeList<UEdge<EdgeValue>>(this);
            this._vertices = new VertexList<bool>(this);
        }

        public UGraphEdge(int VertexCount) {
            this._edges = new EdgeList<UEdge<EdgeValue>>(this);
            this._vertices = new VertexList<bool>(this);
                
                for (int i = 0; i < VertexCount; i++) {
                    this.Vertices.Add(true); 
                }
        }

        public UGraphEdge<EdgeValue> Duplicate() {
            UGraphEdge<EdgeValue> ng = new UGraphEdge<EdgeValue>(this.Vertices.Count);
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

            foreach (UEdge<EdgeValue> ed in this.Edges) {
                if (ed.IsValid()) {
                    adj[ed.PointA].Add(ed.PointB);
                    adj[ed.PointB].Add(ed.PointA);
                }
            }

            return adj;
        }

        public override List<int> GetAdjacent(int index) {
            List<int> nl = new List<int>();

            foreach (UEdge<EdgeValue> ed in Edges) {
                if (!ed.IsValid()) { continue; }

                if (ed.IsCycle() & ed.PointA == index) { nl.Add(ed.PointA); continue; }

                if (ed.PointA == index) { nl.Add(ed.PointB); }
                if (ed.PointB == index) { nl.Add(ed.PointA); }
            }

            return nl;
        }



        public override List<int> GetAdjacent(int index, List<int>[] CachedAMatrix) {
            return CachedAMatrix[index];
        }

        public override void OnRemove(int Vertex) {
            foreach (UEdge<EdgeValue> item in this.Edges) {
                item.OnVertexRemove(Vertex);
            }
        }

    }

}



