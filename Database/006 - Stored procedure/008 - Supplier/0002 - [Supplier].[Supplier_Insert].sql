USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Supplier].[Supplier_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Supplier].[Supplier_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-25
-- Description:	Insert new supplier into [Supplier].[Supplier] table
-- =============================================

ALTER PROCEDURE [Supplier].[Supplier_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @SupplierGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-25 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT TOP 1 1 FROM [Supplier].[Supplier] WHERE SupplierGUID = @SupplierGUID
        AND EffectiveToDateTime = '9999-12-31')
        BEGIN
            INSERT INTO [Supplier].[Supplier]
            (
                CreatedByUserId,
                SourceId,
                SupplierGUID
            )
            VALUES
            (
                @CreatedByUserId,
                @SourceId,
                @SupplierGUID
            )
        END
END
GO
