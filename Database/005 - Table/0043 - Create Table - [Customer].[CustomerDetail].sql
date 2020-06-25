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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[Customer].[CustomerDetail]') AND type in (N'U'))
DROP TABLE [Customer].[CustomerDetail]
GO
CREATE TABLE [Customer].[CustomerDetail]
	(
	CustomerDetailId BIGINT IDENTITY(1,1) NOT NULL,
	EffectiveFromDateTime DATETIME NOT NULL,
	EffectiveToDateTime DATETIME NOT NULL,
	CreatedDateTime DATETIME NOT NULL,
	CreatedByUserId BIGINT NOT NULL,
	SourceId BIGINT NOT NULL,
	CustomerId BIGINT NOT NULL,
	CustomerAttributeId BIGINT NOT NULL,
	CustomerDetailDescription VARCHAR(200) NOT NULL
	)  ON [Customer]
GO
ALTER TABLE [Customer].[CustomerDetail] ADD CONSTRAINT
	PK_CustomerDetail PRIMARY KEY CLUSTERED 
	(
	CustomerDetailId
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [Customer]

GO
ALTER TABLE [Customer].[CustomerDetail] ADD CONSTRAINT
	DF_CustomerDetail_EffectiveFromDateTime DEFAULT GETUTCDATE() FOR EffectiveFromDateTime
GO
ALTER TABLE [Customer].[CustomerDetail] ADD CONSTRAINT
	DF_CustomerDetail_EffectiveToDateTime DEFAULT '9999-12-31' FOR EffectiveToDateTime
GO
ALTER TABLE [Customer].[CustomerDetail] ADD CONSTRAINT
	DF_CustomerDetail_CreatedDateTime DEFAULT GETUTCDATE() FOR CreatedDateTime
GO
ALTER TABLE [Customer].[CustomerDetail] ADD CONSTRAINT
	FK_CustomerDetail_CreatedByUserId FOREIGN KEY
	(
	CreatedByUserId
	) REFERENCES [Administration.User].[User]
	(
	UserId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[CustomerDetail].CreatedByUserId to [Administration.User].[User].UserId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'CustomerDetail', N'CONSTRAINT', N'FK_CustomerDetail_CreatedByUserId'
GO
ALTER TABLE [Customer].[CustomerDetail] ADD CONSTRAINT
	FK_CustomerDetail_CustomerId FOREIGN KEY
	(
	CustomerId
	) REFERENCES [Customer].[Customer]
	(
	CustomerId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[CustomerDetail].CustomerId to [Customer].[Customer].CustomerId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'CustomerDetail', N'CONSTRAINT', N'FK_CustomerDetail_CustomerId'
GO
ALTER TABLE [Customer].[CustomerDetail] ADD CONSTRAINT
	FK_CustomerDetail_CustomerAttributeId FOREIGN KEY
	(
	CustomerAttributeId
	) REFERENCES [Customer].[CustomerAttribute]
	(
	CustomerAttributeId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[CustomerDetail].CustomerAttributeId to [Customer].[CustomerAttribute].CustomerAttributeId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'CustomerDetail', N'CONSTRAINT', N'FK_CustomerDetail_CustomerAttributeId'
GO
ALTER TABLE [Customer].[CustomerDetail] ADD CONSTRAINT
	FK_CustomerDetail_SourceId FOREIGN KEY
	(
	SourceId
	) REFERENCES [Information].[Source]
	(
	SourceId
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
DECLARE @v sql_variant 
SET @v = N'Foreign Key constraint joining [Customer].[CustomerDetail].SourceId to [Information].[Source].SourceId'
EXECUTE sp_addextendedproperty N'MS_Description', @v, N'SCHEMA', N'Customer', N'TABLE', N'CustomerDetail', N'CONSTRAINT', N'FK_CustomerDetail_SourceId'
GO
ALTER TABLE [Customer].[CustomerDetail] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
