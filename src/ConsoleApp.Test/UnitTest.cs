using Xunit;

namespace ConsoleApp.Test
{
    public class UnitTest
    {
        [Fact]
        public void Test_depends_aa()
        {
            var rs = RuleSet.NewRuleSet();

            rs.AddDep("a", "a");

            Assert.True(rs.IsCoherent(), "rs.isCoherent failed");
        }

        [Fact]
        public void Test_depends_ab_ba()
        {
            var rs = RuleSet.NewRuleSet();

            rs.AddDep("a", "b");
            rs.AddDep("b", "a");

            Assert.True(rs.IsCoherent(), "rs.IsCoherent failed");
        }

        [Fact]
        public void Test_exclusive_ab()
        {
            var rs = RuleSet.NewRuleSet();

            rs.AddDep("a", "b");
            rs.AddConflict("a", "b");

            Assert.False(rs.IsCoherent(), "rs.IsCoherent failed");
        }

        [Fact]
        public void Test_exclusive_ab_bc()
        {
            var rs = RuleSet.NewRuleSet();

            rs.AddDep("a", "b");
            rs.AddDep("b", "c");
            rs.AddConflict("a", "c");

            Assert.False(rs.IsCoherent(), "rs.IsCoherent failed");
        }

        [Fact]
        public void Test_deep_deps()
        {
            var rs = RuleSet.NewRuleSet();

            rs.AddDep("a", "b");
            rs.AddDep("b", "c");
            rs.AddDep("c", "d");
            rs.AddDep("d", "e");
            rs.AddDep("a", "f");
            rs.AddConflict("e", "f");

            Assert.False(rs.IsCoherent(), "rs.IsCoherent failed");
        }

        [Fact]
        public void Test_exclusive_ab_bc_ca_de()
        {
            var rs = RuleSet.NewRuleSet();

            rs.AddDep("a", "b");
            rs.AddDep("b", "c");
            rs.AddDep("c", "a");
            rs.AddDep("d", "e");
            rs.AddConflict("c", "e");

            Assert.True(rs.IsCoherent(), "rs.isCoherent failed");

            var opts = Option.New(rs);

            opts.Toggle("a");
            Assert.Equal(new[] { "a", "b", "c" }, opts.GetSelections());

            rs.AddDep("f", "f");
            opts.Toggle("f");
            Assert.Equal(new[] { "a", "b", "c", "f" }, opts.GetSelections());

            opts.Toggle("e");
            Assert.Equal(new[] { "f", "e" }, opts.GetSelections());

            opts.Toggle("b");
            Assert.Equal(new[] { "f", "b", "c", "a" }, opts.GetSelections());

            rs.AddDep("b", "g");
            opts.Toggle("g");
            opts.Toggle("b");
            Assert.Equal(new[] { "f", "g" }, opts.GetSelections());
        }

        [Fact]
        public void Test_ab_bc_toggle()
        {
            var rs = RuleSet.NewRuleSet();

            rs.AddDep("a", "b");
            rs.AddDep("b", "c");
            var opts = Option.New(rs);
            opts.Toggle("c");

            Assert.Equal(new[] { "c" }, opts.GetSelections());
        }

        [Fact]
        public void Test_ab_ac()
        {
            var rs = RuleSet.NewRuleSet();

            rs.AddDep("a", "b");
            rs.AddDep("a", "c");
            rs.AddConflict("b", "d");
            rs.AddConflict("b", "e");

            Assert.True(rs.IsCoherent(), "rs.IsCoherent failed");

            var opts = new Option(rs);
            opts.Toggle("d");
            opts.Toggle("e");
            opts.Toggle("a");

            Assert.Equal(new[] { "a", "c", "b" }, opts.GetSelections());
        }
    }
}