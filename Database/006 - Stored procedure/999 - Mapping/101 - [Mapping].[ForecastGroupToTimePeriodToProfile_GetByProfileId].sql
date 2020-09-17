USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ForecastGroupToTimePeriodToProfile_GetByProfileId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ForecastGroupToTimePeriodToProfile_GetByProfileId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-14
-- Description:	Get ForecastGroupToTimePeriodToProfile info from [Mapping].[ForecastGroupToTimePeriodToProfile] table by Profile Id
-- =============================================

ALTER PROCEDURE [Mapping].[ForecastGroupToTimePeriodToProfile_GetByProfileId]
    @ProfileId BIGINT,
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
        ForecastGroupToTimePeriodToProfileId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ForecastGroupToTimePeriodId,
        ProfileId
    FROM 
        [Mapping].[ForecastGroupToTimePeriodToProfile]
    WHERE
        ProfileId = @ProfileId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
