USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[PageGroup_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[PageGroup_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-10-24
-- Description:	Insert new PageGroupGroup into [System].[PageGroup] table
-- =============================================

ALTER PROCEDURE [System].[PageGroup_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @PageGroupGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-10-24 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [System].PageGroup
    (
        CreatedByUserId,
        SourceId,
        PageGroupGUID
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @PageGroupGUID
    )
END
GO
