USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[CustomertoChildCustomer_DeleteByCustomerIdAndChildCustomerId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[CustomertoChildCustomer_DeleteByCustomerIdAndChildCustomerId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-06
-- Description:	Delete mapping of a Customer to a ChildCustomer from [Mapping].[CustomerToChildCustomer] table by Customer Id and Child Customer Id
-- =============================================

ALTER PROCEDURE [Mapping].[CustomertoChildCustomer_DeleteByCustomerIdAndChildCustomerId]
    @CustomerId BIGINT,
    @ChildCustomerId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-06 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE
        [Mapping].[CustomerDetail]
    SET
        EffectiveToDateTime = GETUTCDATE()
    WHERE
        CustomerId = @CustomerId
        AND ChildCustomerId = @ChildCustomerId
END
GO
