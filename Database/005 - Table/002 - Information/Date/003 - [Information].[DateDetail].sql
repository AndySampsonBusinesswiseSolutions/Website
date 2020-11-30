USE [EMaaS]
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Information].[DateDetail]') AND type in (N'U'))
DROP TABLE [Information].[DateDetail]
GO
CREATE TABLE [Information].[DateDetail]
	(
	DateDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	DateId BIGINT NOT NULL,
	DateAttributeId BIGINT NOT NULL,
	DateDetailDescription VARCHAR(255) NOT NULL
	)  ON [Information]
GO
ALTER TABLE [Information].[DateDetail] ADD CONSTRAINT
	PK_DateDetail PRIMARY KEY CLUSTERED 
	(
	DateDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Information]

GO
ALTER TABLE [Information].[DateDetail] ADD CONSTRAINT
	DF_DateDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Information].[DateDetail] ADD CONSTRAINT
	DF_DateDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Information].[DateDetail] ADD CONSTRAINT
	DF_DateDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Information].[DateDetail] ADD CONSTRAINT
	FK_DateDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[DateDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'DateDetail', N'CONSTRAINT', N'FK_DateDetail_CreatedByUserId'
GO
ALTER TABLE [Information].[DateDetail] ADD CONSTRAINT
	FK_DateDetail_DateId FOREIGN KEY
	(
	DateId
	) REFERENCES [Information].[Date]
	(
	DateId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[DateDetail].DateId to [Information].[Date].DateId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'DateDetail', N'CONSTRAINT', N'FK_DateDetail_DateId'
GO
ALTER TABLE [Information].[DateDetail] ADD CONSTRAINT
	FK_DateDetail_DateAttributeId FOREIGN KEY
	(
	DateAttributeId
	) REFERENCES [Information].[DateAttribute]
	(
	DateAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[DateDetail].DateAttributeId to [Information].[DateAttribute].DateAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'DateDetail', N'CONSTRAINT', N'FK_DateDetail_DateAttributeId'
GO
ALTER TABLE [Information].[DateDetail] ADD CONSTRAINT
	FK_DateDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Information].[DateDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Information', N'TABLE', N'DateDetail', N'CONSTRAINT', N'FK_DateDetail_SourceId'
GO
ALTER TABLE [Information].[DateDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
