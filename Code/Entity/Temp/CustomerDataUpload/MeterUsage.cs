using System;
using System.Data;

namespace Entity
{
    public partial class Temp
    {
        public partial class CustomerDataUpload
        {
            public class MeterUsage
            {
                // Parameterless constructor
                public MeterUsage() { }

                // Constructor taking in datarow
                public MeterUsage(DataRow dataRow)
                {
                    ProcessQueueGUID = (Guid)dataRow["ProcessQueueGUID"];
                    SheetName = (string)dataRow["SheetName"];
                    RowId = (int)dataRow["RowId"];
                    MPXN = (string)dataRow["MPXN"];
                    Date = (string)dataRow["Date"];
                    TimePeriod = (string)dataRow["TimePeriod"];
                    Value = (string)dataRow["Value"];
                    CanCommit = (bool)dataRow["CanCommit"];
                }

                // Properties
                public Guid ProcessQueueGUID {get;}
                public string SheetName {get;}
                public int RowId {get;}
                public string MPXN {get;}
                public string Date {get;}
                public string TimePeriod {get;}
                public string Value {get;}
                public bool CanCommit {get;}
            }
        }
    }
}