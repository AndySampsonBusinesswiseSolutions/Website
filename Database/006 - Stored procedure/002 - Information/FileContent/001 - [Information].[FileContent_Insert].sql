USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[FileContent_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[FileContent_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-14
-- Description:	Insert new file content into [Information].[FileContent] table
-- =============================================

ALTER PROCEDURE [Information].[FileContent_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @FileId BIGINT,
    @FileContent NVARCHAR(MAX)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-14 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Information].[FileContent]
    (
        CreatedByUserId,
        SourceId,
        FileId,
        FileContent
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @FileId,
        @FileContent
    )
END
GO