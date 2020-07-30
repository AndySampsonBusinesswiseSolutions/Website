USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supply.Meter].[Schema_Create]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supply.Meter].[Schema_Create] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-28
-- Description:	Create new supply schema for MeterId
-- =============================================

ALTER PROCEDURE [Supply.Meter].[Schema_Create]
    @MeterId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-28 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @SQL NVARCHAR(255) = N'CREATE SCHEMA [Supply.Meter' + @MeterId + ']'
    EXEC sp_sqlexec @SQL
END
GO
