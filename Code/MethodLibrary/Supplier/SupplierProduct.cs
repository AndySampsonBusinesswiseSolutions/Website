using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class SupplierSchema
        {
            public long ProductAttribute_GetProductAttributeIdByProductAttributeDescription(string productAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSupplierEnums.ProductAttribute_GetByProductAttributeDescription, 
                    productAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProductAttributeId"))
                    .FirstOrDefault();
            }

            public long ProductDetail_GetProductIdByProductAttributeIdAndProductDetailDescription(long productAttributeId, string productDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSupplierEnums.ProductDetail_GetByProductAttributeIdAndProductDetailDescription, 
                    productAttributeId, productDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProductId"))
                    .FirstOrDefault();
            }
        }
    }
}