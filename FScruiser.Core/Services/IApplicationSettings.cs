using System;
namespace FSCruiser.Core
{
    public interface IApplicationSettings
    {
        System.Collections.Generic.List<FSCruiser.Core.Models.RecentProject> RecentProjects { get; set; }
        
        void AddRecentProject(FSCruiser.Core.Models.RecentProject project);
        void ClearRecentProjects();
        
        string BackupDir { get; set; }
        BackUpMethod BackUpMethod { get; set; }
        bool BackUpToCurrentDir { get; set; }
                
        event EventHandler CruisersChanged;
        float DataGridFontSize { get; set; }
        
        bool EnablePageChangeSound { get; set; }
        bool EnableTallySound { get; set; }
        
        System.Collections.Generic.List<FSCruiser.Core.Models.Cruiser> Cruisers { get; set; }

        void AddCruiser(string initials);
        void RemoveCruiser(FSCruiser.Core.Models.Cruiser cruiser);
        void NotifyCruisersChanged();

        bool EnableCruiserPopup { get; set; }
    }
}
