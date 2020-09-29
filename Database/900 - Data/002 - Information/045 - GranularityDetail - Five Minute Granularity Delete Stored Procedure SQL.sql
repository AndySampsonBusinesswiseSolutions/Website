USE [EMaaS]

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @ForecastUsageHistoryDeleteStoredProcedureSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage History Delete Stored Procedure SQL')
DECLARE @ForecastUsageLatestDeleteStoredProcedureSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage Latest Delete Stored Procedure SQL')

--Five Minute
DECLARE @GranularityId BIGINT = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = '4D55BB09-9F8F-4AB6-917E-23B1D09E71AD')

DECLARE @SQL NVARCHAR(MAX) = N'
    USE [EMaaS]

    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[Supply.X].[ForecastUsageFiveMinuteHistory_Delete]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [Supply.X].[ForecastUsageFiveMinuteHistory_Delete] AS BEGIN SET NOCOUNT ON; END'')
    END
    GO

	-- =============================================
    -- Author:		System Generated
    -- Create date: 2020-09-28
    -- Description:	Delete usage from [Supply.X].[ForecastUsageFiveMinuteHistory] table
    -- =============================================

    ALTER PROCEDURE [Supply.X].[ForecastUsageFiveMinuteHistory_Delete]
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
            [Supply.X].[ForecastUsageFiveMinuteHistory]
        SET
            ForecastUsageFiveMinuteHistory.EffectiveToDateTime = GETUTCDATE()
        FROM
            [Supply.X].[ForecastUsageFiveMinuteHistory]
        INNER JOIN
            [Supply.X].[ForecastUsageFiveMinuteHistory_Temp]
            ON ForecastUsageFiveMinuteHistory_Temp.DateId = ForecastUsageFiveMinuteHistory.DateId
            AND ForecastUsageFiveMinuteHistory_Temp.TimePeriodId = ForecastUsageFiveMinuteHistory.TimePeriodId
        WHERE
            ForecastUsageFiveMinuteHistory.EffectiveToDateTime = ''9999-12-31''
    END'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageHistoryDeleteStoredProcedureSQLGranularityAttributeId, @SQL

SET @SQL = N'
    USE [EMaaS]

    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[Supply.X].[ForecastUsageFiveMinuteLatest_Delete]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [Supply.X].[ForecastUsageFiveMinuteLatest_Delete] AS BEGIN SET NOCOUNT ON; END'')
    END
    GO

	-- =============================================
    -- Author:		System Generated
    -- Create date: 2020-09-28
    -- Description:	Delete usage from [Supply.X].[ForecastUsageFiveMinuteLatest] table
    -- =============================================

    ALTER PROCEDURE [Supply.X].[ForecastUsageFiveMinuteLatest_Delete]
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
            [Supply.X].[ForecastUsageFiveMinuteLatest]
        FROM
            [Supply.X].[ForecastUsageFiveMinuteLatest]
        INNER JOIN
            [Supply.X].[ForecastUsageFiveMinuteLatest_Temp]
            ON ForecastUsageFiveMinuteLatest_Temp.DateId = ForecastUsageFiveMinuteLatest.DateId
            AND ForecastUsageFiveMinuteLatest_Temp.TimePeriodId = ForecastUsageFiveMinuteLatest.TimePeriodId
    END'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageLatestDeleteStoredProcedureSQLGranularityAttributeId, @SQL