USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[FolderExtensionType_GetByFolderExtensionTypeDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[FolderExtensionType_GetByFolderExtensionTypeDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-10
-- Description:	Get FolderExtensionType info from [Information].[FolderExtensionType] table by Description
-- =============================================

ALTER PROCEDURE [Information].[FolderExtensionType_GetByFolderExtensionTypeDescription]
    @FolderExtensionTypeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-10 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        FolderExtensionTypeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        FolderExtensionTypeDescription
    FROM 
        [Information].[FolderExtensionType] 
    WHERE 
        FolderExtensionTypeDescription = @FolderExtensionTypeDescription
END
GO
