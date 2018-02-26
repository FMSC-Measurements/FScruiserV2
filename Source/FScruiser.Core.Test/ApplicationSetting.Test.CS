using FluentAssertions;
using FSCruiser.Core;
using FSCruiser.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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

        [Fact]
        public void SerializeDeserialize_Test()
        {
            var recientProjectsList = new List<RecentProject>();
            recientProjectsList.Add(new RecentProject());

            var cruisersList = new List<Cruiser>();
            cruisersList.Add(new Cruiser("A"));
            cruisersList.Add(new Cruiser("B"));

            var appSettings = new ApplicationSettings()
            {
                RecentProjects = recientProjectsList,
                Cruisers = cruisersList
            };
            


            var writer = new StringWriter();
            ApplicationSettings.Serialize(writer, appSettings);

            var xmlText = writer.ToString();

            //var xmlRoot = XElement.Parse(xmlText);
            //var recientProjects = xmlRoot.Element(nameof(ApplicationSettings.RecentProjects));
            //recientProjects.Should().NotBeNull();
            //recientProjects.Elements().Should().HaveCount(recientProjectsList.Count());

            //var cruiserElement = xmlRoot.Element(nameof(ApplicationSettings.Cruisers));
            //cruiserElement.Should().NotBeNull();
            //cruiserElement.Elements().Should().HaveCount(cruisersList.Count());


            var appSettingsResult = ApplicationSettings.Deserialize(new StringReader(xmlText));
            appSettings.Cruisers.Should().BeSameAs(cruisersList);
            appSettings.RecentProjects.Should().BeSameAs(recientProjectsList);
        }
    }
}