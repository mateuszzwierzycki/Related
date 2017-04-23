﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Related.Graphs;

namespace Related.Vertices {
    class GraphVertexList<T> : ICollection<T> where T : struct {

        private List<T> _data = new List<T>();
        private Graphs.GraphBase _owner = null;

        public List<T> Data { get => _data; set => _data = value; }
        public GraphBase Owner { get => _owner; set => _owner = value; }

        internal GraphVertexList(GraphBase Owner) : base() { this.Owner = Owner; }

        public int Count => Data.Count();

        public bool IsReadOnly => false;

        public void Add(T item) {
            Data.Add(item);
        }

        public void Clear() {
            Data.Clear();
        }

        public bool Contains(T item) {
            return Data.Contains(item);
        }

        public T this[int index] {
            get { return Data[index]; }
            set { Data[index] = value; }
        }

        public void CopyTo(T[] array, int arrayIndex) {
            for (int i = 0; i < Data.Count(); i++) {
                array[arrayIndex + i] = this[i];
            }
        }

        public IEnumerator<T> GetEnumerator() {
            return Data.GetEnumerator();
        }

        public bool Remove(T item) {
            int index = Data.IndexOf(item);
            if (index != -1) { Data.RemoveAt(index); return true; }
            return false;
        }

        public void RemoveAt(int index) {
            Data.RemoveAt(index);
            OnVertexRemove(index);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Data.GetEnumerator();
        }

        private void OnVertexRemove(int Vertex) { Owner.OnRemove(Vertex); }

    }
}
