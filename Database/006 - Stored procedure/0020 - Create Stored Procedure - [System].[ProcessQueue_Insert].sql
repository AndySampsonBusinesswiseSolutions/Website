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
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @APIId BIGINT,
    @HasError BIT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-02 -> Andrew Sampson -> Initial development of script
    -- 2020-06-16 -> Andrew Sampson -> Updated @GUID to @ProcessQueueGUID to start matching code variable names
    -- 2020-06-17 -> Andrew Sampson -> Updated as part of code refactor
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO
        [System].[ProcessQueue]
        (
            ProcessQueueGUID,
            CreatedByUserId,
            SourceId,
            APIId,
            HasError
        )
    VALUES  
        (
            @ProcessQueueGUID,
            @CreatedByUserId,
            @SourceId,
            @APIId,
            @HasError
        )
END
GO
