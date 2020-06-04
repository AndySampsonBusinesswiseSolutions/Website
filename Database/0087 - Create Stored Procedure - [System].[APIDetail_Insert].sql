USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[APIDetail_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [System].[APIDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new API detail into [System].[APIDetail] table
-- =============================================

ALTER PROCEDURE [System].[APIDetail_Insert]
	@UserGUID UNIQUEIDENTIFIER,
    @SourceTypeDescription VARCHAR(255),
    @APIGUID UNIQUEIDENTIFIER,
    @APIAttributeDescription VARCHAR(255),
    @APIDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @APIId BIGINT = (SELECT APIId FROM [System].[API] WHERE GUID = @APIGUID)
    DECLARE @APIAttributeId BIGINT = (SELECT APIAttributeId FROM [System].[APIAttribute] WHERE APIAttributeDescription = @APIAttributeDescription)

    IF NOT EXISTS(SELECT TOP 1 1 FROM [System].[APIDetail] WHERE APIId = @APIId AND APIAttributeId = @APIAttributeId AND APIDetailDescription = @APIDetailDescription)
        BEGIN
            DECLARE @AllowsMultipleActiveInstances BIT = (SELECT AllowsMultipleActiveInstances FROM [System].[APIAttribute] WHERE APIAttributeId = @APIAttributeId)

            IF @AllowsMultipleActiveInstances = 0
                BEGIN
                    DECLARE @APIDetailId BIGINT = (SELECT APIDetailId FROM [System].[APIDetail] WHERE APIId = @APIId AND APIAttributeId = @APIAttributeId)

                    WHILE APIDetailId > 0
                        BEGIN
                            EXEC [System].[APIDetail_DeleteByAPIDetailId] APIDetailId
                            SET @APIDetailId = (SELECT APIDetailId FROM [System].[APIDetail] WHERE APIId = @APIId AND APIAttributeId = @APIAttributeId)
                        END
                END
                
            DECLARE @UserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE GUID = @UserGUID)
            DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = @SourceTypeDescription)
            DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId)

            INSERT INTO [System].[APIDetail]
            (
                CreatedByUserId,
                SourceId,
                APIId,
                APIAttributeId,
                APIDetailDescription
            )
            VALUES
            (
                @UserId,
                @SourceId,
                @APIId,
                @APIAttributeId,
                @APIDetailDescription
            )
        END
END
GO
