using System;
using System.Data;

namespace Entity
{
    public partial class Temp
    {
        public partial class CustomerDataUpload
        {
            public class FlexContract
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

                private string _ContractReference;
                public string ContractReference
                {
                    get { return _ContractReference; }
                    set { _ContractReference = value; }
                }

                private string _BasketReference;
                public string BasketReference
                {
                    get { return _BasketReference; }
                    set { _BasketReference = value; }
                }

                private string _MPXN;
                public string MPXN
                {
                    get { return _MPXN; }
                    set { _MPXN = value; }
                }

                private string _Supplier;
                public string Supplier
                {
                    get { return _Supplier; }
                    set { _Supplier = value; }
                }

                private string _ContractStartDate;
                public string ContractStartDate
                {
                    get { return _ContractStartDate; }
                    set { _ContractStartDate = value; }
                }

                private string _ContractEndDate;
                public string ContractEndDate
                {
                    get { return _ContractEndDate; }
                    set { _ContractEndDate = value; }
                }

                private string _Product;
                public string Product
                {
                    get { return _Product; }
                    set { _Product = value; }
                }

                private string _RateType;
                public string RateType
                {
                    get { return _RateType; }
                    set { _RateType = value; }
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

                public FlexContract(Guid ProcessQueueGUID_,int RowId_,string ContractReference_,string BasketReference_,string MPXN_,string Supplier_,string ContractStartDate_,string ContractEndDate_,string Product_,string RateType_,string Value_,bool CanCommit_)
                {
                    this.ProcessQueueGUID = ProcessQueueGUID_;
                    this.RowId = RowId_;
                    this.ContractReference = ContractReference_;
                    this.BasketReference = BasketReference_;
                    this.MPXN = MPXN_;
                    this.Supplier = Supplier_;
                    this.ContractStartDate = ContractStartDate_;
                    this.ContractEndDate = ContractEndDate_;
                    this.Product = Product_;
                    this.RateType = RateType_;
                    this.Value = Value_;
                    this.CanCommit = CanCommit_;
                }

                public FlexContract(DataRow dataRow)
                {
                    this.ProcessQueueGUID = (Guid)dataRow["ProcessQueueGUID"];
                    this.RowId = (int)dataRow["RowId"];
                    this.ContractReference = (string)dataRow["ContractReference"];
                    this.BasketReference = (string)dataRow["BasketReference"];
                    this.MPXN = (string)dataRow["MPXN"];
                    this.Supplier = (string)dataRow["Supplier"];
                    this.ContractStartDate = (string)dataRow["ContractStartDate"];
                    this.ContractEndDate = (string)dataRow["ContractEndDate"];
                    this.Product = (string)dataRow["Product"];
                    this.RateType = (string)dataRow["RateType"];
                    this.Value = (string)dataRow["Value"];
                    this.CanCommit = (bool)dataRow["CanCommit"];
                }
            }
        }
    }
}