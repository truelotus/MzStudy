USE [Article]
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertArticleComment]    Script Date: 10/24/2014 16:32:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_InsertArticleComment] 
	@Article_Id varchar(50),
	@Id varchar(50),
	@Contents varchar(max),
	--@Date varchar(50),
	@Writer varchar(50),
	@Password varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO ARTICLE_COMMENTS(ARTICLE_ID,ID,CONTENTS,WRITER,DATE,PASSWORD) 
	VALUES(@Article_Id,@Id,@Contents,@Writer,GETDATE(),@Password)
END
