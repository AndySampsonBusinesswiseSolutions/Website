USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supply].[DateMapping_CreateDeleteStoredProcedure]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supply].[DateMapping_CreateDeleteStoredProcedure] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-27
-- Description:	Create new DateMapping Delete Stored Procedure for MeterId
-- =============================================

ALTER PROCEDURE [Supply].[DateMapping_CreateDeleteStoredProcedure]
    @MeterId BIGINT,
    @MeterType VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-27 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @SchemaName NVARCHAR(255) = 'Supply.' + @MeterType + CONVERT(NVARCHAR, @MeterId)
    DECLARE @TodaysDate NVARCHAR(10) = (SELECT FORMAT(GetDate(), 'yyyy-MM-dd'))

    DECLARE @SQL NVARCHAR(MAX) = N'
    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[' + @SchemaName +'].[DateMapping_Delete]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [' + @SchemaName +'].[DateMapping_Delete] AS BEGIN SET NOCOUNT ON; END'')
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
    -- Description:	Delete usage from [' + @SchemaName +'].[DateMapping] table
    -- =============================================

    ALTER PROCEDURE [' + @SchemaName +'].[DateMapping_Delete]
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
            [' + @SchemaName +'].[DateMapping]
        SET
            DateMapping.EffectiveToDateTime = GETUTCDATE()
        FROM
            [' + @SchemaName +'].[DateMapping]
        INNER JOIN
            [' + @SchemaName +'].[DateMapping_Temp]
            ON DateMapping_Temp.DateId = DateMapping.DateId
        WHERE
            DateMapping.EffectiveToDateTime = ''9999-12-31''
    END'

    SET @MetaSQL = '
	USE [EMaaS]
	EXEC (''' + REPLACE(@SQL, '''', '''''') + ''')
	'

    EXEC sp_sqlexec @MetaSQL
END
GO
