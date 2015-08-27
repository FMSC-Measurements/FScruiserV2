using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using CruiseDAL;
using System.Windows.Forms;
using FMSC.Controls;
using CruiseDAL.DataObjects;
using FSCruiserV2.Logic;

namespace FSCruiserV2
{
    public static class DataGridAdjuster
    {
        //depreciated see Constants.cs
        //private static TreeFieldSetupDO[] _defaultFields = new TreeFieldSetupDO[]{
        //    new TreeFieldSetupDO(){
        //        Field = "TreeNumber", Heading = "Tree", FieldOrder = 1, ColumnType = "Text" },
        //    new TreeFieldSetupDO() {
        //        Field = "Stratum", Heading = "St", Format = "[Code]" , FieldOrder = 2, ColumnType = "Text"  },
        //    new TreeFieldSetupDO() {
        //        Field = "SampleGroup", Heading = "SG", Format = "[Code]" , FieldOrder = 3, ColumnType = "Text" },
        //    new TreeFieldSetupDO() {
        //        Field = "Species", Heading = "Sp", FieldOrder = 4, ColumnType = "Combo" },
        //    new TreeFieldSetupDO() {
        //        Field = "DBH", Heading = "DBH", FieldOrder = 5, ColumnType = "Text" },
        //    new TreeFieldSetupDO() {
        //        Field = "TotalHeight", Heading = "THT", FieldOrder = 6, ColumnType = "Text" },
        //    new TreeFieldSetupDO() {
        //        Field = "SeenDefectPrimary", Heading = "Def", FieldOrder = 7, ColumnType = "Text" }
        //};

        private static LogFieldSetupDO[] _defaultLogFields = new LogFieldSetupDO[]{
            new LogFieldSetupDO(){
                Field = CruiseDAL.Schema.LOG.LOGNUMBER, Heading = "LogNum", FieldOrder = 1, ColumnType = "Text" },
            new LogFieldSetupDO(){
                Field = CruiseDAL.Schema.LOG.GRADE, Heading = "Grade", FieldOrder = 2, ColumnType = "Text"},
            new LogFieldSetupDO() {
                Field = CruiseDAL.Schema.LOG.SEENDEFECT, Heading = "PctSeenDef", FieldOrder = 3, ColumnType = "Text"}
        };

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

        //private List<CruiseDAL.DataObjects.TreeFieldSetupDO> myTreeFieldSetups = null;
        //private Services.TreeFieldSetupService myTreeFieldSetupService = null;

        //private System.Windows.Forms.DataGridTableStyle myDataGridTableStyle;
        //private CustomColumnBase myCustomColumnBase;

        //private DAL myDAL = null;
        //private FMSC.Controls.EditableDataGrid myGrid = null;
        //private long myStratum_CN;
        //private string myMappingName;

        //// Default grid-level parameters
        //public bool rowHeaderVisible;
        //public bool alternatingRowBackColor;
        //public bool allowNewRow;
        //public int preferredRowHeight;
        //public int homeColumnIndex;
        //public string fontString;
        //public float fontSize;
        //public FontStyle fontStyle;
        //public Color fontColor;
        //public Color gridLineColor;
        //public Color alternatingBackColor;
        //public Color backColor; // background color of the actual grid cells
        //public Color backGroundColor; // background color of the control, but outside the grid.
        //public Color headerBackColor;
        //public Color headerForeColor;        
        //public Color selectionBackColor;
        //public Color selectionForeColor;

        // CTOR     
    //    public DataGridAdjuster(DAL DAL, System.Windows.Forms.DataGrid grid, string stratum_cn, string mappingName)
        public static void InitializeGrid(EditableDataGrid grid)
        {  

            //if (mappingName.Length == 0)
            //{
            //    throw new ArgumentException("An empty mappingName was passed to DataGridAdjuster().");
            //}
            //myMappingName = mappingName;

            // Default grid-level parameters
            grid.RowHeadersVisible = false;
            //alternatingRowBackColor = true;
            //grid.AllowUserToAddRows = true;
            grid.HomeColumnIndex = 0;
            //preferredRowHeight = 20;
            // Default fonts available on Windows Mobile are "Tahoma" (variable width) and "Courier New"(fixed width).
            //fontString = "Courier New";
            //fontSize = 12.0F;
            //fontStyle = FontStyle.Regular;
            grid.Font = new Font("Courier New", 12.0F, FontStyle.Regular);
            grid.ForeColor = Color.Black;
            grid.GridLineColor = Color.Black;
            grid.ErrorColor = Color.Red;
            grid.BackColor = Color.White; // background color of the actual grid cells
            grid.BackgroundColor = Color.Black; // background color of the control, but outside the grid.
            grid.HeaderBackColor = Color.Black;
            grid.HeaderForeColor = Color.White;
            grid.SelectionBackColor = Color.Yellow;
            grid.SelectionForeColor = Color.Black;


            Color alternatingBackColor = Color.LightGray;
            if (grid is EditableDataGrid && true)
            {
                ((EditableDataGrid)grid).AlternatingBackColor = alternatingBackColor;
            }
        }


