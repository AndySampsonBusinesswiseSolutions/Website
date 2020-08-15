namespace enums
{
    public partial class Enums
    {
        public class Information
        {
            public class Commodity
            {
                public string Electricity = "Electricity";
                public string Gas = "Gas";
            }

            public class ContractType
            {
                public string Fixed = "Fixed";
                public string Flex = "Flex";
            }

            public class File
            {
                public class Attribute
                {
                    public string FileName ="File Name";
                    public string ProcessQueueGUID ="Process Queue GUID";
                }

                public class Type
                {
                    public string UsageUpload = "Usage Upload";
                    public string LetterOfAuthority = "Letter Of Authority";
                    public string SupplierContract = "Supplier Contract";
                    public string EMaaSContract = "EMaaS Contract";
                    public string FlexContract = "Flex Contract";
                    public string Invoice = "Invoice";
                    public string SupplierBill = "Supplier Bill";
                }
            }

            public class GridSupplyPoint
            {
                public class Attribute
                {
                    public string GridSupplyPointGroupId = "Grid Supply Point Group Id";
                }
            }

            public class LocalDistributionZone
            {
                public class Attribute
                {
                    public string LocalDistributionZoneCode = "Local Distribution Zone Code";
                }
            }

            public class MeterExemption
            {
                public class Attribute
                {
                    public string MeterExemptionProduct = "Meter Exemption Product";
                    public string MeterExemptionProportion = "Meter Exemption Proportion";
                    public string UseDefaultValue = "Use Default Value?";
                }
            }

            public class MeterTimeswitchCode
            {
                public class Attribute
                {
                    public string MeterTimeswitchRangeStart = "Meter Timeswitch Class Range Start";
                    public string MeterTimeswitchRangeEnd = "Meter Timeswitch Class Range End";
                }
            }

            public class ProfileClass
            {
                public class Attribute
                {
                    public string ProfileClassCode = "Profile Class Code";
                }
            }

            public class Source
            {
                public class Attribute
                {
                    public string UserGenerated = "User Generated";
                }
            }
        }
    }
}