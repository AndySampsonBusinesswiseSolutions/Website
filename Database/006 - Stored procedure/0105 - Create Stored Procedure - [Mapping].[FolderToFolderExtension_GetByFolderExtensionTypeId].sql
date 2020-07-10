USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[FolderToFolderExtensionType_GetByFolderExtensionTypeId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[FolderToFolderExtensionType_GetByFolderExtensionTypeId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-10
-- Description:	Get FolderToFolderExtensionType info from [Mapping].[FolderToFolderExtensionType] table by Folder Extension Type Id
-- =============================================

ALTER PROCEDURE [Mapping].[FolderToFolderExtensionType_GetByFolderExtensionTypeId]
    @FolderExtensionTypeId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-10 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        FolderToFolderExtensionTypeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        FolderId,
        FolderExtensionTypeId
    FROM 
        [Mapping].[FolderToFolderExtensionType]
    WHERE 
        FolderExtensionTypeId = @FolderExtensionTypeId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
