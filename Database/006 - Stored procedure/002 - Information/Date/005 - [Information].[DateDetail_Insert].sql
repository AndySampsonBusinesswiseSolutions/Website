USE [EMaaS]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('[Information].[DateDetail_Insert]'))
    BEGIN
        EXEC('CREATE PROCEDURE [Information].[DateDetail_Insert] AS BEGIN SET NOCOUNT ON; END')
    END
GO

-- =============================================
-- Author:		Andrew Sampson
-- Create date: 2020-09-02
-- Description:	Insert new Date detail into [Information].[DateDetail] table
-- =============================================

ALTER PROCEDURE [Information].[DateDetail_Insert]
    @CreatedByUserId BIGINT,
    @SourceId BIGINT,
    @DateId BIGINT,
    @DateAttributeId BIGINT,
    @DateDetailDescription VARCHAR(255)
AS
BEGIN
    -- =============================================
    --              CHANGE HISTORY
    -- 2020-09-02 -> Andrew Sampson -> Initial development of script
    -- =============================================

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [Information].[DateDetail]
    (
        CreatedByUserId,
        SourceId,
        DateId,
        DateAttributeId,
        DateDetailDescription
    )
    VALUES
    (
        @CreatedByUserId,
        @SourceId,
        @DateId,
        @DateAttributeId,
        @DateDetailDescription
    )
END
GO