USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[ProcessDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-02
-- Description:	Insert new process detail into [System].[ProcessDetail] table
-- =============================================

ALTER PROCEDURE [System].[ProcessDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ProcessId BIGINT,
    @ProcessAttributeId BIGINT,
    @ProcessDetailDescription VARCHAR(255)
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

    INSERT INTO [System].[ProcessDetail]
    (
        CreatedByUserId,
        SourceId,
        ProcessId,
        ProcessAttributeId,
        ProcessDetailDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ProcessId,
        @ProcessAttributeId,
        @ProcessDetailDescription
    )
END
GO