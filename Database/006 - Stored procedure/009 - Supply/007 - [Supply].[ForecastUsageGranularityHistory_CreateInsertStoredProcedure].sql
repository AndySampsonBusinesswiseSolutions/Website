USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supply].[ForecastUsageGranularityHistory_CreateInsertStoredProcedure]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supply].[ForecastUsageGranularityHistory_CreateInsertStoredProcedure] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-30
-- Description:	Create new ForecastUsageGranularityHistory Insert Stored Procedure for MeterId
-- =============================================

ALTER PROCEDURE [Supply].[ForecastUsageGranularityHistory_CreateInsertStoredProcedure]
    @MeterId BIGINT,
    @GranularityCode VARCHAR(255),
    @MeterType VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-30 -> Andrew Sampson -> Initial development of script
    -- 2020-08-14 -> Andrew Sampson -> Added MeterType parameter
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @SchemaName NVARCHAR(255) = 'Supply.' + @MeterType + CONVERT(NVARCHAR, @MeterId)
    DECLARE @StoredProcedureName NVARCHAR(255) = 'ForecastUsage' + @GranularityCode + 'History_Insert'
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
    -- Description:	Insert usage into [' + @SchemaName +'].[ForecastUsage' + @GranularityCode + 'History] table
    -- =============================================

    ALTER PROCEDURE [' + @SchemaName +'].[' + @StoredProcedureName + ']
        @CreatedByUserId BIGINT,
        @SourceId BIGINT,
        @' + @GranularityCode + 'Id BIGINT,'
        
    IF @RequiresDateParameter = 1
        BEGIN
            SET @SQL = @SQL + '
            @DateId BIGINT,'
        END

    SET @SQL = @SQL + '
        @Usage DECIMAL(18,10)
    AS
    BEGIN
        -- =============================================
        --              CHANGE HISTORY
        -- ' + @TodaysDate + ' -> System Generated -> Initial development of script
        -- =============================================

        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        IF NOT EXISTS(SELECT TOP 1 1 FROM [' + @SchemaName +'].[' + @StoredProcedureName + '] WHERE ' + @GranularityCode + 'Id = @' + @GranularityCode + 'Id'
        
    IF @RequiresDateParameter = 1
        BEGIN
            SET @SQL = @SQL + '
            AND DateId = @DateId'
        END

    SET @SQL = @SQL + ')
        BEGIN
            INSERT INTO [' + @SchemaName +'].[' + @StoredProcedureName + ']
            (
                CreatedByUserId,
                SourceId,
                ' + @GranularityCode + 'Id,'
        
    IF @RequiresDateParameter = 1
        BEGIN
            SET @SQL = @SQL + '
            DateId,'
        END

    SET @SQL = @SQL + '
                Usage
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @' + @GranularityCode + 'Id,'
        
    IF @RequiresDateParameter = 1
        BEGIN
            SET @SQL = @SQL + '
            @DateId,'
        END

    SET @SQL = @SQL + '
                @Usage
            )
        END
	END'

    SET @MetaSQL = '
	USE [EMaaS]
	EXEC (''' + REPLACE(@SQL, '''', '''''') + ''')
	'

    EXEC sp_sqlexec @MetaSQL
END
GO
