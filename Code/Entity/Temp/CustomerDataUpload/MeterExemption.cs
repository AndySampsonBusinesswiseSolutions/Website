using System;
using System.Data;

namespace Entity
{
    public partial class Temp
    {
        public partial class CustomerDataUpload
        {
            public class MeterExemption
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

                private string _MPXN;
                public string MPXN
                {
                    get { return _MPXN; }
                    set { _MPXN = value; }
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

                private string _ExemptionProduct;
                public string ExemptionProduct
                {
                    get { return _ExemptionProduct; }
                    set { _ExemptionProduct = value; }
                }

                private string _ExemptionProportion;
                public string ExemptionProportion
                {
                    get { return _ExemptionProportion; }
                    set { _ExemptionProportion = value; }
                }

                private bool _CanCommit;
                public bool CanCommit
                {
                    get { return _CanCommit; }
                    set { _CanCommit = value; }
                }

                public MeterExemption(Guid ProcessQueueGUID_,int RowId_,string MPXN_,string DateFrom_,string DateTo_,string ExemptionProduct_,string ExemptionProportion_,bool CanCommit_)
                {
                    this.ProcessQueueGUID = ProcessQueueGUID_;
                    this.RowId = RowId_;
                    this.MPXN = MPXN_;
                    this.DateFrom = DateFrom_;
                    this.DateTo = DateTo_;
                    this.ExemptionProduct = ExemptionProduct_;
                    this.ExemptionProportion = ExemptionProportion_;
                    this.CanCommit = CanCommit_;
                }

                public MeterExemption(DataRow dataRow)
                {
                    this.ProcessQueueGUID = (Guid)dataRow["ProcessQueueGUID"];
                    this.RowId = (int)dataRow["RowId"];
                    this.MPXN = (string)dataRow["MPXN"];
                    this.DateFrom = (string)dataRow["DateFrom"];
                    this.DateTo = (string)dataRow["DateTo"];
                    this.ExemptionProduct = (string)dataRow["ExemptionProduct"];
                    this.ExemptionProportion = (string)dataRow["ExemptionProportion"];
                    this.CanCommit = (bool)dataRow["CanCommit"];
                }
            }
        }
    }
}