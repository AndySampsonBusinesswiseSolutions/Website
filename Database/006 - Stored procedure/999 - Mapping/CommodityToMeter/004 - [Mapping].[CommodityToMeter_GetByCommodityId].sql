USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[CommodityToMeter_GetByCommodityId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[CommodityToMeter_GetByCommodityId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-10-19
-- Description:	Get CommodityToMeter info from [Mapping].[CommodityToMeter] table by Commodity Id
-- =============================================

ALTER PROCEDURE [Mapping].[CommodityToMeter_GetByCommodityId]
    @CommodityId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-10-19 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        CommodityToMeterId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        CommodityId,
        MeterId
    FROM 
        [Mapping].[CommodityToMeter]
    WHERE 
        CommodityId = @CommodityId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
