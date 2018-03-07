-- =============================================
-- Author:		AKardash
-- Create date: 10/2/2015
-- Description:	Register new user
-- =============================================
CREATE PROCEDURE [dbo].[Users_Insert]
	@UserName nvarchar(50),
    @FirstName nvarchar(100),
    @LastName nvarchar(255),
    @BirthDate date,
    @Email nvarchar(255),
	@Roles nvarchar(255),
    @Password nvarchar(255),
	@ActivationSalt nvarchar(255),
	@IsActive bit,
	@RegisteredDate date
AS
BEGIN
	INSERT INTO [dbo].[Users]
           ([UserName]
           ,[FirstName]
           ,[LastName]
           ,[BirthDate]
           ,[Email]
		   ,[Roles]
           ,[Password]
		   ,[IsActive]
		   ,[ActivationSalt]
		   ,[RegisteredDate])
    VALUES
           (@UserName
           ,@FirstName
           ,@LastName
           ,@BirthDate
           ,@Email
		   ,@Roles
           ,@Password
		   ,@IsActive
		   ,@ActivationSalt
		   ,@RegisteredDate);

	SELECT SCOPE_IDENTITY();
END