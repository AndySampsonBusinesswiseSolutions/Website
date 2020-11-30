USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[BasketToTrade_GetByBasketIdAndTradeId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[BasketToTrade_GetByBasketIdAndTradeId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-18
-- Description:	Get BasketToTrade info from [Mapping].[BasketToTrade] table by Basket Id and Trade Id
-- =============================================

ALTER PROCEDURE [Mapping].[BasketToTrade_GetByBasketIdAndTradeId]
    @BasketId BIGINT,
    @TradeId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-18 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        BasketToTradeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        BasketId,
        TradeId
    FROM 
        [Mapping].[BasketToTrade]
    WHERE 
        BasketId = @BasketId
        AND TradeId = @TradeId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
