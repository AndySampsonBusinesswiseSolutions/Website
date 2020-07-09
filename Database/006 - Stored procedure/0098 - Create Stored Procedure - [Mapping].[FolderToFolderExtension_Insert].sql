USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[FolderToFolderExtension_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[FolderToFolderExtension_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-09
-- Description:	Insert new mapping of a Folder to a FolderExtension into [Mapping].[FolderToFolderExtension] table
-- =============================================

ALTER PROCEDURE [Mapping].[FolderToFolderExtension_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @FolderId BIGINT,
    @FolderExtensionId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-09 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].FolderToFolderExtension
    (
        CreatedByUserId,
        SourceId,
        FolderId,
        FolderExtensionId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @FolderId,
        @FolderExtensionId
    )
END
GO
