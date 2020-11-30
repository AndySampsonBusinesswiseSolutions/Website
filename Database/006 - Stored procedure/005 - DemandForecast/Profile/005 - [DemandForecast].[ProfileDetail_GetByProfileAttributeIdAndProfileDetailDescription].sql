USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[DemandForecast].[ProfileDetail_GetByProfileAttributeIdAndProfileDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [DemandForecast].[ProfileDetail_GetByProfileAttributeIdAndProfileDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-14
-- Description:	Get ProfileDetail info from [DemandForecast].[ProfileDetail] table by Profile Attribute Id and Profile Detail Description
-- =============================================

ALTER PROCEDURE [DemandForecast].[ProfileDetail_GetByProfileAttributeIdAndProfileDetailDescription]
    @ProfileAttributeId BIGINT,
    @ProfileDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-14 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        ProfileDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ProfileId,
        ProfileAttributeId,
        ProfileDetailDescription
    FROM 
        [DemandForecast].[ProfileDetail] 
    WHERE 
        ProfileAttributeId = @ProfileAttributeId
        AND ProfileDetailDescription = @ProfileDetailDescription
END
GO
