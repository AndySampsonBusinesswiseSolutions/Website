USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supply.Meter].[LoadedUsage_CreateDeleteStoredProcedure]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supply.Meter].[LoadedUsage_CreateDeleteStoredProcedure] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-30
-- Description:	Create new LoadedUsage Delete Stored Procedure for MeterId
-- =============================================

ALTER PROCEDURE [Supply.Meter].[LoadedUsage_CreateDeleteStoredProcedure]
    @MeterId BIGINT
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
    DECLARE @TodaysDate NVARCHAR(10) = (SELECT FORMAT(GetDate(), 'yyyy-MM-dd'))

    DECLARE @SQL NVARCHAR(255) = N'
    USE [EMaaS]
    GO

    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[' + @SchemaName +'].[LoadedUsage_Delete]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [' + @SchemaName +'].[LoadedUsage_Delete] AS BEGIN SET NOCOUNT ON; END'')
    END
    GO
    
    -- =============================================
    -- Author:		System Generated
    -- Create date: ' + @TodaysDate + '
    -- Description:	Delete usage from [' + @SchemaName +'].[LoadedUsage] table
    -- =============================================

    ALTER PROCEDURE [' + @SchemaName +'].[LoadedUsage_Delete]
        @DateId BIGINT,
        @TimePeriodId BIGINT
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
            [' + @SchemaName +'].[LoadedUsage]
        SET
            EffectiveToDateTime = GETUTCDATE()
        WHERE
            DateId = @DateId
            AND TimePeriodId = @TimePeriodId
    END'

    EXEC sp_sqlexec @SQL
END
GO
