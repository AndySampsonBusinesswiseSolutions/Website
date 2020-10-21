USE [EMaaS]

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @ForecastUsageHistoryGetLatestStoredProcedureSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage History Get Latest Stored Procedure SQL')
DECLARE @ForecastUsageLatestGetLatestStoredProcedureSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage Latest Get Latest Stored Procedure SQL')

--Year
DECLARE @GranularityId BIGINT = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = '3799717D-303B-458F-8A38-5DFA934ED431')

DECLARE @SQL NVARCHAR(MAX) = N'
    USE [EMaaS]

    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[Supply.X].[ForecastUsageYearHistory_GetLatest]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [Supply.X].[ForecastUsageYearHistory_GetLatest] AS BEGIN SET NOCOUNT ON; END'')
    END
    GO

	-- =============================================
    -- Author:		System Generated
    -- Create date: 2020-09-28
    -- Description:	Get latest usage from [Supply.X].[ForecastUsageYearHistory] table
    -- =============================================

    ALTER PROCEDURE [Supply.X].[ForecastUsageYearHistory_GetLatest]
    AS
    BEGIN
        -- =============================================
        --              CHANGE HISTORY
        -- 2020-09-28 -> System Generated -> Initial development of script
        -- =============================================

        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        WITH Latest AS
        (
            SELECT
                YearId,
                MAX(ForecastUsageYearHistoryId) AS ForecastUsageYearHistoryId
            FROM
                [Supply.X].[ForecastUsageYearHistory]
            GROUP BY
                YearId
        )

        SELECT
            ForecastUsageYearHistory.ForecastUsageYearHistoryId,
            ForecastUsageYearHistory.CreatedDateTime,
            ForecastUsageYearHistory.CreatedByUserId,
            ForecastUsageYearHistory.SourceId,
            ForecastUsageYearHistory.YearId,
            ForecastUsageYearHistory.Usage
        FROM
            [Supply.X].[ForecastUsageYearHistory]
        INNER JOIN
            Latest
            ON Latest.ForecastUsageYearHistoryId = ForecastUsageYearHistory.ForecastUsageYearHistoryId
    END'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageHistoryGetLatestStoredProcedureSQLGranularityAttributeId, @SQL

SET @SQL = N'
    USE [EMaaS]

    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[Supply.X].[ForecastUsageYearLatest_GetLatest]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [Supply.X].[ForecastUsageYearLatest_GetLatest] AS BEGIN SET NOCOUNT ON; END'')
    END
    GO

	-- =============================================
    -- Author:		System Generated
    -- Create date: 2020-09-28
    -- Description:	Get latest usage from [Supply.X].[ForecastUsageYearLatest] table
    -- =============================================

    ALTER PROCEDURE [Supply.X].[ForecastUsageYearLatest_GetLatest]
    AS
    BEGIN
        -- =============================================
        --              CHANGE Latest
        -- 2020-09-28 -> System Generated -> Initial development of script
        -- =============================================

        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        SELECT
            YearId,
            Usage
        FROM
            [Supply.X].[ForecastUsageYearLatest]
    END'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageLatestGetLatestStoredProcedureSQLGranularityAttributeId, @SQL