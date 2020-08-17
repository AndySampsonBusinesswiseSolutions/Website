using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public class Supplier
        {
            public long SupplierAttribute_GetSupplierAttributeIdBySupplierAttributeDescription(string supplierAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSupplierEnums.SupplierAttribute_GetBySupplierAttributeDescription, 
                    supplierAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SupplierAttributeId"))
                    .FirstOrDefault();
            }

            public long SupplierDetail_GetSupplierIdBySupplierAttributeIdAndSupplierDetailDescription(long supplierAttributeId, string supplierDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSupplierEnums.SupplierDetail_GetBySupplierAttributeIdAndSupplierDetailDescription, 
                    supplierAttributeId, supplierDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SupplierId"))
                    .FirstOrDefault();
            }

            public long SupplierProductAttribute_GetSupplierProductAttributeIdBySupplierProductAttributeDescription(string supplierProductAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSupplierEnums.SupplierProductAttribute_GetBySupplierProductAttributeDescription, 
                    supplierProductAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SupplierProductAttributeId"))
                    .FirstOrDefault();
            }

            public long SupplierProductDetail_GetSupplierProductIdBySupplierProductAttributeIdAndSupplierProductDetailDescription(long supplierProductAttributeId, string supplierProductDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSupplierEnums.SupplierProductDetail_GetBySupplierProductAttributeIdAndSupplierProductDetailDescription, 
                    supplierProductAttributeId, supplierProductDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SupplierProductId"))
                    .FirstOrDefault();
            }
        }
    }
}
