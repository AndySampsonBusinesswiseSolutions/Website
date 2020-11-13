using System;
using System.Data;

namespace Entity
{
    public partial class Customer
    {
        public class TradeDetail
        {
            private long _TradeDetailId;
            public long TradeDetailId
            {
                get { return _TradeDetailId; }
                set { _TradeDetailId = value; }
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

            private long _TradeId;
            public long TradeId
            {
                get { return _TradeId; }
                set { _TradeId = value; }
            }

            private long _TradeAttributeId;
            public long TradeAttributeId
            {
                get { return _TradeAttributeId; }
                set { _TradeAttributeId = value; }
            }

            private string _TradeDetailDescription;
            public string TradeDetailDescription
            {
                get { return _TradeDetailDescription; }
                set { _TradeDetailDescription = value; }
            }

            public TradeDetail(long TradeDetailId_,DateTime EffectiveFromDateTime_,DateTime EffectiveToDateTime_,DateTime CreatedDateTime_,long CreatedByUserId_,long SourceId_,long TradeId_,long TradeAttributeId_,string TradeDetailDescription_)
            {
                this.TradeDetailId = TradeDetailId_;
                this.EffectiveFromDateTime = EffectiveFromDateTime_;
                this.EffectiveToDateTime = EffectiveToDateTime_;
                this.CreatedDateTime = CreatedDateTime_;
                this.CreatedByUserId = CreatedByUserId_;
                this.SourceId = SourceId_;
                this.TradeId = TradeId_;
                this.TradeAttributeId = TradeAttributeId_;
                this.TradeDetailDescription = TradeDetailDescription_;
            }

            public TradeDetail(DataRow dataRow)
            {
                this.TradeDetailId = (long)dataRow["TradeDetailId"];
                this.EffectiveFromDateTime = (DateTime)dataRow["EffectiveFromDateTime"];
                this.EffectiveToDateTime = (DateTime)dataRow["EffectiveToDateTime"];
                this.CreatedDateTime = (DateTime)dataRow["CreatedDateTime"];
                this.CreatedByUserId = (long)dataRow["CreatedByUserId"];
                this.SourceId = (long)dataRow["SourceId"];
                this.TradeId = (long)dataRow["TradeId"];
                this.TradeAttributeId = (long)dataRow["TradeAttributeId"];
                this.TradeDetailDescription = (string)dataRow["TradeDetailDescription"];
            }
        }
    }
}