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
        }
    }
}
