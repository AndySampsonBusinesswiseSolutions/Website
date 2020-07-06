USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[CustomerDetail_GetByCustomerIdAndCustomerAttributeId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[CustomerDetail_GetByCustomerIdAndCustomerAttributeId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-06
-- Description:	Get CustomerDetail info from [Customer].[CustomerDetail] table by Customer Id and Customer Attribute Id
-- =============================================

ALTER PROCEDURE [Customer].[CustomerDetail_GetByCustomerIdAndCustomerAttributeId]
    @CustomerId BIGINT,
    @CustomerAttributeId BIGINT,
    @EffectiveDateTime DATETIME = NULL
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-06 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SET @EffectiveDateTime = ISNULL(@EffectiveDateTime, GETUTCDATE())

    SELECT 
        CustomerDetailId,
        EffectiveFromDateTime,
        EffectiveToDateTime,
        CreatedDateTime,
        CreatedByUserId,
        SourceId,
        CustomerId,
        CustomerAttributeId,
        CustomerDetailDescription
    FROM 
        [Customer].[CustomerDetail] 
    WHERE 
        CustomerId = @CustomerId
        AND CustomerAttributeId = @CustomerAttributeId
        AND @EffectiveDateTime BETWEEN EffectiveFromDateTime AND EffectiveToDateTime
END
GO
