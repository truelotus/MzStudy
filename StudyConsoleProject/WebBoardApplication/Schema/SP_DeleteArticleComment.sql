USE [Article]
GO
/****** Object:  StoredProcedure [dbo].[SP_DeleteArticleComment]    Script Date: 10/24/2014 16:32:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_DeleteArticleComment]
	@Id varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   DELETE FROM ARTICLE_COMMENTS WHERE ID = @Id
END
