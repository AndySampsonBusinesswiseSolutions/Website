using System;
using System.Data;

namespace Entity
{
    public partial class Customer
    {
        public class CustomerDetail
        {
            private long _CustomerDetailId;
            public long CustomerDetailId
            {
                get { return _CustomerDetailId; }
                set { _CustomerDetailId = value; }
            }

            private DateTime _EffectiveFromDateTime;
            public DateTime EffectiveFromDateTime
            {
                get { return _EffectiveFromDateTime; }
                set { _EffectiveFromDateTime = value; }
            }

            private DateTime _EffectiveToDateTime;
            public DateTime EffectiveToDateTime
            {
                get { return _EffectiveToDateTime; }
                set { _EffectiveToDateTime = value; }
            }

            private DateTime _CreatedDateTime;
            public DateTime CreatedDateTime
            {
                get { return _CreatedDateTime; }
                set { _CreatedDateTime = value; }
            }

            private long _CreatedByUserId;
            public long CreatedByUserId
            {
                get { return _CreatedByUserId; }
                set { _CreatedByUserId = value; }
            }

            private long _SourceId;
            public long SourceId
            {
                get { return _SourceId; }
                set { _SourceId = value; }
            }

            private long _CustomerId;
            public long CustomerId
            {
                get { return _CustomerId; }
                set { _CustomerId = value; }
            }

            private long _CustomerAttributeId;
            public long CustomerAttributeId
            {
                get { return _CustomerAttributeId; }
                set { _CustomerAttributeId = value; }
            }

            private string _CustomerDetailDescription;
            public string CustomerDetailDescription
            {
                get { return _CustomerDetailDescription; }
                set { _CustomerDetailDescription = value; }
            }

            public CustomerDetail(long CustomerDetailId_,DateTime EffectiveFromDateTime_,DateTime EffectiveToDateTime_,DateTime CreatedDateTime_,long CreatedByUserId_,long SourceId_,long CustomerId_,long CustomerAttributeId_,string CustomerDetailDescription_)
            {
                this.CustomerDetailId = CustomerDetailId_;
                this.EffectiveFromDateTime = EffectiveFromDateTime_;
                this.EffectiveToDateTime = EffectiveToDateTime_;
                this.CreatedDateTime = CreatedDateTime_;
                this.CreatedByUserId = CreatedByUserId_;
                this.SourceId = SourceId_;
                this.CustomerId = CustomerId_;
                this.CustomerAttributeId = CustomerAttributeId_;
                this.CustomerDetailDescription = CustomerDetailDescription_;
            }

            public CustomerDetail(DataRow dataRow)
            {
                this.CustomerDetailId = (long)dataRow["CustomerDetailId"];
                this.EffectiveFromDateTime = (DateTime)dataRow["EffectiveFromDateTime"];
                this.EffectiveToDateTime = (DateTime)dataRow["EffectiveToDateTime"];
                this.CreatedDateTime = (DateTime)dataRow["CreatedDateTime"];
                this.CreatedByUserId = (long)dataRow["CreatedByUserId"];
                this.SourceId = (long)dataRow["SourceId"];
                this.CustomerId = (long)dataRow["CustomerId"];
                this.CustomerAttributeId = (long)dataRow["CustomerAttributeId"];
                this.CustomerDetailDescription = (string)dataRow["CustomerDetailDescription"];
            }
        }
    }
}