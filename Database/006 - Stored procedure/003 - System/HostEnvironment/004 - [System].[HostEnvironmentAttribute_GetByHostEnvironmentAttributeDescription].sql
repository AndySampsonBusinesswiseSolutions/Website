USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[System].[HostEnvironmentAttribute_GetByHostEnvironmentAttributeDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [System].[HostEnvironmentAttribute_GetByHostEnvironmentAttributeDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-04
-- Description:	Get HostEnvironmentAttribute info from [System].[HostEnvironmentAttribute] table by HostEnvironmentAttributeDescription
-- =============================================

ALTER PROCEDURE [System].[HostEnvironmentAttribute_GetByHostEnvironmentAttributeDescription]
    @HostEnvironmentAttributeDescription VARCHAR(255),
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-04 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        HostEnvironmentAttributeId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        HostEnvironmentAttributeDescription
    FROM 
        [System].[HostEnvironmentAttribute] 
    WHERE 
        HostEnvironmentAttributeDescription = @HostEnvironmentAttributeDescription
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
