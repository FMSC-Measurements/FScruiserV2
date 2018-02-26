using FSCruiser.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FSCruiser.Core
{
    public interface IApplicationSettings : INotifyPropertyChanged
    {
        event EventHandler CruisersChanged;

        List<RecentProject> RecentProjects { get; }

        bool EnableCruiserPopup { get; set; }

        bool EnableAskEnterTreeData { get; set; }

        void AddCruiser(string initials);

        void RemoveCruiser(Cruiser cruiser);

        void AddRecentProject(RecentProject project);

        void ClearRecentProjects();
    }
}