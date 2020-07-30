USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supply.Meter].[LoadedUsage_CreateInsertStoredProcedure]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supply.Meter].[LoadedUsage_CreateInsertStoredProcedure] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-30
-- Description:	Create new LoadedUsage Insert Stored Procedure for MeterId
-- =============================================

ALTER PROCEDURE [Supply.Meter].[LoadedUsage_CreateInsertStoredProcedure]
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
    IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = ''P'' AND OBJECT_ID = OBJECT_ID(''[' + @SchemaName +'].[LoadedUsage_Insert]''))
    BEGIN
        EXEC(''CREATE PROCEDURE [' + @SchemaName +'].[LoadedUsage_Insert] AS BEGIN SET NOCOUNT ON; END'')
    END
    GO
    
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

        INSERT INTO [' + @SchemaName +'].[LoadedUsage_Insert]
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
        )'

    EXEC sp_sqlexec @SQL
END
GO
