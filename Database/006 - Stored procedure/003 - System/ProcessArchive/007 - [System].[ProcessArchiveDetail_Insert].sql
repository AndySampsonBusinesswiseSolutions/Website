USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessArchiveDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[ProcessArchiveDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new ProcessArchive detail into [System].[ProcessArchiveDetail] table
-- =============================================

ALTER PROCEDURE [System].[ProcessArchiveDetail_Insert]
	@CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ProcessArchiveId BIGINT,
    @ProcessArchiveAttributeId BIGINT,
    @ProcessArchiveDetailDescription VARCHAR(255)
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
        @CreatedByUserId,
        @SourceId,
        @ProcessArchiveId,
        @ProcessArchiveAttributeId,
        @ProcessArchiveDetailDescription
    )
END
GO