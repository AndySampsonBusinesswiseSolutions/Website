USE [EMaaS]
GO

DECLARE @RoutingAPIGUID UNIQUEIDENTIFIER = 'A4F25D07-86AA-42BD-ACD7-51A8F772A92B'
DECLARE @UserGUID UNIQUEIDENTIFIER =  '743E21EE-2185-45D4-9003-E35060B751E2'
DECLARE @SourceTypeDescription VARCHAR(255) = 'User Generated'

EXEC [System].[API_Insert] @UserGUID, @SourceTypeDescription, @RoutingAPIGUID
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @RoutingAPIGUID, 'API Name', 'Routing.api'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @RoutingAPIGUID, 'HTTP Application URL', 'http://localhost:5002/'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @RoutingAPIGUID, 'HTTPS Application URL', 'https://localhost:5003/'