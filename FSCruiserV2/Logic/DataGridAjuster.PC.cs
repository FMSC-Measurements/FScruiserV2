using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using CruiseDAL;

namespace FSCruiserV2.Logic
{
    public static class DataGridAdjuster
    {
        public static List<TreeFieldSetupDO> GetTreeFieldSetupByStratum(DAL dal, long stratum_cn)
        {
            //   return base.DAL.Read<TreeFieldSetupDO>(TREEFIELDSETUP._NAME, TREEFIELDSETUP.STRATUM_CN + " = ?", new string[] { stratum_cn });
            return dal.Read<TreeFieldSetupDO>(CruiseDAL.Schema.TREEFIELDSETUP._NAME, "WHERE " + CruiseDAL.Schema.TREEFIELDSETUP.STRATUM_CN + " = ?  ORDER BY " + CruiseDAL.Schema.TREEFIELDSETUP.FIELDORDER, stratum_cn);

        }

        public static List<TreeFieldSetupDO> GetTreeFieldSetupByUnit(DAL dal, long unit_cn)
        {
            return dal.Read<TreeFieldSetupDO>(CruiseDAL.Schema.TREEFIELDSETUP._NAME,
                    @"JOIN CuttingUnitStratum 
                    ON TreeFieldSetup.Stratum_CN = CuttingUnitStratum.Stratum_CN 
                    WHERE CuttingUnitStratum.CuttingUnit_CN = ?  
                    Group BY TreeFieldSetup.Field 
                    ORDER BY TreeFieldSetup.FieldOrder;", unit_cn);

        }

        //TODO GetTreeFieldSetups and GetTreeFieldNames should be extracted to a common base class
        private static List<TreeFieldSetupDO> GetTreeFieldSetups(DAL db, CuttingUnitDO unit, StratumVM stratum)
        {
            List<TreeFieldSetupDO> fieldSetups = null;
            if (stratum != null)
            {
                fieldSetups = GetTreeFieldSetupByStratum(db, stratum.Stratum_CN.Value);
            }
            else
            {
                fieldSetups = GetTreeFieldSetupByUnit(db, unit.CuttingUnit_CN.Value);
            }

            if (fieldSetups.Count == 0)
            {
                fieldSetups.Clear();
                fieldSetups.AddRange(Constants.DEFAULT_TREE_FIELDS);
            }

            bool isPlotLayout = stratum != null && (Array.IndexOf(CruiseDAL.Schema.Constants.CruiseMethods.PLOT_METHODS, stratum.Method) >= 0);
            if(isPlotLayout  && fieldSetups.FindIndex(((tfs) => tfs.Field == CruiseDAL.Schema.TREE.COUNTORMEASURE)) < 0)
            {
                fieldSetups.Insert(5, new TreeFieldSetupDO() { Field = CruiseDAL.Schema.TREE.COUNTORMEASURE, Heading = "C/M" });
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

        public static DataGridViewColumn[] MakeTreeColumns(DAL db, CuttingUnitDO unit, StratumVM stratum, bool enableLogs)
        {
            List<TreeFieldSetupDO> fieldSetups = GetTreeFieldSetups(db, unit, stratum);
            List<DataGridViewColumn> columns = new List<DataGridViewColumn>();

            DataGridViewColumn col;
            for (int i = 0; i < fieldSetups.Count; i++)
            {
                string columnType = fieldSetups[i].ColumnType;
                col = null;

                switch (fieldSetups[i].Field)
                {
                    case "Species":
                        {
                            col = new DataGridViewComboBoxColumn();
                            ((DataGridViewComboBoxColumn)col).DisplayMember = "Species";
                            ((DataGridViewComboBoxColumn)col).ValueMember = "Self";
                            //col.DataPropertyName = "TreeDefaultValue";
                            break;
                        }
                    case "CountOrMeasure":
                        {
                            col = new DataGridViewComboBoxColumn();
                            ((DataGridViewComboBoxColumn)col).DataSource = new string[] { "C", "M", "I" };
                            break;
                        }
                    case "LiveDead":
                        {
                            col = new DataGridViewComboBoxColumn();
                            ((DataGridViewComboBoxColumn)col).DataSource = new string[] { "L", "D" };
                            break;
                        }
                    case "Stratum":
                        {
                            if (unit != null && unit.Strata.Count > 1)
                            {
                                col = new DataGridViewComboBoxColumn();
                                ((DataGridViewComboBoxColumn)col).DisplayMember = "Code";
                                ((DataGridViewComboBoxColumn)col).ValueMember = "Self";
                                break;
                            }
                            else
                            {
                                continue;//go to next field in list
                            }
                        }
                    case "SampleGroup":
                        {
                            col = new DataGridViewComboBoxColumn();
                            ((DataGridViewComboBoxColumn)col).DisplayMember = "Code";
                            ((DataGridViewComboBoxColumn)col).ValueMember = "Self";
                            break;
                        }
                    case "KPI":
                        {
                            col = MakeColumn(columnType);
                            col.ReadOnly = true;
                            break;
                        }
                    case "Initials":
                        {
                            col = new DataGridViewComboBoxColumn();
                            ((DataGridViewComboBoxColumn)col).DisplayMember = "Initials";
                            ((DataGridViewComboBoxColumn)col).ValueMember = "Initials";
                            break;
                        }
                    default:
                        {
                            col = MakeColumn(columnType);
                            break;
                        }

                }

                if (String.IsNullOrEmpty(col.DataPropertyName)) //see if we have already set the Mapping Name
                {
                    col.DataPropertyName = fieldSetups[i].Field;
                }
                col.Name = col.DataPropertyName;

                if (String.IsNullOrEmpty(col.HeaderText))
                {
                    col.HeaderText = fieldSetups[i].Heading;
                }
                
                if ((col.DefaultCellStyle == null || string.IsNullOrEmpty(col.DefaultCellStyle.Format)) //if format not alread defined (has no default cell style or default cell style has no format)
                    && (!string.IsNullOrEmpty(fieldSetups[i].Format)))                                  // AND field has format 
                {
                    col.DefaultCellStyle = new DataGridViewCellStyle()
                        {
                            Format = fieldSetups[i].Format
                        };
                }

                //col.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                //col.Resizable = DataGridViewTriState.True;

                columns.Add(col);
            }

            return columns.ToArray();
        }

        private static DataGridViewColumn MakeColumn(string columnType)
        {
            switch (columnType)
            {
                case "Text":
                    {
                        return new DataGridViewTextBoxColumn();
                    }
                case "Combo":
                    {
                        return new DataGridViewComboBoxColumn();
                    }
                case "Button":
                    {
                        return new DataGridViewButtonColumn();
                    }
                case "Check":
                    {
                        return new DataGridViewCheckBoxColumn();
                    }
                case "UpDown":
                    {
                        return new DataGridViewTextBoxColumn();
                    }
                case "DateTime":
                    {
                        return new DataGridViewTextBoxColumn();
                    }
                default:
                    {
                        return new DataGridViewTextBoxColumn();
                    }
            }//end switch
        }//end method
    }//end class


}
