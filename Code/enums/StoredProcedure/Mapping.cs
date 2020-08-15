namespace enums
{
    public partial class Enums
    {
        public partial class StoredProcedure
        {
            public class Mapping
            {
                public string APIToProcess_GetByProcessId = "[Mapping].[APIToProcess_GetByProcessId]";
                public string APIToProcessArchiveDetail_Insert = "[Mapping].[APIToProcessArchiveDetail_Insert]";
                public string PasswordToUser_GetByPasswordIdAndUserId = "[Mapping].[PasswordToUser_GetByPasswordIdAndUserId]";
                public string LoginToUser_Insert = "[Mapping].[LoginToUser_Insert]";
                public string LoginToUser_GetByUserId = "[Mapping].[LoginToUser_GetByUserId]";
                public string ProcessToProcessArchive_Insert = "[Mapping].[ProcessToProcessArchive_Insert]";
                public string CustomerToChildCustomer_Insert = "[Mapping].[CustomerToChildCustomer_Insert]";
                public string CustomerToChildCustomer_GetByChildCustomerId = "[Mapping].[CustomerToChildCustomer_GetByChildCustomerId]";
                public string CustomerToChildCustomer_GetByCustomerId = "[Mapping].[CustomerToChildCustomer_GetByCustomerId]";
                public string CustomertoChildCustomer_DeleteByCustomerIdAndChildCustomerId = "[Mapping].[CustomertoChildCustomer_DeleteByCustomerIdAndChildCustomerId]";
                public string CustomerToChildCustomer_GetList = "[Mapping].[CustomerToChildCustomer_GetList]";
                public string APIToProcessArchiveDetail_GetByAPIId = "[Mapping].[APIToProcessArchiveDetail_GetByAPIId]";
                public string CustomerToFile_Insert = "[Mapping].[CustomerToFile_Insert]";
                public string FileToFileType_Insert = "[Mapping].[FileToFileType_Insert]";
                public string ContractToContractMeter_GetByContractId = "[Mapping].[ContractToContractMeter_GetByContractId]";
                public string ContractMeterToMeter_GetByContractMeterId = "[Mapping].[ContractMeterToMeter_GetByContractMeterId]";
                public string BasketToContractMeter_GetByBasketId = "[Mapping].[BasketToContractMeter_GetByBasketId]";
                public string FileTypeToProcess_GetByFileTypeId = "[Mapping].[FileTypeToProcess_GetByFileTypeId]";
                public string DataUploadValidationErrorToFile_Insert = "[Mapping].[DataUploadValidationErrorToFile_Insert]";
                public string MeterToMeterExemption_Insert = "[Mapping].[MeterToMeterExemption_Insert]";
                public string MeterToMeterExemption_GetByMeterIdAndMeterExemptionId = "[Mapping].[MeterToMeterExemption_GetByMeterIdAndMeterExemptionId]";
                public string MeterExemptionToMeterExemptionProduct_Insert = "[Mapping].[MeterExemptionToMeterExemptionProduct_Insert]";
                public string MeterToMeterExemptionToMeterExemptionProduct_Insert = "[Mapping].[MeterToMeterExemptionToMeterExemptionProduct_Insert]";
                public string AreaToMeter_Insert = "[Mapping].[AreaToMeter_Insert]";
                public string SubAreaToSubMeter_Insert = "[Mapping].[SubAreaToSubMeter_Insert]";
            }
        }
    }
}