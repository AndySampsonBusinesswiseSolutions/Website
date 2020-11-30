USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ApplicationDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[ApplicationDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-24
-- Description:	Insert new Application detail into [System].[ApplicationDetail] table
-- =============================================

ALTER PROCEDURE [System].[ApplicationDetail_Insert]
	@CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ApplicationId BIGINT,
    @ApplicationAttributeId BIGINT,
    @ApplicationDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-24 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [System].[ApplicationDetail]
    (
        CreatedByUserId,
        SourceId,
        ApplicationId,
        ApplicationAttributeId,
        ApplicationDetailDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ApplicationId,
        @ApplicationAttributeId,
        @ApplicationDetailDescription
    )
END
GO