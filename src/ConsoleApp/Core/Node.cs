namespace ConsoleApp.Core
{
    public class Node
    {
        private List<Node> Sources { get; set; }
        private List<Node> Targets { get; set; }
        public string Value { get; set; }

        public Node(string value)
        {
            Targets = new List<Node>();
            Sources = new List<Node>();
            Value = value;
        }

        /// <summary>
        /// Nesneye bağınlılık ekler.
        /// </summary>
        /// <param name="node">Bağımlı olunacak nesne.</param>
        public void AddTargets(Node node)
        {
            node.Sources.Add(this);
            Targets.Add(node);
        }

        /// <summary>
        /// Nesnenin bağımlı olduğu diğer nesneleri getirir.
        /// </summary>
        public IEnumerable<Node> GetTargets()
        {
            return GetGenealogy(false).Skip(1);
        }

        /// <summary>
        /// Nesneye bağımlı olan diğer nesneleri getirir.
        /// </summary>
        public IEnumerable<Node> GetSources()
        {
            return GetGenealogy().Skip(1);
        }

        /// <summary>
        /// Nesnenin bağlantılı olduğu diğer nesneleri getirir.
        /// </summary>
        /// <param name="isReverse">Yönü belirtir.</param>
        private IEnumerable<Node> GetGenealogy(bool isReverse = true)
        {
            var nodes = new Stack<Node>(new[] { this });
            while (nodes.Any())
            {
                Node node = nodes.Pop();
                yield return node;
                foreach (var n in (isReverse ? node.Sources : node.Targets)) nodes.Push(n);
            }
        }

        /// <summary>
        /// Nesnenin liste içerisinde bir döngüde yer alıp almadığını kontrol eder.
        /// </summary>
        /// <param name="nodes">Kontrol edilecek liste.</param>
        public bool IsCyclic(IEnumerable<Node> nodes)
        {
            return nodes.Any(x => x.Value == this.Value);
        }
    }
}
