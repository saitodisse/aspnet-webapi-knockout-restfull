USE [master]

-- Enable Integrated Security=False
-- Restart your SQL after that
GO
EXEC xp_instance_regwrite N'HKEY_LOCAL_MACHINE', N'Software\Microsoft\MSSQLServer\MSSQLServer', N'LoginMode', REG_DWORD, 2
GO

-- Create the 'qweasd' user
DECLARE @SID VARBINARY(85);
SELECT @SID = sid FROM dbo.syslogins WHERE name = 'aUser'
SELECT @SID

IF @SID IS NULL
BEGIN
   CREATE LOGIN [aUser] WITH PASSWORD=N'aUserPass123456789', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
   EXEC master..sp_addsrvrolemember @loginame = N'aUser', @rolename = N'sysadmin'
END