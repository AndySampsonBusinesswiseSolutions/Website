USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[TradeDetailToVolumeUnit_GetByTradeDetailIdAndVolumeUnitId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[TradeDetailToVolumeUnit_GetByTradeDetailIdAndVolumeUnitId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-18
-- Description:	Get TradeDetailToVolumeUnit info from [Mapping].[TradeDetailToVolumeUnit] table by TradeDetail Id and VolumeUnit Id
-- =============================================

ALTER PROCEDURE [Mapping].[TradeDetailToVolumeUnit_GetByTradeDetailIdAndVolumeUnitId]
    @TradeDetailId BIGINT,
    @VolumeUnitId BIGINT,
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
        TradeDetailToVolumeUnitId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        TradeDetailId,
        VolumeUnitId
    FROM 
        [Mapping].[TradeDetailToVolumeUnit]
    WHERE 
        TradeDetailId = @TradeDetailId
        AND VolumeUnitId = @VolumeUnitId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
