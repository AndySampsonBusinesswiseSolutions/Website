using System;
using System.Data;

namespace Entity
{
    public partial class Mapping
    {
        public class ForecastGroupToTimePeriodToProfile
        {
            private long _ForecastGroupToTimePeriodToProfileId;
            public long ForecastGroupToTimePeriodToProfileId
            {
                get { return _ForecastGroupToTimePeriodToProfileId; }
                set { _ForecastGroupToTimePeriodToProfileId = value; }
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

            private long _ForecastGroupToTimePeriodId;
            public long ForecastGroupToTimePeriodId
            {
                get { return _ForecastGroupToTimePeriodId; }
                set { _ForecastGroupToTimePeriodId = value; }
            }

            private long _ProfileId;
            public long ProfileId
            {
                get { return _ProfileId; }
                set { _ProfileId = value; }
            }


            public ForecastGroupToTimePeriodToProfile(long ForecastGroupToTimePeriodToProfileId_,DateTime EffectiveFromDateTime_,DateTime EffectiveToDateTime_,DateTime CreatedDateTime_,long CreatedByUserId_,long SourceId_,long ForecastGroupToTimePeriodId_,long ProfileId_)
            {
                this.ForecastGroupToTimePeriodToProfileId = ForecastGroupToTimePeriodToProfileId_;
                this.EffectiveFromDateTime = EffectiveFromDateTime_;
                this.EffectiveToDateTime = EffectiveToDateTime_;
                this.CreatedDateTime = CreatedDateTime_;
                this.CreatedByUserId = CreatedByUserId_;
                this.SourceId = SourceId_;
                this.ForecastGroupToTimePeriodId = ForecastGroupToTimePeriodId_;
                this.ProfileId = ProfileId_;
            }

            public ForecastGroupToTimePeriodToProfile(DataRow dataRow)
            {
                this.ForecastGroupToTimePeriodToProfileId = (long)dataRow["ForecastGroupToTimePeriodToProfileId"];
                this.EffectiveFromDateTime = (DateTime)dataRow["EffectiveFromDateTime"];
                this.EffectiveToDateTime = (DateTime)dataRow["EffectiveToDateTime"];
                this.CreatedDateTime = (DateTime)dataRow["CreatedDateTime"];
                this.CreatedByUserId = (long)dataRow["CreatedByUserId"];
                this.SourceId = (long)dataRow["SourceId"];
                this.ForecastGroupToTimePeriodId = (long)dataRow["ForecastGroupToTimePeriodId"];
                this.ProfileId = (long)dataRow["ProfileId"];
            }
        }
    }
}