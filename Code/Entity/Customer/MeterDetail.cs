using System;
using System.Data;

namespace Entity
{
    public partial class Customer
    {
        public class MeterDetail
        {
            private long _MeterDetailId;
            public long MeterDetailId
            {
                get { return _MeterDetailId; }
                set { _MeterDetailId = value; }
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

            private long _MeterId;
            public long MeterId
            {
                get { return _MeterId; }
                set { _MeterId = value; }
            }

            private long _MeterAttributeId;
            public long MeterAttributeId
            {
                get { return _MeterAttributeId; }
                set { _MeterAttributeId = value; }
            }

            private string _MeterDetailDescription;
            public string MeterDetailDescription
            {
                get { return _MeterDetailDescription; }
                set { _MeterDetailDescription = value; }
            }


            public MeterDetail(long MeterDetailId_,DateTime EffectiveFromDateTime_,DateTime EffectiveToDateTime_,DateTime CreatedDateTime_,long CreatedByUserId_,long SourceId_,long MeterId_,long MeterAttributeId_,string MeterDetailDescription_)
            {
                this.MeterDetailId = MeterDetailId_;
                this.EffectiveFromDateTime = EffectiveFromDateTime_;
                this.EffectiveToDateTime = EffectiveToDateTime_;
                this.CreatedDateTime = CreatedDateTime_;
                this.CreatedByUserId = CreatedByUserId_;
                this.SourceId = SourceId_;
                this.MeterId = MeterId_;
                this.MeterAttributeId = MeterAttributeId_;
                this.MeterDetailDescription = MeterDetailDescription_;
            }

            public MeterDetail(DataRow dataRow)
            {
                this.MeterDetailId = (long)dataRow["MeterDetailId"];
                this.EffectiveFromDateTime = (DateTime)dataRow["EffectiveFromDateTime"];
                this.EffectiveToDateTime = (DateTime)dataRow["EffectiveToDateTime"];
                this.CreatedDateTime = (DateTime)dataRow["CreatedDateTime"];
                this.CreatedByUserId = (long)dataRow["CreatedByUserId"];
                this.SourceId = (long)dataRow["SourceId"];
                this.MeterId = (long)dataRow["MeterId"];
                this.MeterAttributeId = (long)dataRow["MeterAttributeId"];
                this.MeterDetailDescription = (string)dataRow["MeterDetailDescription"];
            }
        }
    }
}