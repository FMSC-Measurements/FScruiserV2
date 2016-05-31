using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms
{
    public static class ITreeFieldProviderExtentions
    {
        public static IEnumerable<DataGridViewColumn> MakeTreeColumns(this ITreeFieldProvider provider)
        {
            List<TreeFieldSetupDO> fieldSetups = provider.ReadTreeFields();
            List<DataGridViewColumn> columns = new List<DataGridViewColumn>();

            foreach (var fieldSetup in fieldSetups)
            {
                DataGridViewColumn col = null;
                switch (fieldSetup.Field)
                {
                    case "Species":
                        {
                            col = new DataGridViewComboBoxColumn()
                            {
                                DataPropertyName = "TreeDefaultValue",
                                DisplayMember = "Species",
                                ValueMember = "Self",
                                FlatStyle = FlatStyle.Flat
                            };
                            break;
                        }
                    case "CountOrMeasure":
                        {
                            col = new DataGridViewComboBoxColumn()
                            {
                                DataSource = new string[] { "C", "M", "I" },
                                FlatStyle = FlatStyle.Flat
                            };
                            break;
                        }
                    case "LiveDead":
                        {
                            col = new DataGridViewComboBoxColumn()
                            {
                                DataSource = new string[] { "L", "D" },
                                FlatStyle = FlatStyle.Flat
                            };

                            break;
                        }
                    case "Stratum":
                        {
                            col = new DataGridViewComboBoxColumn()
                                {
                                    DisplayMember = "Code",
                                    ValueMember = "Self",
                                    FlatStyle = FlatStyle.Flat
                                };
                            break;
                        }
                    case "SampleGroup":
                        {
                            col = new DataGridViewComboBoxColumn()
                            {
                                DisplayMember = "Code",
                                ValueMember = "Self",
                                FlatStyle = FlatStyle.Flat
                            };
                            break;
                        }
                    case "KPI":
                        {
                            col = MakeColumn(fieldSetup.ColumnType);
                            // col.ReadOnly = true;
                            break;
                        }
                    case "Initials":
                        {
                            col = new DataGridViewTextBoxColumn()
                            {
                                MaxInputLength = 3
                            };
                            break;
                        }
                    default:
                        {
                            col = MakeColumn(fieldSetup.ColumnType);
                            break;
                        }
                }

                if (String.IsNullOrEmpty(col.DataPropertyName)) //see if we have already set the Mapping Name
                {
                    col.DataPropertyName = fieldSetup.Field;
                }
                col.Name = col.DataPropertyName;

                if (String.IsNullOrEmpty(col.HeaderText))
                {
                    col.HeaderText = fieldSetup.Heading;
                }

                if (!string.IsNullOrEmpty(fieldSetup.Format)    //field has format
                    && (col.DefaultCellStyle == null            //and column doesn't have format set yet
                    || string.IsNullOrEmpty(col.DefaultCellStyle.Format)))
                {
                    col.DefaultCellStyle = new DataGridViewCellStyle()
                        {
                            Format = fieldSetup.Format
                        };
                }

                //col.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                //col.Resizable = DataGridViewTriState.True;

                columns.Add(col);
            }

            columns.Add(new DataGridViewButtonColumn()
            {
                Name = "Logs",
                HeaderText = "Logs",
                DataPropertyName = "LogCountActual"
            });

            return columns.ToArray();
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