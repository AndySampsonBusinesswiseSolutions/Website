USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[DemandForecast].[ProfileAgentAttribute_GetByProfileAgentAttributeDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [DemandForecast].[ProfileAgentAttribute_GetByProfileAgentAttributeDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-13
-- Description:	Get ProfileAgentAttribute info from [DemandForecast].[ProfileAgentAttribute] table by ProfileAgentAttributeDescription
-- =============================================

ALTER PROCEDURE [DemandForecast].[ProfileAgentAttribute_GetByProfileAgentAttributeDescription]
    @ProfileAgentAttributeDescription VARCHAR(255),
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-13 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        ProfileAgentAttributeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ProfileAgentAttributeDescription
    FROM 
        [DemandForecast].[ProfileAgentAttribute] 
    WHERE 
        ProfileAgentAttributeDescription = @ProfileAgentAttributeDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
