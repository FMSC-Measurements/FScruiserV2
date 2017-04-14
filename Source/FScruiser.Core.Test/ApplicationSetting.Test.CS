using FluentAssertions;
using FSCruiser.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FScruiser.Core.Test
{
    public class ApplicationSettingTest
    {
        string ADD_PLOT_DEFAULT = "Add";
        string ADD_TREE_DEFAULT = "F3";
        string JUMP_TREE_TALLY_DEFAULT = "Escape";

        [Fact]
        public void TestHotKeyDefaults()
        {
            var settings = new ApplicationSettings();

            settings.AddPlotKeyStr.Should().BeEquivalentTo(ADD_PLOT_DEFAULT);
            settings.AddTreeKeyStr.Should().BeEquivalentTo(ADD_TREE_DEFAULT);
            settings.JumpTreeTallyKeyStr.Should().BeEquivalentTo(JUMP_TREE_TALLY_DEFAULT);
            settings.UntallyKeyStr.Should().BeNullOrEmpty();
        }

        [Fact]
        public void TestHotKeyAssign()
        {
            var settings = new ApplicationSettings();

            settings.AddPlotKeyStr.Should().BeEquivalentTo(ADD_PLOT_DEFAULT);
            settings.AddTreeKeyStr.Should().BeEquivalentTo(ADD_TREE_DEFAULT);

            settings.AddPlotKeyStr = ADD_TREE_DEFAULT;
            settings.AddPlotKeyStr.Should().BeEquivalentTo(ADD_TREE_DEFAULT);
            settings.AddTreeKeyStr.Should().BeNullOrEmpty();

            settings.AddTreeKeyStr = ADD_PLOT_DEFAULT;
            settings.AddTreeKeyStr.Should().BeEquivalentTo(ADD_PLOT_DEFAULT);
            settings.AddPlotKeyStr.Should().BeEquivalentTo(ADD_TREE_DEFAULT);
        }
    }
}