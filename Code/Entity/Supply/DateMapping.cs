using System;
using System.Data;

namespace Entity
{
    public partial class Supply
    {
        public class DateMapping
        {
            private long _DateMappingId;
            public long DateMappingId
            {
                get { return _DateMappingId; }
                set { _DateMappingId = value; }
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

            private long _MappedDateId;
            public long MappedDateId
            {
                get { return _MappedDateId; }
                set { _MappedDateId = value; }
            }

            public DateMapping(long DateMappingId_,DateTime EffectiveFromDateTime_,DateTime EffectiveToDateTime_,DateTime CreatedDateTime_,long CreatedByUserId_,long SourceId_,long DateId_,long MappedDateId_)
            {
                this.DateMappingId = DateMappingId_;
                this.EffectiveFromDateTime = EffectiveFromDateTime_;
                this.EffectiveToDateTime = EffectiveToDateTime_;
                this.CreatedDateTime = CreatedDateTime_;
                this.CreatedByUserId = CreatedByUserId_;
                this.SourceId = SourceId_;
                this.DateId = DateId_;
                this.MappedDateId = MappedDateId_;
            }

            public DateMapping(DataRow dataRow)
            {
                this.DateMappingId = (long)dataRow["DateMappingId"];
                this.EffectiveFromDateTime = (DateTime)dataRow["EffectiveFromDateTime"];
                this.EffectiveToDateTime = (DateTime)dataRow["EffectiveToDateTime"];
                this.CreatedDateTime = (DateTime)dataRow["CreatedDateTime"];
                this.CreatedByUserId = (long)dataRow["CreatedByUserId"];
                this.SourceId = (long)dataRow["SourceId"];
                this.DateId = (long)dataRow["DateId"];
                this.MappedDateId = (long)dataRow["MappedDateId"];
            }
        }
    }
}