USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @ForecastUsageHistoryInsertStoredProcedureSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage History Insert Stored Procedure SQL')
DECLARE @ForecastUsageLatestInsertStoredProcedureSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage Latest Insert Stored Procedure SQL')

--Half Hour
DECLARE @GranularityId BIGINT = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = 'CEA433FB-5327-4747-95CB-0FEFD1D2AD6B')

DECLARE @SQL NVARCHAR(MAX) = N'
    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[Supply.X].[ForecastUsageHalfHourHistory_Insert]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [Supply.X].[ForecastUsageHalfHourHistory_Insert] AS BEGIN SET NOCOUNT ON; END'')
    END

	-- =============================================
    -- Author:		System Generated
    -- Create date: 2020-09-24
    -- Description:	Insert usage into [Supply.X].[ForecastUsageHalfHourHistory] table
    -- =============================================

    CREATE PROCEDURE [Supply.X].[ForecastUsageHalfHourHistory_Insert]
        @CreatedByUserId BIGINT,
        @SourceId BIGINT,
		@DateId BIGINT,
        @TimePeriodId BIGINT,
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

        INSERT INTO [Supply.X].[ForecastUsageHalfHourHistory]
        (
            CreatedByUserId,
            SourceId,
			DateId,
            TimePeriodId,
            Usage
        )
        VALUES
        (
            @CreatedByUserId,
            @SourceId,
			@DateId,
            @TimePeriodId,
            @Usage
        )
	END'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageHistoryInsertStoredProcedureSQLGranularityAttributeId, @SQL

SET @SQL = N'
    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[Supply.X].[ForecastUsageHalfHourLatest_Insert]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [Supply.X].[ForecastUsageHalfHourLatest_Insert] AS BEGIN SET NOCOUNT ON; END'')
    END

	-- =============================================
    -- Author:		System Generated
    -- Create date: 2020-09-24
    -- Description:	Insert usage into [Supply.X].[ForecastUsageHalfHourLatest] table
    -- =============================================

    CREATE PROCEDURE [Supply.X].[ForecastUsageHalfHourLatest_Insert]
		@DateId BIGINT,
        @TimePeriodId BIGINT,
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

        INSERT INTO [Supply.X].[ForecastUsageHalfHourLatest]
        (
			DateId,
            TimePeriodId,
            Usage
        )
        VALUES
        (
			@DateId,
            @TimePeriodId,
            @Usage
        )
	END'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageLatestInsertStoredProcedureSQLGranularityAttributeId, @SQL