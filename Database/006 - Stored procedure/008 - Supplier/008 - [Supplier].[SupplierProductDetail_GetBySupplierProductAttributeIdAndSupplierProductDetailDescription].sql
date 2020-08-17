USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supplier].[SupplierProductDetail_GetBySupplierProductAttributeIdAndSupplierProductDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supplier].[SupplierProductDetail_GetBySupplierProductAttributeIdAndSupplierProductDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-17
-- Description:	Get SupplierProductDetail info from [Supplier].[SupplierProductDetail] table by Supplier Attribute Id and Supplier Detail Description
-- =============================================

ALTER PROCEDURE [Supplier].[SupplierProductDetail_GetBySupplierProductAttributeIdAndSupplierProductDetailDescription]
    @SupplierProductAttributeId BIGINT,
    @SupplierProductDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-17 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        SupplierProductDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        SupplierProductId,
        SupplierProductAttributeId,
        SupplierProductDetailDescription
    FROM 
        [Supplier].[SupplierProductDetail] 
    WHERE 
        SupplierProductAttributeId = @SupplierProductAttributeId
        AND SupplierProductDetailDescription = @SupplierProductDetailDescription
END
GO
