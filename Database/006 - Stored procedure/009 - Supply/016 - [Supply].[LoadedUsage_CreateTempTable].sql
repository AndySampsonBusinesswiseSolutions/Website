USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supply].[LoadedUsage_CreateTempTable]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supply].[LoadedUsage_CreateTempTable] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-26
-- Description:	Create new LoadedUsage_Temp table for MeterId
-- =============================================

ALTER PROCEDURE [Supply].[LoadedUsage_CreateTempTable]
    @MeterId BIGINT,
    @MeterType VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-26 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @SchemaName NVARCHAR(255) = 'Supply.' + @MeterType + CONVERT(NVARCHAR, @MeterId)
    DECLARE @v sql_variant 

    --Create base table
    DECLARE @SQL NVARCHAR(MAX) = N'
    USE [EMaaS]

    CREATE TABLE [' + @SchemaName + '].[LoadedUsage_Temp]
	(
        ProcessQueueGUID VARCHAR(255) NOT NULL,
        CreatedByUserId BIGINT NOT NULL,
        SourceId BIGINT NOT NULL,
        DateId BIGINT NOT NULL,
        TimePeriodId BIGINT NOT NULL,
        UsageTypeId BIGINT NOT NULL,
        Usage DECIMAL(18,10) NOT NULL
	)  ON [Supply]'
    EXEC sp_sqlexec @SQL
END
GO
