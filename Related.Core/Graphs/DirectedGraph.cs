using Related.Abstract;
using Related.Edges;
using Related.Vertices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Related.Graphs {

    [Serializable]
    public class DirectedGraph<VertexValue> : DirectedGraphBase where VertexValue : struct {

        private GraphEdgeList<DirectedEdge> _edges = null;
        private GraphVertexList<VertexValue> _vertices = null;

        public GraphEdgeList<DirectedEdge> Edges { get => _edges; set => _edges = value; }
        public GraphVertexList<VertexValue> Vertices { get => _vertices; set => _vertices = value; }

        public DirectedGraph() : base() {
            Edges = new GraphEdgeList<DirectedEdge>(this);
            Vertices = new GraphVertexList<VertexValue>(this);
        }

        public DirectedGraph(IEnumerable<VertexValue> Vertices) {
            _edges = new GraphEdgeList<DirectedEdge>(this);
            _vertices = new GraphVertexList<VertexValue>(this);
            if (Vertices != null) { this.Vertices.AddRange(Vertices); }
        }

        public DirectedGraph<VertexValue> Duplicate() {
            DirectedGraph<VertexValue> ng = new DirectedGraph<VertexValue>(this.Vertices);
            ng.Edges = this._edges.Duplicate(ng);
            return ng;
        }

        public UndirectedGraph<VertexValue> GetUndirected()
        {
            UndirectedGraph<VertexValue> ng = new UndirectedGraph<VertexValue>(this.Vertices);

            foreach (EdgeBase item in this.Edges) {
                ng.Edges.Add(new UndirectedEdge(item));
            }

            return ng; 
        }

        public override int VertexCount => Vertices.Count();

        public int EdgeCount => Edges.Count();

        public List<int>[] GetAdjacencyMatrixUndirected() {
            List<int>[] adj = new List<int>[this.VertexCount];

            int cnt = 0;
            foreach (VertexValue item in this.Vertices) {
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
            List<int>[] adj = new List<int>[this.VertexCount];

            int cnt = 0;
            foreach (VertexValue item in this.Vertices) {
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
            return "Directed Graph <T> (V:" + VertexCount.ToString() + " E:" + EdgeCount.ToString() + ")";
        }

    }
 
    /// <summary>
    /// A directed graph storing values both in verices and edges. 
    /// </summary>
    /// <typeparam name="VertexValue">Has to be struct</typeparam>
    /// <typeparam name="EdgeValue">Has to be struct, has to implement the IComparable<Q> interface (required for binary search).</typeparam>
[Serializable]
public class DirectedGraph<VertexValue,EdgeValue> : DirectedGraphBase 
        where VertexValue : struct
        where EdgeValue : struct, IComparable<EdgeValue>
    {
         
    private GraphEdgeList<DirectedEdge<EdgeValue>> _edges = null;
    private GraphVertexList<VertexValue> _vertices = null;

    public GraphEdgeList<DirectedEdge<EdgeValue>> Edges { get => _edges; set => _edges = value; }
    public GraphVertexList<VertexValue> Vertices { get => _vertices; set => _vertices = value; }

    public DirectedGraph() : base() {
        Edges = new GraphEdgeList<DirectedEdge<EdgeValue>>(this);
        Vertices = new GraphVertexList<VertexValue>(this);
    }

    public DirectedGraph(IEnumerable<VertexValue> Vertices) {
        _edges = new GraphEdgeList<DirectedEdge<EdgeValue>>(this);
        _vertices = new GraphVertexList<VertexValue>(this);
        if (Vertices != null) { this.Vertices.AddRange(Vertices); }
    }

    public DirectedGraph<VertexValue,EdgeValue> Duplicate() {
        DirectedGraph<VertexValue,EdgeValue> ng = new DirectedGraph<VertexValue,EdgeValue>(this.Vertices);
        ng.Edges = this._edges.Duplicate(ng);
        return ng;
    }

    public override int VertexCount => Vertices.Count();

    public int EdgeCount => Edges.Count();

    public List<int>[] GetAdjacencyMatrixUndirected() {
        List<int>[] adj = new List<int>[this.VertexCount];

        int cnt = 0;
        foreach (VertexValue item in this.Vertices) {
            adj[cnt] = new List<int>();
            cnt += 1;
        }

        foreach (DirectedEdge<EdgeValue> item in Edges) {
            if (item.IsValid()) {
                adj[item.From].Add(item.To);
                adj[item.To].Add(item.From);
            }
        }

        return adj;
    }

    public override List<int>[] GetAdjacencyMatrix() {
        List<int>[] adj = new List<int>[this.VertexCount];

        int cnt = 0;
        foreach (VertexValue item in this.Vertices) {
            adj[cnt] = new List<int>();
            cnt += 1;
        }

        foreach (DirectedEdge<EdgeValue> ed in this.Edges) {
            if (ed.IsValid()) { adj[ed.From].Add(ed.To); }
        }

        return adj;
    }

    public override List<int> GetAdjacent(int index) {
        List<int> nl = new List<int>();

        foreach (DirectedEdge<EdgeValue> ed in Edges) {
            if (ed.From != index) { continue; }
            if (ed.IsValid()) { nl.Add(ed.To); }
        }

        return nl;
    }

    public override void OnRemove(int Vertex) {
        foreach (DirectedEdge<EdgeValue> item in Edges) {
            item.OnVertexRemove(Vertex);
        }
    }

    public override string ToString() {
        return "Directed Graph <T,Q> (V:" + VertexCount.ToString() + " E:" + EdgeCount.ToString() + ")";
    }

}
}
