USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supplier].[SupplierAttribute_GetBySupplierAttributeDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supplier].[SupplierAttribute_GetBySupplierAttributeDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-25
-- Description:	Get SupplierAttribute info from [Supplier].[SupplierAttribute] table by SupplierAttributeDescription
-- =============================================

ALTER PROCEDURE [Supplier].[SupplierAttribute_GetBySupplierAttributeDescription]
    @SupplierAttributeDescription VARCHAR(255),
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-25 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        SupplierAttributeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        SupplierAttributeDescription
    FROM 
        [Supplier].[SupplierAttribute] 
    WHERE 
        SupplierAttributeDescription = @SupplierAttributeDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
