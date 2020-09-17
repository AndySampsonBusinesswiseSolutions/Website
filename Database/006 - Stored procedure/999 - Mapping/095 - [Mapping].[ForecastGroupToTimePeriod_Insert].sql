USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ForecastGroupToTimePeriod_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ForecastGroupToTimePeriod_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-09
-- Description:	Insert new mapping of a ForecastGroup to a TimePeriod into [Mapping].[ForecastGroupToTimePeriod] table
-- =============================================

ALTER PROCEDURE [Mapping].[ForecastGroupToTimePeriod_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ForecastGroupId BIGINT,
    @TimePeriodId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-09 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].ForecastGroupToTimePeriod
    (
        CreatedByUserId,
        SourceId,
        ForecastGroupId,
        TimePeriodId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ForecastGroupId,
        @TimePeriodId
    )
END
GO
