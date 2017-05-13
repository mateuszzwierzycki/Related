using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Related.Graphs;

namespace Related.Edges {
    [Serializable]
    public class GraphEdgeList<T> : ICollection<T>, IEnumerable<T> where T : EdgeBase {
        
        private SortedList<T, T> _data = new SortedList<T, T>();
        private Graphs.GraphBase _owner = null;
        private SortedList<T, T> Data { get => _data; set => _data = value; }
        private GraphBase Owner { get => _owner; set => _owner = value; }

        public bool IsReadOnly => false;

        int ICollection<T>.Count => Data.Count();

        internal GraphEdgeList(GraphBase Owner) : base() { this.Owner = Owner; }

        public GraphEdgeList<T> Duplicate(GraphBase NewOwner) {
            if (NewOwner.VertexCount != this.Owner.VertexCount) {
                throw new ArgumentOutOfRangeException("NewOwner", "New owner has a different number of vertices than the old one.");
            }

            GraphEdgeList<T> ng = new GraphEdgeList<T>(NewOwner);

            foreach (T ed in this) {
                T dup = (T)ed.Duplicate();
                ng.Add(dup);
            }

            return ng;
        }

        public T this[T Key] {
            get { return Data[Key]; }
            set { Data[Key] = value; }
        }

        public void Add(T item) {
            if (item.PointA > Owner.VertexCount - 1) { throw new IndexOutOfRangeException(); }
            if (item.PointB > Owner.VertexCount - 1) { throw new IndexOutOfRangeException(); }
            if (!Data.ContainsKey(item)) { Data.Add(item, item);  } 
        }

        public void AddRange(IEnumerable<T> Values) {
            foreach (T item in Values) {
                this.Add(item);
            }
        }

        public bool Remove(T item) {
            Data.Remove(item);
            return true;
        }

        public void RemoveAt(int index) { Data.RemoveAt(index); }

        public void Clear() { Data.Clear(); }

        public void CleanUp() {
            SortedList<T, T> nl = new SortedList<T, T>();

            foreach (T ed in this) {
                if (ed.IsValid()) { nl[ed] = ed; }
            }

            this._data = nl;
        }

        public void OnVertexRemove(int Vertex) {

            foreach (EdgeBase ed in this) {
                EdgeBase thised = ed;
                this.Remove((T)thised);  //TODO check if this works, casting seems unnecessary.

                if (thised.PointA > Vertex) { thised.PointA -= 1; }
                else if (thised.PointA == Vertex) { thised.PointA = -1; }

                if (thised.PointB > Vertex) { thised.PointB -= 1; }
                else if (thised.PointB == Vertex) { thised.PointB = -1; }

                this[(T)thised]= (T)thised;
            }

        }

        public bool Contains(T item) {
            return Data.ContainsValue(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            for (int i = 0; i < this.Count(); i++) {
                array[arrayIndex + i] = Data.Keys[i];
            }
        }

        public IEnumerator<T> GetEnumerator() {
            return Data.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Data.Values.GetEnumerator();
        }
    }
}
