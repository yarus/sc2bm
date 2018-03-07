CREATE PROCEDURE [dbo].[Users_Search]
	@UserName NVARCHAR(50) = NULL,
	@FirstName NVARCHAR(100) = NULL,
	@LastName NVARCHAR(255) = NULL,	
	@Email NVARCHAR(255) = NULL,
	@IsStrict BIT = 0,
	@OrderBy NVARCHAR(50) = '',
	@OrderDirection CHAR(1) = 'A',					-- 'A' = Ascending, 'D' = Descending
	@PageNumber INT = 1,							-- Note: Pass in 99999 to indicate Last Page
	@RowsPerPage INT = 25,
	@TotalCount INT OUTPUT	
AS
BEGIN
	SET NOCOUNT ON;
    
	DECLARE
		@StartRow INT,
		@EndRow INT,
		@TotalPages INT,
		@TotalPage_Float FLOAT;

	CREATE TABLE #searchResults (
		ID INT, 
		UserName NVARCHAR(50),
		FirstName NVARCHAR(100),
		LastName VARCHAR(255),
		BirthDate DATE,
		Email NVARCHAR(255),
		Roles NVARCHAR(255),
		Password NVARCHAR(255),
		ActivationSalt VARCHAR(220),
		IsActive BIT,
		RegisteredDate DATE
	);
	
	-- Search Users Start

	INSERT INTO #searchResults (ID, UserName, FirstName, LastName, BirthDate, Email, Roles, Password, ActivationSalt, IsActive, RegisteredDate)
	SELECT DISTINCT
		item.ID,
		item.UserName,
		item.FirstName,
		item.LastName,
		item.BirthDate,
		item.Email,
		item.Roles,
		item.Password,
		item.ActivationSalt,
		item.IsActive,
		item.RegisteredDate
	FROM dbo.Users item
	WHERE
		(@UserName IS NULL OR (@IsStrict = 0 AND item.UserName LIKE '%' + @UserName + '%') OR (@IsStrict = 1 AND item.UserName = @UserName))
		AND (@FirstName IS NULL OR (@IsStrict = 0 AND item.FirstName LIKE '%' + @FirstName + '%') OR (@IsStrict = 1 AND item.FirstName = @FirstName))
		AND (@LastName IS NULL OR (@IsStrict = 0 AND item.LastName LIKE '%' + @LastName + '%') OR (@IsStrict = 1 AND item.LastName = @LastName))
		AND (@Email IS NULL OR (@IsStrict = 0 AND item.Email LIKE '%' + @Email + '%') OR (@IsStrict = 1 AND item.Email = @Email));

	-- Search Users End

	-- Paging Start

	SELECT @TotalPage_Float = CAST(COUNT(*) AS FLOAT) / CAST(@RowsPerPage AS FLOAT)
	FROM #searchResults;

	IF @TotalPage_Float <> cast(@TotalPage_Float as INT)
		SET @TotalPages = cast(@TotalPage_Float as INT) + 1
	ELSE
		SET @TotalPages = cast(@TotalPage_Float as INT) 

	IF @PageNumber < 1
		SET @PageNumber = 1

	IF @PageNumber > @TotalPages
		SET @PageNumber = @TotalPages

	SET @StartRow = ((@PageNumber * @RowsPerPage) - @RowsPerPage) + 1
	SET @EndRow   = @StartRow + @RowsPerPage - 1	

	SELECT @TotalCount = COUNT(1)
	FROM #searchResults;

	-- Paging End

	-- Return paged results Start

	SELECT 		
		ID,
		UserName,
		FirstName,
		LastName,
		BirthDate,
		Email,
		Roles,
		Password,
		ActivationSalt,
		IsActive,
		RegisteredDate
	FROM (
		SELECT
			ROW_NUMBER() OVER 
				(ORDER BY
					CASE
						WHEN @OrderDirection = 'A' THEN
							CASE
								WHEN @OrderBy = 'LastName' THEN LastName
								WHEN @OrderBy = 'FirstName' THEN FirstName
								WHEN @OrderBy = 'UserName' THEN UserName
								WHEN @OrderBy = 'Email' THEN Email
								ELSE LastName
							END
						END ASC,
					CASE
						WHEN @OrderDirection = 'D' THEN
							CASE
								WHEN @OrderBy = 'LastName' THEN LastName
								WHEN @OrderBy = 'FirstName' THEN FirstName
								WHEN @OrderBy = 'UserName' THEN UserName
								WHEN @OrderBy = 'Email' THEN Email
								ELSE LastName
							END
						END DESC
				) Row_Num,
			t.*
		FROM #searchResults t
	) a
	WHERE Row_Num BETWEEN @StartRow AND @EndRow
	ORDER BY Row_Num;
	-- Return paged results End

	-- Clean temporary table
	DROP TABLE #searchResults;
END