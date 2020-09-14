USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ProfileToProfileClass_GetByProfileId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ProfileToProfileClass_GetByProfileId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-14
-- Description:	Get ProfileToProfileClass info from [Mapping].[ProfileToProfileClass] table by Profile Id
-- =============================================

ALTER PROCEDURE [Mapping].[ProfileToProfileClass_GetByProfileId]
    @ProfileId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-14 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        ProfileToProfileClassId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        ProfileId,
        ProfileClassId
    FROM 
        [Mapping].[ProfileToProfileClass]
    WHERE
        ProfileId = @ProfileId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
