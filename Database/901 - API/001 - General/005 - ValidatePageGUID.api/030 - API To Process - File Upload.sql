USE [EMaaS]
GO

DECLARE @APIGUID UNIQUEIDENTIFIER = 'F916F19F-9408-4969-84DC-9905D2FEFB0B'
DECLARE @APIId BIGINT = (SELECT APIId FROM [System].[API] WHERE APIGUID = @APIGUID)
DECLARE @CreatedByUserId BIGINT = (SELECT UserId FROM [Administration.User].[User] WHERE UserGUID = '743E21EE-2185-45D4-9003-E35060B751E2')
DECLARE @SourceAttributeId BIGINT = (SELECT SourceAttributeId FROM [Information].[SourceAttribute] WHERE SourceAttributeDescription = 'User Generated')
DECLARE @SourceId BIGINT = (SELECT SourceId FROM [Information].[SourceDetail] WHERE SourceAttributeId = @SourceAttributeId AND SourceDetailDescription = @CreatedByUserId)
DECLARE @ProcessId BIGINT = (SELECT ProcessId FROM [System].[Process] WHERE ProcessGUID ='3AFF25CB-06BD-4BD1-A409-13D10A08044F')

EXEC [Mapping].[APIToProcess_Insert] @CreatedByUserId, @SourceId, @APIId, @ProcessId