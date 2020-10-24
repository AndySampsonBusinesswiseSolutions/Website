USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[APIToProcess_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[APIToProcess_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-03
-- Description:	Insert new mapping of an API to a process into [Mapping].[APIToProcess] table
-- =============================================

ALTER PROCEDURE [Mapping].[APIToProcess_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @APIId BIGINT,
    @ProcessId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-03 -> Andrew Sampson -> Initial development of script
    -- 2020-06-17 -> Andrew Sampson -> Updated as part of code refactor
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].APIToProcess
    (
        CreatedByUserId,
        SourceId,
        APIId,
        ProcessId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @APIId,
        @ProcessId
    )
END
GO