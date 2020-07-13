USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ProcessToProcessArchive_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ProcessToProcessArchive_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new mapping of a Process to a ProcessArchive into [Mapping].[ProcessToProcessArchive] table
-- =============================================

ALTER PROCEDURE [Mapping].[ProcessToProcessArchive_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ProcessId BIGINT,
    @ProcessArchiveId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-22 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].ProcessToProcessArchive
    (
        CreatedByUserId,
        SourceId,
        ProcessId,
        ProcessArchiveId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ProcessId,
        @ProcessArchiveId
    )
END
GO
