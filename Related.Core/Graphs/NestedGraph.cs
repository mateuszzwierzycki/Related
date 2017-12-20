using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Related.Graphs {
    /// <summary>
    ///  A nested class representing directed graphs. Each node stores connectivity information on parents and children.
    /// </summary>
    [Serializable]
    public class NodeGraph {

        int _id = 0;
        double _value = 0;
        List<NodeGraph> _parent = new List<NodeGraph>();
        List<NodeGraph> _children = new List<NodeGraph>();

        public int ID { get => _id; set => _id = value; }
        public double Value { get => _value; set => _value = value; }
        public List<NodeGraph> Parents { get => _parent; set => _parent = value; }
        public List<NodeGraph> Children { get => _children; set => _children = value; }

        public NodeGraph() { }

        public NodeGraph(int ID = 0, double Value = 0) {
            this.ID = this.ID;
            this.Value = Value;
        }

        /// <summary>
        /// If the node doesn't store any parent, it is considered a root node.
        /// </summary>
        /// <returns></returns>
        public bool IsRoot() { return (Parents.Count == 0); }

        /// <summary>
        /// If the node doesn't store any child, it is considered a leaf node.
        /// </summary>
        /// <returns></returns>
        public bool IsLeaf() { return (Children.Count == 0); }

        public List<NodeGraph> GetLeafs() {
            List<NodeGraph> nl = new List<NodeGraph>();
            GetLeafNodes(ref nl);
            return nl;
        }

        private void GetLeafNodes(ref List<NodeGraph> Nodes) {
            if (this.IsLeaf()) {
                Nodes.Add(this);
            }
            else {
                for (int i = 0; i < this.Children.Count; i++) {
                    this.Children[i].GetLeafNodes(ref Nodes);
                }
            }
        }

        public static void GetAllNodes(ref NodeGraph Node, ref SortedList<int, NodeGraph> SortedByID) {
            SortedByID[Node.ID] = Node;

            for (int i = 0; i < Node.Children.Count; i++) {
                NodeGraph kid = Node.Children[i];
                GetAllNodes(ref kid, ref SortedByID);
            }
        }

        public void AddChild(NodeGraph Child) {
            if (Child != this) {
                this.Children.Add(Child);
                Child.Parents.Add(this);
            }
        }

        public void AddParent(NodeGraph Parent) {
            if (Parent != this) {
                Parents.Add(Parent);
                Parent.Children.Add(this);
            }
        }

        public NodeGraph FindNode(int ID) {
            return FindNode(this, ID);
        }

        private NodeGraph FindNode(NodeGraph who, int ID) {
            if (who.ID == ID) { return who; }

            foreach (NodeGraph item in Children) {
                NodeGraph res = FindNode(item, ID);
                if (res != null) { return res; }
            }

            return null;
        }

        public override string ToString() {
            return "<ID:" + this.ID.ToString() + ", Value:" + this.Value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + ">";
        }

        public string ToSuperString() {
            return this.ToSuperString(0);
        }

        private string ToSuperString(int level) {
            string str = string.Empty;
            str += new string('.', level * 3) + this.ToString();

            for (int i = 0; i < this.Children.Count; i++) {
                str += "\n" + this.Children[i].ToSuperString(level + 1);
            }

            return str;
        }

        public static void FlatTree(NodeGraph G, SortedList<int, NodeGraph> Srt) {
            Srt[G.ID] = G;
            foreach (NodeGraph n in G.Children) { FlatTree(n, Srt); }
        }
         
    }
}
