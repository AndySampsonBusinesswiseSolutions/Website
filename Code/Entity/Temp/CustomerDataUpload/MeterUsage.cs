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
                private Guid _ProcessQueueGUID;
                public Guid ProcessQueueGUID
                {
                    get { return _ProcessQueueGUID; }
                    set { _ProcessQueueGUID = value; }
                }

                private string _SheetName;
                public string SheetName
                {
                    get { return _SheetName; }
                    set { _SheetName = value; }
                }

                private int? _RowId;
                public int? RowId
                {
                    get { return _RowId; }
                    set { _RowId = value; }
                }

                private string _MPXN;
                public string MPXN
                {
                    get { return _MPXN; }
                    set { _MPXN = value; }
                }

                private string _Date;
                public string Date
                {
                    get { return _Date; }
                    set { _Date = value; }
                }

                private string _TimePeriod;
                public string TimePeriod
                {
                    get { return _TimePeriod; }
                    set { _TimePeriod = value; }
                }

                private string _Value;
                public string Value
                {
                    get { return _Value; }
                    set { _Value = value; }
                }

                private bool _CanCommit;
                public bool CanCommit
                {
                    get { return _CanCommit; }
                    set { _CanCommit = value; }
                }

                public MeterUsage(Guid ProcessQueueGUID_,string SheetName_,int RowId_,string MPXN_,string Date_,string TimePeriod_,string Value_,bool CanCommit_)
                {
                    this.ProcessQueueGUID = ProcessQueueGUID_;
                    this.SheetName = SheetName_;
                    this.RowId = RowId_;
                    this.MPXN = MPXN_;
                    this.Date = Date_;
                    this.TimePeriod = TimePeriod_;
                    this.Value = Value_;
                    this.CanCommit = CanCommit_;
                }

                public MeterUsage(DataRow dataRow)
                {
                    this.ProcessQueueGUID = (Guid)dataRow["ProcessQueueGUID"];
                    this.SheetName = (string)dataRow["SheetName"];
                    this.RowId = (int)dataRow["RowId"];
                    this.MPXN = (string)dataRow["MPXN"];
                    this.Date = (string)dataRow["Date"];
                    this.TimePeriod = (string)dataRow["TimePeriod"];
                    this.Value = (string)dataRow["Value"];
                    this.CanCommit = (bool)dataRow["CanCommit"];
                }
            }
        }
    }
}