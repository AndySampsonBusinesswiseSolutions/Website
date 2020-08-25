USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supply].[LoadedUsage_CreateInsertStoredProcedure]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supply].[LoadedUsage_CreateInsertStoredProcedure] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-30
-- Description:	Create new LoadedUsage Insert Stored Procedure for MeterId
-- =============================================

ALTER PROCEDURE [Supply].[LoadedUsage_CreateInsertStoredProcedure]
    @MeterId BIGINT,
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
    DECLARE @TodaysDate NVARCHAR(10) = (SELECT FORMAT(GetDate(), 'yyyy-MM-dd'))

    DECLARE @SQL NVARCHAR(MAX) = N'
    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[' + @SchemaName +'].[LoadedUsage_Insert]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [' + @SchemaName +'].[LoadedUsage_Insert] AS BEGIN SET NOCOUNT ON; END'')
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
    -- Description:	Insert usage into [' + @SchemaName +'].[LoadedUsage] table
    -- =============================================

    ALTER PROCEDURE [' + @SchemaName +'].[LoadedUsage_Insert]
        @CreatedByUserId BIGINT,
        @SourceId BIGINT,
        @DateId BIGINT,
        @TimePeriodId BIGINT,
        @UsageTypeId BIGINT,
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

        INSERT INTO [' + @SchemaName +'].[LoadedUsage]
        (
            CreatedByUserId,
            SourceId,
            DateId,
            TimePeriodId,
            UsageTypeId,
            Usage
        )
        VALUES
        (
            @CreatedByUserId,
            @SourceId,
            @DateId,
            @TimePeriodId,
            @UsageTypeId,
            @Usage
        )
    END'

    SET @MetaSQL = '
	USE [EMaaS]
	EXEC (''' + REPLACE(@SQL, '''', '''''') + ''')
	'

    EXEC sp_sqlexec @MetaSQL
END
GO
