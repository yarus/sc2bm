CREATE PROCEDURE [dbo].[Blogs_Search]
	@BlogID INT = NULL,
	@Title NVARCHAR(255) = NULL,	
	@OwnerUserID INT = NULL,
	@IsDeleted BIT = NULL,
	@FromDate DATETIME = NULL,
	@ToDate DATETIME = NULL,	
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
		OwnerUserID int,
		OwnerUserName nvarchar(50),
		Title NVARCHAR(255),
		[Description] NVARCHAR(max),
		AddedDate datetime,
		LogoPath NVARCHAR(255),
		IsDeleted bit
	);
	
	-- Search Users Start

	INSERT INTO #searchResults (ID, OwnerUserID, OwnerUserName, Title, [Description], AddedDate, LogoPath, IsDeleted)
	SELECT
		item.ID,
		item.OwnerUserID,
		u.UserName as OwnerUserName,
		item.Title,
		item.[Description],
		item.AddedDate,
		item.LogoPath,
		item.IsDeleted
	FROM dbo.Blogs item
	JOIN dbo.Users u ON item.OwnerUserID = u.ID
	WHERE
		(@IsDeleted IS NULL OR [IsDeleted] = @IsDeleted)
		AND (@BlogID IS NULL OR item.ID = @BlogID)
		AND (@OwnerUserID IS NULL OR item.OwnerUserID = @OwnerUserID)
		AND (@Title IS NULL OR (@IsStrict = 0 AND item.Title LIKE '%' + @Title + '%') OR (@IsStrict = 1 AND item.Title = @Title))
		AND (@FromDate IS NULL OR item.AddedDate >= @FromDate)
		AND (@ToDate IS NULL OR item.AddedDate <= @ToDate)

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

	-- Search Users End	
	SELECT 
		ID, OwnerUserID, OwnerUserName, Title, [Description], AddedDate, LogoPath, IsDeleted
	FROM (
		SELECT 
			ROW_NUMBER() OVER (ORDER BY
			CASE
				WHEN @OrderDirection = 'A' THEN
					CASE
						WHEN @OrderBy = 'Title' THEN Title
						WHEN @OrderBy = 'OwnerUserName' THEN [OwnerUserName]
						WHEN @OrderBy = 'AddedDate' THEN CONVERT(VARCHAR, AddedDate, 20)
						ELSE AddedDate
					END
				END ASC,
			CASE
				WHEN @OrderDirection = 'D' THEN
					CASE
						WHEN @OrderBy = 'Title' THEN Title
						WHEN @OrderBy = 'OwnerUserName' THEN [OwnerUserName]
						WHEN @OrderBy = 'AddedDate' THEN CONVERT(VARCHAR, AddedDate, 20)
						ELSE AddedDate
					END
				END DESC
			) Row_Num,
			t.*
		FROM #searchResults t
	) a
	WHERE Row_Num BETWEEN @StartRow AND @EndRow
	ORDER BY Row_Num 


	-- Clean temporary table
	DROP TABLE #searchResults;
END