USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE Meter = 'P' AND OBJECT_ID = OBJECT_ID('[Mapping].[ContractMeterToMeter_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Mapping].[ContractMeterToMeter_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-16
-- Description:	Insert new mapping of a ContractMeter to a Meter into [Mapping].[ContractMeterToMeter] table
-- =============================================

ALTER PROCEDURE [Mapping].[ContractMeterToMeter_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @ContractMeterId BIGINT,
    @MeterId BIGINT
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-16 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Mapping].ContractMeterToMeter
    (
        CreatedByUserId,
        SourceId,
        ContractMeterId,
        MeterId
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @ContractMeterId,
        @MeterId
    )
END
GO
