CREATE PROCEDURE [dbo].[BuildOrders_Search]
	@BuildOrderID int = NULL,
	@Name NVARCHAR(255) = NULL,
	@SC2VersionID NVARCHAR(50) = NULL,
	@Description NVARCHAR(MAX) = NULL,
	@Race NVARCHAR(10) = NULL,
	@VsRace NVARCHAR(10) = NULL,
	@OwnerUserID int = NULL,	
	@IsDeleted BIT = NULL,
	@IsStrict BIT = 0,
	@OrderBy NVARCHAR(50) = '',
	@OrderDirection CHAR(1) = 'A',					-- 'A' = Ascending, 'D' = Descending
	@PageNumber INT = 1,							-- Note: Pass in 99999 to indicate Last Page
	@RowsPerPage INT = 25,
	@TotalCount INT OUTPUT	
AS
BEGIN
	SET NOCOUNT ON;
    
	--DECLARE @TotalCount INT;
	--DECLARE @BuildOrderID int = NULL;
	--DECLARE @Name NVARCHAR(255) = '1 Rax FE into 2 Medivac Push';
	--DECLARE @SC2VersionID NVARCHAR(50) = NULL;
	--DECLARE @Description NVARCHAR(MAX) = NULL;
	--DECLARE @Race NVARCHAR(10) = NULL;--'Terran';
	--DECLARE @VsRace NVARCHAR(10) = NULL;
	--DECLARE @OwnerUserID int = NULL;
	--DECLARE @IsStrict BIT = 0;
	--DECLARE @OrderBy NVARCHAR(50) = 'Race';
	--DECLARE @OrderDirection CHAR(1) = 'A';					-- 'A' = Ascending, 'D' = Descending
	--DECLARE @PageNumber INT = 1;							-- Note: Pass in 99999 to indicate Last Page
	--DECLARE @RowsPerPage INT = 25;
	--DECLARE @IsDeleted BIT = NULL;

	DECLARE
		@StartRow INT,
		@EndRow INT,
		@TotalPages INT,
		@TotalPage_Float FLOAT;

	CREATE TABLE #searchResults (
		ID INT, 
		Name NVARCHAR(255),
		SC2VersionID NVARCHAR(50),
		Description NVARCHAR(max),
		Race NVARCHAR(10),
		VsRace NVARCHAR(10),
		BuildItems NVARCHAR(max),		
		OwnerUserID int,
		OwnerUserName nvarchar(50),
		AddedDate datetime,
		IsDeleted bit,
		MicroRate int,
		MacroRate int,
		AggressionRate int,
		DefenceRate int
	);
	
	-- Search Users Start

	INSERT INTO #searchResults (ID, Name, SC2VersionID, Description, Race, VsRace, 
	BuildItems, OwnerUserID, OwnerUserName, AddedDate, IsDeleted, MicroRate, MacroRate, AggressionRate, DefenceRate)
	SELECT
		item.ID,
		item.Name,
		item.SC2VersionID,
		item.Description,
		item.Race,
		item.VsRace,
		item.BuildItems,
		item.OwnerUserID,
		u.UserName as OwnerUserName,
		item.AddedDate,
		item.IsDeleted,
		item.MicroRate,
		item.MacroRate,
		item.AggressionRate,
		item.DefenceRate
	FROM dbo.BuildOrders item
	JOIN dbo.Users u ON item.OwnerUserID = u.ID
	WHERE
		(@IsDeleted IS NULL OR [IsDeleted] = @IsDeleted)
		AND (@BuildOrderID IS NULL OR item.ID = @BuildOrderID)
		AND (@OwnerUserID IS NULL OR item.OwnerUserID = @OwnerUserID)
		AND (@Name IS NULL OR (@IsStrict = 0 AND item.Name LIKE '%' + @Name + '%') OR (@IsStrict = 1 AND item.Name = @Name))
		AND (@SC2VersionID IS NULL OR item.SC2VersionID = @SC2VersionID)
		AND (@Race IS NULL OR item.Race = @Race)
		AND (@VsRace IS NULL OR item.VsRace = @VsRace)
		AND (@Description IS NULL OR (@IsStrict = 0 AND item.Description LIKE '%' + @Description + '%') OR (@IsStrict = 1 AND item.Description = @Description))

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
		ID,
		Name,
		SC2VersionID,
		Description,
		Race,
		VsRace,
		BuildItems,
		OwnerUserID,
		OwnerUserName,
		AddedDate,
		MicroRate, 
		MacroRate,
		AggressionRate, 
		DefenceRate
	FROM (
		SELECT 
			ROW_NUMBER() OVER (ORDER BY
			CASE
				WHEN @OrderDirection = 'A' THEN
					CASE
						WHEN @OrderBy = 'Name' THEN Name
						WHEN @OrderBy = 'SC2VersionID' THEN SC2VersionID
						WHEN @OrderBy = 'Race' THEN [Race]
						WHEN @OrderBy = 'VsRace' THEN [VsRace]
						WHEN @OrderBy = 'OwnerUserName' THEN [OwnerUserName]
						WHEN @OrderBy = 'AddedDate' THEN CONVERT(VARCHAR, AddedDate, 20)
						ELSE Name
					END
				END ASC,
			CASE
				WHEN @OrderDirection = 'D' THEN
					CASE
						WHEN @OrderBy = 'Name' THEN Name
						WHEN @OrderBy = 'SC2VersionID' THEN SC2VersionID
						WHEN @OrderBy = 'Race' THEN [Race]
						WHEN @OrderBy = 'VsRace' THEN [VsRace]
						WHEN @OrderBy = 'OwnerUserName' THEN [OwnerUserName]
						WHEN @OrderBy = 'AddedDate' THEN CONVERT(VARCHAR, AddedDate, 20)
						ELSE Name
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