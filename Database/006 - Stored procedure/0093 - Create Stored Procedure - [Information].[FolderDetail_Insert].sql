USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[FolderDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[FolderDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-09
-- Description:	Insert new folder detail into [Information].[FolderDetail] table
-- =============================================

ALTER PROCEDURE [Information].[FolderDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @FolderId BIGINT,
    @FolderAttributeId BIGINT,
    @FolderDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-09 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[FolderDetail] WHERE FolderId = @FolderId 
        AND FolderAttributeId = @FolderAttributeId 
        AND FolderDetailDescription = @FolderDetailDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[FolderDetail]
            (
                CreatedByUserId,
                SourceId,
                FolderId,
                FolderAttributeId,
                FolderDetailDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @FolderId,
                @FolderAttributeId,
                @FolderDetailDescription
            )
        END
END
GO
