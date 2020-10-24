USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[PageToPageGroup_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[PageToPageGroup_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-10-24
-- Description:	Insert new mapping of a page to a process into [Mapping].[PageToPageGroup] table
-- =============================================

ALTER PROCEDURE [Mapping].[PageToPageGroup_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @PageId BIGINT,
    @PageGroupId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-10-24 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].PageToPageGroup
    (
        CreatedByUserId,
        SourceId,
        PageId,
        PageGroupId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @PageId,
        @PageGroupId
    )
END
GO
