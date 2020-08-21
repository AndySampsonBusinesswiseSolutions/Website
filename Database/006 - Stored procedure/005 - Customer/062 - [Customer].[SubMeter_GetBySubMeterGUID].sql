USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[SubMeter_GetBySubMeterGUID]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[SubMeter_GetBySubMeterGUID] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-14
-- Description:	Get SubMeter info from [Customer].[SubMeter] table by GUID
-- =============================================

ALTER PROCEDURE [Customer].[SubMeter_GetBySubMeterGUID]
    @SubMeterGUID UNIQUEIDENTIFIER,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-14 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        SubMeterId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        SubMeterGUID
    FROM 
        [Customer].[SubMeter] 
    WHERE 
        SubMeterGUID = @SubMeterGUID
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
