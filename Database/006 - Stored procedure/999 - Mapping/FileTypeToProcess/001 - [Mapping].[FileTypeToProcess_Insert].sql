USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[FileTypeToProcess_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[FileTypeToProcess_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-08
-- Description:	Insert new mapping of an FileType to a process into [Mapping].[FileTypeToProcess] table
-- =============================================

ALTER PROCEDURE [Mapping].[FileTypeToProcess_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @FileTypeId BIGINT,
    @ProcessId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-08 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].FileTypeToProcess
    (
        CreatedByUserId,
        SourceId,
        FileTypeId,
        ProcessId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @FileTypeId,
        @ProcessId
    )
END
GO