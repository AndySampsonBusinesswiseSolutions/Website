namespace enums
{
    public partial class Enums
    {
        public class Information
        {
            public class File
            {
                public class Attribute
                {
                    public string FileName ="File Name";
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