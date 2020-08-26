USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ProductToSupplier_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ProductToSupplier_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-26
-- Description:	Insert new mapping of a Product to a Supplier into [Mapping].[ProductToSupplier] table
-- =============================================

ALTER PROCEDURE [Mapping].[ProductToSupplier_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ProductId BIGINT,
    @SupplierId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-26 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].ProductToSupplier
    (
        CreatedByUserId,
        SourceId,
        ProductId,
        SupplierId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ProductId,
        @SupplierId
    )
END
GO
