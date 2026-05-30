USE [igroup131_test2]
GO

/****** Object:  StoredProcedure [dbo].[SP_ADD_GAME_TO_USER_2026]    Script Date: 30/05/2026 18:44:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_ADD_GAME_TO_USER_2026]
    @UserId int,
    @GameId int
AS
BEGIN
    -- בדיקה קטנה כדי למנוע כפילויות אם המשתמש לוחץ פעמיים
    IF NOT EXISTS (SELECT 1 FROM UsersGames_2026 WHERE userId = @UserId AND gameId = @GameId)
    BEGIN
        INSERT INTO UsersGames_2026 (userId, gameId)
        VALUES (@UserId, @GameId);
    END
END
GO


USE [igroup131_test2]
GO

/****** Object:  StoredProcedure [dbo].[SP_DELETE_GAME_2026]    Script Date: 30/05/2026 18:46:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_DELETE_GAME_2026]
    @Id int
AS
BEGIN
    DELETE FROM Games_2026
    WHERE id = @Id;
END
GO


USE [igroup131_test2]
GO

/****** Object:  StoredProcedure [dbo].[SP_DELETE_GAME_TAGS_2026]    Script Date: 30/05/2026 18:46:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_DELETE_GAME_TAGS_2026]
    @GameId INT
AS
BEGIN
    DELETE FROM TagGame_2026 WHERE gameId = @GameId;
END

GO


USE [igroup131_test2]
GO

/****** Object:  StoredProcedure [dbo].[SP_DELETE_USER_2026]    Script Date: 30/05/2026 18:47:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:	<Noam Lavi, Nadav Meir, Ofek Cohen>
-- Create date: <27/05/2026>
-- Description:	<delete a user from the Users_2026 Table>
-- =============================================
CREATE PROCEDURE [dbo].[SP_DELETE_USER_2026] 
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM Users_2026
	WHERE id = @Id 
END
GO


USE [igroup131_test2]
GO

/****** Object:  StoredProcedure [dbo].[SP_GET_ALL_DISTINCT_TAGS_2026]    Script Date: 30/05/2026 18:47:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_GET_ALL_DISTINCT_TAGS_2026]
AS
BEGIN
    SELECT DISTINCT TagName FROM TagGame_2026 WHERE TagName IS NOT NULL AND TagName != '';
END

GO


USE [igroup131_test2]
GO

/****** Object:  StoredProcedure [dbo].[SP_GET_ALL_GAMES_WITH_TAGS_2026]    Script Date: 30/05/2026 18:47:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_GET_ALL_GAMES_WITH_TAGS_2026]
AS
BEGIN
    SELECT g.*, t.TagName 
    FROM Games_2026 g
    LEFT JOIN TagGame_2026 t ON g.id = t.gameId
    ORDER BY g.id;
END

GO


USE [igroup131_test2]
GO

/****** Object:  StoredProcedure [dbo].[SP_GET_USER_GAMES_2026]    Script Date: 30/05/2026 18:47:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_GET_USER_GAMES_2026]
    @UserId int
AS
BEGIN
    SELECT g.* FROM Games_2026 g
    JOIN UsersGames_2026 ug ON g.id = ug.gameId
    WHERE ug.userId = @UserId;
END
GO


USE [igroup131_test2]
GO

/****** Object:  StoredProcedure [dbo].[SP_GET_USER_GAMES_WITH_TAGS_2026]    Script Date: 30/05/2026 18:47:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_GET_USER_GAMES_WITH_TAGS_2026]
    @UserId INT
AS
BEGIN
    SELECT g.*, t.TagName 
    FROM Games_2026 g
    JOIN UsersGames_2026 ug ON g.id = ug.gameId
    LEFT JOIN TagGame_2026 t ON g.id = t.gameId
    WHERE ug.userId = @UserId
    ORDER BY g.id;
END

GO


USE [igroup131_test2]
GO

/****** Object:  StoredProcedure [dbo].[SP_GET_USERS_2026]    Script Date: 30/05/2026 18:47:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:	<Noam Lavi, Nadav Meir, Ofek Cohen>
-- Create date: <27/05/2026>
-- Description:	<get users from the Users_2026 Table>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GET_USERS_2026] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Users_2026
END
GO


USE [igroup131_test2]
GO

/****** Object:  StoredProcedure [dbo].[SP_INSERT_GAME_2026]    Script Date: 30/05/2026 18:48:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_INSERT_GAME_2026]
    @Name NVARCHAR(150),
    @SteamUrl VARCHAR(500),
    @Image VARCHAR(500),
    @ReleaseDate VARCHAR(100),
    @ReviewSummary NVARCHAR(255),
    @Price INT,
    @Windows BIT,
    @Mac BIT,
    @Linux BIT
AS
BEGIN
    -- קודם מכניסים את המשחק לטבלה
    INSERT INTO Games_2026(name, steamUrl, image, releaseDate, reviewSummary, price, windows, mac, linux)
    VALUES (@Name, @SteamUrl, @Image, @ReleaseDate, @ReviewSummary, @Price, @Windows, @Mac, @Linux);
    
    -- שורת הקסם שחסרה: מחזירה את ה-ID שנוצר הרגע אוטומטית!
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewGameId;
END

GO


USE [igroup131_test2]
GO

/****** Object:  StoredProcedure [dbo].[SP_INSERT_TAG_2026]    Script Date: 30/05/2026 18:48:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_INSERT_TAG_2026]
    @GameId INT,
    @TagName VARCHAR(100)
AS
BEGIN
    -- שינינו כאן את שם הטבלה לשם המדויק שיש לך במסד הנתונים
    INSERT INTO TagGame_2026 (gameId, TagName)
    VALUES (@GameId, @TagName);
END

GO


USE [igroup131_test2]
GO

/****** Object:  StoredProcedure [dbo].[SP_INSERT_USER_2026]    Script Date: 30/05/2026 18:48:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:	<Noam Lavi, Nadav Meir, Ofek Cohen>
-- Create date: <27/05/2026>
-- Description:	<Insert a user into the Users_2026 Table>
-- =============================================
CREATE PROCEDURE [dbo].[SP_INSERT_USER_2026]
	-- Add the parameters for the stored procedure here
	
	@name nvarchar(30), 
	@email varchar(255),
	@password varchar(255),
	@isActive bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.

	--SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Users_2026 (name, email, password, isActive) VALUES (@name, @email, @password, @isActive)
END
GO


USE [igroup131_test2]
GO

/****** Object:  StoredProcedure [dbo].[SP_LOGIN_USER_2026]    Script Date: 30/05/2026 18:48:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:	<Noam Lavi, Nadav Meir, Ofek Cohen>
-- Create date: <27/05/2026>
-- Description:	<login a user to the Users_2026 Table>
-- =============================================
CREATE PROCEDURE [dbo].[SP_LOGIN_USER_2026] 
	-- Add the parameters for the stored procedure here
	@Email varchar(255),
    @Password varchar(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Users_2026 
    WHERE email = @Email AND password = @Password
END
GO


USE [igroup131_test2]
GO

/****** Object:  StoredProcedure [dbo].[SP_REMOVE_GAME_FROM_USER_2026]    Script Date: 30/05/2026 18:48:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_REMOVE_GAME_FROM_USER_2026]
    @UserId INT,
    @GameId INT
AS
BEGIN
    DELETE FROM UsersGames_2026 
    WHERE userId = @UserId AND gameId = @GameId;
END

GO


USE [igroup131_test2]
GO

/****** Object:  StoredProcedure [dbo].[SP_UPDATE_GAME_2026]    Script Date: 30/05/2026 18:48:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_UPDATE_GAME_2026]
    @Id int,
    @Name nvarchar(150),
    @SteamUrl varchar(500),
    @Image varchar(500),
    @ReleaseDate date,
    @ReviewSummary nvarchar(255),
    @Price decimal(10,2),
    @Windows bit,
    @Mac bit,
    @Linux bit
AS
BEGIN
    UPDATE Games_2026
    SET name = @Name,
        steamUrl = @SteamUrl,
        image = @Image,
        releaseDate = @ReleaseDate,
        reviewSummary = @ReviewSummary,
        price = @Price,
        windows = @Windows,
        mac = @Mac,
        linux = @Linux
    WHERE id = @Id;
END
GO


USE [igroup131_test2]
GO

/****** Object:  StoredProcedure [dbo].[SP_UPDATE_USER_2026]    Script Date: 30/05/2026 18:49:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:	<Noam Lavi, Nadav Meir, Ofek Cohen>
-- Create date: <27/05/2026>
-- Description:	<update a user in the Users_2026 Table>
-- =============================================
CREATE PROCEDURE [dbo].[SP_UPDATE_USER_2026] 
	-- Add the parameters for the stored procedure here
	@Id int,
	@Name nvarchar(30),
	@Password varchar(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Users_2026
	SET name = @Name,
		password = @Password
	WHERE id = @Id
END
GO

USE [igroup131_test2]
GO

/****** Object:  StoredProcedure [dbo].[SP_GET_GAMES_BY_TAG_2026]    Script Date: 30/05/2026 18:55:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_GET_GAMES_BY_TAG_2026]
    @TagsList NVARCHAR(MAX)
AS
BEGIN
    SELECT DISTINCT G.*
    FROM Games_2026 G
    INNER JOIN TagGame_2026 TG ON G.id = TG.gameId
    -- הפונקציה הזו מפרקת את הפסיקים ובודקת אם התגית נמצאת ברשימה ששלחנו!
    WHERE TG.TagName IN (SELECT value FROM STRING_SPLIT(@TagsList, ','))
END
GO


USE [igroup131_test2]
GO

/****** Object:  StoredProcedure [dbo].[SP_GET_RECOMMENDED_GAMES_2026]    Script Date: 30/05/2026 18:55:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_GET_RECOMMENDED_GAMES_2026]
    @UserId INT
AS
BEGIN
    -- שלב 1: מוצאים את כל התגיות הייחודיות מתוך המשחקים שיש למשתמש
    WITH UserTags AS (
        SELECT DISTINCT TG.TagName
        FROM TagGame_2026 TG
        INNER JOIN UsersGames_2026 UG ON TG.gameId = UG.gameId
        WHERE UG.userId = @UserId
    )
    -- שלב 2: שולפים משחקים שיש להם את התגיות האלו, אבל המשתמש לא קנה אותם עדיין
    SELECT DISTINCT G.*
    FROM Games_2026 G
    INNER JOIN TagGame_2026 TG ON G.id = TG.gameId
    INNER JOIN UserTags UT ON TG.TagName = UT.TagName
    WHERE G.id NOT IN (
        SELECT gameId FROM UsersGames_2026 WHERE userId = @UserId
    );
END
GO



