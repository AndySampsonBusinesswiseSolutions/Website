USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supplier].[ProductDetail_GetByProductAttributeIdAndProductDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supplier].[ProductDetail_GetByProductAttributeIdAndProductDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-17
-- Description:	Get ProductDetail info from [Supplier].[ProductDetail] table by Product Attribute Id and Product Detail Description
-- =============================================

ALTER PROCEDURE [Supplier].[ProductDetail_GetByProductAttributeIdAndProductDetailDescription]
    @ProductAttributeId BIGINT,
    @ProductDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-17 -> Andrew Sampson -> Initial development of script
    -- 2020-08-25 -> Andrew Sampson -> Correct SupplierProduct to Product
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        ProductDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ProductId,
        ProductAttributeId,
        ProductDetailDescription
    FROM 
        [Supplier].[ProductDetail] 
    WHERE 
        ProductAttributeId = @ProductAttributeId
        AND ProductDetailDescription = @ProductDetailDescription
END
GO
