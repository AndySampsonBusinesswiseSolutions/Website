USE [EMaaS]
GO

DECLARE @WebsiteAPIGUID UNIQUEIDENTIFIER = 'CBB27186-B65F-4F6C-9FFA-B1E6C63C04EE'
DECLARE @UserGUID UNIQUEIDENTIFIER =  '743E21EE-2185-45D4-9003-E35060B751E2'
DECLARE @SourceTypeDescription VARCHAR(255) = 'User Generated'

EXEC [System].[API_Insert] @UserGUID, @SourceTypeDescription, @WebsiteAPIGUID
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @WebsiteAPIGUID, 'API Name', 'Website.api'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @WebsiteAPIGUID, 'HTTP Application URL', 'http://localhost:5000/'
EXEC [System].[APIDetail_Insert] @UserGUID, @SourceTypeDescription, @WebsiteAPIGUID, 'HTTPS Application URL', 'https://localhost:5001/'