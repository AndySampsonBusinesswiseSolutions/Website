using System;
using System.Data;

namespace Entity
{
    public partial class Temp
    {
        public partial class CustomerDataUpload
        {
            public class Customer
            {
                private Guid _ProcessQueueGUID;
                public Guid ProcessQueueGUID
                {
                    get { return _ProcessQueueGUID; }
                    set { _ProcessQueueGUID = value; }
                }

                private int _RowId;
                public int RowId
                {
                    get { return _RowId; }
                    set { _RowId = value; }
                }

                private string _CustomerName;
                public string CustomerName
                {
                    get { return _CustomerName; }
                    set { _CustomerName = value; }
                }

                private string _ContactName;
                public string ContactName
                {
                    get { return _ContactName; }
                    set { _ContactName = value; }
                }

                private string _ContactTelephoneNumber;
                public string ContactTelephoneNumber
                {
                    get { return _ContactTelephoneNumber; }
                    set { _ContactTelephoneNumber = value; }
                }

                private string _ContactEmailAddress;
                public string ContactEmailAddress
                {
                    get { return _ContactEmailAddress; }
                    set { _ContactEmailAddress = value; }
                }

                private bool _CanCommit;
                public bool CanCommit
                {
                    get { return _CanCommit; }
                    set { _CanCommit = value; }
                }

                public Customer(Guid ProcessQueueGUID_,int RowId_,string CustomerName_,string ContactName_,string ContactTelephoneNumber_,string ContactEmailAddress_,bool CanCommit_)
                {
                    this.ProcessQueueGUID = ProcessQueueGUID_;
                    this.RowId = RowId_;
                    this.CustomerName = CustomerName_;
                    this.ContactName = ContactName_;
                    this.ContactTelephoneNumber = ContactTelephoneNumber_;
                    this.ContactEmailAddress = ContactEmailAddress_;
                    this.CanCommit = CanCommit_;
                }

                public Customer(DataRow dataRow)
                {
                    this.ProcessQueueGUID = (Guid)dataRow["ProcessQueueGUID"];
                    this.RowId = (int)dataRow["RowId"];
                    this.CustomerName = (string)dataRow["CustomerName"];
                    this.ContactName = (string)dataRow["ContactName"];
                    this.ContactTelephoneNumber = (string)dataRow["ContactTelephoneNumber"];
                    this.ContactEmailAddress = (string)dataRow["ContactEmailAddress"];
                    this.CanCommit = (bool)dataRow["CanCommit"];
                }
            }
        }
    }
}