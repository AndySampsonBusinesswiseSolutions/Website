USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supplier].[SupplierDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supplier].[SupplierDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-25
-- Description:	Insert new customer detail into [Supplier].[SupplierDetail] table
-- =============================================

ALTER PROCEDURE [Supplier].[SupplierDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @SupplierId BIGINT,
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

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Supplier].[SupplierDetail] WHERE SupplierId = @SupplierId 
        AND SupplierAttributeId = @SupplierAttributeId 
        AND SupplierDetailDescription = @SupplierDetailDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Supplier].[SupplierDetail]
            (
                CreatedByUserId,
                SourceId,
                SupplierId,
                SupplierAttributeId,
                SupplierDetailDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @SupplierId,
                @SupplierAttributeId,
                @SupplierDetailDescription
            )
        END
END
GO
