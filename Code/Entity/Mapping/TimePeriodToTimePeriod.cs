using System;
using System.Data;

namespace Entity
{
    public partial class Mapping
    {
        public class TimePeriodToTimePeriod
        {
            private long _TimePeriodToTimePeriodId;
            public long TimePeriodToTimePeriodId
            {
                get { return _TimePeriodToTimePeriodId; }
                set { _TimePeriodToTimePeriodId = value; }
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

            private long _TimePeriodId;
            public long TimePeriodId
            {
                get { return _TimePeriodId; }
                set { _TimePeriodId = value; }
            }

            private long _MappedTimePeriodId;
            public long MappedTimePeriodId
            {
                get { return _MappedTimePeriodId; }
                set { _MappedTimePeriodId = value; }
            }


            public TimePeriodToTimePeriod(long TimePeriodToTimePeriodId_,DateTime EffectiveFromDateTime_,DateTime EffectiveToDateTime_,DateTime CreatedDateTime_,long CreatedByUserId_,long SourceId_,long TimePeriodId_,long MappedTimePeriodId_)
            {
                this.TimePeriodToTimePeriodId = TimePeriodToTimePeriodId_;
                this.EffectiveFromDateTime = EffectiveFromDateTime_;
                this.EffectiveToDateTime = EffectiveToDateTime_;
                this.CreatedDateTime = CreatedDateTime_;
                this.CreatedByUserId = CreatedByUserId_;
                this.SourceId = SourceId_;
                this.TimePeriodId = TimePeriodId_;
                this.MappedTimePeriodId = MappedTimePeriodId_;
            }

            public TimePeriodToTimePeriod(DataRow dataRow)
            {
                this.TimePeriodToTimePeriodId = (long)dataRow["TimePeriodToTimePeriodId"];
                this.EffectiveFromDateTime = (DateTime)dataRow["EffectiveFromDateTime"];
                this.EffectiveToDateTime = (DateTime)dataRow["EffectiveToDateTime"];
                this.CreatedDateTime = (DateTime)dataRow["CreatedDateTime"];
                this.CreatedByUserId = (long)dataRow["CreatedByUserId"];
                this.SourceId = (long)dataRow["SourceId"];
                this.TimePeriodId = (long)dataRow["TimePeriodId"];
                this.MappedTimePeriodId = (long)dataRow["MappedTimePeriodId"];
            }
        }
    }
}