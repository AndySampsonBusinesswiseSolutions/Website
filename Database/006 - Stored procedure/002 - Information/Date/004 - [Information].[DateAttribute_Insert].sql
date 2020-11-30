USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[DateAttribute_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[DateAttribute_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-02
-- Description:	Insert new Date attribute into [Information].[DateAttribute] table
-- =============================================

ALTER PROCEDURE [Information].[DateAttribute_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @DateAttributeDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-02 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Information].[DateAttribute]
    (
        CreatedByUserId,
        SourceId,
        DateAttributeDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @DateAttributeDescription
    )
END
GO