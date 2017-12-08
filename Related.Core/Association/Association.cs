using Related.Abstract;
using Related.Edges;
using Related.Graphs;
using Related.Vertices;
using System.Collections.Generic;
using System.Diagnostics;

namespace Related.Association {

  
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
public struct AssociationPair {
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
    private int _a;
    private int _b;
    private double _v;

    public static AssociationPair Create(int A, int B, double Similarity) {
        return new AssociationPair(A, B, Similarity);
    }

    public AssociationPair(int A, int B, double Similarity) : this() {
        this.A = A;
        this.B = B;
        this.Similarity = Similarity;
    }

    public int A { get => _a; set => _a = value; }
    public int B { get => _b; set => _b = value; }
    public double Similarity { get => _v; set => _v = value; }

    public override string ToString() {
        return "Pair(" + A.ToString() + ", " + B.ToString() + ": " + Similarity.ToString("0.000") + ")";
    }

    public static bool operator ==(AssociationPair First, AssociationPair Second) {
        bool ca = First.A == Second.A;
        bool cb = First.B == Second.B;
        bool cv = First.Similarity == Second.Similarity;

        return ca & cb & cv;
    }

    public static bool operator !=(AssociationPair First, AssociationPair Second) { return !(First == Second); }

}

public class AssociationGraph : UndirectedGraphBase {

    private GraphVertexList<AssociationPair> _vertices = null;
    private GraphEdgeList<UndirectedEdge> _edges = null;

    public GraphEdgeList<UndirectedEdge> Edges { get => _edges; set => _edges = value; }
    public GraphVertexList<AssociationPair> Vertices { get => _vertices; set => _vertices = value; }

    public override int VertexCount => _vertices.Count;

    public AssociationGraph() {
        Edges = new GraphEdgeList<UndirectedEdge>(this);
        Vertices = new GraphVertexList<AssociationPair>(this);
    }

    public override List<int>[] GetAdjacencyMatrix() {
        List<int>[] adj = new List<int>[this.VertexCount];

        int cnt = 0;
        foreach (AssociationPair item in this.Vertices) {
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
}

/// <summary>
/// Implemenation of "Heuristics for Chemical Compound Matching", Hattori et al., 2003.
/// </summary>
/// <typeparam name="T"></typeparam>
public class GraphComparison<T> where T : struct {

    private GraphBase GA = null;
    private GraphBase GB = null;

    private AssociationGraph AG = null;

    private HashSet<AssociationPair>[] Assocs = null;
    private double[] Scores = null;

    public GraphComparison(UndirectedGraph<T> A, UndirectedGraph<T> B, SimilarityMetric Metric) {

        GA = A;
        GB = B;
        AG = new AssociationGraph();

        for (int i = 0; i < A.VertexCount; i++) {
            for (int j = 0; j < B.VertexCount; j++) {
                double val = Metric(A.Vertices[i], B.Vertices[j]);

                if (val > 0) {
                    AG.Vertices.Add(AssociationPair.Create(i, j, val));
                }
            }
        }

        List<int>[] adjA = A.GetAdjacencyMatrix();
        List<int>[] adjB = B.GetAdjacencyMatrix();

        for (int i = 0; i < AG.VertexCount; i++) {
            AssociationPair thisitem = AG.Vertices[i];

            HashSet<int> nAthis = new HashSet<int>(adjA[thisitem.A]);
            HashSet<int> nBthis = new HashSet<int>(adjB[thisitem.B]);

            for (int j = 0; j < AG.VertexCount; j++) {
                if (i == j) { continue; }
                AssociationPair thatitem = AG.Vertices[j];

                if (nAthis.Contains(thatitem.A) & nBthis.Contains(thatitem.B)) {
                    AG.Edges.Add(UndirectedEdge.Create(i, j));
                }

                if (!(nAthis.Contains(thatitem.A) | nBthis.Contains(thatitem.B))) {
                    AG.Edges.Add(UndirectedEdge.Create(i, j));
                }

            }
        }

    }

    public void ComputeAssociations(int MaxCount) {
        List<int>[] Matrix = AG.GetAdjacencyMatrix();
        HashSet<HashSet<int>> cli = UndirectedGraphBase.FindMaximalCliques(Matrix, MaxCount);

        Assocs = new HashSet<AssociationPair>[cli.Count];
        Scores = new double[cli.Count];

        int i = 0;

        foreach (HashSet<int> item in cli) {
            double countA = GA.VertexCount;
            double countB = GB.VertexCount;
            double common = item.Count;
            double score = (common / (countA + countB - common));
            HashSet<AssociationPair> thisassoc = new HashSet<AssociationPair>();

            foreach (int index in item) {
                AssociationPair thispair = this.AG.Vertices[index];
                thisassoc.Add(thispair);
            }

            Assocs[i] = thisassoc;
            Scores[i] = score;

            i += 1;
        }

        System.Array.Sort(Scores, Assocs);
        System.Array.Reverse(Scores);
        System.Array.Reverse(Assocs);

    }

    public HashSet<AssociationPair>[] GetAssociations() {
        return Assocs;
    }

    public double[] GetScores() {
        return Scores;
    }



    public delegate double SimilarityMetric(T valueA, T valueB);

}

//  static class Test {

//    private static void Run() {

//        UndirectedGraph<System.Drawing.Color> na = new UndirectedGraph<System.Drawing.Color>();
//        UndirectedGraph<System.Drawing.Color> nb = new UndirectedGraph<System.Drawing.Color>();

//        na.Vertices.Add(System.Drawing.Color.Blue);
//        na.Vertices.Add(System.Drawing.Color.Blue);
//        na.Vertices.Add(System.Drawing.Color.Red);
//        na.Vertices.Add(System.Drawing.Color.Red);
//        na.Vertices.Add(System.Drawing.Color.Red);

//        nb.Vertices.Add(System.Drawing.Color.Blue);
//        nb.Vertices.Add(System.Drawing.Color.Blue);
//        nb.Vertices.Add(System.Drawing.Color.Red);
//        nb.Vertices.Add(System.Drawing.Color.Yellow);
//        nb.Vertices.Add(System.Drawing.Color.Red);

//        na.Edges.Add(UndirectedEdge.Create(0, 1));
//        na.Edges.Add(UndirectedEdge.Create(1, 2));
//        na.Edges.Add(UndirectedEdge.Create(2, 3));
//        na.Edges.Add(UndirectedEdge.Create(3, 4));
//        na.Edges.Add(UndirectedEdge.Create(4, 2));

//        nb.Edges.Add(UndirectedEdge.Create(0, 1));
//        nb.Edges.Add(UndirectedEdge.Create(1, 2));
//        nb.Edges.Add(UndirectedEdge.Create(2, 3));
//        nb.Edges.Add(UndirectedEdge.Create(3, 4));
//        nb.Edges.Add(UndirectedEdge.Create(4, 2));


//        GraphComparison<System.Drawing.Color> nc = new GraphComparison<System.Drawing.Color>(na, nb, ColorDistance);

//        nc.ComputeAssociations(3);
//        HashSet<AssociationPair>[] assoc = nc.GetAssociations();
//        double[] score = nc.GetScores();

//        Debug.WriteLine("here");
//    }

//    private static double ColorDistance(System.Drawing.Color A, System.Drawing.Color B) {
//        if (A == B) {
//            return 1;
//        }

//        return 0;
//    }


//}

}
