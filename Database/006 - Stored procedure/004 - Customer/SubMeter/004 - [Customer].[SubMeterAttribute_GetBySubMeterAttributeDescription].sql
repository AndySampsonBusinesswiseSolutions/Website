USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[SubMeterAttribute_GetBySubMeterAttributeDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[SubMeterAttribute_GetBySubMeterAttributeDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-03
-- Description:	Get SubMeterAttribute info from [Customer].[SubMeterAttribute] table by SubMeterAttributeDescription
-- =============================================

ALTER PROCEDURE [Customer].[SubMeterAttribute_GetBySubMeterAttributeDescription]
    @SubMeterAttributeDescription VARCHAR(255),
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-03 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        SubMeterAttributeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        SubMeterAttributeDescription
    FROM 
        [Customer].[SubMeterAttribute] 
    WHERE 
        SubMeterAttributeDescription = @SubMeterAttributeDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
