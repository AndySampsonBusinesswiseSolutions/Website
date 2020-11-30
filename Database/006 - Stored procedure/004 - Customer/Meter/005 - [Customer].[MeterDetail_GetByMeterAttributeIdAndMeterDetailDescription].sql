USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[MeterDetail_GetByMeterAttributeIdAndMeterDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[MeterDetail_GetByMeterAttributeIdAndMeterDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-03
-- Description:	Get MeterDetail info from [Customer].[MeterDetail] table by Customer Attribute Id and Customer Detail Description
-- =============================================

ALTER PROCEDURE [Customer].[MeterDetail_GetByMeterAttributeIdAndMeterDetailDescription]
    @MeterAttributeId BIGINT,
    @MeterDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-03 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        MeterDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        MeterId,
        MeterAttributeId,
        MeterDetailDescription
    FROM 
        [Customer].[MeterDetail] 
    WHERE 
        MeterAttributeId = @MeterAttributeId
        AND MeterDetailDescription = @MeterDetailDescription
END
GO
