USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supplier].[Product_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supplier].[Product_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-26
-- Description:	Insert new Product into [Supplier].[Product] table
-- =============================================

ALTER PROCEDURE [Supplier].[Product_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ProductGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-26 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Supplier].[Product] WHERE ProductGUID = @ProductGUID
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Supplier].[Product]
            (
                CreatedByUserId,
                SourceId,
                ProductGUID
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @ProductGUID
            )
        END
END
GO
