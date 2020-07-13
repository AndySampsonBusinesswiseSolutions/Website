USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[FolderDetail_GetByFolderIdAndFolderAttributeId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[FolderDetail_GetByFolderIdAndFolderAttributeId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-09
-- Description:	Get FolderDetail info from [Information].[FolderDetail] table by Folder Id and Folder Attribute Id
-- =============================================

ALTER PROCEDURE [Information].[FolderDetail_GetByFolderIdAndFolderAttributeId]
    @FolderId BIGINT,
    @FolderAttributeId BIGINT,
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
        FolderDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        FolderId,
        FolderAttributeId,
        FolderDetailDescription
    FROM 
        [Information].[FolderDetail] 
    WHERE 
        FolderId = @FolderId
        AND FolderAttributeId = @FolderAttributeId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
