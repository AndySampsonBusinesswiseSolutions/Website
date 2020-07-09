USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[RootFolderType_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[RootFolderType_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-09
-- Description:	Insert new root folder type into [Information].[RootFolderType] table
-- =============================================

ALTER PROCEDURE [Information].[RootFolderType_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @RootFolderTypeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-09 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Information].[RootFolderType] WHERE RootFolderTypeDescription = @RootFolderTypeDescription
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Information].[RootFolderType]
            (
                CreatedByUserId,
                SourceId,
                RootFolderTypeDescription
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @RootFolderTypeDescription
            )
        END
END
GO
