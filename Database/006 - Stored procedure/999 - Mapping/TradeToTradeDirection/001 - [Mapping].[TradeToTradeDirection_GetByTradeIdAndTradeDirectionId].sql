USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[TradeToTradeDirection_GetByTradeIdAndTradeDirectionId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[TradeToTradeDirection_GetByTradeIdAndTradeDirectionId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-18
-- Description:	Get TradeToTradeDirection info from [Mapping].[TradeToTradeDirection] table by Trade Id and TradeDirection Id
-- =============================================

ALTER PROCEDURE [Mapping].[TradeToTradeDirection_GetByTradeIdAndTradeDirectionId]
    @TradeId BIGINT,
    @TradeDirectionId BIGINT,
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
        TradeToTradeDirectionId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        TradeId,
        TradeDirectionId
    FROM 
        [Mapping].[TradeToTradeDirection]
    WHERE 
        TradeId = @TradeId
        AND TradeDirectionId = @TradeDirectionId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
