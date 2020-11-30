USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ApplicationRunAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[ApplicationRunAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-24
-- Description:	Insert new ApplicationRun attribute into [System].[ApplicationRunAttribute] table
-- =============================================

ALTER PROCEDURE [System].[ApplicationRunAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ApplicationRunAttributeDescription VARCHAR(255),
    @AllowsMultipleActiveInstances BIT = 0
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-24 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [System].[ApplicationRunAttribute]
    (
        CreatedByUserId,
        SourceId,
        ApplicationRunAttributeDescription,
        AllowsMultipleActiveInstances
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ApplicationRunAttributeDescription,
        @AllowsMultipleActiveInstances
    )
END
GO