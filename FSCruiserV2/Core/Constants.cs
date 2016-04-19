using System;
using System.Collections.Generic;
using System.Text;
using CruiseDAL.DataObjects;

namespace FSCruiser.Core
{
    public static class Constants
    {
        public static bool NEW_SPECIES_OPTION = false;

        public const string FSCRUISER_VERSION = "2016.04.19";

        public const string APP_TITLE = "FScruiser - " + FSCRUISER_VERSION;

        public const string CRUISERS_FILENAME = "\\Cruisers.xml";//depeciated, cruiser info now stored in setting file 
        public const string APP_SETTINGS_PATH = "Settings.xml";

        public const string BACKUP_PREFIX = "BACK_";
        public const string BACKUP_TIME_FORMAT = "(yyyy_MM_dd__HH_mm)";

        public const bool ALLOW_STR_SYSTEMATIC = true;
        public const int MAX_TALLY_HISTORY_SIZE = 20;
        public const int SAVE_INTERVAL = 10;

        public const int LOG_COLUMN_WIDTH = 40;

        public const System.Threading.ThreadPriority VALIDATE_TREE_THREAD_PRIORITY = System.Threading.ThreadPriority.BelowNormal;
        public const System.Threading.ThreadPriority SAVE_TREES_THREAD_PRIORISTY = System.Threading.ThreadPriority.BelowNormal;
        public const System.Threading.ThreadPriority LOAD_CUTTINGUNITDATA_PRIORITY = System.Threading.ThreadPriority.BelowNormal;

        public static readonly decimal DEFAULT_VOL_FACTOR = new Decimal(333, 0, 0, false, 3);

        internal static readonly TreeDefaultValueDO[] EMPTY_SPECIES_LIST = new TreeDefaultValueDO[] { };
        internal static readonly SampleGroupDO[] EMPTY_SG_LIST = new SampleGroupDO[] { };

        public static readonly TreeFieldSetupDO[] DEFAULT_TREE_FIELDS = new TreeFieldSetupDO[]{
            new TreeFieldSetupDO(){
                Field = "TreeNumber", Heading = "Tree", FieldOrder = 1, ColumnType = "Text" },
            new TreeFieldSetupDO() {
                Field = "Stratum", Heading = "St", Format = "[Code]" , FieldOrder = 2, ColumnType = "Text"  },
            new TreeFieldSetupDO() {
                Field = "SampleGroup", Heading = "SG", Format = "[Code]" , FieldOrder = 3, ColumnType = "Text" },
            new TreeFieldSetupDO() {
                Field = "Species", Heading = "Sp", FieldOrder = 4, ColumnType = "Combo" },
            new TreeFieldSetupDO() {
                Field = "DBH", Heading = "DBH", FieldOrder = 5, ColumnType = "Text" },
            new TreeFieldSetupDO() {
                Field = "TotalHeight", Heading = "THT", FieldOrder = 6, ColumnType = "Text" },
            new TreeFieldSetupDO() {
                Field = "SeenDefectPrimary", Heading = "Def", FieldOrder = 7, ColumnType = "Text" }
        };

        public static readonly LogFieldSetupDO[] DEFAULT_LOG_FIELDS = new LogFieldSetupDO[]{
            new LogFieldSetupDO(){
                Field = CruiseDAL.Schema.LOG.LOGNUMBER, Heading = "LogNum", FieldOrder = 1, ColumnType = "Text" },
            new LogFieldSetupDO(){
                Field = CruiseDAL.Schema.LOG.GRADE, Heading = "Grade", FieldOrder = 2, ColumnType = "Text"},
            new LogFieldSetupDO() {
                Field = CruiseDAL.Schema.LOG.SEENDEFECT, Heading = "PctSeenDef", FieldOrder = 3, ColumnType = "Text"}
        };

        public static readonly String[] PRODUCT_CODES = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "26" };

        public static readonly String[] UOM_CODES = new string[] { "01", "02", "03", "04", "05"};

        public static readonly char[] HOTKEY_KEYS = new char[] {'A','B','C','D','E','F'
            ,'G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
            ,'1','2','3','4','5','6','7','8','9','0'
            ,'*','.' };

    }
}
