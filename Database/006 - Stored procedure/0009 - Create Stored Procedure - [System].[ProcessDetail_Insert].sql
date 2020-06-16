USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessDetail_Insert]'))
    BEGIN
        exec('CREATE PROCEDURE [System].[ProcessDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new process detail into [System].[ProcessDetail] table
-- =============================================

ALTER PROCEDURE [System].[ProcessDetail_Insert]
    @UserGUID UNIQUEIDENTIFIER,
    @SourceTypeDescription VARCHAR(255),
    @ProcessGUID UNIQUEIDENTIFIER,
    @ProcessAttributeDescription VARCHAR(255),
    @ProcessDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @ProcessId BIGINT = (SELECT ProcessId FROM [System].[Process] WHERE GUID = @ProcessGUID)
    DECLARE @ProcessAttributeId BIGINT = (SELECT ProcessAttributeId FROM [System].[ProcessAttribute] WHERE ProcessAttributeDescription = @ProcessAttributeDescription)

    IF NOT EXISTS(SELECT TOP 1 1 FROM [System].[ProcessDetail] WHERE ProcessId = @ProcessId AND ProcessAttributeId = @ProcessAttributeId AND ProcessDetailDescription = @ProcessDetailDescription)
        BEGIN
            UPDATE
                [System].[ProcessDetail]
            SET
                EffectiveToDateTime = GETUTCDATE()
            WHERE
                ProcessId = @ProcessId
                AND ProcessAttributeId = @ProcessAttributeId
                AND ProcessDetailDescription <> @ProcessDetailDescription
                AND EffectiveToDateTime = '9999-12-31'
            
            DECLARE @UserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE GUID = @UserGUID)
            DECLARE @SourceTypeId BIGINT = (SELECT SourceTypeId FROM [Information].[SourceType] WHERE SourceTypeDescription = @SourceTypeDescription)
            DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[Source] WHERE SourceTypeId = @SourceTypeId)
            
            INSERT INTO [System].[ProcessDetail]
            (
                CreatedByUserId,
                SourceId,
                ProcessId,
                ProcessAttributeId,
                ProcessDetailDescription
            )
            VALUES
            (
                @UserId,
                @SourceId,
                @ProcessId,
                @ProcessAttributeId,
                @ProcessDetailDescription
            )
        END
END
GO
