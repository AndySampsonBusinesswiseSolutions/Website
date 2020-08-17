USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supplier].[SupplierProductAttribute_GetBySupplierProductAttributeDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supplier].[SupplierProductAttribute_GetBySupplierProductAttributeDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-17
-- Description:	Get SupplierProductAttribute info from [Supplier].[SupplierProductAttribute] table by SupplierProductAttributeDescription
-- =============================================

ALTER PROCEDURE [Supplier].[SupplierProductAttribute_GetBySupplierProductAttributeDescription]
    @SupplierProductAttributeDescription VARCHAR(255),
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-17 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        SupplierProductAttributeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        SupplierProductAttributeDescription
    FROM 
        [Supplier].[SupplierProductAttribute] 
    WHERE 
        SupplierProductAttributeDescription = @SupplierProductAttributeDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
