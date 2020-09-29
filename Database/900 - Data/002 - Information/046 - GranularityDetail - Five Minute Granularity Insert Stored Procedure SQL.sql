USE [EMaaS]

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @ForecastUsageHistoryInsertStoredProcedureSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage History Insert Stored Procedure SQL')
DECLARE @ForecastUsageLatestInsertStoredProcedureSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage Latest Insert Stored Procedure SQL')

--Five Minute
DECLARE @GranularityId BIGINT = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = '4D55BB09-9F8F-4AB6-917E-23B1D09E71AD')

DECLARE @SQL NVARCHAR(MAX) = N'
    USE [EMaaS]

    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[Supply.X].[ForecastUsageFiveMinuteHistory_Insert]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [Supply.X].[ForecastUsageFiveMinuteHistory_Insert] AS BEGIN SET NOCOUNT ON; END'')
    END
    GO

	-- =============================================
    -- Author:		System Generated
    -- Create date: 2020-09-28
    -- Description:	Insert usage into [Supply.X].[ForecastUsageFiveMinuteHistory] table
    -- =============================================

    ALTER PROCEDURE [Supply.X].[ForecastUsageFiveMinuteHistory_Insert]
        @ProcessQueueGUID VARCHAR(255)
    AS
    BEGIN
        -- =============================================
        --              CHANGE HISTORY
        -- 2020-09-28 -> System Generated -> Initial development of script
        -- =============================================

        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        INSERT INTO [Supply.X].[ForecastUsageFiveMinuteHistory]
        (
            CreatedByUserId,
            SourceId,
			DateId,
            TimePeriodId,
            Usage
        )
        SELECT
            CreatedByUserId,
            SourceId,
			DateId,
            TimePeriodId,
            Usage
        FROM
            [Supply.X].[ForecastUsageFiveMinuteHistory_Temp]
        WHERE
            ProcessQueueGUID = @ProcessQueueGUID

        DELETE
        FROM
            [Supply.X].[ForecastUsageFiveMinuteHistory_Temp]
        WHERE
            ProcessQueueGUID = @ProcessQueueGUID
	END'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageHistoryInsertStoredProcedureSQLGranularityAttributeId, @SQL

SET @SQL = N'
    USE [EMaaS]

    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[Supply.X].[ForecastUsageFiveMinuteLatest_Insert]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [Supply.X].[ForecastUsageFiveMinuteLatest_Insert] AS BEGIN SET NOCOUNT ON; END'')
    END
    GO

	-- =============================================
    -- Author:		System Generated
    -- Create date: 2020-09-28
    -- Description:	Insert usage into [Supply.X].[ForecastUsageFiveMinuteLatest] table
    -- =============================================

    ALTER PROCEDURE [Supply.X].[ForecastUsageFiveMinuteLatest_Insert]
        @ProcessQueueGUID VARCHAR(255)
    AS
    BEGIN
        -- =============================================
        --              CHANGE Latest
        -- 2020-09-28 -> System Generated -> Initial development of script
        -- =============================================

        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        INSERT INTO [Supply.X].[ForecastUsageFiveMinuteLatest]
        (
			DateId,
            TimePeriodId,
            Usage
        )
        SELECT
			DateId,
            TimePeriodId,
            Usage
        FROM
            [Supply.X].[ForecastUsageFiveMinuteLatest_Temp]
        WHERE
            ProcessQueueGUID = @ProcessQueueGUID

        DELETE
        FROM
            [Supply.X].[ForecastUsageFiveMinuteLatest_Temp]
        WHERE
            ProcessQueueGUID = @ProcessQueueGUID
	END'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageLatestInsertStoredProcedureSQLGranularityAttributeId, @SQL