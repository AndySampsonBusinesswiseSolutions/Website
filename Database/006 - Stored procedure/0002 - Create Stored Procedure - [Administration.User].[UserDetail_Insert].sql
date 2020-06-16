USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Administration.User].[UserDetail_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [Administration.User].[UserDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new user detail into [Administration.User].[UserDetail] table
-- =============================================

ALTER PROCEDURE [Administration.User].[UserDetail_Insert]
    @UserGUID UNIQUEIDENTIFIER,
    @SourceTypeDescription VARCHAR(255),
    @UserAttributeDescription VARCHAR(255),
    @UserDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @UserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE GUID = @UserGUID)
    DECLARE @UserAttributeId BIGINT = (SELECT UserAttributeId FROM [Administration.User].[UserAttribute] WHERE UserAttributeDescription = @UserAttributeDescription)

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Administration.User].[UserDetail] WHERE UserId = @UserId AND UserAttributeId = @UserAttributeId AND UserDetailDescription = @UserDetailDescription)
        BEGIN
            UPDATE
                [Administration.User].[UserDetail]
            SET
                EffectiveToDateTime = GETUTCDATE()
            WHERE
                UserId = @UserId
                AND UserAttributeId = @UserAttributeId
                AND UserDetailDescription <> @UserDetailDescription
                AND EffectiveToDateTime = '9999-12-31'
            
            DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = @SourceTypeDescription)
            DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId)
            
            INSERT INTO [Administration.User].[UserDetail]
            (
                CreatedByUserId,
                SourceId,
                UserId,
                UserAttributeId,
                UserDetailDescription
            )
            VALUES
            (
                @UserId,
                @SourceId,
                @UserId,
                @UserAttributeId,
                @UserDetailDescription
            )
        END
END
GO
