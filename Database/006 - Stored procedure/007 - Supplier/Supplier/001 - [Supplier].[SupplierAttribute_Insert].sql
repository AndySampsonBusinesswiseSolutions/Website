USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supplier].[SupplierAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supplier].[SupplierAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-25
-- Description:	Insert new supplier attribute into [Supplier].[SupplierAttribute] table
-- =============================================

ALTER PROCEDURE [Supplier].[SupplierAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @SupplierAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-25 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Supplier].[SupplierAttribute]
    (
        CreatedByUserId,
        SourceId,
        SupplierAttributeDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @SupplierAttributeDescription
    )
END
GO