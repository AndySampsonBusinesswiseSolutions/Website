USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ForecastGroupToTimePeriod_GetList]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ForecastGroupToTimePeriod_GetList] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-14
-- Description:	Get ForecastGroupToTimePeriod info from [Mapping].[ForecastGroupToTimePeriod] table
-- =============================================

ALTER PROCEDURE [Mapping].[ForecastGroupToTimePeriod_GetList]
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-14 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        ForecastGroupToTimePeriodId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ForecastGroupId,
        TimePeriodId
    FROM 
        [Mapping].[ForecastGroupToTimePeriod]
    WHERE
        @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
