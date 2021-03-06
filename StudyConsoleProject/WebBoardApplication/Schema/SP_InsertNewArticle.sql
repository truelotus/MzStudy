USE [Article]
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertNewArticle]    Script Date: 10/24/2014 15:01:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		고윤하
-- Create date: 2014/10/24
-- Description:	새 게시물을 추가 합니다.
-- =============================================
ALTER PROCEDURE [dbo].[SP_InsertNewArticle]
	-- Add the parameters for the stored procedure here
	@Id varchar(50),
	--@No int,
	@Title varchar(50),
	@Contents varchar(max),
	--@Date datetime,
	@Writer varchar(50),
	@Password varchar(50)
	--@Hits int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO ARTICLE(ID,TITLE,CONTENTS,WRITER,DATE,PASSWORD) 
	VALUES(@Id,@Title,@Contents,@Writer,GETDATE(),@Password)
	
	DECLARE @IDX INT
	SET @IDX = @@IDENTITY
	 
END