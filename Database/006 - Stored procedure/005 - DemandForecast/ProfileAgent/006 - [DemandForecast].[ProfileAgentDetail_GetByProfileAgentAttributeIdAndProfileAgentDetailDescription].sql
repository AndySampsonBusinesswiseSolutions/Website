USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[DemandForecast].[ProfileAgentDetail_GetByProfileAgentAttributeIdAndProfileAgentDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [DemandForecast].[ProfileAgentDetail_GetByProfileAgentAttributeIdAndProfileAgentDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-13
-- Description:	Get ProfileAgentDetail info from [DemandForecast].[ProfileAgentDetail] table by ProfileAgent Attribute Id and ProfileAgent Detail Description
-- =============================================

ALTER PROCEDURE [DemandForecast].[ProfileAgentDetail_GetByProfileAgentAttributeIdAndProfileAgentDetailDescription]
    @ProfileAgentAttributeId BIGINT,
    @ProfileAgentDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-13 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        ProfileAgentDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ProfileAgentId,
        ProfileAgentAttributeId,
        ProfileAgentDetailDescription
    FROM 
        [DemandForecast].[ProfileAgentDetail] 
    WHERE 
        ProfileAgentAttributeId = @ProfileAgentAttributeId
        AND ProfileAgentDetailDescription = @ProfileAgentDetailDescription
END
GO
