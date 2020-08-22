using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Supplier
        {
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
