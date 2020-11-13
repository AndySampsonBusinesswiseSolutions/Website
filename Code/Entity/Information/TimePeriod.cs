using System;
using System.Data;

namespace Entity
{
    public partial class Information
    {
        public class TimePeriod
        {
            private long _TimePeriodId;
            public long TimePeriodId
            {
                get { return _TimePeriodId; }
                set { _TimePeriodId = value; }
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

            private TimeSpan _StartTime;
            public TimeSpan StartTime
            {
                get { return _StartTime; }
                set { _StartTime = value; }
            }

            private TimeSpan _EndTime;
            public TimeSpan EndTime
            {
                get { return _EndTime; }
                set { _EndTime = value; }
            }


            public TimePeriod(long TimePeriodId_,DateTime EffectiveFromDateTime_,DateTime EffectiveToDateTime_,DateTime CreatedDateTime_,long CreatedByUserId_,long SourceId_,TimeSpan StartTime_,TimeSpan EndTime_)
            {
                this.TimePeriodId = TimePeriodId_;
                this.EffectiveFromDateTime = EffectiveFromDateTime_;
                this.EffectiveToDateTime = EffectiveToDateTime_;
                this.CreatedDateTime = CreatedDateTime_;
                this.CreatedByUserId = CreatedByUserId_;
                this.SourceId = SourceId_;
                this.StartTime = StartTime_;
                this.EndTime = EndTime_;
            }

            public TimePeriod(DataRow dataRow)
            {
                this.TimePeriodId = (long)dataRow["TimePeriodId"];
                this.EffectiveFromDateTime = (DateTime)dataRow["EffectiveFromDateTime"];
                this.EffectiveToDateTime = (DateTime)dataRow["EffectiveToDateTime"];
                this.CreatedDateTime = (DateTime)dataRow["CreatedDateTime"];
                this.CreatedByUserId = (long)dataRow["CreatedByUserId"];
                this.SourceId = (long)dataRow["SourceId"];
                this.StartTime = (TimeSpan)dataRow["StartTime"];
                this.EndTime = (TimeSpan)dataRow["EndTime"];
            }
        }
    }
}