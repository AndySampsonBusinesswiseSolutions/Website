using System;
using System.Data;

namespace Entity
{
    public partial class Temp
    {
        public partial class CustomerDataUpload
        {
            public class Site
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

                private string _SiteName;
                public string SiteName
                {
                    get { return _SiteName; }
                    set { _SiteName = value; }
                }

                private string _SiteAddress;
                public string SiteAddress
                {
                    get { return _SiteAddress; }
                    set { _SiteAddress = value; }
                }

                private string _SiteTown;
                public string SiteTown
                {
                    get { return _SiteTown; }
                    set { _SiteTown = value; }
                }

                private string _SiteCounty;
                public string SiteCounty
                {
                    get { return _SiteCounty; }
                    set { _SiteCounty = value; }
                }

                private string _SitePostCode;
                public string SitePostCode
                {
                    get { return _SitePostCode; }
                    set { _SitePostCode = value; }
                }

                private string _SiteDescription;
                public string SiteDescription
                {
                    get { return _SiteDescription; }
                    set { _SiteDescription = value; }
                }

                private string _ContactName;
                public string ContactName
                {
                    get { return _ContactName; }
                    set { _ContactName = value; }
                }

                private string _ContactRole;
                public string ContactRole
                {
                    get { return _ContactRole; }
                    set { _ContactRole = value; }
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

                public Site(Guid ProcessQueueGUID_,int RowId_,string CustomerName_,string SiteName_,string SiteAddress_,string SiteTown_,string SiteCounty_,string SitePostCode_,string SiteDescription_,string ContactName_,string ContactRole_,string ContactTelephoneNumber_,string ContactEmailAddress_,bool CanCommit_)
                {
                    this.ProcessQueueGUID = ProcessQueueGUID_;
                    this.RowId = RowId_;
                    this.CustomerName = CustomerName_;
                    this.SiteName = SiteName_;
                    this.SiteAddress = SiteAddress_;
                    this.SiteTown = SiteTown_;
                    this.SiteCounty = SiteCounty_;
                    this.SitePostCode = SitePostCode_;
                    this.SiteDescription = SiteDescription_;
                    this.ContactName = ContactName_;
                    this.ContactRole = ContactRole_;
                    this.ContactTelephoneNumber = ContactTelephoneNumber_;
                    this.ContactEmailAddress = ContactEmailAddress_;
                    this.CanCommit = CanCommit_;
                }

                public Site(DataRow dataRow)
                {
                    this.ProcessQueueGUID = (Guid)dataRow["ProcessQueueGUID"];
                    this.RowId = (int)dataRow["RowId"];
                    this.CustomerName = (string)dataRow["CustomerName"];
                    this.SiteName = (string)dataRow["SiteName"];
                    this.SiteAddress = (string)dataRow["SiteAddress"];
                    this.SiteTown = (string)dataRow["SiteTown"];
                    this.SiteCounty = (string)dataRow["SiteCounty"];
                    this.SitePostCode = (string)dataRow["SitePostCode"];
                    this.SiteDescription = (string)dataRow["SiteDescription"];
                    this.ContactName = (string)dataRow["ContactName"];
                    this.ContactRole = (string)dataRow["ContactRole"];
                    this.ContactTelephoneNumber = (string)dataRow["ContactTelephoneNumber"];
                    this.ContactEmailAddress = (string)dataRow["ContactEmailAddress"];
                    this.CanCommit = (bool)dataRow["CanCommit"];
                }
            }
        }
    }
}