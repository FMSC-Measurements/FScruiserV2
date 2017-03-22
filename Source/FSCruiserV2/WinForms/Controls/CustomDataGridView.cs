using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FSCruiser.WinForms.Controls
{
    public class CustomDataGridView : DataGridView
    {
        public bool MoveFirstEmptyCell()
        {
            var row = CurrentRow;
            if (row == null) { return false; }
            try
            {
                var cells = row.Cells.OfType<DataGridViewCell>()
                    .Where(c => !c.ReadOnly && c.Visible && !(c is DataGridViewButtonCell));
                foreach (var cell in cells)
                {
                    var cellValue = cell.Value;
                    var cellType = cell.ValueType;
                    if (cellValue == null
                        || (cellValue is String && string.IsNullOrEmpty(cellValue as string)) //is empty string
                        || (cellType.IsValueType && cellValue.Equals(Activator.CreateInstance(cellType)))) //is the default value of a value type
                    {
                        CurrentCell = cell;
                        return true;
                    }
                }

                return false;
            }
            catch
            { return false; }
        }
    }
}