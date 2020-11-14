using System;
using System.Data;

namespace Entity
{
    public partial class Mapping
    {
        public class ForecastGroupToTimePeriod
        {
            private long _ForecastGroupToTimePeriodId;
            public long ForecastGroupToTimePeriodId
            {
                get { return _ForecastGroupToTimePeriodId; }
                set { _ForecastGroupToTimePeriodId = value; }
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

            private long _ForecastGroupId;
            public long ForecastGroupId
            {
                get { return _ForecastGroupId; }
                set { _ForecastGroupId = value; }
            }

            private long _TimePeriodId;
            public long TimePeriodId
            {
                get { return _TimePeriodId; }
                set { _TimePeriodId = value; }
            }


            public ForecastGroupToTimePeriod(long ForecastGroupToTimePeriodId_,DateTime EffectiveFromDateTime_,DateTime EffectiveToDateTime_,DateTime CreatedDateTime_,long CreatedByUserId_,long SourceId_,long ForecastGroupId_,long TimePeriodId_)
            {
                this.ForecastGroupToTimePeriodId = ForecastGroupToTimePeriodId_;
                this.EffectiveFromDateTime = EffectiveFromDateTime_;
                this.EffectiveToDateTime = EffectiveToDateTime_;
                this.CreatedDateTime = CreatedDateTime_;
                this.CreatedByUserId = CreatedByUserId_;
                this.SourceId = SourceId_;
                this.ForecastGroupId = ForecastGroupId_;
                this.TimePeriodId = TimePeriodId_;
            }

            public ForecastGroupToTimePeriod(DataRow dataRow)
            {
                this.ForecastGroupToTimePeriodId = (long)dataRow["ForecastGroupToTimePeriodId"];
                this.EffectiveFromDateTime = (DateTime)dataRow["EffectiveFromDateTime"];
                this.EffectiveToDateTime = (DateTime)dataRow["EffectiveToDateTime"];
                this.CreatedDateTime = (DateTime)dataRow["CreatedDateTime"];
                this.CreatedByUserId = (long)dataRow["CreatedByUserId"];
                this.SourceId = (long)dataRow["SourceId"];
                this.ForecastGroupId = (long)dataRow["ForecastGroupId"];
                this.TimePeriodId = (long)dataRow["TimePeriodId"];
            }
        }
    }
}