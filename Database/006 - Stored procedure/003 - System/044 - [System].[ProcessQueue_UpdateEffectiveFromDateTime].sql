USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessQueue_UpdateEffectiveFromDateTime]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[ProcessQueue_UpdateEffectiveFromDateTime] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-28
-- Description:	End-date queue entry in [System].[ProcessQueue] table
-- =============================================

ALTER PROCEDURE [System].[ProcessQueue_UpdateEffectiveFromDateTime]
	@ProcessQueueGUID UNIQUEIDENTIFIER,
    @APIId BIGINT,
    @HasError BIT,
    @ErrorMessage VARCHAR(MAX) = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-28 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE
        [System].[ProcessQueue]
    SET
        EffectiveFromDateTime = GETUTCDATE()
    WHERE
        ProcessQueueGUID = @ProcessQueueGUID
        AND APIId = @APIId
END
GO
