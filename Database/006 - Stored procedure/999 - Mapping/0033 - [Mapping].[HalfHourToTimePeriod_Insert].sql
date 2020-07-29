USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[HalfHourToTimePeriod_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[HalfHourToTimePeriod_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-29
-- Description:	Insert new mapping of a HalfHour to a TimePeriod into [Mapping].[HalfHourToTimePeriod] table
-- =============================================

ALTER PROCEDURE [Mapping].[HalfHourToTimePeriod_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @HalfHourId BIGINT,
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

    INSERT INTO [Mapping].HalfHourToTimePeriod
    (
        CreatedByUserId,
        SourceId,
        HalfHourId,
        TimePeriodId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @HalfHourId,
        @TimePeriodId
    )
END
GO
