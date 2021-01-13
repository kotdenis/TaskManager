using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfWorkManager.Infrastrucure
{

    public class EmployeeTreeNode<T>
    {
        private T value;
        private bool hasParent;
        private List<EmployeeTreeNode<T>> children;

        public EmployeeTreeNode(T value)
        {
            if (value != null)
                this.value = value;
            children = new List<EmployeeTreeNode<T>>();
        }

        public int ChildrenCount => children.Count;

        public T Value { get => value; set => this.value = value; }

        public void AddChild(EmployeeTreeNode<T> child)
        {
            if (child == null && child.hasParent)
                return;
            child.hasParent = true;
            this.children.Add(child);
        }

        public EmployeeTreeNode<T> GetChild(int index)
        {
            return this.children[index];
        }
    }

    public class EmployeeTree<T>
    {
        EmployeeTreeNode<T> root;

        public EmployeeTree(T value)
        {
            root = new EmployeeTreeNode<T>(value);
        }

        public EmployeeTree(T value, params EmployeeTree<T>[] children) : this(value)
        {
            foreach (var t in children)
                root.AddChild(t.root);
        }

        public EmployeeTreeNode<T> Root => root;

        EmployeeTreeNode<T> child = null;
        public void GetDFS(EmployeeTreeNode<T> root)
        {
            for (int i = 0; i < Root.ChildrenCount; i++)
            {
                child = Root.GetChild(i);
                GetDFS(child);
            }
        }
    }
}
