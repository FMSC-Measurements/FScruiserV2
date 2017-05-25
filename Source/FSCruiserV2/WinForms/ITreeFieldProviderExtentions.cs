using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms
{
    public static class ITreeFieldProviderExtentions
    {
        public static IEnumerable<DataGridViewColumn> MakeTreeColumns(this ITreeFieldProvider provider, int fontWidth)
        {
            var fieldSetups = provider.TreeFields;
            List<DataGridViewColumn> columns = new List<DataGridViewColumn>();

            foreach (var fieldSetup in fieldSetups)
            {
                DataGridViewColumn col = null;

                if (fieldSetup.Field == "SureToMeasure")
                { fieldSetup.Field = "STM"; }

                switch (fieldSetup.Field)
                {
                    case "CuttingUnit":
                        {
                            continue;
                        }
                    case "Species":
                        {
                            col = new DataGridViewComboBoxColumn()
                            {
                                DataPropertyName = nameof(Tree.TreeDefaultValue),
                                DisplayMember = nameof(TreeDefaultValueDO.Species),
                                ValueMember = nameof(TreeDefaultValueDO.Self),
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
                                DataPropertyName = nameof(Tree.Stratum),
                                DisplayMember = nameof(Stratum.Code),
                                ValueMember = nameof(Stratum.Self),
                                FlatStyle = FlatStyle.Flat
                            };
                            break;
                        }
                    case "SampleGroup":
                        {
                            col = new DataGridViewComboBoxColumn()
                            {
                                DataPropertyName = nameof(Tree.SampleGroup),
                                DisplayMember = nameof(SampleGroup.Code),
                                ValueMember = nameof(SampleGroup.Self),
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
                            col = new DataGridViewComboBoxColumn()
                            {
                                //DataPropertyName = nameof(Tree.Initials),
                                //DisplayMember = nameof(Cruiser.Initials),
                                //ValueMember = nameof(Cruiser.Initials),
                                FlatStyle = FlatStyle.Flat
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

                if (fieldSetup.Width > 0)
                {
                    col.Width = (int)fieldSetup.Width * fontWidth;
                    col.Resizable = DataGridViewTriState.True;
                }
                else
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    col.Resizable = DataGridViewTriState.True;
                }

                yield return col;
            }

            yield return new DataGridViewButtonColumn()
            {
                Name = "Logs",
                HeaderText = "Logs",
                DataPropertyName = nameof(Tree.LogCountActual),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
            };
        }

        private static DataGridViewColumn MakeColumn(string columnType)
        {
            switch (columnType)
            {
                //case "Combo":
                //    {
                //        return new DataGridViewComboBoxColumn() { FlatStyle = FlatStyle.Flat };
                //    }
                //case "Button":
                //    {
                //        return new DataGridViewButtonColumn();
                //    }
                case "Check":
                    {
                        return new DataGridViewCheckBoxColumn() { FlatStyle = FlatStyle.Flat };
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