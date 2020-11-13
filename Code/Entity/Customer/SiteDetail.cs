using System;
using System.Data;

namespace Entity
{
    public partial class Customer
    {
        public class SiteDetail
        {
            private long _SiteDetailId;
            public long SiteDetailId
            {
                get { return _SiteDetailId; }
                set { _SiteDetailId = value; }
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

            private long _SiteId;
            public long SiteId
            {
                get { return _SiteId; }
                set { _SiteId = value; }
            }

            private long _SiteAttributeId;
            public long SiteAttributeId
            {
                get { return _SiteAttributeId; }
                set { _SiteAttributeId = value; }
            }

            private string _SiteDetailDescription;
            public string SiteDetailDescription
            {
                get { return _SiteDetailDescription; }
                set { _SiteDetailDescription = value; }
            }


            public SiteDetail(long SiteDetailId_,DateTime EffectiveFromDateTime_,DateTime EffectiveToDateTime_,DateTime CreatedDateTime_,long CreatedByUserId_,long SourceId_,long SiteId_,long SiteAttributeId_,string SiteDetailDescription_)
            {
                this.SiteDetailId = SiteDetailId_;
                this.EffectiveFromDateTime = EffectiveFromDateTime_;
                this.EffectiveToDateTime = EffectiveToDateTime_;
                this.CreatedDateTime = CreatedDateTime_;
                this.CreatedByUserId = CreatedByUserId_;
                this.SourceId = SourceId_;
                this.SiteId = SiteId_;
                this.SiteAttributeId = SiteAttributeId_;
                this.SiteDetailDescription = SiteDetailDescription_;
            }

            public SiteDetail(DataRow dataRow)
            {
                this.SiteDetailId = (long)dataRow["SiteDetailId"];
                this.EffectiveFromDateTime = (DateTime)dataRow["EffectiveFromDateTime"];
                this.EffectiveToDateTime = (DateTime)dataRow["EffectiveToDateTime"];
                this.CreatedDateTime = (DateTime)dataRow["CreatedDateTime"];
                this.CreatedByUserId = (long)dataRow["CreatedByUserId"];
                this.SourceId = (long)dataRow["SourceId"];
                this.SiteId = (long)dataRow["SiteId"];
                this.SiteAttributeId = (long)dataRow["SiteAttributeId"];
                this.SiteDetailDescription = (string)dataRow["SiteDetailDescription"];
            }
        }
    }
}