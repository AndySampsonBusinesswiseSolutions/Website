namespace enums
{
    public partial class Enums
    {
        public class Customer
        {
            public class Asset
            {
                public class Attribute
                {
                    public string AssetName = "Asset Name";
                }
            }

            public class Attribute
            {
                public string CustomerName = "Customer Name";
                public string AddressLines = "Address Lines";
                public string AddressTown = "Address Town";
                public string AddressCounty = "Address County";
                public string AddressPostCode = "Address PostCode";
                public string ContactName = "Contact Name";
                public string ContactTelephoneNumber = "Contact Telephone Number";
                public string ContactEmailAddress = "Contact Email Address";
            }

            public class Basket
            {
                public class Attribute
                {
                    public string BasketReference = "Basket Reference";
                }
            }

            public class Contract
            {
                public class Attribute
                {
                    public string ContractReference = "Contract Reference";
                }
            }

            public class ContractMeter
            {
                public class Attribute
                {
                    public string ContractStartDate = "Contract Start Date";
                    public string ContractEndDate = "Contract End Date";
                    public string RateCount = "Rate Count";
                }
            }

            public class DataUploadValidation
            {
                public class Entity
                {
                    public string CustomerName = "CustomerName";
                    public string ContactName = "ContactName";
                    public string ContactRole = "ContactRole";
                    public string ContactTelephoneNumber = "ContactTelephoneNumber";
                    public string ContactEmailAddress = "ContactEmailAddress";
                    public string MPXN = "MPXN";
                    public string GridSupplyPoint = "GridSupplyPoint";
                    public string ProfileClass = "ProfileClass";
                    public string MeterTimeswitchCode = "MeterTimeswitchCode";
                    public string LineLossFactorClass = "LineLossFactorClass";
                    public string LocalDistributionZone = "LocalDistributionZone";
                    public string Capacity = "Capacity";
                    public string StandardOfftakeQuantity = "StandardOfftakeQuantity";
                    public string AnnualUsage = "AnnualUsage";
                    public string ImportExport = "ImportExport";
                    public string MeterSerialNumber = "MeterSerialNumber";
                    public string Area = "Area";
                    public string SiteName = "SiteName";
                    public string SiteAddress = "SiteAddress";
                    public string SiteTown = "SiteTown";
                    public string SiteCounty = "SiteCounty";
                    public string SitePostCode = "SitePostCode";
                    public string SiteDescription = "SiteDescription";
                    public string SubMeterIdentifier = "SubMeterIdentifier";
                    public string SerialNumber = "SerialNumber";
                    public string SubArea = "SubArea";
                    public string Asset = "Asset";
                    public string ExemptionProduct = "ExemptionProduct";
                    public string ExemptionProportion = "ExemptionProportion";
                    public string Date = "Date";
                    public string DateFrom = "DateFrom";
                    public string DateTo = "DateTo";
                    public string TimePeriod = "TimePeriod";
                    public string ContractReference = "ContractReference";
                    public string Supplier = "Supplier";
                    public string ContractStartDate = "ContractStartDate";
                    public string ContractEndDate = "ContractEndDate";
                    public string Product = "Product";
                    public string RateCount = "RateCount";
                    public string StandingCharge = "StandingCharge";
                    public string CapacityCharge = "CapacityCharge";
                    public string Rate = "Rate";
                    public string Value = "Value";
                    public string BasketReference = "BasketReference";
                    public string RateType = "RateType";
                    public string Volume = "Volume";
                    public string TradeReference = "TradeReference";
                    public string TradeDate = "TradeDate";
                    public string TradeProduct = "TradeProduct";
                    public string Price = "Price";
                    public string Direction = "Direction";
                }

                public class SheetName
                {
                    public string Customer = "Customers";
                    public string Site = "Sites";
                    public string Meter = "Meters";
                    public string SubMeter = "SubMeters";
                    public string MeterUsage = "Meter HH Data";
                    public string MeterExemption = "Meter Exemptions";
                    public string SubMeterUsage = "SubMeter HH Data";
                    public string FixedContract = "Fixed Contracts";
                    public string FlexContract = "Flex Contracts";
                    public string FlexReferenceVolume = "Flex Reference Volumes";
                    public string FlexTrade = "Flex Trades";
                }
            }

            public class FlexContract
            {
                public class Attribute
                {
                    public string BasketReference = "Basket Reference";
                    public string ContractReference = "Contract Reference";
                }
            }

            public class Meter
            {
                public class Attribute
                {
                    public string MeterIdentifier = "Meter Identifier";
                    public string MeterSerialNumber = "Meter Serial Number";
                    public string SupplyCapacity = "Supply Capacity";
                    public string StandardOfftakeQuantity = "Standard Offtake Quantity";
                    public string AnnualUsage = "Annual Usage";
                    public string ImportExport = "Import/Export";
                }
            }

            public class MeterExemption
            {
                public class Attribute
                {
                    public string DateFrom = "Date From";
                    public string DateTo = "Date To";
                    public string ExemptionProportion = "Exemption Proportion";
                }
            }

            public class Site
            {
                public class Attribute
                {
                    public string SiteName = "Site Name";
                    public string SiteAddress = "Site Address";
                    public string SiteTown = "Site Town";
                    public string SiteCounty = "Site County";
                    public string SitePostCode = "Site PostCode";
                    public string SiteDescription = "Site Description";
                    public string ContactName = "Contact Name";
                    public string ContactTelephoneNumber = "Contact Telephone Number";
                    public string ContactEmailAddress = "Contact Email Address";
                    public string ContactRole = "Contact Role";
                }
            }

            public class SubMeter
            {
                public class Attribute
                {
                    public string SubMeterIdentifier = "SubMeter Identifier";
                    public string SerialNumber = "Serial Number";
                }
            }
        }
    }
}