using System;
using System.Data;

namespace Entity
{
    public partial class Temp
    {
        public partial class CustomerDataUpload
        {
            public class SubMeter
            {
                private Guid _ProcessQueueGUID;
                public Guid ProcessQueueGUID
                {
                    get { return _ProcessQueueGUID; }
                    set { _ProcessQueueGUID = value; }
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

                private string _SubMeterIdentifier;
                public string SubMeterIdentifier
                {
                    get { return _SubMeterIdentifier; }
                    set { _SubMeterIdentifier = value; }
                }

                private string _SerialNumber;
                public string SerialNumber
                {
                    get { return _SerialNumber; }
                    set { _SerialNumber = value; }
                }

                private string _SubArea;
                public string SubArea
                {
                    get { return _SubArea; }
                    set { _SubArea = value; }
                }

                private string _Asset;
                public string Asset
                {
                    get { return _Asset; }
                    set { _Asset = value; }
                }

                private bool _CanCommit;
                public bool CanCommit
                {
                    get { return _CanCommit; }
                    set { _CanCommit = value; }
                }

                public SubMeter(Guid ProcessQueueGUID_,int RowId_,string MPXN_,string SubMeterIdentifier_,string SerialNumber_,string SubArea_,string Asset_,bool CanCommit_)
                {
                    this.ProcessQueueGUID = ProcessQueueGUID_;
                    this.RowId = RowId_;
                    this.MPXN = MPXN_;
                    this.SubMeterIdentifier = SubMeterIdentifier_;
                    this.SerialNumber = SerialNumber_;
                    this.SubArea = SubArea_;
                    this.Asset = Asset_;
                    this.CanCommit = CanCommit_;
                }

                public SubMeter(DataRow dataRow)
                {
                    this.ProcessQueueGUID = (Guid)dataRow["ProcessQueueGUID"];
                    this.RowId = (int)dataRow["RowId"];
                    this.MPXN = (string)dataRow["MPXN"];
                    this.SubMeterIdentifier = (string)dataRow["SubMeterIdentifier"];
                    this.SerialNumber = (string)dataRow["SerialNumber"];
                    this.SubArea = (string)dataRow["SubArea"];
                    this.Asset = (string)dataRow["Asset"];
                    this.CanCommit = (bool)dataRow["CanCommit"];
                }
            }
        }
    }
}