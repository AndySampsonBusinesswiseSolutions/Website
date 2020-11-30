USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[TradeToTradeProduct_GetByTradeIdAndTradeProductId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[TradeToTradeProduct_GetByTradeIdAndTradeProductId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-18
-- Description:	Get TradeToTradeProduct info from [Mapping].[TradeToTradeProduct] table by Trade Id and TradeProduct Id
-- =============================================

ALTER PROCEDURE [Mapping].[TradeToTradeProduct_GetByTradeIdAndTradeProductId]
    @TradeId BIGINT,
    @TradeProductId BIGINT,
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
        TradeToTradeProductId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        TradeId,
        TradeProductId
    FROM 
        [Mapping].[TradeToTradeProduct]
    WHERE 
        TradeId = @TradeId
        AND TradeProductId = @TradeProductId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
