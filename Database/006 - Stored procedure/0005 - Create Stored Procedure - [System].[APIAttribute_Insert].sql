USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[APIAttribute_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [System].[APIAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new API attribute into [System].[APIAttribute] table
-- =============================================

ALTER PROCEDURE [System].[APIAttribute_Insert]
    @UserGUID UNIQUEIDENTIFIER,
    @SourceTypeDescription VARCHAR(255),
    @APIAttributeDescription VARCHAR(255),
    @AllowsMultipleActiveInstances BIT = 0
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [System].[APIAttribute] WHERE APIAttributeDescription = @APIAttributeDescription)
        BEGIN
            DECLARE @UserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE GUID = @UserGUID)
            DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = @SourceTypeDescription)
            DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId)
            
            INSERT INTO [System].[APIAttribute]
            (
                CreatedByUserId,
                SourceId,
                APIAttributeDescription,
                AllowsMultipleActiveInstances
            )
            VALUES
            (
                @UserId,
                @SourceId,
                @APIAttributeDescription,
                @AllowsMultipleActiveInstances
            )
        END
END
GO
