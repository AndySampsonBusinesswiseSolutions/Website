USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[CustomerToSite_GetByCustomerIdAndSiteId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[CustomerToSite_GetByCustomerIdAndSiteId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-11-10
-- Description:	Get CustomerToSite info from [Mapping].[CustomerToSite] table by Customer Id and Site Id
-- =============================================

ALTER PROCEDURE [Mapping].[CustomerToSite_GetByCustomerIdAndSiteId]
    @CustomerId BIGINT,
    @SiteId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-11-10 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        CustomerToSiteId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        SiteId,
        CustomerId
    FROM 
        [Mapping].[CustomerToSite]
    WHERE 
        CustomerId = @CustomerId
        AND SiteId = @SiteId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
