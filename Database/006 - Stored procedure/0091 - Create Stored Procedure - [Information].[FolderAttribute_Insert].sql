USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[FolderAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[FolderAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-09
-- Description:	Insert new folder attribute into [Information].[FolderAttribute] table
-- =============================================

ALTER PROCEDURE [Information].[FolderAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @FolderAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-09 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[FolderAttribute] WHERE FolderAttributeDescription = @FolderAttributeDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[FolderAttribute]
            (
                CreatedByUserId,
                SourceId,
                FolderAttributeDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @FolderAttributeDescription
            )
        END
END
GO
