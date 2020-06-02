USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[PageDetail_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [System].[PageDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new page detail into [System].[PageDetail] table
-- =============================================

ALTER PROCEDURE [System].[PageDetail_Insert]
    @UserGUID UNIQUEIDENTIFIER,
    @SourceTypeDescription VARCHAR(255),
    @PageGUID UNIQUEIDENTIFIER,
    @PageAttributeDescription VARCHAR(255),
    @PageDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @PageId BIGINT = (SELECT PageId FROM [System].[Page] WHERE GUID = @PageGUID)
    DECLARE @PageAttributeId BIGINT = (SELECT PageAttributeId FROM [System].[PageAttribute] WHERE PageAttributeDescription = @PageAttributeDescription)

    IF NOT EXISTS(SELECT TOP 1 1 FROM [System].[PageDetail] WHERE PageId = @PageId AND PageAttributeId = @PageAttributeId AND PageDetailDescription = @PageDetailDescription)
        BEGIN
            UPDATE
                [System].[PageDetail]
            SET
                EffectiveToDateTime = GETUTCDATE()
            WHERE
                PageId = @PageId
                AND PageAttributeId = @PageAttributeId
                AND PageDetailDescription <> @PageDetailDescription
                AND EffectiveToDateTime = '9999-12-31'
            
            DECLARE @UserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE GUID = @UserGUID)
            DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = @SourceTypeDescription)
            DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId)
            
            INSERT INTO [System].[PageDetail]
            (
                CreatedByUserId,
                SourceId,
                PageId,
                PageAttributeId,
                PageDetailDescription
            )
            VALUES
            (
                @UserId,
                @SourceId,
                @PageId,
                @PageAttributeId,
                @PageDetailDescription
            )
        END
END
GO
