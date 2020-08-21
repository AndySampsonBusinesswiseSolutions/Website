USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[BasketToTrade_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[BasketToTrade_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-18
-- Description:	Insert new mapping of a Basket to a Trade into [Mapping].[BasketToTrade] table
-- =============================================

ALTER PROCEDURE [Mapping].[BasketToTrade_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @BasketId BIGINT,
    @TradeId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-18 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].BasketToTrade
    (
        CreatedByUserId,
        SourceId,
        BasketId,
        TradeId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @BasketId,
        @TradeId
    )
END
GO
