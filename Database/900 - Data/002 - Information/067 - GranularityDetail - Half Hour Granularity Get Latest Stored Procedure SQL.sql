USE [EMaaS]

DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)

DECLARE @ForecastUsageHistoryGetLatestStoredProcedureSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage History Get Latest Stored Procedure SQL')
DECLARE @ForecastUsageLatestGetLatestStoredProcedureSQLGranularityAttributeId BIGINT = (SELECT GranularityAttributeId FROM [Information].[GranularityAttribute] WHERE GranularityAttributeDescription = 'Forecast Usage Latest Get Latest Stored Procedure SQL')

--Half Hour
DECLARE @GranularityId BIGINT = (SELECT GranularityId FROM [Information].[Granularity] WHERE GranularityGUID = 'CEA433FB-5327-4747-95CB-0FEFD1D2AD6B')

DECLARE @SQL NVARCHAR(MAX) = N'
    USE [EMaaS]

    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[Supply.X].[ForecastUsageHalfHourHistory_GetLatest]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [Supply.X].[ForecastUsageHalfHourHistory_GetLatest] AS BEGIN SET NOCOUNT ON; END'')
    END
    GO

	-- =============================================
    -- Author:		System Generated
    -- Create date: 2020-09-28
    -- Description:	Delete usage from [Supply.X].[ForecastUsageHalfHourHistory] table
    -- =============================================

    ALTER PROCEDURE [Supply.X].[ForecastUsageHalfHourHistory_GetLatest]
    AS
    BEGIN
        -- =============================================
        --              CHANGE HISTORY
        -- 2020-09-28 -> System Generated -> Initial development of script
        -- =============================================

        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        SELECT
            ForecastUsageHalfHourHistoryId,
            EffectiveFromDateTime,
            EffectiveToDateTime,
            CreatedDateTime,
            CreatedByUserId,
            SourceId,
            DateId,
            TimePeriodId,
            Usage
        FROM
            [Supply.X].[ForecastUsageHalfHourHistory]
        WHERE
            EffectiveToDateTime = ''9999-12-31''
    END'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageHistoryGetLatestStoredProcedureSQLGranularityAttributeId, @SQL

SET @SQL = N'
    USE [EMaaS]

    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[Supply.X].[ForecastUsageHalfHourLatest_GetLatest]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [Supply.X].[ForecastUsageHalfHourLatest_GetLatest] AS BEGIN SET NOCOUNT ON; END'')
    END
    GO

	-- =============================================
    -- Author:		System Generated
    -- Create date: 2020-09-28
    -- Description:	Delete usage from [Supply.X].[ForecastUsageHalfHourLatest] table
    -- =============================================

    ALTER PROCEDURE [Supply.X].[ForecastUsageHalfHourLatest_GetLatest]
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
            DateId,
            TimePeriodId,
            Usage
        FROM
            [Supply.X].[ForecastUsageHalfHourLatest]
    END'

EXEC [Information].[GranularityDetail_Insert] @CreatedByUserId, @SourceId, @GranularityId, @ForecastUsageLatestGetLatestStoredProcedureSQLGranularityAttributeId, @SQL