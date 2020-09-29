using System.Reflection;
using System.Data;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Temp
        {
            public partial class CustomerDataUpload
            {
                public void Meter_Insert(string processQueueGUID, int rowId, string siteName, string sitePostCode, string MPXN, string gridSupplyPoint, string profileClass, string meterTimeswitchCode, string lineLossFactorClass, string capacity, string localDistributionZone, string standardOfftakeQuantity, string annualUsage, string meterSerialNumber, string area, string importExport)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerDataUploadEnums.Meter_Insert, 
                        processQueueGUID, rowId, siteName, sitePostCode, MPXN, gridSupplyPoint, profileClass, meterTimeswitchCode, lineLossFactorClass, capacity, localDistributionZone, standardOfftakeQuantity, annualUsage, meterSerialNumber, area, importExport);
                }

                public List<DataRow> Meter_GetByProcessQueueGUID(string processQueueGUID)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureTempCustomerDataUploadEnums.Meter_GetByProcessQueueGUID, 
                        processQueueGUID);

                    return CleanedUpDataTable(dataTable);
                }

                public void Meter_DeleteByProcessQueueGUID(string processQueueGUID)
                {
                    ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                        _storedProcedureTempCustomerDataUploadEnums.Meter_DeleteByProcessQueueGUID, 
                        processQueueGUID);
                }
            }
        }
    }
}