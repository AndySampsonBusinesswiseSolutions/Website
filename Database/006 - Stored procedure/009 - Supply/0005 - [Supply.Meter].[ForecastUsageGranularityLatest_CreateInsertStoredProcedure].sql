USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supply.Meter].[ForecastUsageGranularityLatest_CreateInsertStoredProcedure]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supply.Meter].[ForecastUsageGranularityLatest_CreateInsertStoredProcedure] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-30
-- Description:	Create new ForecastUsageGranularityLatest Insert Stored Procedure for MeterId
-- =============================================

ALTER PROCEDURE [Supply.Meter].[ForecastUsageGranularityLatest_CreateInsertStoredProcedure]
    @MeterId BIGINT,
    @Granularity VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-30 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @SchemaName NVARCHAR(255) = 'Supply.Meter' + @MeterId
    DECLARE @StoredProcedureName NVARCHAR(255) = 'ForecastUsage' + @Granularity + 'Latest_Insert'
    DECLARE @TodaysDate NVARCHAR(10) = (SELECT FORMAT(GetDate(), 'yyyy-MM-dd'))
    DECLARE @RequiresDateParameter BIT = (SELECT IsTimePeriod FROM [Information].[Granularity] WHERE GranularityDescription = @Granularity)

    DECLARE @SQL NVARCHAR(255) = N'
    USE [EMaaS]
    GO

    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[' + @SchemaName +'].[' + @StoredProcedureName + ']''))
    BEGIN
        EXEC(''CREATE PROCEDURE [' + @SchemaName +'].[' + @StoredProcedureName + '] AS BEGIN SET NOCOUNT ON; END'')
    END
    GO
    
    -- =============================================
    -- Author:		System Generated
    -- Create date: ' + @TodaysDate + '
    -- Description:	Insert usage into [' + @SchemaName +'].[ForecastUsage' + @Granularity + 'Latest] table
    -- =============================================

    ALTER PROCEDURE [' + @SchemaName +'].[' + @StoredProcedureName + ']
        @' + @Granularity + 'Id BIGINT'
        
    IF @RequiresDateParameter = 1
        BEGIN
            SET @SQL = @SQL + ',
            @DateId BIGINT'
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

        IF NOT EXISTS(SELECT TOP 1 1 FROM [' + @SchemaName +'].[' + @StoredProcedureName + '] WHERE ' + @Granularity + 'Id = @' + @Granularity + 'Id'
        
    IF @RequiresDateParameter = 1
        BEGIN
            SET @SQL = @SQL + '
            AND DateId = @DateId'
        END

    SET @SQL = @SQL + ')
        BEGIN
            INSERT INTO [' + @SchemaName +'].[' + @StoredProcedureName + ']
            (
                ' + @Granularity + 'Id,'
        
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
                @' + @Granularity + 'Id,'
        
    IF @RequiresDateParameter = 1
        BEGIN
            SET @SQL = @SQL + '
            @DateId,'
        END

    SET @SQL = @SQL + '
                @Usage
            )
        END'

    EXEC sp_sqlexec @SQL
END
GO
