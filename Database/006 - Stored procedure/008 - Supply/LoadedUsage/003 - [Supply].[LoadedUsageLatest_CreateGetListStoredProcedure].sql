USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supply].[LoadedUsageLatest_CreateGetListStoredProcedure]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supply].[LoadedUsageLatest_CreateGetListStoredProcedure] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-31
-- Description:	Create new LoadedUsageLatest GetList Stored Procedure for MeterId
-- =============================================

ALTER PROCEDURE [Supply].[LoadedUsageLatest_CreateGetListStoredProcedure]
    @MeterId BIGINT,
    @MeterType VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-31 -> Andrew Sampson -> Initial development of script
    -- 2020-11-11 -> Andrew Sampson -> Updated to use new LoadedUsageLatest table
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @SchemaName NVARCHAR(255) = 'Supply.' + @MeterType + CONVERT(NVARCHAR, @MeterId)
    DECLARE @TodaysDate NVARCHAR(10) = (SELECT FORMAT(GetDate(), 'yyyy-MM-dd'))

    DECLARE @SQL NVARCHAR(MAX) = N'
    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[' + @SchemaName +'].[LoadedUsageLatest_GetList]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [' + @SchemaName +'].[LoadedUsageLatest_GetList] AS BEGIN SET NOCOUNT ON; END'')
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
    -- Description:	Get usage from [' + @SchemaName +'].[LoadedUsageLatest] table
    -- =============================================

    ALTER PROCEDURE [' + @SchemaName +'].[LoadedUsageLatest_GetList]
    AS
    BEGIN
        -- =============================================
        --              CHANGE HISTORY
        -- ' + @TodaysDate + ' -> System Generated -> Initial development of script
        -- =============================================

        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        SELECT
            DateId,
            TimePeriodId,
            Usage
        FROM
            [' + @SchemaName +'].[LoadedUsageLatest]
    END'

    SET @MetaSQL = '
	USE [EMaaS]
	EXEC (''' + REPLACE(@SQL, '''', '''''') + ''')
	'

    EXEC sp_sqlexec @MetaSQL
END
GO
