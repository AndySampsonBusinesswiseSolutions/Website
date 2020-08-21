USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[GranularityToTimePeriod_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[GranularityToTimePeriod_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-21
-- Description:	Insert new mapping of a Granularity to a TimePeriod into [Mapping].[GranularityToTimePeriod] table
-- =============================================

ALTER PROCEDURE [Mapping].[GranularityToTimePeriod_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @GranularityId BIGINT,
    @TimePeriodId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-21 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].GranularityToTimePeriod
    (
        CreatedByUserId,
        SourceId,
        GranularityId,
        TimePeriodId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @GranularityId,
        @TimePeriodId
    )
END
GO
