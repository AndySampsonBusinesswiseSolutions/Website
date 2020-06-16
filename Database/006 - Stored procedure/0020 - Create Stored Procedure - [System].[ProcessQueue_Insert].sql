USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessQueue_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [System].[ProcessQueue_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new queue entry into [System].[ProcessQueue] table
-- =============================================

ALTER PROCEDURE [System].[ProcessQueue_Insert]
	@ProcessQueueGUID UNIQUEIDENTIFIER,
    @UserGUID UNIQUEIDENTIFIER,
    @SourceTypeDescription VARCHAR(255),
    @APIGUID UNIQUEIDENTIFIER,
    @HasError BIT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- 2020-06-16 -> Andrew Sampson -> Updated @GUID to @ProcessQueueGUID to start matching code variable names
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @UserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE GUID = @UserGUID)
    DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = @SourceTypeDescription)
    DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId)
    DECLARE @APIId BIGINT = (SELECT APIId FROM [System].[API] WHERE GUID = @APIGUID)

    INSERT INTO
        [System].[ProcessQueue]
        (
            GUID,
            CreatedByUserId,
            SourceId,
            APIId,
            HasError
        )
    VALUES  
        (
            @ProcessQueueGUID,
            @UserId,
            @SourceId,
            @APIId,
            @HasError
        )
END
GO
