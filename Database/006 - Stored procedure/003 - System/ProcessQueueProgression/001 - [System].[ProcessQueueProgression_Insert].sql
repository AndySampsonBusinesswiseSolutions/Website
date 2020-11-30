USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessQueueProgression_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[ProcessQueueProgression_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-08
-- Description:	Insert new ProcessQueueProgression into [System].[ProcessQueueProgression] table
-- =============================================

ALTER PROCEDURE [System].[ProcessQueueProgression_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @FromProcessQueueGUID UNIQUEIDENTIFIER,
    @ToProcessQueueGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-08 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [System].[ProcessQueueProgression]
    (
        CreatedByUserId,
        SourceId,
        FromProcessQueueGUID,
        ToProcessQueueGUID
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @FromProcessQueueGUID,
        @ToProcessQueueGUID
    )
END
GO
