using System;
using System.Data;

namespace Entity
{
    public partial class Mapping
    {
        public class GranularityToTimePeriod_StandardDate
        {
            private long _GranularityToTimePeriod_StandardDateId;
            public long GranularityToTimePeriod_StandardDateId
            {
                get { return _GranularityToTimePeriod_StandardDateId; }
                set { _GranularityToTimePeriod_StandardDateId = value; }
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

            private long _GranularityId;
            public long GranularityId
            {
                get { return _GranularityId; }
                set { _GranularityId = value; }
            }

            private long _TimePeriodId;
            public long TimePeriodId
            {
                get { return _TimePeriodId; }
                set { _TimePeriodId = value; }
            }


            public GranularityToTimePeriod_StandardDate(long GranularityToTimePeriod_StandardDateId_,DateTime EffectiveFromDateTime_,DateTime EffectiveToDateTime_,DateTime CreatedDateTime_,long CreatedByUserId_,long SourceId_,long GranularityId_,long TimePeriodId_)
            {
                this.GranularityToTimePeriod_StandardDateId = GranularityToTimePeriod_StandardDateId_;
                this.EffectiveFromDateTime = EffectiveFromDateTime_;
                this.EffectiveToDateTime = EffectiveToDateTime_;
                this.CreatedDateTime = CreatedDateTime_;
                this.CreatedByUserId = CreatedByUserId_;
                this.SourceId = SourceId_;
                this.GranularityId = GranularityId_;
                this.TimePeriodId = TimePeriodId_;
            }

            public GranularityToTimePeriod_StandardDate(DataRow dataRow)
            {
                this.GranularityToTimePeriod_StandardDateId = (long)dataRow["GranularityToTimePeriod_StandardDateId"];
                this.EffectiveFromDateTime = (DateTime)dataRow["EffectiveFromDateTime"];
                this.EffectiveToDateTime = (DateTime)dataRow["EffectiveToDateTime"];
                this.CreatedDateTime = (DateTime)dataRow["CreatedDateTime"];
                this.CreatedByUserId = (long)dataRow["CreatedByUserId"];
                this.SourceId = (long)dataRow["SourceId"];
                this.GranularityId = (long)dataRow["GranularityId"];
                this.TimePeriodId = (long)dataRow["TimePeriodId"];
            }
        }
    }
}