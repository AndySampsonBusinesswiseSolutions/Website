using System;
using System.Data;

namespace Entity
{
    public partial class Information
    {
        public class MeterTimeswitchCodeDetail
        {
            private long _MeterTimeswitchCodeDetailId;
            public long MeterTimeswitchCodeDetailId
            {
                get { return _MeterTimeswitchCodeDetailId; }
                set { _MeterTimeswitchCodeDetailId = value; }
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

            private long _MeterTimeswitchCodeId;
            public long MeterTimeswitchCodeId
            {
                get { return _MeterTimeswitchCodeId; }
                set { _MeterTimeswitchCodeId = value; }
            }

            private long _MeterTimeswitchCodeAttributeId;
            public long MeterTimeswitchCodeAttributeId
            {
                get { return _MeterTimeswitchCodeAttributeId; }
                set { _MeterTimeswitchCodeAttributeId = value; }
            }

            private string _MeterTimeswitchCodeDetailDescription;
            public string MeterTimeswitchCodeDetailDescription
            {
                get { return _MeterTimeswitchCodeDetailDescription; }
                set { _MeterTimeswitchCodeDetailDescription = value; }
            }


            public MeterTimeswitchCodeDetail(long MeterTimeswitchCodeDetailId_,DateTime EffectiveFromDateTime_,DateTime EffectiveToDateTime_,DateTime CreatedDateTime_,long CreatedByUserId_,long SourceId_,long MeterTimeswitchCodeId_,long MeterTimeswitchCodeAttributeId_,string MeterTimeswitchCodeDetailDescription_)
            {
                this.MeterTimeswitchCodeDetailId = MeterTimeswitchCodeDetailId_;
                this.EffectiveFromDateTime = EffectiveFromDateTime_;
                this.EffectiveToDateTime = EffectiveToDateTime_;
                this.CreatedDateTime = CreatedDateTime_;
                this.CreatedByUserId = CreatedByUserId_;
                this.SourceId = SourceId_;
                this.MeterTimeswitchCodeId = MeterTimeswitchCodeId_;
                this.MeterTimeswitchCodeAttributeId = MeterTimeswitchCodeAttributeId_;
                this.MeterTimeswitchCodeDetailDescription = MeterTimeswitchCodeDetailDescription_;
            }

            public MeterTimeswitchCodeDetail(DataRow dataRow)
            {
                this.MeterTimeswitchCodeDetailId = (long)dataRow["MeterTimeswitchCodeDetailId"];
                this.EffectiveFromDateTime = (DateTime)dataRow["EffectiveFromDateTime"];
                this.EffectiveToDateTime = (DateTime)dataRow["EffectiveToDateTime"];
                this.CreatedDateTime = (DateTime)dataRow["CreatedDateTime"];
                this.CreatedByUserId = (long)dataRow["CreatedByUserId"];
                this.SourceId = (long)dataRow["SourceId"];
                this.MeterTimeswitchCodeId = (long)dataRow["MeterTimeswitchCodeId"];
                this.MeterTimeswitchCodeAttributeId = (long)dataRow["MeterTimeswitchCodeAttributeId"];
                this.MeterTimeswitchCodeDetailDescription = (string)dataRow["MeterTimeswitchCodeDetailDescription"];
            }
        }
    }
}