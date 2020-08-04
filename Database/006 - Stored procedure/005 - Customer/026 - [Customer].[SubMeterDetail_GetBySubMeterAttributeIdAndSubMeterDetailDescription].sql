USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[SubMeterDetail_GetBySubMeterAttributeIdAndSubMeterDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[SubMeterDetail_GetBySubMeterAttributeIdAndSubMeterDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-03
-- Description:	Get SubMeterDetail info from [Customer].[SubMeterDetail] table by Customer Attribute Id and Customer Detail Description
-- =============================================

ALTER PROCEDURE [Customer].[SubMeterDetail_GetBySubMeterAttributeIdAndSubMeterDetailDescription]
    @SubMeterAttributeId BIGINT,
    @SubMeterDetailDescription VARCHAR(255)
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
        SubMeterDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        SubMeterId,
        SubMeterAttributeId,
        SubMeterDetailDescription
    FROM 
        [Customer].[SubMeterDetail] 
    WHERE 
        SubMeterAttributeId = @SubMeterAttributeId
        AND SubMeterDetailDescription = @SubMeterDetailDescription
END
GO
