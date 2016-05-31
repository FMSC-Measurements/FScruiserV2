using System;
using System.Drawing;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using FMSC.Controls;
using FSCruiser.Core.Models;

namespace FSCruiser.WinForms
{
    public static class DataGridAdjuster
    {
        private static EditableTextBoxColumn MakeErrorColumn(int screenWidth)
        {
            return new EditableTextBoxColumn()
                {
                    HeaderText = "Error Message",
                    MappingName = "Error",
                    Width = screenWidth,
                    ReadOnly = true,
                    MultiLine = true
                };
        }

        public static void InitializeGrid(EditableDataGrid grid)
        {
            // Default grid-level parameters
            grid.RowHeadersVisible = false;
            grid.HomeColumnIndex = 0;
            //preferredRowHeight = 20;
            grid.Font = new Font("Courier New", 12.0F, FontStyle.Regular);
            grid.ForeColor = Color.Black;
            grid.GridLineColor = Color.Black;
            grid.ErrorColor = Color.Red;
            grid.BackColor = Color.White; // background color of the actual grid cells
            grid.BackgroundColor = Color.Black; // background color of the control, but outside the grid.
            grid.HeaderBackColor = Color.DarkGray;
            grid.HeaderForeColor = Color.White;
            grid.SelectionBackColor = Color.Yellow;
            grid.SelectionForeColor = Color.Black;
            grid.AlternatingBackColor = Color.LightGray;
        }

        public static DataGridTableStyle InitializeLogColumns(this ILogFieldProvider stratum, EditableDataGrid grid)
        {
            var fields = stratum.ReadLogFields();

            DataGridTableStyle tblStyle = new DataGridTableStyle();
            tblStyle.MappingName = "LogDO";

            foreach (LogFieldSetupDO field in fields)
            {
                CustomColumnBase col = MakeColumn(field.ColumnType);
                col.MappingName = field.Field;
                col.HeaderText = field.Heading;
                col.Format = field.Format; // 'C' = currency, 'N' = number (E.G. "N1" means one decimal place), #0.00
                col.FormatInfo = null;
                col.NullText = String.Empty;// <- look into this

                if (field.Width == 0.0)
                {
                    col.Width = MeasureTextWidth(grid, field.Heading.Trim()) + 18;//plus 18 to allow for padding
                }
                else
                {
                    col.Width = (int)field.Width;
                }

                tblStyle.GridColumnStyles.Add(col);
            }

            grid.TableStyles.Add(tblStyle);

            return tblStyle;
        }

        private static int MeasureTextWidth(Control c, string text)
        {
            if (c == null)
            { return -1; }
            using (Graphics g = c.CreateGraphics())
            {
                return (int)Math.Ceiling(g.MeasureString(text, c.Font).Width);
            }
        }

