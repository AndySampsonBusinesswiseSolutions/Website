namespace enums
{
    public partial class Enums
    {
        public class Information
        {
            public class Folder
            {
                public class Attribute
                {
                    public string FolderPath = "Folder Path";
                    public string FolderType = "Folder Type";
                }

                public class ExtensionType
                {
                    public string UsageUpload = "Usage Upload";
                    public string LetterOfAuthority = "Letter Of Authority";
                    public string SupplierContract = "Supplier Contract";
                    public string EMaaSContract = "EMaaS Contract";
                    public string FlexContract = "Flex Contract";
                    public string Invoice = "Invoice";
                    public string SupplierBill = "Supplier Bill";
                }

                public class RootFolderType
                {
                    public string CustomerFiles = "Customer Files";
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