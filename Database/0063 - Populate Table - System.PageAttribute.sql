USE [EMaas]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM System.PageAttribute WHERE PageAttributeDescription = 'PageName')
    BEGIN
        INSERT INTO [System].PageAttribute
        (
            PageAttributeDescription,
            CreatedByUserId
        )
        VALUES
        ('PageName', 1)
    END