using System;
using System.Data;

namespace Entity
{
    public partial class Mapping
    {
        public class DateToWeek
        {
            private long _DateToWeekId;
            public long DateToWeekId
            {
                get { return _DateToWeekId; }
                set { _DateToWeekId = value; }
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

            private long _DateId;
            public long DateId
            {
                get { return _DateId; }
                set { _DateId = value; }
            }

            private long _WeekId;
            public long WeekId
            {
                get { return _WeekId; }
                set { _WeekId = value; }
            }

            public DateToWeek(long DateToWeekId_,DateTime EffectiveFromDateTime_,DateTime EffectiveToDateTime_,DateTime CreatedDateTime_,long CreatedByUserId_,long SourceId_,long DateId_,long WeekId_)
            {
                this.DateToWeekId = DateToWeekId_;
                this.EffectiveFromDateTime = EffectiveFromDateTime_;
                this.EffectiveToDateTime = EffectiveToDateTime_;
                this.CreatedDateTime = CreatedDateTime_;
                this.CreatedByUserId = CreatedByUserId_;
                this.SourceId = SourceId_;
                this.DateId = DateId_;
                this.WeekId = WeekId_;
            }

            public DateToWeek(DataRow dataRow)
            {
                this.DateToWeekId = (long)dataRow["DateToWeekId"];
                this.EffectiveFromDateTime = (DateTime)dataRow["EffectiveFromDateTime"];
                this.EffectiveToDateTime = (DateTime)dataRow["EffectiveToDateTime"];
                this.CreatedDateTime = (DateTime)dataRow["CreatedDateTime"];
                this.CreatedByUserId = (long)dataRow["CreatedByUserId"];
                this.SourceId = (long)dataRow["SourceId"];
                this.DateId = (long)dataRow["DateId"];
                this.WeekId = (long)dataRow["WeekId"];
            }
        }
    }
}