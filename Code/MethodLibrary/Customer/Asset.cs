using System.Data;
using System.Linq;
using System.Reflection;
using System;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Customer
        {
            public long InsertNewAsset(long createdByUserId, long sourceId)
            {
                //Create new AssetGUID
                var GUID = Guid.NewGuid().ToString();

                while (Asset_GetAssetIdByAssetGUID(GUID) > 0)
                {
                    GUID = Guid.NewGuid().ToString();
                }

                //Insert into [Customer].[Asset]
                Asset_Insert(createdByUserId, sourceId, GUID);
                return Asset_GetAssetIdByAssetGUID(GUID);
            }

            public long GetAssetId(string asset, long createdByUserId, long sourceId, long assetNameAssetAttributeId)
            {
                var assetId = Asset_GetAssetIdByAssetAttributeIdAndAssetDetailDescription(assetNameAssetAttributeId, asset);

                if(assetId == 0)
                {
                    assetId = InsertNewAsset(createdByUserId, sourceId);

                    //Insert into [Customer].[AssetDetail]
                    AssetDetail_Insert(createdByUserId, sourceId, assetId, assetNameAssetAttributeId, asset);
                }

                return assetId;
            }

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

            public Guid Asset_GetAssetGUIDByAssetId(long assetId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.Asset_GetByAssetId, 
                    assetId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<Guid>("AssetGUID"))
                    .FirstOrDefault();
            }

            public string AssetDetail_GetAssetDetailDescriptionByAssetIdAndAssetAttributeId(long assetId, long assetAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.AssetDetail_GetByAssetIdAndAssetAttributeId, 
                    assetId, assetAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("AssetDetailDescription"))
                    .FirstOrDefault();
            }

            public void AssetDetail_Insert(long createdByUserId, long sourceId, long assetId, long assetAttributeId, string assetDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.AssetDetail_Insert, 
                    createdByUserId, sourceId, assetId, assetAttributeId, assetDetailDescription);
            }
        }
    }
}