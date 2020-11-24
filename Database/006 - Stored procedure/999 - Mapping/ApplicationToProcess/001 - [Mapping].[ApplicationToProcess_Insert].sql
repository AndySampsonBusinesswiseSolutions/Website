USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ApplicationToProcess_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ApplicationToProcess_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-24
-- Description:	Insert new mapping of an Application to a process into [Mapping].[ApplicationToProcess] table
-- =============================================

ALTER PROCEDURE [Mapping].[ApplicationToProcess_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ApplicationId BIGINT,
    @ProcessId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-24 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].ApplicationToProcess
    (
        CreatedByUserId,
        SourceId,
        ApplicationId,
        ProcessId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ApplicationId,
        @ProcessId
    )
END
GO