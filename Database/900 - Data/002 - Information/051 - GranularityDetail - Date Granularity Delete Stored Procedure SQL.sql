USE [EMaaS]
GO

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @ForecastUsageHistoryDeleteStoredProcedureSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage History Delete Stored Procedure SQL')
DECLARE @ForecastUsageLatestDeleteStoredProcedureSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage Latest Delete Stored Procedure SQL')

--Date
DECLARE @GranularityId BIGINT = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = '71C54EC0-6415-42D4-9C8C-6D8B8513F2FE')

DECLARE @SQL NVARCHAR(MAX) = N'
    USE [EMaaS]

    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[Supply.X].[ForecastUsageDateHistory_Delete]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [Supply.X].[ForecastUsageDateHistory_Delete] AS BEGIN SET NOCOUNT ON; END'')
    END
    GO

	-- =============================================
    -- Author:		System Generated
    -- Create date: 2020-09-28
    -- Description:	Delete usage from [Supply.X].[ForecastUsageDateHistory] table
    -- =============================================

    ALTER PROCEDURE [Supply.X].[ForecastUsageDateHistory_Delete]
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
            [Supply.X].[ForecastUsageDateHistory]
        SET
            ForecastUsageDateHistory.EffectiveToDateTime = GETUTCDATE()
        FROM
            [Supply.X].[ForecastUsageDateHistory]
        INNER JOIN
            [Supply.X].[ForecastUsageDateHistory_Temp]
            ON ForecastUsageDateHistory_Temp.DateId = ForecastUsageDateHistory.DateId
        WHERE
            ForecastUsageDateHistory.EffectiveToDateTime = ''9999-12-31''
    END'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageHistoryDeleteStoredProcedureSQLGranularityAttributeId, @SQL

SET @SQL = N'
    USE [EMaaS]

    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[Supply.X].[ForecastUsageDateLatest_Delete]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [Supply.X].[ForecastUsageDateLatest_Delete] AS BEGIN SET NOCOUNT ON; END'')
    END
    GO

	-- =============================================
    -- Author:		System Generated
    -- Create date: 2020-09-28
    -- Description:	Delete usage from [Supply.X].[ForecastUsageDateLatest] table
    -- =============================================

    ALTER PROCEDURE [Supply.X].[ForecastUsageDateLatest_Delete]
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
            [Supply.X].[ForecastUsageDateLatest]
        FROM
            [Supply.X].[ForecastUsageDateLatest]
        INNER JOIN
            [Supply.X].[ForecastUsageDateLatest_Temp]
            ON ForecastUsageDateLatest_Temp.DateId = ForecastUsageDateLatest.DateId
    END'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageLatestDeleteStoredProcedureSQLGranularityAttributeId, @SQL