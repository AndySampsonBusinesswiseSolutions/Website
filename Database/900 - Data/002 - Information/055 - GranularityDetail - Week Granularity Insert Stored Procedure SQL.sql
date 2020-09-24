USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @ForecastUsageHistoryInsertStoredProcedureSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage History Insert Stored Procedure SQL')
DECLARE @ForecastUsageLatestInsertStoredProcedureSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage Latest Insert Stored Procedure SQL')

--Week
DECLARE @GranularityId BIGINT = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = '8FD4C63A-84D5-4A03-B488-1A99C2331726')

DECLARE @SQL NVARCHAR(MAX) = N'
    USE [EMaaS]

    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[Supply.X].[ForecastUsageWeekHistory_Insert]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [Supply.X].[ForecastUsageWeekHistory_Insert] AS BEGIN SET NOCOUNT ON; END'')
    END
    GO

	-- =============================================
    -- Author:		System Generated
    -- Create date: 2020-09-24
    -- Description:	Insert usage into [Supply.X].[ForecastUsageWeekHistory] table
    -- =============================================

    ALTER PROCEDURE [Supply.X].[ForecastUsageWeekHistory_Insert]
        @CreatedByUserId BIGINT,
        @SourceId BIGINT,
		@WeekId BIGINT,
        @YearId BIGINT,
        @Usage DECIMAL(18,10)
    AS
    BEGIN
        -- =============================================
        --              CHANGE HISTORY
        -- 2020-09-24 -> System Generated -> Initial development of script
        -- =============================================

        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        INSERT INTO [Supply.X].[ForecastUsageWeekHistory]
        (
            CreatedByUserId,
            SourceId,
			WeekId,
            YearId,
            Usage
        )
        VALUES
        (
            @CreatedByUserId,
            @SourceId,
			@WeekId,
            @YearId,
            @Usage
        )
	END'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageHistoryInsertStoredProcedureSQLGranularityAttributeId, @SQL

SET @SQL = N'
    USE [EMaaS]

    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[Supply.X].[ForecastUsageWeekLatest_Insert]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [Supply.X].[ForecastUsageWeekLatest_Insert] AS BEGIN SET NOCOUNT ON; END'')
    END
    GO

	-- =============================================
    -- Author:		System Generated
    -- Create date: 2020-09-24
    -- Description:	Insert usage into [Supply.X].[ForecastUsageWeekLatest] table
    -- =============================================

    ALTER PROCEDURE [Supply.X].[ForecastUsageWeekLatest_Insert]
		@WeekId BIGINT,
        @YearId BIGINT,
        @Usage DECIMAL(18,10)
    AS
    BEGIN
        -- =============================================
        --              CHANGE Latest
        -- 2020-09-24 -> System Generated -> Initial development of script
        -- =============================================

        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        INSERT INTO [Supply.X].[ForecastUsageWeekLatest]
        (
			WeekId,
            YearId,
            Usage
        )
        VALUES
        (
			@WeekId,
            @YearId,
            @Usage
        )
	END'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageLatestInsertStoredProcedureSQLGranularityAttributeId, @SQL