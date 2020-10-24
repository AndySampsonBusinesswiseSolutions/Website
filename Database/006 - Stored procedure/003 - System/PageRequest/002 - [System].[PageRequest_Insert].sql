USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[PageRequest_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[PageRequest_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-10-07
-- Description:	Insert new page request into [System].[PageRequest] table
-- =============================================

ALTER PROCEDURE [System].[PageRequest_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @PageId BIGINT,
    @ProcessQueueGUID UNIQUEIDENTIFIER,
	@PageRequestResult NVARCHAR(MAX)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-10-07 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [System].[PageRequest]
    (
        CreatedByUserId,
        SourceId,
        PageId,
        ProcessQueueGUID,
        PageRequestResult
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @PageId,
        @ProcessQueueGUID,
        @PageRequestResult
    )
END
GO
