USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessArchive_Update]'))
    BEGIN
        exec('CREATE PROCEDURE [System].[ProcessArchive_Update] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	End-date entry in [System].[ProcessArchive] table
-- =============================================

ALTER PROCEDURE [System].[ProcessArchive_Update]
	@ProcessArchiveGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE
        [System].[ProcessArchive]
    SET
        EffectiveToDateTime = GETUTCDATE()
    WHERE
        GUID = @ProcessArchiveGUID
END
GO
