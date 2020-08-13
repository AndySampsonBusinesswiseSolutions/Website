namespace enums
{
    public partial class Enums
    {
        public class System
        {
            public class API
            {
                public class Attribute
                {
                    public string APIName = "API Name";
                    public string HTTPSApplicationURL = "HTTPS Application URL";
                    public string HTTPApplicationURL = "HTTP Application URL";
                    public string PrerequisiteAPIGUID = "Prerequisite API GUID";
                    public string POSTRoute = "POST Route";
                    public string IsRunningRoute = "IsRunning Route";
                    public string RequiredDataKey = "Required Data Key";
                    public string MaximumInvalidLoginAttempts = "Maximum Invalid Login Attempts";
                    public string TimeoutSeconds = "Timeout Seconds";
                }
                
                public class GUID
                {
                    public string CommitBasketDataAPI = "26EBBD73-45DA-4514-B663-662D9127C65B";
                    public string CommitContractMeterToProductDataAPI = "";
                    public string CommitContractToSupplierDataAPI = "8CD1C26A-C3F6-4CCE-8544-D58C8E49B43D";
                    public string CommitContractDataAPI = "E6A652C5-EE89-4E2B-A6A2-A2BEE750384A";
                    public string CommitTradeToBasketDataAPI = "8EFE04B3-9F9B-406A-81E8-3187E7848F76";
                    public string CommitReferenceVolumeToContractDataAPI = "5035107A-66F1-4736-ACBA-AE331A5ECF62";
                    public string CommitContractToMeterDataAPI = "522D6970-D5F0-4F7E-BC43-6121B9228BA9";
                    public string CommitExemptionToMeterDataAPI = "BDBC12DE-B9F9-4BD4-B164-FFA2BCF59CE5";
                    public string CommitSubAreaToSubMeterDataAPI = "664A01BE-A6E2-4557-80DC-4BF52E0533E4";
                    public string CommitAssetToSubMeterDataAPI = "B0026B5C-CA13-42ED-9BA7-3CEBEC589FE9";
                    public string CommitCommodityToMeterDataAPI = "5BAA55DE-1F5B-40EF-988A-E8E85E2B594C";
                    public string CommitAreaToMeterDataAPI = "8D6FE490-F451-4F2F-B429-CBBD3C217220";
                    public string CommitMeterToSubMeterDataAPI = "BBA77F1B-F9A3-4CAA-B213-D11285994B0A";
                    public string CommitMeterToSiteDataAPI = "6A49C79C-37F3-4D86-BCAE-27602E5DF416";
                    public string CommitCustomerToSiteDataAPI = "68E818C7-6F72-4586-8F75-A01E52680AE5";
                    public string CommitSubMeterUsageDataAPI = "A246C65A-5471-43F2-9836-F02306B8ECF5";
                    public string CommitMeterUsageDataAPI = "95DDBA83-F519-437D-A31D-21D8E0061131";
                    public string CommitFlexTradeDataAPI = "8C14B5FD-90D4-409D-A69C-140FC103814A";
                    public string CommitFlexReferenceVolumeDataAPI = "A397F2B9-4B0F-4432-8E0C-627A8B2753AB";
                    public string CommitFlexContractDataAPI = "2A904C29-A848-40BB-9993-916A2376B571";
                    public string CommitFixedContractDataAPI = "8CF42052-7798-40FD-A786-987CB53A159E";
                    public string CommitMeterExemptionDataAPI = "0E525DC3-C108-4A00-8A5C-E69CF9DF8359";
                    public string CommitSubMeterDataAPI = "89E15527-A337-446F-9C67-E270D23D6CE2";
                    public string CommitMeterDataAPI = "94475084-969D-431F-A8D8-EB591CCFCE26";
                    public string CommitSiteDataAPI = "EB061C46-A893-4C44-9EB7-E9D5CF6B0895";
                    public string CommitCustomerDataAPI = "D69532B2-384D-44E0-B72E-43934276D0B6";
                    public string ProcessCustomerDataUploadValidationAPI = "069FA45E-2757-4383-BB66-52470B952F7F";
                    public string ValidateCrossSheetEntityDataAPI = "E78B2351-B1E6-464F-8D90-3793FACDABF4";
                    public string ValidateFlexTradeDataAPI = "AFFEE0C0-D660-4859-A7E9-93C3F2BC5492";
                    public string ValidateFlexReferenceVolumeDataAPI = "D520F4DC-F582-45D7-A53C-608156991C6E";
                    public string ValidateFlexContractDataAPI = "BE59D94B-61BE-4900-B336-36AEB8904973";
                    public string ValidateFixedContractDataAPI = "B8F5A9D3-CD9F-44F2-B3EA-DCECA0F7CCFF";
                    public string ValidateMeterExemptionDataAPI = "0BA4C8E7-3723-4106-A117-656F6871BC99";
                    public string ValidateCustomerDataAPI = "8253F798-A8F4-404D-B2BC-5DC87EFE839B";
                    public string StoreFlexTradeDataAPI = "115603E8-400A-43F0-AC36-44DDCF7031D8";
                    public string StoreFlexReferenceVolumeDataAPI = "D2973575-B026-4D17-8879-3E963E9C438E";
                    public string StoreFlexContractDataAPI = "A8476294-813A-44E2-952E-A51CB27207FE";
                    public string StoreFixedContractDataAPI = "14405C13-7A14-4648-B19C-7A8D1AF974A5";
                    public string StoreMeterExemptionDataAPI = "A66500F1-9CD5-413C-BD74-A8A45CFDB06E";
                    public string StoreCustomerDataAPI = "8E2142F5-28D4-41A6-AB6A-FC1388855C40";
                    public string ValidateSubMeterUsageDataAPI = "4E754A46-5F17-47BF-9D40-C7A95412EEFB";
                    public string ValidateMeterUsageDataAPI = "77B71231-880F-4470-83C9-0ED845BDDDCA";
                    public string ValidateSubMeterDataAPI = "35FA9EE3-77EC-4D1D-B622-443D28DA1608";
                    public string ValidateMeterDataAPI = "1DDDA8F8-F996-4B08-A28A-19F4FB0C922D";
                    public string ValidateSiteDataAPI = "535FDE7A-8720-4B72-BF68-DAC8FB95FBE9";
                    public string StoreSubMeterUsageDataAPI = "0AF991E7-EE90-4CB0-AD03-CE57F45450EF";
                    public string StoreMeterUsageDataAPI = "3B054332-65F9-40A7-AD7E-8E8E4089DBC9";
                    public string StoreSubMeterDataAPI = "BDC8BA2C-C97A-4445-9B3E-31E0CE520AB8";
                    public string StoreMeterDataAPI = "9EDA4294-4FF7-45C3-8FD1-1CD50BD4B7C3";
                    public string StoreSiteDataAPI = "AB6466A5-4CF0-4DA5-AE55-7EA93ACE8E13";
                    public string DetermineFileTypeAPI = "4C7CACF3-22BF-4943-B03C-83E1B4DADC35";
                    public string UploadFileAPI = "5681C395-98AC-4410-98DE-DBB550106EF2";
                    public string CreateCustomerFoldersAPI = "2AE6E5B2-1D5F-431B-9BCE-A5CDDB52B493";
                    public string StoreLoginAttemptAPI = "3BBFC2B6-2572-43CD-921A-A237000AC248";//
                    public string RoutingAPI = "A4F25D07-86AA-42BD-ACD7-51A8F772A92B";
                    public string WebsiteAPI = "CBB27186-B65F-4F6C-9FFA-B1E6C63C04EE";
                    public string ValidateProcessGUIDAPI = "87AFEDA8-6A0F-4143-BF95-E08E78721CF5";
                    public string ArchiveProcessQueueAPI = "38D3A9E1-A060-4464-B971-8DC523B6A42D";
                    public string ValidatePageGUIDAPI = "F916F19F-9408-4969-84DC-9905D2FEFB0B";
                    public string CheckPrerequisiteAPIAPI = "56371F02-4120-41C9-82F9-4408309684D1";
                    public string ValidateEmailAddressAPI = "99681B37-575F-47E5-95E3-608063EA513E";
                    public string ValidatePasswordAPI = "26FEFFE8-49F7-4458-98ED-FD5F6C65C7C2";
                    public string ValidateEmailAddressPasswordMappingAPI = "CEC56745-C1C5-4E67-805B-159A8A5E991D";
                    public string LockUserAPI = "0C1BAFAA-586D-48BB-8D0B-B0B56BE0CCD2";
                    public string StoreUsageUploadAPI = "94DD0DCB-DDC3-45A9-9C3D-D83922CF3110";
                    public string AddNewCustomerAPI = "1B2E2BA3-D538-47E0-9044-BBBFC6BF3892";
                    public string UpdateCustomerDetailAPI = "C151AD1B-C3AB-448F-8C8D-572328A8D1C7";
                    public string MapCustomerToChildCustomerAPI = "1EDBA521-11E9-4564-94E6-8D84C4192844";
                }

                public class Name
                {
                    public string CommitBasketDataAPI = "CommitBasketData.api";
                    public string CommitContractMeterToProductDataAPI = "CommitContractMeterToProductData.api";
                    public string CommitContractToSupplierDataAPI = "CommitContractToSupplierData.api";
                    public string CommitContractDataAPI = "CommitContractData.api";
                    public string CommitTradeToBasketDataAPI = "CommitTradeToBasketData.api";
                    public string CommitReferenceVolumeToContractDataAPI = "CommitReferenceVolumeToContractData.api";
                    public string CommitContractToMeterDataAPI = "CommitContractToMeterData.api";
                    public string CommitExemptionToMeterDataAPI = "CommitExemptionToMeterData.api";
                    public string CommitSubAreaToSubMeterDataAPI = "CommitSubAreaToSubMeterData.api";
                    public string CommitAssetToSubMeterDataAPI = "CommitAssetToSubMeterData.api";
                    public string CommitCommodityToMeterDataAPI = "CommitCommodityToMeterData.api";
                    public string CommitAreaToMeterDataAPI = "CommitAreaToMeterData.api";
                    public string CommitMeterToSubMeterDataAPI = "CommitMeterToSubMeterData.api";
                    public string CommitMeterToSiteDataAPI = "CommitMeterToSiteData.api";
                    public string CommitCustomerToSiteDataAPI = "CommitCustomerToSiteData.api";
                    public string CommitSubMeterUsageDataAPI = "CommitSubMeterUsageData.api";
                    public string CommitMeterUsageDataAPI = "CommitMeterUsageData.api";
                    public string CommitFlexTradeDataAPI = "CommitFlexTradeData.api";
                    public string CommitFlexReferenceVolumeDataAPI = "CommitFlexReferenceVolumeData.api";
                    public string CommitFlexContractDataAPI = "CommitFlexContractData.api";
                    public string CommitFixedContractDataAPI = "CommitFixedContractData.api";
                    public string CommitMeterExemptionDataAPI = "CommitMeterExemptionData.api";
                    public string CommitSubMeterDataAPI = "CommitSubMeterData.api";
                    public string CommitMeterDataAPI = "CommitMeterData.api";
                    public string CommitSiteDataAPI = "CommitSiteData.api";
                    public string CommitCustomerDataAPI = "CommitCustomerData.api";
                    public string ProcessCustomerDataUploadValidationAPI = "ProcessCustomerDataUploadValidation.api";
                    public string ValidateCrossSheetEntityDataAPI = "ValidateCrossSheetEntityData.api";
                    public string ValidateFlexTradeDataAPI = "ValidateFlexTradeData.api";
                    public string ValidateFlexReferenceVolumeDataAPI = "ValidateFlexReferenceVolumeData.api";
                    public string ValidateFlexContractDataAPI = "ValidateFlexContractData.api";
                    public string ValidateFixedContractDataAPI = "ValidateFixedContractData.api";
                    public string ValidateMeterExemptionDataAPI = "ValidateMeterExemptionData.api";
                    public string ValidateCustomerDataAPI = "ValidateCustomerData.api";
                    public string StoreFlexTradeDataAPI = "StoreFlexTradeData.api";
                    public string StoreFlexReferenceVolumeDataAPI = "StoreFlexReferenceVolumeData.api";
                    public string StoreFlexContractDataAPI = "StoreFlexContractData.api";
                    public string StoreFixedContractDataAPI = "StoreFixedContractData.api";
                    public string StoreMeterExemptionDataAPI = "StoreMeterExemptionData.api";
                    public string StoreCustomerDataAPI = "StoreCustomerData.api";
                    public string ValidateSubMeterUsageDataAPI = "ValidateSubMeterUsageData.api";
                    public string ValidateMeterUsageDataAPI = "ValidateMeterUsageData.api";
                    public string ValidateSubMeterDataAPI = "ValidateSubMeterData.api";
                    public string ValidateMeterDataAPI = "ValidateMeterData.api";
                    public string ValidateSiteDataAPI = "ValidateSiteData.api";
                    public string StoreSubMeterUsageDataAPI = "StoreSubMeterUsageData.api";
                    public string StoreMeterUsageDataAPI = "StoreMeterUsageData.api";
                    public string StoreSubMeterDataAPI = "StoreSubMeterData.api";
                    public string StoreMeterDataAPI = "StoreMeterData.api";
                    public string StoreSiteDataAPI = "StoreSiteData.api";
                    public string DetermineFileTypeAPI = "DetermineFileType.api";
                    public string UploadFileAPI = "UploadFile.api";
                    public string CreateCustomerFoldersAPI = "CreateCustomerFolders.api";
                    public string StoreLoginAttemptAPI = "StoreLoginAttempt.api";
                    public string RoutingAPI = "Routing.api";
                    public string WebsiteAPI = "Website.api";
                    public string ValidateProcessGUIDAPI = "ValidateProcessGUID.api";
                    public string ArchiveProcessQueueAPI = "ArchiveProcessQueue.api";
                    public string ValidatePageGUIDAPI = "ValidatePageGUID.api";
                    public string CheckPrerequisiteAPIAPI = "CheckPrerequisiteAPI.api";
                    public string ValidateEmailAddressAPI = "ValidateEmailAddress.api";
                    public string ValidatePasswordAPI = "ValidatePassword.api";
                    public string ValidateEmailAddressPasswordMappingAPI = "ValidateEmailAddressPasswordMapping.api";
                    public string LockUserAPI = "LockUser.api";
                    public string StoreUsageUploadAPI = "StoreUsageUpload.api";
                    public string AddNewCustomerAPI = "AddNewCustomer.api";
                    public string UpdateCustomerDetailAPI = "UpdateCustomerDetail.api";
                    public string MapCustomerToChildCustomerAPI = "MapCustomerToChildCustomer.api";
                }

                //TODO: Move to config file in each API
                public class Password
                {
                    public string CommitBasketDataAPI = "zkNYKFH6sdsPPXqz";
                    public string CommitContractMeterToProductDataAPI = "";
                    public string CommitContractToSupplierDataAPI = "YVfsgbnrzGh3SCcE";
                    public string CommitContractDataAPI = "wsxbn8B2jTb9bDFM";
                    public string CommitTradeToBasketDataAPI = "uPXLHnw3FFbVMCrw";
                    public string CommitReferenceVolumeToContractDataAPI = "wuMYmJm587ehRtED";
                    public string CommitContractToMeterDataAPI = "6TKhAGYAgf85Xzm4";
                    public string CommitExemptionToMeterDataAPI = "WeJqMap8UMB7Uayq";
                    public string CommitSubAreaToSubMeterDataAPI = "x96nW5RAYrnXu9tc";
                    public string CommitAssetToSubMeterDataAPI = "Upr4fm9NKd8mC5Dt";
                    public string CommitCommodityToMeterDataAPI = "dJPDrGp8DPndzw9w";
                    public string CommitAreaToMeterDataAPI = "x4dzGMHS2CCJgyqD";
                    public string CommitMeterToSubMeterDataAPI = "g89M9Px2AtzgJ3N2";
                    public string CommitMeterToSiteDataAPI = "XRvxMrsZ6metMPUB";
                    public string CommitCustomerToSiteDataAPI = "4XWtk5BbBFkNf34d";
                    public string CommitSubMeterUsageDataAPI = "BahJpvV8hyTwC3Bt";
                    public string CommitMeterUsageDataAPI = "ad9HJxv48Px7gUTj";
                    public string CommitFlexTradeDataAPI = "3HXnZHc5GpUhzEA9";
                    public string CommitFlexReferenceVolumeDataAPI = "6TVnwAK6jeDk2kVb";
                    public string CommitFlexContractDataAPI = "MnXB6w8fSZuKuHL9";
                    public string CommitFixedContractDataAPI = "PQrhQL3PCrchDXnj";
                    public string CommitMeterExemptionDataAPI = "hzRHNnabT4hc6Mzf";
                    public string CommitSubMeterDataAPI = "nZYGJvSe4Ej3dzFH";
                    public string CommitMeterDataAPI = "jC7jNZZnz63nNysc";
                    public string CommitSiteDataAPI = "vZ3RFaCBjSLCbFwc";
                    public string CommitCustomerDataAPI = "yuPPW9N2d346ATSM";
                    public string ProcessCustomerDataUploadValidationAPI = "VcTpcaaHYSFVa5bB";
                    public string ValidateCrossSheetEntityDataAPI = "SQTP72kBj36cntMn";
                    public string ValidateFlexTradeDataAPI = "89zfZ2GTajb4B94y";
                    public string ValidateFlexReferenceVolumeDataAPI = "h9CMbkML68NCyMNj";
                    public string ValidateFlexContractDataAPI = "FMTXVhfa5Yu8s6vQ";
                    public string ValidateFixedContractDataAPI = "aHG2nFuzttAHQDCN";
                    public string ValidateMeterExemptionDataAPI = "XdUWncBgXVs2hmvE";
                    public string ValidateCustomerDataAPI = "Mkf2GTm2crKuk6jh";
                    public string StoreFlexTradeDataAPI = "A5BYZuEtTQE5TENu";
                    public string StoreFlexReferenceVolumeDataAPI = "L5msq6pjxEqMAAf4";
                    public string StoreFlexContractDataAPI = "W92dpvtPzz3uJfYg";
                    public string StoreFixedContractDataAPI = "ReAjquZxWE6SrqjB";
                    public string StoreMeterExemptionDataAPI = "CNs2z2TrsqzZMu2J";
                    public string StoreCustomerDataAPI = "qkaux33qraa6EZ9H";
                    public string ValidateSubMeterUsageDataAPI = "kY4f4KaZCgJcHUnH";
                    public string ValidateMeterUsageDataAPI = "qashfvSa2xB58PXR";
                    public string ValidateSubMeterDataAPI = "nqCLyMb92urhKraj";
                    public string ValidateMeterDataAPI = "TqaV8u53zBrSksr4";
                    public string ValidateSiteDataAPI = "w2fs7druC2jUCNfQ";
                    public string StoreSubMeterUsageDataAPI = "uKxeuMwrdks8nXSa";
                    public string StoreMeterUsageDataAPI = "Xrx7E74XsVQeMqy7";
                    public string StoreSubMeterDataAPI = "HHq85F87Ymc7P4X7";
                    public string StoreMeterDataAPI = "EqsVJUK59sxf8QsE";
                    public string StoreSiteDataAPI = "46PP5VdL6Djet8tA";
                    public string DetermineFileTypeAPI = "dp2juZYYbdjkh43c";
                    public string UploadFileAPI = "puFbyaAvrzMgC3MU";
                    public string CreateCustomerFoldersAPI = "UE9ggtwMq6G4fpYv";
                    public string RoutingAPI = @"E{*Jj5&nLfC}@Q$:";
                    public string WebsiteAPI = @"\wU.D[ArWjPG!F4$";
                    public string ValidateProcessGUIDAPI = @"Y4c?.KT(>HXj@f8D";
                    public string ArchiveProcessQueueAPI = @"nb@89qWEW5!6=2s*";
                    public string ValidatePageGUIDAPI = @"n:Q>V&6P9KtG`(5k";
                    public string CheckPrerequisiteAPIAPI = @"w8chCkRAW]\N[7Hh";
                    public string ValidateEmailAddressAPI = @"}h8FfD2r[Rd~PPNR";
                    public string ValidatePasswordAPI = @"b7.Q!!X3Hp{\mJ}j";
                    public string ValidateEmailAddressPasswordMappingAPI = @"GQzD2!aZNvffr*zC";
                    public string LockUserAPI = @"JM7!?q#g#uTyM^!v";
                    public string StoreLoginAttemptAPI = @"mLdas-Y*x2rbnJ2e";
                    public string StoreUsageUploadAPI = @"Mt35GJs9un!Jq7pg";
                    public string AddNewCustomerAPI = @"$hRXtrCfb$$W3XZ+";
                    public string UpdateCustomerDetailAPI = @"7QJyVNc4+K74F67V";
                    public string MapCustomerToChildCustomerAPI = @"6dFB@tk?7L$UrQ9p";
                }

                public class RequiredDataKey
                {
                    public string APIGUIDList = "APIGUIDList";
                    public string EmailAddress = "EmailAddress";
                    public string PageGUID = "PageGUID";
                    public string Password = "Password";
                    public string ProcessGUID = "ProcessGUID";
                    public string ProcessQueueGUID = "ProcessQueueGUID";
                    public string CallingGUID = "CallingGUID";
                    public string CustomerGUID = "CustomerGUID";
                    public string CustomerData = "CustomerData";
                    public string ChildCustomerData = "ChildCustomerData";
                    public string APIGUID = "APIGUID";
                    public string FileContent = "FileContent";
                    public string FileGUID = "FileGUID";
                    public string FileType = "FileType";
                    public string FileName = "FileName";
                    public string ContractType = "ContractType";
                    public string ContractData = "ContractData";
                }
            }

            public class Page
            {
                public class Attribute
                {
                    public string PageName = "Page Name";
                }

                public class GUID
                {
                    public string Login = "6641A1BF-84C8-48F8-9D79-70D0AB2BB787";
                    public string SiteManagement = "714F10C4-ACF3-4409-97A8-C605E8E2FD0C";
                    public string ManageCustomers = "80B1CC99-7C91-4D07-A541-9D69AC4CC304";
                }

                public class Name
                {
                    public string Login = "Login";
                    public string SiteManagement = "Site Management";
                    public string ManageCustomers = "Manage Customers";
                }
            }

            public class ProcessArchive
            {
                public class Attribute
                {
                    public string Response = "Response";
                    public string APIResponse = "API Response";
                }
            }

            public class Process
            {
                public class Attribute
                {
                    public string ProcessName = "Process Name";
                }

                public class GUID
                {
                    public string Login = "AF10359F-FD78-4345-9F26-EF5A921E72FD";
                    public string AddNewCustomer = "D39E768A-D06D-4EB3-80E3-895EDC556A6B";
                    public string FileUpload = "3AFF25CB-06BD-4BD1-A409-13D10A08044F";
                    public string CustomerDataUpload = "63D6E7CA-591F-4BF3-BECA-57A9E350879A";
                    public string CommitCustomerDataUpload = "BF0B7C03-5201-4600-9123-8CC88D13CEBD";
                    public string SendEmail = "B68550D3-0283-4DC8-A048-9A1AD0899A7D";
                }

                public class Name
                {
                    public string Login = "Login";
                    public string AddNewCustomer = "Add New Customer";
                    public string FileUpload = "File Upload";
                    public string CustomerDataUpload = "Customer Data Upload";
                    public string CommitCustomerDataUpload = "Commit Customer Data Upload";
                    public string SendEmail = "Send Email";
                }
            }
        }
    }
}
