USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[BasketAttribute_GetByBasketAttributeDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[BasketAttribute_GetByBasketAttributeDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-03
-- Description:	Get BasketAttribute info from [Customer].[BasketAttribute] table by BasketAttributeDescription
-- =============================================

ALTER PROCEDURE [Customer].[BasketAttribute_GetByBasketAttributeDescription]
    @BasketAttributeDescription VARCHAR(255),
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
        BasketAttributeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        BasketAttributeDescription
    FROM 
        [Customer].[BasketAttribute] 
    WHERE 
        BasketAttributeDescription = @BasketAttributeDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
