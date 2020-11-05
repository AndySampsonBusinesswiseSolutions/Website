USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[HostEnvironmentDetail_GetByHostEnvironmentAttributeIdAndHostEnvironmentDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[HostEnvironmentDetail_GetByHostEnvironmentAttributeIdAndHostEnvironmentDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-04
-- Description:	Get HostEnvironmentDetail info from [System].[HostEnvironmentDetail] table by System Attribute Id and System Detail Description
-- =============================================

ALTER PROCEDURE [System].[HostEnvironmentDetail_GetByHostEnvironmentAttributeIdAndHostEnvironmentDetailDescription]
    @HostEnvironmentAttributeId BIGINT,
    @HostEnvironmentDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-04 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        HostEnvironmentDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        HostEnvironmentId,
        HostEnvironmentAttributeId,
        HostEnvironmentDetailDescription
    FROM 
        [System].[HostEnvironmentDetail] 
    WHERE 
        HostEnvironmentAttributeId = @HostEnvironmentAttributeId
        AND HostEnvironmentDetailDescription = @HostEnvironmentDetailDescription
END
GO
