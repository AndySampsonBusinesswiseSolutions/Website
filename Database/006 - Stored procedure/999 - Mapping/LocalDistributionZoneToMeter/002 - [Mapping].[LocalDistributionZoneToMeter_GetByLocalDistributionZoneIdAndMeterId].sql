USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[LocalDistributionZoneToMeter_GetByLocalDistributionZoneIdAndMeterId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[LocalDistributionZoneToMeter_GetByLocalDistributionZoneIdAndMeterId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-12
-- Description:	Get LocalDistributionZoneToMeter info from [Mapping].[LocalDistributionZoneToMeter] table by LocalDistributionZone Id and Meter Id
-- =============================================

ALTER PROCEDURE [Mapping].[LocalDistributionZoneToMeter_GetByLocalDistributionZoneIdAndMeterId]
    @LocalDistributionZoneId BIGINT,
    @MeterId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-12 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        LocalDistributionZoneToMeterId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        LocalDistributionZoneId,
        MeterId
    FROM 
        [Mapping].[LocalDistributionZoneToMeter]
    WHERE 
        LocalDistributionZoneId = @LocalDistributionZoneId
        AND MeterId = @MeterId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
