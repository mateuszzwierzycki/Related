using Related.Abstract;
using Related.Collections;
using Related.Edges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Related.Graphs {

    /// <summary>
    /// A graph storing values at each edge. 
    /// </summary>
    /// <typeparam name="EdgeValue"></typeparam>
    [Serializable]
    public class DGraphEdge<EdgeValue> : DGraphBase  {

        private EdgeList<DEdge<EdgeValue>> _edges = null;
        private VertexList<bool> _vertices = null;

        public override int VertexCount => Vertices.Count();
        public int EdgeCount => Edges.Count();

        public EdgeList<DEdge<EdgeValue>> Edges { get => _edges; set => _edges = value; }
        public VertexList<bool> Vertices { get => _vertices; set => _vertices = value; }

        public DGraphEdge() : base() {
            this._edges = new EdgeList<DEdge<EdgeValue>>(this);
            this._vertices = new VertexList<bool>(this);
        }

        public DGraphEdge(int VertexCount) {
            this._edges = new EdgeList<DEdge<EdgeValue>>(this);
            this._vertices = new VertexList<bool>(this);

            for (int i = 0; i < VertexCount; i++) {
                this.Vertices.Add(true);
            }
        }

        //public DGraphEdge<EdgeValue> Duplicate() {
        //    DGraphEdge<EdgeValue> ng = new DGraphEdge<EdgeValue>(this.Vertices.Count);
        //    ng.Edges = this.Edges.Duplicate(ng);
        //    return ng;
        //}

        public override List<int>[] GetAdjacencyMatrix() {
            List<int>[] adj = new List<int>[this.VertexCount];

            int cnt = 0;
            foreach (bool item in this.Vertices) {
                adj[cnt] = new List<int>();
                cnt += 1;
            }

            foreach (DEdge<EdgeValue> ed in this.Edges) {
                if (ed.IsValid()) {
                    adj[ed.From].Add(ed.To);
                }
            }

            return adj;
        }

        /// <summary>
        /// Gets all the connected vertices.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override List<int> GetAdjacent(int index) {
            List<int> nl = new List<int>();

            foreach (DEdge<EdgeValue> ed in Edges) {
                if (!ed.IsValid()) { continue; }

                if (ed.From == index & ed.IsCycle()) { nl.Add(ed.From); continue; }
                if (ed.From  == index) { nl.Add(ed.To); }
            }

            return nl;
        }
        
        public override List<int> GetAdjacent(int index, List<int>[] CachedAMatrix) {
            return CachedAMatrix[index];
        }

        public override void OnRemove(int Vertex) {
            foreach (DEdge<EdgeValue> item in this.Edges) {
                item.OnVertexRemove(Vertex);
            }
        }

    }

    /// <summary>
    /// A graph storing value at each vertex. 
    /// </summary>
    /// <typeparam name="VertexValue"></typeparam>
    [Serializable]
    public class DGraph<VertexValue> : DGraphBase {

        private EdgeList<DEdge> _edges = null;
        private VertexList<VertexValue> _vertices = null;

        public EdgeList<DEdge> Edges { get => _edges; set => _edges = value; }
        public VertexList<VertexValue> Vertices { get => _vertices; set => _vertices = value; }

        public DGraph() : base() {
            Edges = new EdgeList<DEdge>(this);
            Vertices = new VertexList<VertexValue>(this);
        }

        public DGraph(IEnumerable<VertexValue> Vertices) {
            _edges = new EdgeList<DEdge>(this);
            _vertices = new VertexList<VertexValue>(this);
            if (Vertices != null) { this.Vertices.AddRange(Vertices); }
        }

        //public DGraph<VertexValue> Duplicate() {
        //    DGraph<VertexValue> ng = new DGraph<VertexValue>(this.Vertices);
        //    ng.Edges = this._edges.Duplicate(ng);
        //    return ng;
        //}

        public UGraph<VertexValue> GetUndirected()
        {
            UGraph<VertexValue> ng = new UGraph<VertexValue>(this.Vertices);

            foreach (EdgeBase item in this.Edges) {
                ng.Edges.Add(new UEdge(item));
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

            foreach (DEdge item in Edges) {
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

            foreach (DEdge ed in this.Edges) {
                if (ed.IsValid()) { adj[ed.From].Add(ed.To); }
            }

            return adj;
        }

        public override List<int> GetAdjacent(int index) {
            List<int> nl = new List<int>();

            foreach (DEdge ed in Edges) {
                if (ed.From != index) { continue; }
                if (ed.IsValid()) { nl.Add(ed.To); }
            }

            return nl;
        }
        
        public override List<int> GetAdjacent(int index, List<int>[] CachedAMatrix) {
            return CachedAMatrix[index];
        }

        public override void OnRemove(int Vertex) {
            foreach (DEdge item in Edges) {
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
    /// <typeparam name="EdgeValue">Has to be struct.</typeparam>
    [Serializable]
    public class DGraph<VertexValue, EdgeValue> : DGraphBase {

        private EdgeList<DEdge<EdgeValue>> _edges = null;
        private VertexList<VertexValue> _vertices = null;

        public EdgeList<DEdge<EdgeValue>> Edges { get => _edges; set => _edges = value; }
        public VertexList<VertexValue> Vertices { get => _vertices; set => _vertices = value; }

        public DGraph() : base() {
            Edges = new EdgeList<DEdge<EdgeValue>>(this);
            Vertices = new VertexList<VertexValue>(this);
        }

        public DGraph(IEnumerable<VertexValue> Vertices) {
            _edges = new EdgeList<DEdge<EdgeValue>>(this);
            _vertices = new VertexList<VertexValue>(this);
            if (Vertices != null) { this.Vertices.AddRange(Vertices); }
        }

        //public DGraph<VertexValue, EdgeValue> Duplicate() {
        //    DGraph<VertexValue, EdgeValue> ng = new DGraph<VertexValue, EdgeValue>(this.Vertices);
        //    ng.Edges = this._edges.Duplicate(ng);
        //    return ng;
        //}

        public override int VertexCount => Vertices.Count();

        public int EdgeCount => Edges.Count();

        public List<int>[] GetAdjacencyMatrixUndirected() {
            List<int>[] adj = new List<int>[this.VertexCount];

            int cnt = 0;
            foreach (VertexValue item in this.Vertices) {
                adj[cnt] = new List<int>();
                cnt += 1;
            }

            foreach (DEdge<EdgeValue> item in Edges) {
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

            foreach (DEdge<EdgeValue> ed in this.Edges) {
                if (ed.IsValid()) { adj[ed.From].Add(ed.To); }
            }

            return adj;
        }


        public override List<int> GetAdjacent(int index, List<int>[] CachedAMatrix) {
            return CachedAMatrix[index];
        }

        public override List<int> GetAdjacent(int index) {
            List<int> nl = new List<int>();

            foreach (DEdge<EdgeValue> ed in Edges) {
                if (ed.From != index) { continue; }
                if (ed.IsValid()) { nl.Add(ed.To); }
            }

            return nl;
        }

        public override void OnRemove(int Vertex) {
            foreach (DEdge<EdgeValue> item in Edges) {
                item.OnVertexRemove(Vertex);
            }
        }

        public override string ToString() {
            return "Directed Graph <T,Q> (V:" + VertexCount.ToString() + " E:" + EdgeCount.ToString() + ")";
        }

    }
}
