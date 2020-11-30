using System;
using System.Data;

namespace Entity
{
    public partial class Customer
    {
        public class SubMeterDetail
        {
            private long _SubMeterDetailId;
            public long SubMeterDetailId
            {
                get { return _SubMeterDetailId; }
                set { _SubMeterDetailId = value; }
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

            private long _SubMeterId;
            public long SubMeterId
            {
                get { return _SubMeterId; }
                set { _SubMeterId = value; }
            }

            private long _SubMeterAttributeId;
            public long SubMeterAttributeId
            {
                get { return _SubMeterAttributeId; }
                set { _SubMeterAttributeId = value; }
            }

            private string _SubMeterDetailDescription;
            public string SubMeterDetailDescription
            {
                get { return _SubMeterDetailDescription; }
                set { _SubMeterDetailDescription = value; }
            }


            public SubMeterDetail(long SubMeterDetailId_,DateTime EffectiveFromDateTime_,DateTime EffectiveToDateTime_,DateTime CreatedDateTime_,long CreatedByUserId_,long SourceId_,long SubMeterId_,long SubMeterAttributeId_,string SubMeterDetailDescription_)
            {
                this.SubMeterDetailId = SubMeterDetailId_;
                this.EffectiveFromDateTime = EffectiveFromDateTime_;
                this.EffectiveToDateTime = EffectiveToDateTime_;
                this.CreatedDateTime = CreatedDateTime_;
                this.CreatedByUserId = CreatedByUserId_;
                this.SourceId = SourceId_;
                this.SubMeterId = SubMeterId_;
                this.SubMeterAttributeId = SubMeterAttributeId_;
                this.SubMeterDetailDescription = SubMeterDetailDescription_;
            }

            public SubMeterDetail(DataRow dataRow)
            {
                this.SubMeterDetailId = (long)dataRow["SubMeterDetailId"];
                this.EffectiveFromDateTime = (DateTime)dataRow["EffectiveFromDateTime"];
                this.EffectiveToDateTime = (DateTime)dataRow["EffectiveToDateTime"];
                this.CreatedDateTime = (DateTime)dataRow["CreatedDateTime"];
                this.CreatedByUserId = (long)dataRow["CreatedByUserId"];
                this.SourceId = (long)dataRow["SourceId"];
                this.SubMeterId = (long)dataRow["SubMeterId"];
                this.SubMeterAttributeId = (long)dataRow["SubMeterAttributeId"];
                this.SubMeterDetailDescription = (string)dataRow["SubMeterDetailDescription"];
            }
        }
    }
}