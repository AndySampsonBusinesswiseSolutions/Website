USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[Basket_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[Basket_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Insert new Basket into [Customer].[Basket] table
-- =============================================

ALTER PROCEDURE [Customer].[Basket_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @BasketGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-15 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Customer].[Basket]
    (
        CreatedByUserId,
        SourceId,
        BasketGUID
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @BasketGUID
    )
END
GO