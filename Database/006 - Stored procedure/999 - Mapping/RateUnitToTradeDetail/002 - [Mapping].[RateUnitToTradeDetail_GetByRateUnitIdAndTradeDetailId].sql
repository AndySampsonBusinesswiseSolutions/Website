USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[RateUnitToTradeDetail_GetByRateUnitIdAndTradeDetailId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[RateUnitToTradeDetail_GetByRateUnitIdAndTradeDetailId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-18
-- Description:	Get RateUnitToTradeDetail info from [Mapping].[RateUnitToTradeDetail] table by RateUnit Id and TradeDetail Id
-- =============================================

ALTER PROCEDURE [Mapping].[RateUnitToTradeDetail_GetByRateUnitIdAndTradeDetailId]
    @RateUnitId BIGINT,
    @TradeDetailId BIGINT,
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
        RateUnitToTradeDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        RateUnitId,
        TradeDetailId
    FROM 
        [Mapping].[RateUnitToTradeDetail]
    WHERE 
        RateUnitId = @RateUnitId
        AND TradeDetailId = @TradeDetailId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
