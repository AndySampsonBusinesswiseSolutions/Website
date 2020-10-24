USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[ContractMeterRate_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[ContractMeterRate_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-17
-- Description:	Insert new ContractMeterRate into [Customer].[ContractMeterRate] table
-- =============================================

ALTER PROCEDURE [Customer].[ContractMeterRate_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ContractMeterRateGUID UNIQUEIDENTIFIER
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-17 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Customer].[ContractMeterRate]
    (
        CreatedByUserId,
        SourceId,
        ContractMeterRateGUID
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ContractMeterRateGUID
    )
END
GO