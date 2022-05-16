using ConsoleApp;

var rs = RuleSet.NewRuleSet();

rs.AddDep("a", "b");
rs.AddDep("a", "c");
rs.AddConflict("b", "d");
rs.AddConflict("b", "e");

Console.WriteLine(rs.IsCoherent());