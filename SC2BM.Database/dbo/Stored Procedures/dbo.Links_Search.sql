CREATE PROCEDURE [dbo].[Links_Search]
	@LinkID int = NULL,
	@EntityType NVARCHAR(25) = NULL,
	@EntityID int = NULL,
	@OwnerUserID int = NULL,		
	@Type nvarchar(100) = NULL,
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
		EntityType NVARCHAR(100),		
		EntityID int,		
		OwnerUserID int,
		OwnerUserName nvarchar(50),
		AddedDate datetime,
		DisplayText nvarchar(255),
		MainLink nvarchar(1000),
		SecondaryLink nvarchar(1000),
		[Type] nvarchar(100)
	);
	
	-- Search Users Start

	INSERT INTO #searchResults (ID, EntityType, EntityID, OwnerUserID, OwnerUserName, AddedDate, DisplayText, MainLink, SecondaryLink, [Type])
	SELECT
		item.ID,
		item.EntityType,
		item.EntityID,		
		item.OwnerUserID,
		u.UserName as OwnerUserName,
		item.AddedDate,
		item.DisplayText,
		item.MainLink,
		item.SecondaryLink,
		item.[Type]
	FROM dbo.Links item
	JOIN dbo.Users u ON item.OwnerUserID = u.ID
	WHERE		
		(@EntityID IS NULL OR item.EntityID = @EntityID)
		AND (@LinkID IS NULL OR item.ID = @LinkID)
		AND (@OwnerUserID IS NULL OR item.OwnerUserID = @OwnerUserID)
		AND (@EntityType IS NULL OR item.EntityType = @EntityType)
		AND (@Type IS NULL OR item.[Type] = @Type)

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
		ID, EntityType, EntityID, OwnerUserID, OwnerUserName, AddedDate, DisplayText, MainLink, SecondaryLink, [Type]
	FROM (
		SELECT 
			ROW_NUMBER() OVER (ORDER BY
			CASE
				WHEN @OrderDirection = 'A' THEN
					CASE
						WHEN @OrderBy = 'Type' THEN [Type]
						WHEN @OrderBy = 'EntityType' THEN EntityType
						WHEN @OrderBy = 'OwnerUserName' THEN [OwnerUserName]
						WHEN @OrderBy = 'AddedDate' THEN CONVERT(VARCHAR, AddedDate, 20)
						ELSE AddedDate
					END
				END ASC,
			CASE
				WHEN @OrderDirection = 'D' THEN
					CASE
						WHEN @OrderBy = 'Type' THEN [Type]
						WHEN @OrderBy = 'EntityType' THEN EntityType
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