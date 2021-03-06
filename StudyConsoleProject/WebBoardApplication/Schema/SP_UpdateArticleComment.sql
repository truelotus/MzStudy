USE [Article]
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateArticleComment]    Script Date: 10/24/2014 16:35:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_UpdateArticleComment]
	@Id varchar(50),
	@Contents varchar(max),
	@Writer varchar(50),
	@Password varchar(50)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE ARTICLE_COMMENTS SET CONTENTS = @Contents, WRITER = @Writer, PASSWORD = @Password where ID = @Id
END
