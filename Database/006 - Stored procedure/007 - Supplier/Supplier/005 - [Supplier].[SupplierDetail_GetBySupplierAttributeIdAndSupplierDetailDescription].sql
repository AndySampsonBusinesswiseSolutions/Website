USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supplier].[SupplierDetail_GetBySupplierAttributeIdAndSupplierDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supplier].[SupplierDetail_GetBySupplierAttributeIdAndSupplierDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-25
-- Description:	Get SupplierDetail info from [Supplier].[SupplierDetail] table by Supplier Attribute Id and Supplier Detail Description
-- =============================================

ALTER PROCEDURE [Supplier].[SupplierDetail_GetBySupplierAttributeIdAndSupplierDetailDescription]
    @SupplierAttributeId BIGINT,
    @SupplierDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-25 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        SupplierDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        SupplierId,
        SupplierAttributeId,
        SupplierDetailDescription
    FROM 
        [Supplier].[SupplierDetail] 
    WHERE 
        SupplierAttributeId = @SupplierAttributeId
        AND SupplierDetailDescription = @SupplierDetailDescription
END
GO
