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
    public class DirectedGraph<T> : DirectedGraphBase where T : struct {

        private GraphEdgeList<DirectedEdge> _edges = null;
        private GraphVertexList<T> _vertices = null;

        public GraphEdgeList<DirectedEdge> Edges { get => _edges; set => _edges = value; }
        public GraphVertexList<T> Vertices { get => _vertices; set => _vertices = value; }

        public DirectedGraph() : base() {
            Edges = new GraphEdgeList<DirectedEdge>(this);
            Vertices = new GraphVertexList<T>(this);
        }

        public DirectedGraph(IEnumerable<T> Vertices) {
            _edges = new GraphEdgeList<DirectedEdge>(this);
            _vertices = new GraphVertexList<T>(this);
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
            List<int>[] adj = new List<int>[this.VertexCount];

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
            List<int>[] adj = new List<int>[this.VertexCount];

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
            return "Directed Graph(V:" + VertexCount.ToString() + " E:" + EdgeCount.ToString() + ")";
        }

    }
}
