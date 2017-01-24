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
            return ctrls.Find<Control>(name);
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

        public static Control FindByDataMember(this System.Windows.Forms.Control.ControlCollection ctrls, string dm)
        {
            return FindByDataMember<Control>(ctrls, dm);
        }

        public static T FindByDataMember<T>(this System.Windows.Forms.Control.ControlCollection ctrls, string dm) where T : Control
        {
            for (int i = 0; i < ctrls.Count; i++)
            {
                var ctrl = ctrls[i];
                if (ctrl is T)
                {
                    for (int j = 0; j < ctrl.DataBindings.Count; j++)
                    {
                        var binding = ctrl.DataBindings[j];
                        if (binding.BindingMemberInfo.BindingField == dm)
                        {
                            return (T)ctrl;
                        }
                    }
                }
                ctrl = ctrl.Controls.FindByDataMember<T>(dm);
                if (ctrl != null) { return (T)ctrl; }
            }
            return null;
        }
    }
}