USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[TimePeriod_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[TimePeriod_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-28
-- Description:	Insert new Time Period into [Information].[TimePeriod] table
-- =============================================

ALTER PROCEDURE [Information].[TimePeriod_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @StartTime TIME,
    @EndTime TIME
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-28 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[TimePeriod] WHERE StartTime = @StartTime
        AND EndDate = @EndDate
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[TimePeriod]
            (
                CreatedByUserId,
                SourceId,
                StartTime,
                EndTime
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @StartTime,
                @EndTime
            )
        END
END
GO
