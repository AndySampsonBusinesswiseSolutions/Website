USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[FiveMinuteToTimePeriod_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[FiveMinuteToTimePeriod_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-29
-- Description:	Insert new mapping of a FiveMinute to a TimePeriod into [Mapping].[FiveMinuteToTimePeriod] table
-- =============================================

ALTER PROCEDURE [Mapping].[FiveMinuteToTimePeriod_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @FiveMinuteId BIGINT,
    @TimePeriodId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-29 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].FiveMinuteToTimePeriod
    (
        CreatedByUserId,
        SourceId,
        FiveMinuteId,
        TimePeriodId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @FiveMinuteId,
        @TimePeriodId
    )
END
GO
