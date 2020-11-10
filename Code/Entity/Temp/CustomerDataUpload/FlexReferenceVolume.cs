using System;
using System.Data;

namespace Entity
{
    public partial class Temp
    {
        public partial class CustomerDataUpload
        {
            public class FlexReferenceVolume
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

                private string _ContractReference;
                public string ContractReference
                {
                    get { return _ContractReference; }
                    set { _ContractReference = value; }
                }

                private string _DateFrom;
                public string DateFrom
                {
                    get { return _DateFrom; }
                    set { _DateFrom = value; }
                }

                private string _DateTo;
                public string DateTo
                {
                    get { return _DateTo; }
                    set { _DateTo = value; }
                }

                private string _Volume;
                public string Volume
                {
                    get { return _Volume; }
                    set { _Volume = value; }
                }

                private bool _CanCommit;
                public bool CanCommit
                {
                    get { return _CanCommit; }
                    set { _CanCommit = value; }
                }

                public FlexReferenceVolume(Guid ProcessQueueGUID_,int RowId_,string ContractReference_,string DateFrom_,string DateTo_,string Volume_,bool CanCommit_)
                {
                    this.ProcessQueueGUID = ProcessQueueGUID_;
                    this.RowId = RowId_;
                    this.ContractReference = ContractReference_;
                    this.DateFrom = DateFrom_;
                    this.DateTo = DateTo_;
                    this.Volume = Volume_;
                    this.CanCommit = CanCommit_;
                }

                public FlexReferenceVolume(DataRow dataRow)
                {
                    this.ProcessQueueGUID = (Guid)dataRow["ProcessQueueGUID"];
                    this.RowId = (int)dataRow["RowId"];
                    this.ContractReference = (string)dataRow["ContractReference"];
                    this.DateFrom = (string)dataRow["DateFrom"];
                    this.DateTo = (string)dataRow["DateTo"];
                    this.Volume = (string)dataRow["Volume"];
                    this.CanCommit = (bool)dataRow["CanCommit"];
                }
            }
        }
    }
}