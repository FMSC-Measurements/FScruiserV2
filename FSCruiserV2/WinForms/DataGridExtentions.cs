using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
    public static class DataGridExtentions
    {
        public static void GoToFirstColumn(this DataGridView dgv)
        {
            if (dgv.CurrentCellAddress.Y == -1) { return; }
            try
            {
                dgv.CurrentCell = dgv[0, dgv.CurrentCellAddress.Y];
            }
            catch
            { }
        }

        public static void GoToLastRow(this DataGridView dgv)
        {
            var rows = dgv.Rows.Count;
            if (rows <= 0) { return; }
            dgv.CurrentCell = dgv[dgv.CurrentCellAddress.X, rows - 1];
        }
    }
}
