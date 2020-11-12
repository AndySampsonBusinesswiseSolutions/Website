using System;
using System.Data;

namespace Entity
{
    public partial class Temp
    {
        public partial class CustomerDataUpload
        {
            public class Meter
            {
                private Guid _ProcessQueueGUID;
                public Guid ProcessQueueGUID
                {
                    get { return _ProcessQueueGUID; }
                    set { _ProcessQueueGUID = value; }
                }

                private int? _RowId;
                public int? RowId
                {
                    get { return _RowId; }
                    set { _RowId = value; }
                }

                private string _SiteName;
                public string SiteName
                {
                    get { return _SiteName; }
                    set { _SiteName = value; }
                }

                private string _SitePostCode;
                public string SitePostCode
                {
                    get { return _SitePostCode; }
                    set { _SitePostCode = value; }
                }

                private string _MPXN;
                public string MPXN
                {
                    get { return _MPXN; }
                    set { _MPXN = value; }
                }

                private string _GridSupplyPoint;
                public string GridSupplyPoint
                {
                    get { return _GridSupplyPoint; }
                    set { _GridSupplyPoint = value.StartsWith("_") ? value : $"_{value}"; }
                }

                private string _ProfileClass;
                public string ProfileClass
                {
                    get { return _ProfileClass; }
                    set { _ProfileClass = string.IsNullOrWhiteSpace(value) ? string.Empty : value.PadLeft(2, '0'); }
                }

                private string _MeterTimeswitchCode;
                public string MeterTimeswitchCode
                {
                    get { return _MeterTimeswitchCode; }
                    set { _MeterTimeswitchCode = string.IsNullOrWhiteSpace(value) ? string.Empty : value.PadLeft(3, '0'); }
                }

                private string _LineLossFactorClass;
                public string LineLossFactorClass
                {
                    get { return _LineLossFactorClass; }
                    set { _LineLossFactorClass = string.IsNullOrWhiteSpace(value) ? string.Empty : value.PadLeft(3, '0'); }
                }

                private string _Capacity;
                public string Capacity
                {
                    get { return _Capacity; }
                    set { _Capacity = value; }
                }

                private string _LocalDistributionZone;
                public string LocalDistributionZone
                {
                    get { return _LocalDistributionZone; }
                    set { _LocalDistributionZone = value; }
                }

                private string _StandardOfftakeQuantity;
                public string StandardOfftakeQuantity
                {
                    get { return _StandardOfftakeQuantity; }
                    set { _StandardOfftakeQuantity = value; }
                }

                private string _AnnualUsage;
                public string AnnualUsage
                {
                    get { return _AnnualUsage; }
                    set { _AnnualUsage = value; }
                }

                private string _MeterSerialNumber;
                public string MeterSerialNumber
                {
                    get { return _MeterSerialNumber; }
                    set { _MeterSerialNumber = value; }
                }

                private string _Area;
                public string Area
                {
                    get { return _Area; }
                    set { _Area = value; }
                }

                private string _ImportExport;
                public string ImportExport
                {
                    get { return _ImportExport; }
                    set { _ImportExport = value; }
                }

                private bool _CanCommit;
                public bool CanCommit
                {
                    get { return _CanCommit; }
                    set { _CanCommit = value; }
                }

                public Meter(Guid ProcessQueueGUID_,int RowId_,string SiteName_,string SitePostCode_,string MPXN_,string GridSupplyPoint_,string ProfileClass_,string MeterTimeswitchCode_,string LineLossFactorClass_,string Capacity_,string LocalDistributionZone_,string StandardOfftakeQuantity_,string AnnualUsage_,string MeterSerialNumber_,string Area_,string ImportExport_,bool CanCommit_)
                {
                    this.ProcessQueueGUID = ProcessQueueGUID_;
                    this.RowId = RowId_;
                    this.SiteName = SiteName_;
                    this.SitePostCode = SitePostCode_;
                    this.MPXN = MPXN_;
                    this.GridSupplyPoint = GridSupplyPoint_;
                    this.ProfileClass = ProfileClass_;
                    this.MeterTimeswitchCode = MeterTimeswitchCode_;
                    this.LineLossFactorClass = LineLossFactorClass_;
                    this.Capacity = Capacity_;
                    this.LocalDistributionZone = LocalDistributionZone_;
                    this.StandardOfftakeQuantity = StandardOfftakeQuantity_;
                    this.AnnualUsage = AnnualUsage_;
                    this.MeterSerialNumber = MeterSerialNumber_;
                    this.Area = Area_;
                    this.ImportExport = ImportExport_;
                    this.CanCommit = CanCommit_;
                }

                public Meter(DataRow dataRow)
                {
                    this.ProcessQueueGUID = (Guid)dataRow["ProcessQueueGUID"];
                    this.RowId = (int)dataRow["RowId"];
                    this.SiteName = (string)dataRow["SiteName"];
                    this.SitePostCode = (string)dataRow["SitePostCode"];
                    this.MPXN = (string)dataRow["MPXN"];
                    this.GridSupplyPoint = (string)dataRow["GridSupplyPoint"];
                    this.ProfileClass = (string)dataRow["ProfileClass"];
                    this.MeterTimeswitchCode = (string)dataRow["MeterTimeswitchCode"];
                    this.LineLossFactorClass = (string)dataRow["LineLossFactorClass"];
                    this.Capacity = (string)dataRow["Capacity"];
                    this.LocalDistributionZone = (string)dataRow["LocalDistributionZone"];
                    this.StandardOfftakeQuantity = (string)dataRow["StandardOfftakeQuantity"];
                    this.AnnualUsage = (string)dataRow["AnnualUsage"];
                    this.MeterSerialNumber = (string)dataRow["MeterSerialNumber"];
                    this.Area = (string)dataRow["Area"];
                    this.ImportExport = (string)dataRow["ImportExport"];
                    this.CanCommit = (bool)dataRow["CanCommit"];
                }
            }
        }
    }
}