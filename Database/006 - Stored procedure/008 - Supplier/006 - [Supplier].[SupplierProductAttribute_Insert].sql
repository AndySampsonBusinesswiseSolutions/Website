USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supplier].[SupplierProductAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supplier].[SupplierProductAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-17
-- Description:	Insert new supplier attribute into [Supplier].[SupplierProductAttribute] table
-- =============================================

ALTER PROCEDURE [Supplier].[SupplierProductAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @SupplierProductAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-17 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Supplier].[SupplierProductAttribute] WHERE SupplierProductAttributeDescription = @SupplierProductAttributeDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Supplier].[SupplierProductAttribute]
            (
                CreatedByUserId,
                SourceId,
                SupplierProductAttributeDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @SupplierProductAttributeDescription
            )
        END
END
GO
