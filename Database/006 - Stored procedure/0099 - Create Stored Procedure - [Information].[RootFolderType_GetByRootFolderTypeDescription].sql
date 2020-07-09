USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[RootFolderType_GetByRootFolderTypeDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[RootFolderType_GetByRootFolderTypeDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-09
-- Description:	Get RootFolderType info from [Information].[RootFolderType] table by Description
-- =============================================

ALTER PROCEDURE [Information].[RootFolderType_GetByRootFolderTypeDescription]
    @RootFolderTypeDescription UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-09 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        RootFolderTypeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        RootFolderTypeDescription
    FROM 
        [Information].[RootFolderType] 
    WHERE 
        RootFolderTypeDescription = @RootFolderTypeDescription
END
GO
