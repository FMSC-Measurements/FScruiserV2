using System;
using FSCruiser.Core.Models;
using FSCruiser.Core.ViewInterfaces;
using FScruiser.Core.ViewModels;
using System.Collections.Generic;

namespace FSCruiser.Core
{
    public class FormCruiserSelectionLogic : ViewModelBase
    {
        public ICruiserSelectionView View { get; protected set; }

        ApplicationSettings Settings { get; set; }

        List<KeyValuePair<string, Cruiser>> _cruisers;

        public IEnumerable<KeyValuePair<string, Cruiser>> Cruisers
        {
            get { return _cruisers; }
        }

        Tree _tree;
        public Tree Tree 
        {
            get { return _tree; }
            set { SetValue(ref _tree, value, "Tree"); }
        }

        public FormCruiserSelectionLogic(ApplicationSettings settings, ICruiserSelectionView view)
        {
            View = view;
            Settings = settings;
            Settings.CruisersChanged += new EventHandler(Settings_CruisersChanged);
            Settings_CruisersChanged(null, null);
        }

        void Settings_CruisersChanged(object sender, EventArgs e)
        {
            _cruisers = new List<KeyValuePair<string, Cruiser>>();
            int i = 1;
            foreach (var c in Settings.Cruisers)
            {
                var key = (i < 10) ? i.ToString() : null;
                _cruisers.Add(new KeyValuePair<string,Cruiser>(key, c));
                i++;
            }

            NotifyPropertyChanged("Cruisers");
        }



        public void HandleKeyDown(string key)
        {
            int value = Convert.ToInt32(key.ToString());
            foreach (var c in Cruisers)
            {
                if (c.Key == key)
                {
                    this.HandleCruiserSelected(c.Value);
                    break;
                }
            }
        }

        public void HandleCruiserSelected(Cruiser cruiser)
        {
            this.Tree.Initials = cruiser.Initials;
            this.View.Close();
        }
    }
}