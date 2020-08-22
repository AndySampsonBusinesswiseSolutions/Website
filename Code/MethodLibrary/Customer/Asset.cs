using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Customer
        {
            public long Asset_GetAssetIdByAssetAttributeIdAndAssetDetailDescription(long assetAttributeId, string assetDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.AssetDetail_GetByAssetAttributeIdAndAssetDetailDescription, 
                    assetAttributeId, assetDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("AssetDetailId"))
                    .FirstOrDefault();
            }

            public long AssetAttribute_GetAssetAttributeIdByAssetAttributeDescription(string assetAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.AssetAttribute_GetByAssetAttributeDescription, 
                    assetAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("AssetAttributeId"))
                    .FirstOrDefault();
            }

            public void Asset_Insert(long createdByUserId, long sourceId, string assetGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.Asset_Insert, 
                    createdByUserId, sourceId, assetGUID);
            }

            public long Asset_GetAssetIdByAssetGUID(string assetGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.Asset_GetByAssetGUID, 
                    assetGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("AssetId"))
                    .FirstOrDefault();
            }

            public void AssetDetail_Insert(long createdByUserId, long sourceId, long assetId, long assetAttributeId, string assetDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.AssetDetail_Insert, 
                    createdByUserId, sourceId, assetId, assetAttributeId, assetDetailDescription);
            }

            public DataRow AssetDetail_GetByAssetIdAndAssetAttributeId(long assetId, long assetAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.AssetDetail_GetByAssetIdAndAssetAttributeId, 
                    assetId, assetAttributeId);

                return dataTable.Rows.Cast<DataRow>().FirstOrDefault();
            }
        }
    }
}
