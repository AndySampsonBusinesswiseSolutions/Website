USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supply].[DateMapping_CreateGetLatestStoredProcedure]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supply].[DateMapping_CreateGetLatestStoredProcedure] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-26
-- Description:	Create new DateMapping GetLatest Stored Procedure for MeterId
-- =============================================

ALTER PROCEDURE [Supply].[DateMapping_CreateGetLatestStoredProcedure]
    @MeterId BIGINT,
    @MeterType VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-26 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @SchemaName NVARCHAR(255) = 'Supply.' + @MeterType + CONVERT(NVARCHAR, @MeterId)
    DECLARE @TodaysDate NVARCHAR(10) = (SELECT FORMAT(GetDate(), 'yyyy-MM-dd'))

    DECLARE @SQL NVARCHAR(MAX) = N'
    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[' + @SchemaName +'].[DateMapping_GetLatest]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [' + @SchemaName +'].[DateMapping_GetLatest] AS BEGIN SET NOCOUNT ON; END'')
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
    -- Description:	GetLatest usage from [' + @SchemaName +'].[DateMapping] table
    -- =============================================

    ALTER PROCEDURE [' + @SchemaName +'].[DateMapping_GetLatest]
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
            DateMappingId,
            EffectiveFromDateTime,
            EffectiveToDateTime,
            CreatedDateTime,
            CreatedByUserId,
            SourceId,
            DateId,
            MappedDateId
        FROM
            [' + @SchemaName +'].[DateMapping]
        WHERE
            EffectiveToDateTime = ''9999-12-31''
    END'

    SET @MetaSQL = '
	USE [EMaaS]
	EXEC (''' + REPLACE(@SQL, '''', '''''') + ''')
	'

    EXEC sp_sqlexec @MetaSQL
END
GO
