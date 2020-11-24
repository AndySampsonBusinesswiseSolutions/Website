USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ApplicationRunDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[ApplicationRunDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-24
-- Description:	Insert new ApplicationRun detail into [System].[ApplicationRunDetail] table
-- =============================================

ALTER PROCEDURE [System].[ApplicationRunDetail_Insert]
	@CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ApplicationRunId BIGINT,
    @ApplicationRunAttributeId BIGINT,
    @ApplicationRunDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-24 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [System].[ApplicationRunDetail]
    (
        CreatedByUserId,
        SourceId,
        ApplicationRunId,
        ApplicationRunAttributeId,
        ApplicationRunDetailDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ApplicationRunId,
        @ApplicationRunAttributeId,
        @ApplicationRunDetailDescription
    )
END
GO