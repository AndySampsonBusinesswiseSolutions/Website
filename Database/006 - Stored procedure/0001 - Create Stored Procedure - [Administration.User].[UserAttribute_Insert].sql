USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Administration.User].[UserAttribute_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [Administration.User].[UserAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new user attribute into [Administration.User].[UserAttribute] table
-- =============================================

ALTER PROCEDURE [Administration.User].[UserAttribute_Insert]
    @UserGUID UNIQUEIDENTIFIER,
    @SourceTypeDescription VARCHAR(255),
    @UserAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Administration.User].[UserAttribute] WHERE UserAttributeDescription = @UserAttributeDescription)
        BEGIN
            DECLARE @UserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE GUID = @UserGUID)
            DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = @SourceTypeDescription)
            DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId)
            
            INSERT INTO [Administration.User].[UserAttribute]
            (
                CreatedByUserId,
                SourceId,
                UserAttributeDescription
            )
            VALUES
            (
                @UserId,
                @SourceId,
                @UserAttributeDescription
            )
        END
END
GO
