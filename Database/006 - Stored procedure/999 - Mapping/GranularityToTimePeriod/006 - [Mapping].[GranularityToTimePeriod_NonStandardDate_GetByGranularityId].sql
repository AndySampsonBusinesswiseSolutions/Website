USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[GranularityToTimePeriod_NonStandardDate_GetByGranularityId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[GranularityToTimePeriod_NonStandardDate_GetByGranularityId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-18
-- Description:	Get GranularityToTimePeriod_NonStandardDate info from [Mapping].[GranularityToTimePeriod_NonStandardDate] table by GranularityId Id
-- =============================================

ALTER PROCEDURE [Mapping].[GranularityToTimePeriod_NonStandardDate_GetByGranularityId]
    @GranularityId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-18 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        GranularityToTimePeriod_NonStandardDateId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        GranularityId,
        TimePeriodId,
        DateId
    FROM 
        [Mapping].[GranularityToTimePeriod_NonStandardDate]
    WHERE 
        GranularityId = @GranularityId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
