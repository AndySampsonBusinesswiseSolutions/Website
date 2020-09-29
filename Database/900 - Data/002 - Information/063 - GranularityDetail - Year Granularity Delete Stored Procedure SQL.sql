USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @ForecastUsageHistoryDeleteStoredProcedureSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage History Delete Stored Procedure SQL')
DECLARE @ForecastUsageLatestDeleteStoredProcedureSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage Latest Delete Stored Procedure SQL')

--Year
DECLARE @GranularityId BIGINT = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = '3799717D-303B-458F-8A38-5DFA934ED431')

DECLARE @SQL NVARCHAR(MAX) = N'
    USE [EMaaS]

    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[Supply.X].[ForecastUsageYearHistory_Delete]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [Supply.X].[ForecastUsageYearHistory_Delete] AS BEGIN SET NOCOUNT ON; END'')
    END
    GO

	-- =============================================
    -- Author:		System Generated
    -- Create date: 2020-09-28
    -- Description:	Delete usage from [Supply.X].[ForecastUsageYearHistory] table
    -- =============================================

    ALTER PROCEDURE [Supply.X].[ForecastUsageYearHistory_Delete]
    AS
    BEGIN
        -- =============================================
        --              CHANGE HISTORY
        -- 2020-09-28 -> System Generated -> Initial development of script
        -- =============================================

        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        UPDATE
            [Supply.X].[ForecastUsageYearHistory]
        SET
            ForecastUsageYearHistory.EffectiveToDateTime = GETUTCDATE()
        FROM
            [Supply.X].[ForecastUsageYearHistory]
        INNER JOIN
            [Supply.X].[ForecastUsageYearHistory_Temp]
            ON ForecastUsageYearHistory_Temp.YearId = ForecastUsageYearHistory.YearId
        WHERE
            ForecastUsageYearHistory.EffectiveToDateTime = ''9999-12-31''
    END'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageHistoryDeleteStoredProcedureSQLGranularityAttributeId, @SQL

SET @SQL = N'
    USE [EMaaS]

    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[Supply.X].[ForecastUsageYearLatest_Delete]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [Supply.X].[ForecastUsageYearLatest_Delete] AS BEGIN SET NOCOUNT ON; END'')
    END
    GO

	-- =============================================
    -- Author:		System Generated
    -- Create date: 2020-09-28
    -- Description:	Delete usage from [Supply.X].[ForecastUsageYearLatest] table
    -- =============================================

    ALTER PROCEDURE [Supply.X].[ForecastUsageYearLatest_Delete]
    AS
    BEGIN
        -- =============================================
        --              CHANGE Latest
        -- 2020-09-28 -> System Generated -> Initial development of script
        -- =============================================

        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        DELETE
            [Supply.X].[ForecastUsageYearLatest]
        FROM
            [Supply.X].[ForecastUsageYearLatest]
        INNER JOIN
            [Supply.X].[ForecastUsageYearLatest_Temp]
            ON ForecastUsageYearLatest_Temp.YearId = ForecastUsageYearLatest.YearId
    END'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageLatestDeleteStoredProcedureSQLGranularityAttributeId, @SQL