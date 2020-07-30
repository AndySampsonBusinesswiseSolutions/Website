USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supply.SubMeter].[Schema_Create]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supply.SubMeter].[Schema_Create] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-28
-- Description:	Create new supply schema for SubMeterId
-- =============================================

ALTER PROCEDURE [Supply.SubMeter].[Schema_Create]
    @SubMeterId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-28 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @SQL NVARCHAR(255) = N'CREATE SCHEMA [Supply.SubMeter' + @SubMeterId + ']'
    EXEC sp_sqlexec @SQL
END
GO
