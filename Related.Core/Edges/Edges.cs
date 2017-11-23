using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Related.Edges {

    [Serializable]
    public class UndirectedEdge : EdgeBase, IComparable<UndirectedEdge> {

        public UndirectedEdge() : base() { }
        public UndirectedEdge(int A, int B) : base(A, B) { }
        public UndirectedEdge(EdgeBase Other) : base(Other) { }

        public int CompareTo(UndirectedEdge Other) {
            if (this.Minimum() < Other.Minimum()) { return -1; }
            if (this.Minimum() > Other.Minimum()) { return 1; }
            if (this.Maximum() < Other.Maximum()) { return -1; }
            if (this.Maximum() > Other.Maximum()) { return 1; }
            return 0;
        }

        public static UndirectedEdge Create(int A, int B) { return new UndirectedEdge(A, B); }

        public override EdgeBase Duplicate() { return new UndirectedEdge(this); }

        public void Orient() {
            if (this.PointA > this.PointB) { this.Flip(); }
        }

        public override string ToString() {
            return "Undirected" + base.ToString();
        }

    }

    [Serializable]
    public class UndirectedEdge<T> : EdgeBase, IComparable<UndirectedEdge<T>> where T : IComparable<T> {

        T myval = default(T);

        public T Value {
            get { return myval; }
            set { myval = value; }
        }

        public UndirectedEdge(int A, int B, T Value) : base(A, B) { this.Value = Value; }

        public static UndirectedEdge<T> Create(int A, int B, T Value) { return new UndirectedEdge<T>(A, B, Value); }

            public int CompareTo(UndirectedEdge<T> Other) {
            if (this.Minimum() < Other.Minimum()) { return -1; }
            if (this.Minimum() > Other.Minimum()) { return 1; }
            if (this.Maximum() < Other.Maximum()) { return -1; }
            if (this.Maximum() > Other.Maximum()) { return 1; }
            return this.Value.CompareTo(Other.Value);
        }

        public override EdgeBase Duplicate() { return new UndirectedEdge<T>(this.PointA, this.PointB, this.Value); }

        public void Orient() {
            if (this.PointA > this.PointB) { this.Flip(); }
        }

        public override string ToString() {
            return "UndirectedEdge (" + PointA.ToString() + "-" + PointB.ToString() + "," + this.Value.ToString() + ")";
        }

    }

    [Serializable]
    public class DirectedEdge : EdgeBase, IComparable<DirectedEdge> {

        public DirectedEdge() : base() { }
        public DirectedEdge(int From, int To) : base(From, To) { }
        public DirectedEdge(EdgeBase Other) : base(Other) { }

        public static DirectedEdge Create(int From, int To) { return new DirectedEdge(From, To); }

        public int From { get => PointA; set => PointA = value; }
        public int To { get => PointB; set => PointB = value; }

        public int CompareTo(DirectedEdge Other) {
            if (this.From < Other.From) { return -1; }
            if (this.From > Other.From) { return 1; }
            if (this.To < Other.To) { return -1; }
            if (this.To > Other.To) { return 1; }
            return 0;
        }

        public override EdgeBase Duplicate() { return new DirectedEdge(this); }

        public void Orient() {
            if (this.PointA > this.PointB) { this.Flip(); }
        }

        public override string ToString() {
            return "Directed" + base.ToString();
        }

    }

    [Serializable]
    public class DirectedEdge<T> : EdgeBase, IComparable<DirectedEdge<T>> where T : IComparable<T> {

        T myval = default(T);

        public T Value {
            get { return myval; }
            set { myval = value; }
        }

        public int From { get => PointA; set => PointA = value; }
        public int To { get => PointB; set => PointB = value; }

        public DirectedEdge(int From, int To, T Value) : base(From, To) { this.Value = Value; }

        public static DirectedEdge<T> Create(int From, int To, T Value) { return new DirectedEdge<T>(From, To,Value); }

        public int CompareTo(DirectedEdge<T> Other) {
            if (this.From < Other.From) { return -1; }
            if (this.From > Other.From) { return 1; }
            if (this.To < Other.To) { return -1; }
            if (this.To > Other.To) { return 1; }
            return this.Value.CompareTo(Other.Value);
        }

        public override EdgeBase Duplicate() { return new DirectedEdge<T>(this.PointA, this.PointB, this.Value); }

        public void Orient() {
            if (this.PointA > this.PointB) { this.Flip(); }
        }

        public override string ToString() {
            return "DirectedEdge (" + PointA.ToString() + "-" + PointB.ToString() + "," + this.Value.ToString() + ")";
        }

    }




}


