USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[FolderToFolderExtension_GetByFolderId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[FolderToFolderExtension_GetByFolderId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-09
-- Description:	Get FolderToFolderExtension info from [Mapping].[FolderToFolderExtension] table
-- =============================================

ALTER PROCEDURE [Mapping].[FolderToFolderExtension_GetByFolderId]
    @FolderId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-09 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        FolderToFolderExtensionId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        FolderId,
        FolderExtensionId
    FROM 
        [Mapping].[FolderToFolderExtension]
    WHERE 
        FolderId = @FolderId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
