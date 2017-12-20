using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Related.Edges {

    /// <summary>
    /// Undirected Edge
    /// </summary>
    [Serializable]
    public class UEdge : EdgeBase, IComparable<UEdge> {

        public UEdge() : base() { }
        public UEdge(int A, int B) : base(A, B) { }
        public UEdge(EdgeBase Other) : base(Other) { }

        public int CompareTo(UEdge Other) {
            if (this.Minimum() < Other.Minimum()) { return -1; }
            if (this.Minimum() > Other.Minimum()) { return 1; }
            if (this.Maximum() < Other.Maximum()) { return -1; }
            if (this.Maximum() > Other.Maximum()) { return 1; }
            return 0;
        }

        public static UEdge Create(int A, int B) { return new UEdge(A, B); }

        //public override EdgeBase Duplicate() { return new UEdge(this); }

        public void Orient() {
            if (this.PointA > this.PointB) { this.Flip(); }
        }

        public override string ToString() {
            return "Undirected" + base.ToString();
        }

    }

    /// <summary>
    /// Undirected Edge which can store a value. The value does not take part in CompareTo. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class UEdge<T> : EdgeBase, IComparable<UEdge<T>>{

        T myval = default(T);

        public T Value {
            get { return myval; }
            set { myval = value; }
        }

        public UEdge(int A, int B, T Value) : base(A, B) { this.Value = Value; }

        public static UEdge<T> Create(int A, int B, T Value) { return new UEdge<T>(A, B, Value); }

            public int CompareTo(UEdge<T> Other) {
            if (this.Minimum() < Other.Minimum()) { return -1; }
            if (this.Minimum() > Other.Minimum()) { return 1; }
            if (this.Maximum() < Other.Maximum()) { return -1; }
            if (this.Maximum() > Other.Maximum()) { return 1; }
            return 0;
        }

        //public override EdgeBase Duplicate() { return new UEdge<T>(this.PointA, this.PointB, this.Value); }

        public void Orient() {
            if (this.PointA > this.PointB) { this.Flip(); }
        }

        public override string ToString() {
            return "UndirectedEdge (" + PointA.ToString() + "-" + PointB.ToString() + "," + this.Value.ToString() + ")";
        }

    }

    /// <summary>
    /// Directed Edge
    /// </summary>
    [Serializable]
    public class DEdge : EdgeBase, IComparable<DEdge> {

        public DEdge() : base() { }
        public DEdge(int From, int To) : base(From, To) { }
        public DEdge(EdgeBase Other) : base(Other) { }

        public static DEdge Create(int From, int To) { return new DEdge(From, To); }

        public int From { get => PointA; set => PointA = value; }
        public int To { get => PointB; set => PointB = value; }

        public int CompareTo(DEdge Other) {
            if (this.From < Other.From) { return -1; }
            if (this.From > Other.From) { return 1; }
            if (this.To < Other.To) { return -1; }
            if (this.To > Other.To) { return 1; }
            return 0;
        }

        //public override EdgeBase Duplicate() { return new DEdge(this); }

        public void Orient() {
            if (this.PointA > this.PointB) { this.Flip(); }
        }

        public override string ToString() {
            return "Directed" + base.ToString();
        }

    }

    /// <summary>
    /// Directed Edge which can store a value. The value does not take part in CompareTo. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class DEdge<T> : EdgeBase, IComparable<DEdge<T>> {

        T myval = default(T);

        public T Value {
            get { return myval; }
            set { myval = value; }
        }

        public int From { get => PointA; set => PointA = value; }
        public int To { get => PointB; set => PointB = value; }

        public DEdge(int From, int To, T Value) : base(From, To) { this.Value = Value; }

        public static DEdge<T> Create(int From, int To, T Value) { return new DEdge<T>(From, To,Value); }

        public int CompareTo(DEdge<T> Other) {
            if (this.From < Other.From) { return -1; }
            if (this.From > Other.From) { return 1; }
            if (this.To < Other.To) { return -1; }
            if (this.To > Other.To) { return 1; }
            return 0;
        }

        //public override EdgeBase Duplicate() { return new DEdge<T>(this.PointA, this.PointB, this.Value); }

        public void Orient() {
            if (this.PointA > this.PointB) { this.Flip(); }
        }

        public override string ToString() {
            return "DirectedEdge (" + PointA.ToString() + "-" + PointB.ToString() + "," + this.Value.ToString() + ")";
        }

    }




}


