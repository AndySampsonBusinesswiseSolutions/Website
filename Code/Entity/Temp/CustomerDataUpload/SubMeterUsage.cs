using System;
using System.Data;

namespace Entity
{
    public partial class Temp
    {
        public partial class CustomerDataUpload
        {
            public class SubMeterUsage
            {
                private Guid _ProcessQueueGUID;
                public Guid ProcessQueueGUID
                {
                    get { return _ProcessQueueGUID; }
                    set { _ProcessQueueGUID = value; }
                }

                private int _RowId;
                public int RowId
                {
                    get { return _RowId; }
                    set { _RowId = value; }
                }

                private string _SubMeterIdentifier;
                public string SubMeterIdentifier
                {
                    get { return _SubMeterIdentifier; }
                    set { _SubMeterIdentifier = value; }
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

                public SubMeterUsage(Guid ProcessQueueGUID_,int RowId_,string SubMeterIdentifier_,string Date_,string TimePeriod_,string Value_,bool CanCommit_)
                {
                    this.ProcessQueueGUID = ProcessQueueGUID_;
                    this.RowId = RowId_;
                    this.SubMeterIdentifier = SubMeterIdentifier_;
                    this.Date = Date_;
                    this.TimePeriod = TimePeriod_;
                    this.Value = Value_;
                    this.CanCommit = CanCommit_;
                }

                public SubMeterUsage(DataRow dataRow)
                {
                    this.ProcessQueueGUID = (Guid)dataRow["ProcessQueueGUID"];
                    this.RowId = (int)dataRow["RowId"];
                    this.SubMeterIdentifier = (string)dataRow["SubMeterIdentifier"];
                    this.Date = (string)dataRow["Date"];
                    this.TimePeriod = (string)dataRow["TimePeriod"];
                    this.Value = (string)dataRow["Value"];
                    this.CanCommit = (bool)dataRow["CanCommit"];
                }
            }
        }
    }
}