USE [Article]
GO
/****** Object:  StoredProcedure [dbo].[Sp_DeleteArticle]    Script Date: 10/20/2014 15:19:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[Sp_DeleteArticle]
	-- Add the parameters for the stored procedure here
	@Id varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    DELETE FROM ARTICLE_COMMENT WHERE ARTICLE_ID = @Id
	DELETE FROM ARTICLE_INFORMATION WHERE ID = @Id
END
