USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @ForecastUsageHistoryDeleteStoredProcedureSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage History Delete Stored Procedure SQL')
DECLARE @ForecastUsageLatestDeleteStoredProcedureSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage Latest Delete Stored Procedure SQL')

--Quarter
DECLARE @GranularityId BIGINT = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = '8029270C-1ECB-43B0-B313-9082890CDC8B')

DECLARE @SQL NVARCHAR(MAX) = N'
    USE [EMaaS]

    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[Supply.X].[ForecastUsageQuarterHistory_Delete]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [Supply.X].[ForecastUsageQuarterHistory_Delete] AS BEGIN SET NOCOUNT ON; END'')
    END
    GO

	-- =============================================
    -- Author:		System Generated
    -- Create date: 2020-09-28
    -- Description:	Delete usage from [Supply.X].[ForecastUsageQuarterHistory] table
    -- =============================================

    ALTER PROCEDURE [Supply.X].[ForecastUsageQuarterHistory_Delete]
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
            [Supply.X].[ForecastUsageQuarterHistory]
        SET
            ForecastUsageQuarterHistory.EffectiveToDateTime = GETUTCDATE()
        FROM
            [Supply.X].[ForecastUsageQuarterHistory]
        INNER JOIN
            [Supply.X].[ForecastUsageQuarterHistory_Temp]
            ON ForecastUsageQuarterHistory_Temp.YearId = ForecastUsageQuarterHistory.YearId
            AND ForecastUsageQuarterHistory_Temp.QuarterId = ForecastUsageQuarterHistory.QuarterId
        WHERE
            ForecastUsageQuarterHistory.EffectiveToDateTime = ''9999-12-31''
    END'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageHistoryDeleteStoredProcedureSQLGranularityAttributeId, @SQL

SET @SQL = N'
    USE [EMaaS]

    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[Supply.X].[ForecastUsageQuarterLatest_Delete]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [Supply.X].[ForecastUsageQuarterLatest_Delete] AS BEGIN SET NOCOUNT ON; END'')
    END
    GO

	-- =============================================
    -- Author:		System Generated
    -- Create date: 2020-09-28
    -- Description:	Delete usage from [Supply.X].[ForecastUsageQuarterLatest] table
    -- =============================================

    ALTER PROCEDURE [Supply.X].[ForecastUsageQuarterLatest_Delete]
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
            [Supply.X].[ForecastUsageQuarterLatest]
        FROM
            [Supply.X].[ForecastUsageQuarterLatest]
        INNER JOIN
            [Supply.X].[ForecastUsageQuarterLatest_Temp]
            ON ForecastUsageQuarterLatest_Temp.YearId = ForecastUsageQuarterLatest.YearId
            AND ForecastUsageQuarterLatest_Temp.QuarterId = ForecastUsageQuarterLatest.QuarterId
    END'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageLatestDeleteStoredProcedureSQLGranularityAttributeId, @SQL