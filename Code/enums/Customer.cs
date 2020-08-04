namespace enums
{
    public partial class Enums
    {
        public class Customer
        {
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

            public class DataUploadValidation
            {
                public class Attribute
                {
                    public string SheetName = "Sheet Name";
                    public string RowNumber = "Row Number";
                    public string Entity = "Entity";
                    public string ValidationErrorMessage = "Validation Error Message";
                    public string CanCommit = "Can Commit";
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