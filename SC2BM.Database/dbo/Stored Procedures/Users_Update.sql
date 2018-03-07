
-- =============================================
-- Author:		AKardash
-- Create date: 10/5/2015
-- Description:	Modify existing user
-- =============================================
CREATE PROCEDURE [dbo].[Users_Update]
	@ID int,
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
	UPDATE [dbo].[Users]
	SET [UserName] = @UserName, 
		[FirstName] = @FirstName,
		[LastName] = @LastName,
		[BirthDate] = @BirthDate,
		[Email] = @Email,
		[Roles] = @Roles,
		[Password] = @Password,
		[ActivationSalt] = @ActivationSalt,
		[IsActive] = @IsActive,
		[RegisteredDate] = @RegisteredDate
    WHERE [ID] = @ID;
END