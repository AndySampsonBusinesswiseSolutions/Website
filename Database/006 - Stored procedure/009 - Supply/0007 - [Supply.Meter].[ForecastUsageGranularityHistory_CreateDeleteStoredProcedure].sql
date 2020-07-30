USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supply.Meter].[ForecastUsageGranularityHistory_CreateDeleteStoredProcedure]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supply.Meter].[ForecastUsageGranularityHistory_CreateDeleteStoredProcedure] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-30
-- Description:	Create new ForecastUsageGranularityHistory Delete Stored Procedure for MeterId
-- =============================================

ALTER PROCEDURE [Supply.Meter].[ForecastUsageGranularityHistory_CreateDeleteStoredProcedure]
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
    DECLARE @StoredProcedureName NVARCHAR(255) = 'ForecastUsage' + @Granularity + 'History_Delete'
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
    -- Description:	Delete usage from [' + @SchemaName +'].[ForecastUsage' + @Granularity + 'History] table
    -- =============================================

    ALTER PROCEDURE [' + @SchemaName +'].[' + @StoredProcedureName + ']
        @' + @Granularity + 'Id BIGINT'
        
    IF @RequiresDateParameter = 1
        BEGIN
            SET @SQL = @SQL + ',
            @DateId BIGINT'
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

        UPDATE
            [' + @SchemaName +'].[ForecastUsage' + @Granularity + 'History]
        SET
            EffectiveToDateTime = GETUTCDATE()
        WHERE
            ' + @Granularity + 'Id = @' + @Granularity + 'Id'
        
    IF @RequiresDateParameter = 1
        BEGIN
            SET @SQL = @SQL + '
            AND DateId = @DateId'
        END

    SET @SQL = @SQL + 'END'

    EXEC sp_sqlexec @SQL
END
GO
