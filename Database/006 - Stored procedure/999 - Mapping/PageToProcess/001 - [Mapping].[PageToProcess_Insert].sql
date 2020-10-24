USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[PageToProcess_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[PageToProcess_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new mapping of a page to a process into [Mapping].[PageToProcess] table
-- =============================================

ALTER PROCEDURE [Mapping].[PageToProcess_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @PageId BIGINT,
    @ProcessId BIGINT
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

    INSERT INTO [Mapping].PageToProcess
    (
        CreatedByUserId,
        SourceId,
        PageId,
        ProcessId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @PageId,
        @ProcessId
    )
END
GO