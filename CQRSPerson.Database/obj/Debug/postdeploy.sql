/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

IF Exists (SELECT * from [dbo].[Person])
BEGIN
    INSERT INTO [dbo].[Person] (FirstName,LastName,Age,Interests,Image)
    VALUES ('Jerry','Stiller',92,'Actor','https://i.imgur.com/sxgkv4i.jpeg')

    INSERT INTO [dbo].[Person] (FirstName,LastName,Age,Interests,Image)
    VALUES ('Freddie','Mercury ',73,'Singer','https://i.imgur.com/UjTNr58.jpeg')

    INSERT INTO [dbo].[Person] (FirstName,LastName,Age,Interests,Image)
    VALUES ('Alex','Hosein',29,'Software Development','https://i.imgur.com/kPuKcZW.png')

    INSERT INTO [dbo].[Person] (FirstName,LastName,Age,Interests,Image)
    VALUES ('Scrappy','Doo',5,'Being a good boy','https://i.imgur.com/d2vfF5O.jpg')

    INSERT INTO [dbo].[Person] (FirstName,LastName,Age,Interests,Image)
    VALUES ('Tom','Travolta',45,'Being a CelebrityMashup','https://i.imgur.com/zpgE5G3.jpg')
END
GO
