USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supplier].[ProductDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supplier].[ProductDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-26
-- Description:	Insert new Product detail into [Supplier].[ProductDetail] table
-- =============================================

ALTER PROCEDURE [Supplier].[ProductDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ProductId BIGINT,
    @ProductAttributeId BIGINT,
    @ProductDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-26 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Supplier].[ProductDetail]
    (
        CreatedByUserId,
        SourceId,
        ProductId,
        ProductAttributeId,
        ProductDetailDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ProductId,
        @ProductAttributeId,
        @ProductDetailDescription
    )
END
GO