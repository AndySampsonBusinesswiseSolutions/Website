USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[GridSupplyPointDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[GridSupplyPointDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Insert new grid supply point detail into [Information].[GridSupplyPointDetail] table
-- =============================================

ALTER PROCEDURE [Information].[GridSupplyPointDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @GridSupplyPointId BIGINT,
    @GridSupplyPointAttributeId BIGINT,
    @GridSupplyPointDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-15 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Information].[GridSupplyPointDetail]
    (
        CreatedByUserId,
        SourceId,
        GridSupplyPointId,
        GridSupplyPointAttributeId,
        GridSupplyPointDetailDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @GridSupplyPointId,
        @GridSupplyPointAttributeId,
        @GridSupplyPointDetailDescription
    )
END
GO