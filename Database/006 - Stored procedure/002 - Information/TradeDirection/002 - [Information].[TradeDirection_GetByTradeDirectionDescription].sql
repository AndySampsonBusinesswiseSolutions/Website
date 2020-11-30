USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[TradeDirection_GetByTradeDirectionDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[TradeDirection_GetByTradeDirectionDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-18
-- Description:	Get TradeDirection info from [Information].[TradeDirection] table by TradeDirection Description
-- =============================================

ALTER PROCEDURE [Information].[TradeDirection_GetByTradeDirectionDescription]
    @TradeDirectionDescription VARCHAR(255),
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-18 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        TradeDirectionId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        TradeDirectionDescription
    FROM 
        [Information].[TradeDirection] 
    WHERE 
        TradeDirectionDescription = @TradeDirectionDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
