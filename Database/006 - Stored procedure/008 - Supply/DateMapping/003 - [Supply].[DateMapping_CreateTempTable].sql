USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supply].[DateMapping_CreateTempTable]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supply].[DateMapping_CreateTempTable] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-27
-- Description:	Create new DateMapping_Temp table for a MeterId
-- =============================================

ALTER PROCEDURE [Supply].[DateMapping_CreateTempTable]
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
    DECLARE @v sql_variant 

    --Create base table
    DECLARE @SQL NVARCHAR(MAX) = N'
    USE [EMaaS]

    CREATE TABLE [' + @SchemaName + '].[DateMapping_Temp]
	(
        ProcessQueueGUID VARCHAR(255) NOT NULL,
        CreatedByUserId BIGINT NOT NULL,
        SourceId BIGINT NOT NULL,
        DateId BIGINT,
        MappedDateId BIGINT
	)  ON [Supply]'
    EXEC sp_sqlexec @SQL
END
GO
