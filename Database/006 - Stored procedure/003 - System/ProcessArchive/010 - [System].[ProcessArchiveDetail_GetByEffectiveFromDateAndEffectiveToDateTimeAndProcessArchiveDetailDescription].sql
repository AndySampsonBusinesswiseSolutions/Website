USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[ProcessArchiveDetail_GetByEffectiveFromDateAndEffectiveToDateTimeAndProcessArchiveDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[ProcessArchiveDetail_GetByEffectiveFromDateAndEffectiveToDateTimeAndProcessArchiveDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-06-23
-- Description:	Get ProcessArchiveDetail info from [System].[ProcessArchiveDetail] table by Effective From Date Time, Effective To Date Time and Process Archive Detail Description
-- =============================================

ALTER PROCEDURE [System].[ProcessArchiveDetail_GetByEffectiveFromDateAndEffectiveToDateTimeAndProcessArchiveDetailDescription]
    @EffectiveFromDateTime VARCHAR(255),
    @EffectiveToDateTime VARCHAR(255),
    @ProcessArchiveDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-06-23 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        ProcessArchiveDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ProcessArchiveId,
        ProcessArchiveAttributeId,
        ProcessArchiveDetailDescription
    FROM 
        [System].[ProcessArchiveDetail] 
    WHERE 
        EffectiveFromDateTime = @EffectiveFromDateTime
        AND EffectiveToDateTime = @EffectiveToDateTime
        AND ProcessArchiveDetailDescription = @ProcessArchiveDetailDescription
END
GO