        //public void AdjustGrid()
        //{    
        //    // Grid-Level Customizations
        //    myGrid.RowHeadersVisible = this.rowHeaderVisible;
        //    //myGrid.PreferredRowHeight = this.preferredRowHeight;
        //    myGrid.AllowNewRow = this.allowNewRow;
        //    myGrid.HomeColumnIndex = this.homeColumnIndex;
        //    myGrid.Font = new Font(this.fontString, this.fontSize, this.fontStyle);
        //    myGrid.ForeColor = this.fontColor;
        //    myGrid.GridLineColor = this.gridLineColor;
        //    myGrid.ErrorColor = Color.Red;      //error color
        //    myGrid.BackColor = this.backColor;
        //    myGrid.BackgroundColor = this.backGroundColor;
        //    myGrid.HeaderBackColor = this.headerBackColor;
        //    myGrid.HeaderForeColor = this.headerForeColor;        
        //    myGrid.SelectionBackColor = this.selectionBackColor;
        //    myGrid.SelectionForeColor = this.selectionForeColor;

        //    //  Color tempColor = myGrid.HeaderBackColor;

        //    if (myGrid is EditableDataGrid && this.alternatingRowBackColor)
        //    {
        //        ((EditableDataGrid)myGrid).AlternatingBackColor = this.alternatingBackColor;
        //    }
        //}

