USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[FlexContractDetail_GetByFlexContractAttributeIdAndFlexContractDetailDescription]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[FlexContractDetail_GetByFlexContractAttributeIdAndFlexContractDetailDescription] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-03
-- Description:	Get FlexContractDetail info from [Customer].[FlexContractDetail] table by Customer Attribute Id and Customer Detail Description
-- =============================================

ALTER PROCEDURE [Customer].[FlexContractDetail_GetByFlexContractAttributeIdAndFlexContractDetailDescription]
    @FlexContractAttributeId BIGINT,
    @FlexContractDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-03 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
        FlexContractDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        FlexContractId,
        FlexContractAttributeId,
        FlexContractDetailDescription
    FROM 
        [Customer].[FlexContractDetail] 
    WHERE 
        FlexContractAttributeId = @FlexContractAttributeId
        AND FlexContractDetailDescription = @FlexContractDetailDescription
END
GO
