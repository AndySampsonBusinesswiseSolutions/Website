USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[SourceDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[SourceDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new source detail into [Information].[SourceDetail] table
-- =============================================

ALTER PROCEDURE [Information].[SourceDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @SourceAttributeId BIGINT,
    @SourceDetailDescription VARCHAR(255)
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

    INSERT INTO [Information].[SourceDetail]
    (
        CreatedByUserId,
        SourceId,
        SourceAttributeId,
        SourceDetailDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @SourceAttributeId,
        @SourceDetailDescription
    )
END
GO