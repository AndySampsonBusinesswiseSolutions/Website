USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[FolderToFolderExtensionType_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[FolderToFolderExtensionType_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-09
-- Description:	Insert new mapping of a Folder to a FolderExtensionType into [Mapping].[FolderToFolderExtensionType] table
-- =============================================

ALTER PROCEDURE [Mapping].[FolderToFolderExtensionType_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @FolderId BIGINT,
    @FolderExtensionTypeId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-09 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].FolderToFolderExtensionType
    (
        CreatedByUserId,
        SourceId,
        FolderId,
        FolderExtensionTypeId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @FolderId,
        @FolderExtensionTypeId
    )
END
GO
