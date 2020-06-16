USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[PageToProcess_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [Mapping].[PageToProcess_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new mapping of a page to a process into [Mapping].[PageToProcess] table
-- =============================================

ALTER PROCEDURE [Mapping].[PageToProcess_Insert]
    @UserGUID UNIQUEIDENTIFIER,
    @SourceTypeDescription VARCHAR(255),
    @PageGUID UNIQUEIDENTIFIER,
    @ProcessGUID UNIQUEIDENTIFIER
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
    DECLARE @ProcessId BIGINT = (SELECT ProcessId FROM [System].[Process] WHERE GUID = @ProcessGUID)

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Mapping].[PageToProcess] WHERE PageId = @PageId AND ProcessId = @ProcessId)
        BEGIN
            DECLARE @UserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE GUID = @UserGUID)
            DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = @SourceTypeDescription)
            DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId)
            
            INSERT INTO [Mapping].PageToProcess
            (
                CreatedByUserId,
                SourceId,
                PageId,
                ProcessId
            )
            VALUES
            (
                @UserId,
                @SourceId,
                @PageId,
                @ProcessId
            )
        END
END
GO
