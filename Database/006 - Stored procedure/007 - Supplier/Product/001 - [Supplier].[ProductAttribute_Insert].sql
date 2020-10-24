USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supplier].[ProductAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supplier].[ProductAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-17
-- Description:	Insert new supplier attribute into [Supplier].[ProductAttribute] table
-- =============================================

ALTER PROCEDURE [Supplier].[ProductAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ProductAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-17 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Supplier].[ProductAttribute]
    (
        CreatedByUserId,
        SourceId,
        ProductAttributeDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ProductAttributeDescription
    )
END
GO