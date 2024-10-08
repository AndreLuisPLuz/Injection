using System.Reflection;

namespace Library.Core
{
    class InjectionNode
    {
        private List<InjectionNode> _parents = new();
        private List<InjectionNode> _children = new();

        public Injection Injection;

        public InjectionNode(Injection injection)
        {
            Injection = injection;
        }

        public Stack<InjectionNode> GetAncestors()
        {
            Stack<InjectionNode> ancestors = new();

            if (_parents.Count == 0)
            {
                ancestors.Push(this);
                return new Stack<InjectionNode>();
            }

            foreach (var parent in _parents)
            {
                var parentAncestors = parent.GetAncestors();
                while (parentAncestors.Any())
                    ancestors.Push(parentAncestors.Pop());
                
                ancestors.Push(parent);
            }

            return ancestors;
        }

        public void AssignChild(InjectionNode newChild)
        {
            _children.Add(newChild);
            newChild._parents.Add(this);
        }
    }

    class InjectionTree
    {
        public InjectionNode Root { get; private set; }
        public HashSet<InjectionNode> Leaves { get; private set; } = new();

        public InjectionTree(Injection firstInjection)
        {
            Root = new InjectionNode(firstInjection);
            Leaves.Add(Root);
        }

        public InjectionNode NewLeaf(Injection leaf, InjectionNode parent)
        {
            var newNode = new InjectionNode(leaf);

            parent.AssignChild(newNode);
            Leaves.Add(newNode);

            if (Leaves.Contains(parent))
                Leaves.Remove(parent);

            return newNode;
        }

        public Stack<InjectionNode> GetBranchToRoot(Injection leaf)
        {
            var node = Leaves.Where(l => l.Injection.Type == leaf.Type)
                    .FirstOrDefault()
                ?? throw new InvalidOperationException("Node given is not a leaf.");
            
            return node.GetAncestors();
        }
    }
}