USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[LocalDistributionZone_GetByLocalDistributionZoneGUID]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[LocalDistributionZone_GetByLocalDistributionZoneGUID] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-17
-- Description:	Get LocalDistributionZone info from [Information].[LocalDistributionZone] table by GUID
-- =============================================

ALTER PROCEDURE [Information].[LocalDistributionZone_GetByLocalDistributionZoneGUID]
    @LocalDistributionZoneGUID UNIQUEIDENTIFIER,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-17 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        LocalDistributionZoneId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        LocalDistributionZoneGUID
    FROM 
        [Information].[LocalDistributionZone] 
    WHERE 
        LocalDistributionZoneGUID = @LocalDistributionZoneGUID
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
