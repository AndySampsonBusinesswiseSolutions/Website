USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Customer].[MeterExemptionAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Customer].[MeterExemptionAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-08-15
-- Description:	Insert new MeterExemption attribute into [Customer].[MeterExemptionAttribute] table
-- =============================================

ALTER PROCEDURE [Customer].[MeterExemptionAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @MeterExemptionAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-08-15 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Customer].[MeterExemptionAttribute]
    (
        CreatedByUserId,
        SourceId,
        MeterExemptionAttributeDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @MeterExemptionAttributeDescription
    )
END
GO