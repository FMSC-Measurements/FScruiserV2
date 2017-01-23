using System;
using System.Windows.Forms;
using FSCruiser.Core;

namespace FSCruiser.WinForms
{
    public class ExceptionHandler : IExceptionHandler
    {
        public bool Handel(Exception e)
        {
            if (e is UserFacingException)
            {
                MessageBox.Show(e.Message);
                return true;
            }
            else if (e is FMSC.ORM.ConstraintException)
            {
                var ex = (FMSC.ORM.ConstraintException)e;
                if (e is FMSC.ORM.UniqueConstraintException)
                {
                    MessageBox.Show("Record Already Exists");
                }
                else
                {
                    MessageBox.Show("Value Check Failed:" + ex.FieldName);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}