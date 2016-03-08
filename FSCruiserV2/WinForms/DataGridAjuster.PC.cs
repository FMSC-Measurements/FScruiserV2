using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CruiseDAL.DataObjects;
using CruiseDAL;
using FSCruiser.Core.Models;
using FSCruiser.Core;

namespace FSCruiser.WinForms
{
    public static class DataGridAdjuster
    {
        private static LogFieldSetupDO[] _defaultLogFields = new LogFieldSetupDO[]{
            new LogFieldSetupDO(){
                Field = CruiseDAL.Schema.LOG.LOGNUMBER, Heading = "LogNum", FieldOrder = 1, ColumnType = "Text" },
            new LogFieldSetupDO(){
                Field = CruiseDAL.Schema.LOG.GRADE, Heading = "Grade", FieldOrder = 2, ColumnType = "Text"},
            new LogFieldSetupDO() {
                Field = CruiseDAL.Schema.LOG.SEENDEFECT, Heading = "PctSeenDef", FieldOrder = 3, ColumnType = "Text"}
        };

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

            bool isPlotLayout = stratum != null && (Array.IndexOf(CruiseDAL.Schema.CruiseMethods.PLOT_METHODS, stratum.Method) >= 0);
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

            foreach (var fieldSetup in fieldSetups)
            {
                DataGridViewColumn col = null;
                switch (fieldSetup.Field)
                {
                    case "Species":
                        {
                            col = new DataGridViewComboBoxColumn()
                            {
                                DisplayMember = "Species",
                                ValueMember = "Self"
                            };
                            break;
                        }
                    case "CountOrMeasure":
                        {
                            col = new DataGridViewComboBoxColumn()
                            {
                                DataSource = new string[] { "C", "M", "I" }
                            };
                            break;
                        }
                    case "LiveDead":
                        {
                            col = new DataGridViewComboBoxColumn()
                            {
                                DataSource = new string[] { "L", "D" }
                            };

                            break;
                        }
                    case "Stratum":
                        {
                            if (unit != null && unit.Strata.Count > 1)
                            {
                                col = new DataGridViewComboBoxColumn()
                                {
                                    DisplayMember = "Code",
                                    ValueMember = "Self"
                                };
                                break;
                            }
                            else
                            {
                                continue;//go to next field in list
                            }
                        }
                    case "SampleGroup":
                        {
                            col = new DataGridViewComboBoxColumn()
                            {
                                DisplayMember = "Code",
                                ValueMember = "Self"
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
                DataPropertyName = "LogCount"
            });

            //columns.Add(new DataGridViewTextBoxColumn()
            //{
            //    Name = "Error",
            //    HeaderText = "Error",
            //    DataPropertyName = "Error",
                
            //});

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

        public static List<DataGridViewColumn> MakeLogColumns(DAL dal, long stratum_CN)
        {
            var fieldSetups = dal.From<LogFieldSetupDO>()
                .Where("Stratum_CN = ?")
                .OrderBy("FieldOrder")
                .Query(stratum_CN).ToList();

            if (fieldSetups.Count == 0)
            {
                fieldSetups.AddRange(_defaultLogFields);
            }

            List<DataGridViewColumn> columns = new List<DataGridViewColumn>();

            foreach (LogFieldSetupDO field in fieldSetups)
            {
                var col = MakeColumn(field.ColumnType);
                col.DataPropertyName = field.Field;
                col.HeaderText = field.Heading;

                if(!string.IsNullOrEmpty(field.Format))                                // AND field has format 
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

    }//end class


}
