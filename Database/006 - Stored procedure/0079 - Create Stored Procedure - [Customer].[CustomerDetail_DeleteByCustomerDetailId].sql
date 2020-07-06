USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[CustomerDetail_DeleteByCustomerDetailId]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[CustomerDetail_DeleteByCustomerDetailId] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-06
-- Description:	Delete Customer detail from [Customer].[CustomerDetail] table
-- =============================================

ALTER PROCEDURE [Customer].[CustomerDetail_DeleteByCustomerDetailId]
    @CustomerDetailId BIGINT
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
        [Customer].[CustomerDetail]
    SET
        EffectiveToDateTime = GETUTCDATE()
    WHERE
        CustomerDetailId = @CustomerDetailId
END
GO
