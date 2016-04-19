using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FSCruiser.Core.Models;
using System.Windows.Forms;
using CruiseDAL.DataObjects;

namespace FSCruiser.WinForms
{
    public static class ILogFieldProviderExtentions
    {
        public static List<DataGridViewColumn> MakeLogColumns(this ILogFieldProvider provider)
        {
            var fields = provider.ReadLogFields();

            List<DataGridViewColumn> columns = new List<DataGridViewColumn>();

            foreach (LogFieldSetupDO field in fields)
            {
                var col = MakeColumn(field.ColumnType);
                col.DataPropertyName = field.Field;
                col.HeaderText = field.Heading;

                if (!string.IsNullOrEmpty(field.Format))                                // AND field has format 
                {
                    col.DefaultCellStyle = new DataGridViewCellStyle()
                    {
                        Format = field.Format
                    };
                }

                columns.Add(col);
            }


            return columns;
        }

        private static DataGridViewColumn MakeColumn(string columnType)
        {
            switch (columnType)
            {
                case "Combo":
                    {
                        return new DataGridViewComboBoxColumn() { FlatStyle = FlatStyle.Flat };
                    }
                case "Button":
                    {
                        return new DataGridViewButtonColumn();
                    }
                case "Check":
                    {
                        return new DataGridViewCheckBoxColumn();
                    }
                case "Text":
                case "DateTime":
                case "UpDown":
                default:
                    {
                        return new DataGridViewTextBoxColumn();
                    }
            }//end switch
        }//end method
    }
}
