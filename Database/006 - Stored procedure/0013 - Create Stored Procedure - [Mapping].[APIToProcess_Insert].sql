USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[APIToProcess_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [Mapping].[APIToProcess_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-03
-- Description:	Insert new mapping of an API to a process into [Mapping].[APIToProcess] table
-- =============================================

ALTER PROCEDURE [Mapping].[APIToProcess_Insert]
    @UserGUID UNIQUEIDENTIFIER,
    @SourceTypeDescription VARCHAR(255),
    @APIGUID UNIQUEIDENTIFIER,
    @ProcessGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-03 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @APIId BIGINT = (SELECT APIId FROM [System].[API] WHERE GUID = @APIGUID)
    DECLARE @ProcessId BIGINT = (SELECT ProcessId FROM [System].[Process] WHERE GUID = @ProcessGUID)

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Mapping].[APIToProcess] WHERE APIId = @APIId AND ProcessId = @ProcessId)
        BEGIN
            DECLARE @UserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE GUID = @UserGUID)
            DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = @SourceTypeDescription)
            DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId)
            
            INSERT INTO [Mapping].APIToProcess
            (
                CreatedByUserId,
                SourceId,
                APIId,
                ProcessId
            )
            VALUES
            (
                @UserId,
                @SourceId,
                @APIId,
                @ProcessId
            )
        END
END
GO
