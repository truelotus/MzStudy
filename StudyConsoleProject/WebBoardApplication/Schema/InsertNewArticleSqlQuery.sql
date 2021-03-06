USE [Article]
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertNewArticle]    Script Date: 10/15/2014 18:49:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_InsertNewArticle]
	-- Add the parameters for the stored procedure here
	@Id int,
	@No int,
	@Title varchar(50),
	@Contents varchar(max),
	@Date varchar(50),
	@Writer varchar(50),
	@Password varchar(50),
	@Hits int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO ARTICLE_INFO (ID,NO,TITLE,CONTENTS,WRITER,DATE,PASSWORD,HITS) 
	VALUES(@Id,@No,@Title,@Contents,@Writer,@Date,@Password,@Hits)
END
