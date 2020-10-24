USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Administration.User].[UserAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Administration.User].[UserAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new user attribute into [Administration.User].[UserAttribute] table
-- =============================================

ALTER PROCEDURE [Administration.User].[UserAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @UserAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- 2020-06-17 -> Andrew Sampson -> Updated as part of code refactor
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Administration.User].[UserAttribute]
    (
        CreatedByUserId,
        SourceId,
        UserAttributeDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @UserAttributeDescription
    )
END
GO