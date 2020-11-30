using System;
using System.Data;

namespace Entity
{
    public partial class Mapping
    {
        public class DateToQuarter
        {
            private long _DateToQuarterId;
            public long DateToQuarterId
            {
                get { return _DateToQuarterId; }
                set { _DateToQuarterId = value; }
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

            private long _QuarterId;
            public long QuarterId
            {
                get { return _QuarterId; }
                set { _QuarterId = value; }
            }

            public DateToQuarter(long DateToQuarterId_,DateTime EffectiveFromDateTime_,DateTime EffectiveToDateTime_,DateTime CreatedDateTime_,long CreatedByUserId_,long SourceId_,long DateId_,long QuarterId_)
            {
                this.DateToQuarterId = DateToQuarterId_;
                this.EffectiveFromDateTime = EffectiveFromDateTime_;
                this.EffectiveToDateTime = EffectiveToDateTime_;
                this.CreatedDateTime = CreatedDateTime_;
                this.CreatedByUserId = CreatedByUserId_;
                this.SourceId = SourceId_;
                this.DateId = DateId_;
                this.QuarterId = QuarterId_;
            }

            public DateToQuarter(DataRow dataRow)
            {
                this.DateToQuarterId = (long)dataRow["DateToQuarterId"];
                this.EffectiveFromDateTime = (DateTime)dataRow["EffectiveFromDateTime"];
                this.EffectiveToDateTime = (DateTime)dataRow["EffectiveToDateTime"];
                this.CreatedDateTime = (DateTime)dataRow["CreatedDateTime"];
                this.CreatedByUserId = (long)dataRow["CreatedByUserId"];
                this.SourceId = (long)dataRow["SourceId"];
                this.DateId = (long)dataRow["DateId"];
                this.QuarterId = (long)dataRow["QuarterId"];
            }
        }
    }
}