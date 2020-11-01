USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[PageGroupToPageHeader_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[PageGroupToPageHeader_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-01
-- Description:	Insert new mapping of a page to a process into [Mapping].[PageGroupToPageHeader] table
-- =============================================

ALTER PROCEDURE [Mapping].[PageGroupToPageHeader_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @PageGroupId BIGINT,
    @PageHeaderId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-01 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].PageGroupToPageHeader
    (
        CreatedByUserId,
        SourceId,
        PageGroupId,
        PageHeaderId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @PageGroupId,
        @PageHeaderId
    )
END
GO
