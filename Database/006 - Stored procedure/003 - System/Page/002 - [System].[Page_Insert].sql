USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[Page_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[Page_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new page into [System].[Page] table
-- =============================================

ALTER PROCEDURE [System].[Page_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @PageGUID UNIQUEIDENTIFIER
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

    INSERT INTO [System].[Page]
    (
        CreatedByUserId,
        SourceId,
        PageGUID
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @PageGUID
    )
END
GO