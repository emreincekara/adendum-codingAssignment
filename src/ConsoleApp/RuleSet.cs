using ConsoleApp.Core;

namespace ConsoleApp
{
    public class RuleSet
    {
        public List<Node> Nodes { get; set; }
        public List<KeyValuePair<Node, Node>> Conflicts { get; set; }

        public RuleSet()
        {
            Nodes = new List<Node>();
            Conflicts = new();
        }

        /// <summary>
        /// Listeye yeni bağımlılık ekler.
        /// </summary>
        /// <param name="value1">Eklenecek nesne.</param>
        /// <param name="value2">Bağımlı olduğu nesne.</param>
        public void AddDep(string value1, string value2)
        {
            var node = AddNode(value1, Nodes);
            var target = AddNode(value2, Nodes);
            node.AddTargets(target);
        }

        /// <summary>
        /// Listeye yeni çakışma ekler.
        /// </summary>
        /// <param name="value1">Çakışma olan nesne.</param>
        /// <param name="value2">Çakışma olan nesne.</param>
        public void AddConflict(string value1, string value2)
        {
            var conflict1 = AddNode(value1, Nodes);
            var conflict2 = AddNode(value2, Nodes);

            Conflicts.Add(new KeyValuePair<Node, Node>(conflict1, conflict2));
            Conflicts.Add(new KeyValuePair<Node, Node>(conflict2, conflict1));
        }

        /// <summary>
        /// Listenin tutarlı olup olmadığını kontrol eder.
        /// </summary>
        public bool IsCoherent()
        {
            foreach (var conflict in Conflicts)
            {
                if (CheckConflict(conflict.Key, conflict.Value))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// İki nesne arasındaki çakışmayı kontrol eder.
        /// </summary>
        /// <param name="node">Kontrol edilecek nesne.</param>
        /// <param name="conflict">Karşılaştırılan nesne.</param>
        private bool CheckConflict(Node node, Node conflict)
        {
            if (!node.IsCyclic(node.GetSources()))
            {
                var sources = node.GetSources();
                foreach (var source in sources)
                {
                    if (source.Value == conflict.Value)
                        return true;
                    var deps = source.GetTargets();
                    if (deps.Any(x => x.Value == source.Value))
                        return false;
                    if (deps.Any(x => x.Value == conflict.Value))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Listeye yeni nesne ekler.
        /// </summary>
        /// <param name="value">Eklenecek nesne.</param>
        /// <param name="nodes">Nesnenin ekleneceği liste.</param>
        private Node AddNode(string value, List<Node> nodes)
        {
            var node = nodes.FirstOrDefault(x => x.Value == value);
            if (node != null)
                return node;

            node = new Node(value);
            nodes.Add(node);
            return node;
        }

        /// <summary>
        /// Yeni bir nesne oluşturur.
        /// </summary>
        public static RuleSet NewRuleSet()
        {
            return new RuleSet();
        }
    }
}
