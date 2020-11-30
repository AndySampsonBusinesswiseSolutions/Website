using System;
using System.Data;

namespace Entity
{
    public partial class Mapping
    {
        public class ForecastGroupToTimePeriodToProfileToProfileValue
        {
            private long _ForecastGroupToTimePeriodToProfileToProfileValueId;
            public long ForecastGroupToTimePeriodToProfileToProfileValueId
            {
                get { return _ForecastGroupToTimePeriodToProfileToProfileValueId; }
                set { _ForecastGroupToTimePeriodToProfileToProfileValueId = value; }
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

            private long _ForecastGroupToTimePeriodToProfileId;
            public long ForecastGroupToTimePeriodToProfileId
            {
                get { return _ForecastGroupToTimePeriodToProfileId; }
                set { _ForecastGroupToTimePeriodToProfileId = value; }
            }

            private long _ProfileValueId;
            public long ProfileValueId
            {
                get { return _ProfileValueId; }
                set { _ProfileValueId = value; }
            }


            public ForecastGroupToTimePeriodToProfileToProfileValue(long ForecastGroupToTimePeriodToProfileToProfileValueId_,DateTime EffectiveFromDateTime_,DateTime EffectiveToDateTime_,DateTime CreatedDateTime_,long CreatedByUserId_,long SourceId_,long ForecastGroupToTimePeriodToProfileId_,long ProfileValueId_)
            {
                this.ForecastGroupToTimePeriodToProfileToProfileValueId = ForecastGroupToTimePeriodToProfileToProfileValueId_;
                this.EffectiveFromDateTime = EffectiveFromDateTime_;
                this.EffectiveToDateTime = EffectiveToDateTime_;
                this.CreatedDateTime = CreatedDateTime_;
                this.CreatedByUserId = CreatedByUserId_;
                this.SourceId = SourceId_;
                this.ForecastGroupToTimePeriodToProfileId = ForecastGroupToTimePeriodToProfileId_;
                this.ProfileValueId = ProfileValueId_;
            }

            public ForecastGroupToTimePeriodToProfileToProfileValue(DataRow dataRow)
            {
                this.ForecastGroupToTimePeriodToProfileToProfileValueId = (long)dataRow["ForecastGroupToTimePeriodToProfileToProfileValueId"];
                this.EffectiveFromDateTime = (DateTime)dataRow["EffectiveFromDateTime"];
                this.EffectiveToDateTime = (DateTime)dataRow["EffectiveToDateTime"];
                this.CreatedDateTime = (DateTime)dataRow["CreatedDateTime"];
                this.CreatedByUserId = (long)dataRow["CreatedByUserId"];
                this.SourceId = (long)dataRow["SourceId"];
                this.ForecastGroupToTimePeriodToProfileId = (long)dataRow["ForecastGroupToTimePeriodToProfileId"];
                this.ProfileValueId = (long)dataRow["ProfileValueId"];
            }
        }
    }
}