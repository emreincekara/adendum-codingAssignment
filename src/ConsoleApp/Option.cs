using ConsoleApp.Core;

namespace ConsoleApp
{
    public class Option
    {
        private List<Node> SelectedNodes { get; set; }
        private RuleSet RuleSet { get; set; }

        public Option(RuleSet ruleSet)
        {
            SelectedNodes = new List<Node>();
            RuleSet = ruleSet;
        }

        /// <summary>
        /// Seçili nesneleri getirir.
        /// </summary>
        public List<string> GetSelections()
        {
            return SelectedNodes.Select(x => x.Value).ToList();
        }

        /// <summary>
        /// Seçili nesneleri getirir.
        /// </summary>
        public string StringSlice()
        {
            return $"({string.Join(", ", GetSelections())})";
        }

        /// <summary>
        /// Listede yer alan nesne seçili değil ise seçer eğer seçili ise seçimini kaldırır.
        /// </summary>
        /// <param name="value">Üzerinde işlem yapılacak olan nesne.</param>
        public void Toggle(string value)
        {
            var node = RuleSet.Nodes.Find(x => x.Value == value);
            if (node != null)
            {
                if (!SelectedNodes.Any(x => x.Value == value))
                {
                    SelectedNodes.Add(node);
                    foreach (var item in node.GetTargets())
                    {
                        if (item.Value == node.Value)
                            break;
                        SelectedNodes.Add(item);
                        var itemConflicts = RuleSet.Conflicts.Where(x => x.Key.Value == item.Value);
                        foreach (var conflict in itemConflicts)
                            UnSelect(conflict.Value);
                    }
                    var nodeConflicts = RuleSet.Conflicts.Where(x => x.Key.Value == node.Value);
                    foreach (var conflict in nodeConflicts)
                        UnSelect(conflict.Value);
                }
                else
                    UnSelect(node);
            }
        }

        /// <summary>
        /// Listede seçili olan nesnenin seçimini kaldırır.
        /// </summary>
        /// <param name="node">Seçimi kaldırılacak olan nesne.</param>
        private void UnSelect(Node node)
        {
            SelectedNodes.Remove(node);
            foreach (var item in node.GetSources())
            {
                if (item.Value == node.Value)
                    break;
                if (SelectedNodes.Any(x => x.Value == item.Value))
                    SelectedNodes.Remove(item);
            }
        }

        /// <summary>
        /// Yeni bir nesne oluşturur.
        /// </summary>
        public static Option New(RuleSet rs)
        {
            return new Option(rs);
        }
    }
}
