CREATE PROCEDURE [dbo].[Rates_Search]
	@RateID int = NULL,
	@EntityType NVARCHAR(25) = NULL,
	@EntityID int = NULL,
	@OwnerUserID int = NULL,		
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
		vALUE INT,
		OwnerUserID int,
		OwnerUserName nvarchar(50),
		AddedDate datetime
	);
	
	-- Search Users Start

	INSERT INTO #searchResults (ID, EntityType, EntityID, vALUE, OwnerUserID, OwnerUserName, AddedDate)
	SELECT
		item.ID,
		item.EntityType,
		item.EntityID,
		item.Value,
		item.OwnerUserID,
		u.UserName as OwnerUserName,
		item.AddedDate		
	FROM dbo.Rates item
	JOIN dbo.Users u ON item.OwnerUserID = u.ID
	WHERE		
		(@EntityID IS NULL OR item.EntityID = @EntityID)
		AND (@RateID IS NULL OR item.ID = @RateID)
		AND (@OwnerUserID IS NULL OR item.OwnerUserID = @OwnerUserID)
		AND (@EntityType IS NULL OR item.EntityType = @EntityType)

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
		ID, EntityType, EntityID, Value, OwnerUserID, OwnerUserName, AddedDate
	FROM (
		SELECT 
			ROW_NUMBER() OVER (ORDER BY
			CASE
				WHEN @OrderDirection = 'A' THEN
					CASE
						WHEN @OrderBy = 'EntityType' THEN EntityType
						WHEN @OrderBy = 'OwnerUserName' THEN [OwnerUserName]
						WHEN @OrderBy = 'AddedDate' THEN CONVERT(VARCHAR, AddedDate, 20)
						ELSE AddedDate
					END
				END ASC,
			CASE
				WHEN @OrderDirection = 'D' THEN
					CASE
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