        public static DataGridTableStyle InitializeTreeColumns(this ITreeFieldProvider provider, EditableDataGrid grid)
        {
            DataGridTableStyle tblStyle = new System.Windows.Forms.DataGridTableStyle() { MappingName = "TreeVM" };

            var fields = provider.ReadTreeFields();

            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int charWidth = MeasureTextWidth(grid, "0");

            // Loop through each item in the fields list,
            // set up a column style and add it to the table style.

            foreach (TreeFieldSetupDO field in fields)
            {
                CustomColumnBase col;

                switch (field.Field)
                {
                    case "Species":
                        {
                            col = new FMSC.Controls.EditableComboBoxColumn();
                            ((EditableComboBoxColumn)col).DisplayMember = "Species";
                            //((EditableComboBoxColumn)col).ValueMember = "Self";
                            col.MappingName = "TreeDefaultValue";
                            col.Format = "[Species]";
                            break;
                        }
                    case "CountOrMeasure":
                        {
                            col = new FMSC.Controls.EditableComboBoxColumn();
                            //((EditableComboBoxColumn)col).DisplayMember = "CountOrMeasure";
                            //((EditableComboBoxColumn)col).ValueMember = "CountOrMeasure";
                            ((EditableComboBoxColumn)col).DataSource = new string[] { "C", "M", "I" };
                            break;
                        }
                    case "LiveDead":
                        {
                            col = new FMSC.Controls.EditableComboBoxColumn();
                            //((EditableComboBoxColumn)col).DisplayMember = "LiveDead";
                            //((EditableComboBoxColumn)col).ValueMember = "LiveDead";
                            ((EditableComboBoxColumn)col).DataSource = new string[] { "L", "D" };
                            break;
                        }

                    case "Stratum":
                        {
                            col = new FMSC.Controls.EditableComboBoxColumn();
                            ((EditableComboBoxColumn)col).DisplayMember = "Code";
                            //((EditableComboBoxColumn)col).ValueMember = "Code";
                            break;
                        }
                    case "SampleGroup":
                        {
                            col = new FMSC.Controls.EditableComboBoxColumn();
                            ((EditableComboBoxColumn)col).DisplayMember = "Code";
                            //((EditableComboBoxColumn)col).ValueMember = "Code";
                            break;
                        }
                    case "KPI":
                        {
                            col = MakeColumn(field.ColumnType);
                            //col.ReadOnly = true;
                            break;
                        }
                    case "Initials":
                        {
                            col = new FMSC.Controls.EditableComboBoxColumn();
                            ((EditableComboBoxColumn)col).DisplayMember = "Initials";
                            ((EditableComboBoxColumn)col).ValueMember = "Initials";
                            break;
                        }
                    default://all other columns
                        {
                            col = MakeColumn(field.ColumnType);
                            break;
                        }
                }

                // Set up width: autowidth or fixed width
                if (field.Width == 0.0)
                {
                    col.Width = MeasureTextWidth(grid, field.Heading.Trim()) + 18;//plus 18 to allow for padding
                }
                else
                {
                    int width1 = (int)field.Width;
                    if (width1 <= 10)
                    {
                        int width2 = (charWidth * width1) + 18;
                        col.Width = Math.Min(screenWidth, width2);
                    }
                    else
                    {
                        col.Width = Math.Min(screenWidth, width1);
                    }
                }

                if (String.IsNullOrEmpty(col.MappingName)) //see if we have already set the Mapping Name
                {
                    col.MappingName = field.Field;
                }
                if (String.IsNullOrEmpty(col.HeaderText))
                {
                    col.HeaderText = field.Heading;
                }
                if (string.IsNullOrEmpty(col.Format))
                {
                    col.Format = field.Format; // 'C' = currency, 'N' = number (E.G. "N1" means one decimal place), #0.00
                }
                col.FormatInfo = null;
                //this.myCustomColumnBase.Width = (int)myTreeFieldSetups[i].Width;
                col.NullText = String.Empty;// <- look into this

                // Add the column style to the table style
                tblStyle.GridColumnStyles.Add(col);
            }

            DataGridButtonColumn logsCol = new DataGridButtonColumn();
            logsCol.HeaderText = "Logs";
            logsCol.MappingName = "LogCountActual";
            tblStyle.GridColumnStyles.Add(logsCol);

            tblStyle.GridColumnStyles.Add(MakeErrorColumn(screenWidth));
            // Add the newly created DataGridTableStyle to the grid.
            grid.TableStyles.Add(tblStyle);

            return tblStyle;
        }

        // Grid Column Type Factory
        public static FMSC.Controls.CustomColumnBase MakeColumn(string columnType)
        {
            switch (columnType)
            {
                case "Text":
                    {
                        return new FMSC.Controls.EditableTextBoxColumn();
                    }
                case "Combo":
                    {
                        return new FMSC.Controls.EditableComboBoxColumn();
                    }
                case "Button":
                    {
                        return new FMSC.Controls.DataGridButtonColumn();
                    }
                case "Check":
                    {
                        return new FMSC.Controls.EditableCheckBoxColumn();
                    }
                case "UpDown":
                    {
                        return new FMSC.Controls.EditableUpDownColumn();
                    }
                case "DateTime":
                    {
                        return new FMSC.Controls.EditableDateTimePickerColumn();
                    }
                default:
                    {
                        return new FMSC.Controls.EditableTextBoxColumn();
                    }
            }//end switch
        }//end method
    }
}