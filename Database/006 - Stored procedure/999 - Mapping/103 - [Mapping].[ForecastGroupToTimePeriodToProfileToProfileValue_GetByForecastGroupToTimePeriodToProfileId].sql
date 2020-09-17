USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue_GetByForecastGroupToTimePeriodToProfileId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue_GetByForecastGroupToTimePeriodToProfileId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-14
-- Description:	Get ForecastGroupToTimePeriodToProfileToProfileValue info from [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue] table by ForecastGroupToTimePeriodToProfile Id
-- =============================================

ALTER PROCEDURE [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue_GetByForecastGroupToTimePeriodToProfileId]
    @ForecastGroupToTimePeriodToProfileId BIGINT,
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
        ForecastGroupToTimePeriodToProfileToProfileValueId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ForecastGroupToTimePeriodToProfileId,
        ProfileValueId
    FROM 
        [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue]
    WHERE
        ForecastGroupToTimePeriodToProfileId = @ForecastGroupToTimePeriodToProfileId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
