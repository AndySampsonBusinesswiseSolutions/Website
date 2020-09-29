USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @ForecastUsageHistoryInsertStoredProcedureSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage History Insert Stored Procedure SQL')
DECLARE @ForecastUsageLatestInsertStoredProcedureSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage Latest Insert Stored Procedure SQL')

--Month
DECLARE @GranularityId BIGINT = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = '2AB4DC83-C0D0-4C5F-AC95-6A948802E430')

DECLARE @SQL NVARCHAR(MAX) = N'
    USE [EMaaS]

    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[Supply.X].[ForecastUsageMonthHistory_Insert]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [Supply.X].[ForecastUsageMonthHistory_Insert] AS BEGIN SET NOCOUNT ON; END'')
    END
    GO

	-- =============================================
    -- Author:		System Generated
    -- Create date: 2020-09-28
    -- Description:	Insert usage into [Supply.X].[ForecastUsageMonthHistory] table
    -- =============================================

    ALTER PROCEDURE [Supply.X].[ForecastUsageMonthHistory_Insert]
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

        INSERT INTO [Supply.X].[ForecastUsageMonthHistory]
        (
            CreatedByUserId,
            SourceId,
			YearId,
            MonthId,
            Usage
        )
        SELECT
            CreatedByUserId,
            SourceId,
			YearId,
            MonthId,
            Usage
        FROM
            [Supply.X].[ForecastUsageMonthHistory_Temp]
        WHERE
            ProcessQueueGUID = @ProcessQueueGUID

        DELETE
        FROM
            [Supply.X].[ForecastUsageMonthHistory_Temp]
        WHERE
            ProcessQueueGUID = @ProcessQueueGUID
	END'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageHistoryInsertStoredProcedureSQLGranularityAttributeId, @SQL

SET @SQL = N'
    USE [EMaaS]

    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[Supply.X].[ForecastUsageMonthLatest_Insert]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [Supply.X].[ForecastUsageMonthLatest_Insert] AS BEGIN SET NOCOUNT ON; END'')
    END
    GO

	-- =============================================
    -- Author:		System Generated
    -- Create date: 2020-09-28
    -- Description:	Insert usage into [Supply.X].[ForecastUsageMonthLatest] table
    -- =============================================

    ALTER PROCEDURE [Supply.X].[ForecastUsageMonthLatest_Insert]
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

        INSERT INTO [Supply.X].[ForecastUsageMonthLatest]
        (
			YearId,
            MonthId,
            Usage
        )
        SELECT
			YearId,
            MonthId,
            Usage
        FROM
            [Supply.X].[ForecastUsageMonthLatest_Temp]
        WHERE
            ProcessQueueGUID = @ProcessQueueGUID

        DELETE
        FROM
            [Supply.X].[ForecastUsageMonthLatest_Temp]
        WHERE
            ProcessQueueGUID = @ProcessQueueGUID
	END'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageLatestInsertStoredProcedureSQLGranularityAttributeId, @SQL