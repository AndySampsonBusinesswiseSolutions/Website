USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[HostEnvironmentDetail_GetByHostEnvironmentIdAndHostEnvironmentAttributeId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[HostEnvironmentDetail_GetByHostEnvironmentIdAndHostEnvironmentAttributeId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-04
-- Description:	Get HostEnvironmentDetail info from [System].[HostEnvironmentDetail] table by HostEnvironment Id and HostEnvironment Attribute Id
-- =============================================

ALTER PROCEDURE [System].[HostEnvironmentDetail_GetByHostEnvironmentIdAndHostEnvironmentAttributeId]
    @HostEnvironmentId BIGINT,
    @HostEnvironmentAttributeId BIGINT
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
        HostEnvironmentId = @HostEnvironmentId
        AND HostEnvironmentAttributeId = @HostEnvironmentAttributeId
END
GO
