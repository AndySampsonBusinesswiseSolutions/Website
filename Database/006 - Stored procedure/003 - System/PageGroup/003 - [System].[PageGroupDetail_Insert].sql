USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[PageGroupDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[PageGroupDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-10-24
-- Description:	Insert new PageGroup detail into [System].[PageGroupDetail] table
-- =============================================

ALTER PROCEDURE [System].[PageGroupDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @PageGroupId BIGINT,
    @PageGroupAttributeId BIGINT,
    @PageGroupDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-10-24 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [System].[PageGroupDetail]
    (
        CreatedByUserId,
        SourceId,
        PageGroupId,
        PageGroupAttributeId,
        PageGroupDetailDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @PageGroupId,
        @PageGroupAttributeId,
        @PageGroupDetailDescription
    )
END
GO
