USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessArchiveDetail_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [System].[ProcessArchiveDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new ProcessArchive detail into [System].[ProcessArchiveDetail] table
-- =============================================

ALTER PROCEDURE [System].[ProcessArchiveDetail_Insert]
	@UserGUID UNIQUEIDENTIFIER,
    @SourceTypeDescription VARCHAR(255),
    @ProcessArchiveGUID UNIQUEIDENTIFIER,
    @ProcessArchiveAttributeDescription VARCHAR(255),
    @ProcessArchiveDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @ProcessArchiveId BIGINT = (SELECT ProcessArchiveId FROM [System].[ProcessArchive] WHERE GUID = @ProcessArchiveGUID)
    DECLARE @ProcessArchiveAttributeId BIGINT = (SELECT ProcessArchiveAttributeId FROM [System].[ProcessArchiveAttribute] WHERE ProcessArchiveAttributeDescription = @ProcessArchiveAttributeDescription)

    IF NOT EXISTS(SELECT TOP 1 1 FROM [System].[ProcessArchiveDetail] WHERE ProcessArchiveId = @ProcessArchiveId AND ProcessArchiveAttributeId = @ProcessArchiveAttributeId AND ProcessArchiveDetailDescription = @ProcessArchiveDetailDescription)
        BEGIN
            DECLARE @UserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE GUID = @UserGUID)
            DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = @SourceTypeDescription)
            DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId)

            INSERT INTO [System].[ProcessArchiveDetail]
            (
                CreatedByUserId,
                SourceId,
                ProcessArchiveId,
                ProcessArchiveAttributeId,
                ProcessArchiveDetailDescription
            )
            VALUES
            (
                @UserId,
                @SourceId,
                @ProcessArchiveId,
                @ProcessArchiveAttributeId,
                @ProcessArchiveDetailDescription
            )
        END
END
GO
