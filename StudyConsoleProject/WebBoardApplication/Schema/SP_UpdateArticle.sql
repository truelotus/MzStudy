USE [Article]
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateArticle]    Script Date: 10/24/2014 10:00:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_UpdateArticle] 
	-- Add the parameters for the stored procedure here
	@Id varchar(50),
	@Title varchar(50),
	@Contents varchar(max),
	@Writer varchar(50),
	@Password varchar(50)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	UPDATE ARTICLE SET TITLE= @Title, CONTENTS = @Contents, WRITER = @Writer, PASSWORD = @Password where ID = @Id
END
