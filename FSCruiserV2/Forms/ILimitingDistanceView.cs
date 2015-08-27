using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSCruiserV2.Forms
{
    public interface ILimitingDistanceView
    {
        string DBH { get; set; }
        string BAForFPSize { get; set; }
        string BAFofFPSlabel { get; set; }
        string LimitingDistance { set; }
        string SlopePCT { get; set; }
        bool IsToFace { get; set; }
        string SlopeDistance { get; set; }

        void UpdateInputValid(bool inputValid);
        void UpdateIsTreeIn(int state);
        void Close();
    }
}
