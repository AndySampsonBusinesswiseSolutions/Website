USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-09
-- Description:	Insert new mapping of a ForecastGroupToTimePeriod to a Profile into [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue] table
-- =============================================

ALTER PROCEDURE [Mapping].[ForecastGroupToTimePeriodToProfileToProfileValue_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ForecastGroupToTimePeriodToProfileId BIGINT,
    @ProfileValueId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-09 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent etra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].ForecastGroupToTimePeriodToProfileToProfileValue
    (
        CreatedByUserId,
        SourceId,
        ForecastGroupToTimePeriodToProfileId,
        ProfileValueId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ForecastGroupToTimePeriodToProfileId,
        @ProfileValueId
    )
END
GO