        public static DataGridTableStyle InitializeLogColumns(DAL dal, EditableDataGrid grid, long stratum_CN)
        {
            List<LogFieldSetupDO> fieldSetups = dal.Read<LogFieldSetupDO>("LogFieldSetup", "WHERE Stratum_CN = ? ORDER BY FieldOrder", stratum_CN);

            DataGridTableStyle tblStyle = new DataGridTableStyle();
            tblStyle.MappingName = "LogDO";


            if (fieldSetups.Count == 0)
            {
                fieldSetups.AddRange(_defaultLogFields);
            }

            foreach (LogFieldSetupDO field in fieldSetups)
            {
                CustomColumnBase col = GetNew(field.ColumnType);
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


        public static void ShowHideErrorCol(EditableDataGrid grid)
        {
            DataGridColumnStyle erroCol = null;
            try
            {
                erroCol = grid.TableStyle.GridColumnStyles["Error"];
            }
            catch {}
            if(erroCol != null)
            {
                if(erroCol.Width > 0)
                {
                    erroCol.Width = -1;
                }
                else
                {
                    erroCol.Width = Screen.PrimaryScreen.WorkingArea.Width;
                }
            }
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


        private static List<TreeFieldSetupDO> GetTreeFieldSetups(DAL db, CuttingUnitDO unit, StratumVM stratum)
        {
            List<TreeFieldSetupDO> fieldSetups = null;
            if (stratum != null)
            {
                fieldSetups = Services.TreeFieldSetupService.GetTreeFieldSetupByStratum(db, stratum.Stratum_CN.Value);
            }
            else
            {
                fieldSetups = Services.TreeFieldSetupService.GetTreeFieldSetupByUnit(db, unit.CuttingUnit_CN.Value);
            }

            if (fieldSetups.Count == 0)
            {
                fieldSetups.Clear();
                fieldSetups.AddRange(Constants.DEFAULT_TREE_FIELDS);
            }

            bool isPlotLayout = stratum != null && (Array.IndexOf(CruiseDAL.Schema.Constants.CruiseMethods.PLOT_METHODS, stratum.Method) >= 0);
            if(isPlotLayout  && fieldSetups.FindIndex(((tfs) => tfs.Field == CruiseDAL.Schema.TREE.COUNTORMEASURE)) < 0)
            {
                fieldSetups.Insert(5, new TreeFieldSetupDO() { Field = CruiseDAL.Schema.TREE.COUNTORMEASURE , Heading = "C/M" } );
            }

            //if initializing grid for tree based multi strata
            if (unit != null && unit.Strata.Count > 1)
            {
                //check if the user has a stratum field
                if (fieldSetups.Find(x => x.Field == "Stratum") == null)//if not
                {
                    //find the location of the tree number field
                    int indexOfTreeNum = fieldSetups.FindIndex(x => x.Field == CruiseDAL.Schema.TREE.TREENUMBER);
                    //if user doesn't have a tree number field, fall back to the last field index
                    if (indexOfTreeNum == -1) { indexOfTreeNum = fieldSetups.Count - 1; }//last item index 
                    //add the stratum field to the filed list
                    TreeFieldSetupDO tfs = new TreeFieldSetupDO() { Field = "Stratum", Heading = "St", Format = "[Code]" };
                    fieldSetups.Insert(indexOfTreeNum + 1, tfs);
                }
            }

            return fieldSetups;
        }


        public static String[] GetTreeFieldNames(DAL dal, CuttingUnitDO unit, StratumVM stratum)
        {
            List<TreeFieldSetupDO> treeFields = GetTreeFieldSetups(dal, unit, stratum);
            int numTreeFields = treeFields.Count;
            String[] fieldNames = new String[numTreeFields];
            for (int i = 0; i < numTreeFields; i++)
            {
                fieldNames[i] = treeFields[i].Field;
            }

            return fieldNames;

        }

        public static DataGridTableStyle InitializeTreeColumns(DAL dal, EditableDataGrid grid, CuttingUnitDO unit, StratumVM stratum, bool enableLogs, ButtonCellClickEventHandler logClick)
        {
            // Query TreeFieldSetup table for field info based on Stratum_CN        
            // This function selects FieldOrder > 0 and sorts on it.
            List<TreeFieldSetupDO> fieldSetups = GetTreeFieldSetups(dal, unit, stratum);
            // Create a new table style
            DataGridTableStyle tblStyle = new System.Windows.Forms.DataGridTableStyle();

            // Set the table style's mapping name
            tblStyle.MappingName = "TreeVM";

           

            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int charWidth = MeasureTextWidth(grid, "0");
            // Loop through each item in the myTreeFieldSetups list, set 
            // up a column style and add it to the table style. 
            CustomColumnBase col;
            for (int i = 0; i < fieldSetups.Count; i++)
            {
                // Get the column type
                string columnType = fieldSetups[i].ColumnType;
                col = null;

                switch (fieldSetups[i].Field)
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
                            //if initializing grid for tree based multi stratum
                            //add stratum column, otherwise ignore
                            if (unit != null && unit.Strata.Count > 1)
                            {
                                col = new FMSC.Controls.EditableComboBoxColumn();
                                ((EditableComboBoxColumn)col).DisplayMember = "Code";
                                //((EditableComboBoxColumn)col).ValueMember = "Code";
                                break;
                            }
                            else
                            {
                                continue;//go to next field in list
                            }
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
                            col = GetNew(columnType);
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
                            col = GetNew(columnType);
                            break;
                        }                
                }

                
                // Set up width: autowidth or fixed width
                if (fieldSetups[i].Width == 0.0)
                {
                    col.Width = MeasureTextWidth(grid, fieldSetups[i].Heading.Trim()) + 18;//plus 18 to allow for padding 
                }
                else
                {
                    int width1 = (int)fieldSetups[i].Width;
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
                    col.MappingName = fieldSetups[i].Field;
                }
                if(String.IsNullOrEmpty(col.HeaderText))
                {
                    col.HeaderText = fieldSetups[i].Heading;
                }
                if (string.IsNullOrEmpty(col.Format))
                {
                    col.Format = fieldSetups[i].Format; // 'C' = currency, 'N' = number (E.G. "N1" means one decimal place), #0.00
                }
                col.FormatInfo = null;
                //this.myCustomColumnBase.Width = (int)myTreeFieldSetups[i].Width;
                col.NullText = String.Empty;// <- look into this


                // Add the column style to the table style
                tblStyle.GridColumnStyles.Add(col);

                
            }

            

            if (logClick != null)
            {
                DataGridButtonColumn logsCol = new DataGridButtonColumn();
                logsCol.Click += logClick;
                logsCol.HeaderText = "Logs";
                logsCol.MappingName = "LogCount";
                logsCol.Width = (enableLogs) ? Constants.LOG_COLUMN_WIDTH : -1;
                tblStyle.GridColumnStyles.Add(logsCol);
            }

            tblStyle.GridColumnStyles.Add(MakeErrorColumn(screenWidth));
            // Add the newly created DataGridTableStyle to the grid. 
            grid.TableStyles.Add(tblStyle);
            
            return tblStyle;
        }

        // Grid Column Type Factory
        public static FMSC.Controls.CustomColumnBase GetNew(string columnType)
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
