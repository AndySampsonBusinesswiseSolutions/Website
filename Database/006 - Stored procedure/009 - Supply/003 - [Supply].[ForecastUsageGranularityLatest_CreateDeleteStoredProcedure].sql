USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supply].[ForecastUsageGranularityLatest_CreateDeleteStoredProcedure]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supply].[ForecastUsageGranularityLatest_CreateDeleteStoredProcedure] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-29
-- Description:	Create new ForecastUsageGranularityLatest Delete Stored Procedure for MeterId
-- =============================================

ALTER PROCEDURE [Supply].[ForecastUsageGranularityLatest_CreateDeleteStoredProcedure]
    @MeterId BIGINT,
    @GranularityCode VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-29 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @SchemaName NVARCHAR(255) = 'Supply.Meter' + CONVERT(NVARCHAR, @MeterId)
    DECLARE @StoredProcedureName NVARCHAR(255) = 'ForecastUsage' + @GranularityCode + 'Latest_Delete'
    DECLARE @TodaysDate NVARCHAR(10) = (SELECT FORMAT(GetDate(), 'yyyy-MM-dd'))
    DECLARE @RequiresDateParameter BIT = (SELECT IsTimePeriod FROM [Information].[Granularity] WHERE GranularityCode = @GranularityCode)

    DECLARE @SQL NVARCHAR(MAX) = N'
    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[' + @SchemaName +'].[' + @StoredProcedureName + ']''))
    BEGIN
        EXEC(''CREATE PROCEDURE [' + @SchemaName +'].[' + @StoredProcedureName + '] AS BEGIN SET NOCOUNT ON; END'')
    END'

	DECLARE @MetaSQL NVARCHAR(MAX) = '
	USE [EMaaS]
	EXEC (''' + REPLACE(@SQL, '''', '''''') + ''')
	'

	EXEC sp_sqlexec @MetaSQL
    
    SET @SQL = '
	-- =============================================
    -- Author:		System Generated
    -- Create date: ' + @TodaysDate + '
    -- Description:	Delete usage from [' + @SchemaName +'].[ForecastUsage' + @GranularityCode + 'Latest] table
    -- =============================================

    ALTER PROCEDURE [' + @SchemaName +'].[' + @StoredProcedureName + ']
        @' + @GranularityCode + 'Id BIGINT,'
        
    IF @RequiresDateParameter = 1
        BEGIN
            SET @SQL = @SQL + '
            @DateId BIGINT,'
        END

    SET @SQL = @SQL + '
    AS
    BEGIN
        -- =============================================
        --              CHANGE HISTORY
        -- ' + @TodaysDate + ' -> System Generated -> Initial development of script
        -- =============================================

        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        DELETE
        FROM
            [' + @SchemaName +'].[ForecastUsage' + @GranularityCode + 'Latest]
        WHERE
            ' + @GranularityCode + 'Id = @' + @GranularityCode + 'Id'
        
    IF @RequiresDateParameter = 1
        BEGIN
            SET @SQL = @SQL + '
            AND DateId = @DateId'
        END

    SET @SQL = @SQL + '
    END'

    SET @MetaSQL = '
	USE [EMaaS]
	EXEC (''' + REPLACE(@SQL, '''', '''''') + ''')
	'

    EXEC sp_sqlexec @MetaSQL
END
GO
