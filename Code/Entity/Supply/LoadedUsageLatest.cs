using System;
using System.Data;

namespace Entity
{
    public partial class Supply
    {
        public class LoadedUsageLatest
        {
            private long _DateId;
            public long DateId
            {
                get { return _DateId; }
                set { _DateId = value; }
            }

            private long _TimePeriodId;
            public long TimePeriodId
            {
                get { return _TimePeriodId; }
                set { _TimePeriodId = value; }
            }

            private decimal _Usage;
            public decimal Usage
            {
                get { return _Usage; }
                set { _Usage = value; }
            }

            public LoadedUsageLatest(long DateId_,long TimePeriodId_,decimal Usage_)
            {
                this.DateId = DateId_;
                this.TimePeriodId = TimePeriodId_;
                this.Usage = Usage_;
            }

            public LoadedUsageLatest(DataRow dataRow)
            {
                this.DateId = (long)dataRow["DateId"];
                this.TimePeriodId = (long)dataRow["TimePeriodId"];
                this.Usage = (decimal)dataRow["Usage"];
            }
        }
    }
}