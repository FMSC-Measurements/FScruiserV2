using System;

using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FSCruiserV2.Test
{
    public static class FormsExtentions
    {
        public static Control Find(this System.Windows.Forms.Control.ControlCollection ctrls, string name)
        {
            for (int i = 0; i < ctrls.Count; i++)
            {
                var ctrl = ctrls[i];
                if (ctrl != null && ctrl.Name == name)
                {
                    return ctrl;
                }
                if (ctrl != null && ctrl.Controls.Count > 0)
                {
                    ctrl = ctrl.Controls.Find(name);
                    if (ctrl != null)
                    { return ctrl; }
                }
            }
            return null;
        }

        public static T Find<T>(this System.Windows.Forms.Control.ControlCollection ctrls, string name) where T : Control
        {
            for (int i = 0; i < ctrls.Count; i++)
            {
                var ctrl = ctrls[i];
                if (ctrl != null && ctrl is T && ctrl.Name == name)
                {
                    return ctrl as T;
                }
                if (ctrl != null && ctrl.Controls.Count > 0)
                {
                    ctrl = ctrl.Controls.Find<T>(name);
                    if (ctrl != null)
                    { return ctrl as T; }
                }
            }
            return null;
        }
    }
}