using System;
using System.Collections.Generic;
using System.Text;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms
{
    interface ITallyButton
    {
        event EventHandler TallyButtonClicked;

        event EventHandler SettingsButtonClicked;

        CountTree Count { get; set; }
    }
}