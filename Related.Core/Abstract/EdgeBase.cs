using System;

namespace Related.Edges {
    [Serializable]
    public abstract class EdgeBase {

        #region "Properties" 

        public int PointA { get; set; }
        public int PointB { get; set; }

        #endregion 

        #region "Constructors" 

        public EdgeBase() { }

        public EdgeBase(int A, int B) {
            this.PointA = A;
            this.PointB = B;
        }

        public EdgeBase(EdgeBase Other) {
            PointA = Other.PointA;
            PointB = Other.PointB;
        }

        /// <summary>
        /// Function returning a deep copy.
        /// </summary>
        public abstract EdgeBase Duplicate();

        #endregion 

        /// <summary>
        /// Returns -1 if there is no ThisVertex.
        /// </summary>
        /// <param name="ThisVertex"></param>
        /// <returns></returns>
        public int OtherVertex(int ThisVertex) {
            if (ThisVertex == PointA) {
                return PointB;
            }
            else if (ThisVertex == PointB) {
                return PointA;
            }
            return -1;
        }

        public void Flip() {
            int temp = PointA;
            PointA = PointB;
            PointB = temp;
        }

        public bool IsAscending() {
            return PointA < PointB;
        }

        public bool IsValid() {
            if (PointA == -1) return false;
            if (PointB == -1) return false;
            return true;
        }

        public bool IsCycle() {
            if (IsValid() & PointA == PointB) { return true; }
            return false;
        }

        public int Minimum() { return Math.Min(PointA, PointB); }
        public int Maximum() { return Math.Max(PointA, PointB); }

        /// <summary>
        /// Call whenever there is any vertex removed in a graph.
        /// </summary>
        /// <param name="Vertex"></param>
        public void OnVertexRemove(int Vertex) {
            if (PointA == Vertex) { PointA = -1; }
            if (PointB == Vertex) { PointB = -1; }
            if (PointA > Vertex) { PointA -= 1; }
            if (PointB > Vertex) { PointB -= 1; }
        }

        public override string ToString() {
            return "Edge (" + PointA.ToString() + "-" + PointB.ToString() + ")";
        }

    }
}
