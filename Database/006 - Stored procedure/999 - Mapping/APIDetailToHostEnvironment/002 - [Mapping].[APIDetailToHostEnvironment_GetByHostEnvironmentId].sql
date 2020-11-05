USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[APIDetailToHostEnvironment_GetByHostEnvironmentId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[APIDetailToHostEnvironment_GetByHostEnvironmentId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-04
-- Description:	Get APIDetailToHostEnvironment info from [Mapping].[APIDetailToHostEnvironment] table by HostEnvironment Id
-- =============================================

ALTER PROCEDURE [Mapping].[APIDetailToHostEnvironment_GetByHostEnvironmentId]
    @HostEnvironmentId BIGINT,
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
        APIDetailToHostEnvironmentId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        APIDetailId,
        HostEnvironmentId
    FROM 
        [Mapping].[APIDetailToHostEnvironment] 
    WHERE 
        HostEnvironmentId = @HostEnvironmentId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
