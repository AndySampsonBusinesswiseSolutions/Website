USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Temp.Customer].[FlexReferenceVolume_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Temp.Customer].[FlexReferenceVolume_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-07-20
-- Description:	Insert new FlexReferenceVolume into [Temp.Customer].[FlexReferenceVolume] table
-- =============================================

ALTER PROCEDURE [Temp.Customer].[FlexReferenceVolume_Insert]
    @ProcessQueueGUID UNIQUEIDENTIFIER,
    @ContractReference VARCHAR(255),
    @DateFrom VARCHAR(255),
    @DateTo VARCHAR(255),
    @Volume VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-07-20 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Temp.Customer].[FlexReferenceVolume]
    (
        ProcessQueueGUID,
        ContractReference,
        DateFrom,
        DateTo,
        Volume
    )
    VALUES
    (
        @ProcessQueueGUID,
        @ContractReference,
        @DateFrom,
        @DateTo,
        @Volume
    )
END
GO
