﻿using System.Collections.Generic;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms
{
    public static class ILogFieldProviderExtentions
    {
        public static List<DataGridViewColumn> MakeLogColumns(this ILogFieldProvider provider)
        {
            List<DataGridViewColumn> columns = new List<DataGridViewColumn>();

            foreach (LogFieldSetupDO field in provider.LogFields)
            {
                var col = MakeColumn(field.ColumnType);
                col.DataPropertyName = field.Field;
                col.HeaderText = field.Heading;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

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