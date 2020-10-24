USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[Granularity_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[Granularity_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-17
-- Description:	Insert new granularity into [Information].[Granularity] table
-- =============================================

ALTER PROCEDURE [Information].[Granularity_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @GranularityGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-17 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Information].[Granularity]
    (
        CreatedByUserId,
        SourceId,
        GranularityGUID
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @GranularityGUID
    )
END
GO