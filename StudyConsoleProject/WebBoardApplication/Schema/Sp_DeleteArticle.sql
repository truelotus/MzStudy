USE [Article]
GO
/****** Object:  StoredProcedure [dbo].[Sp_DeleteArticle]    Script Date: 10/24/2014 08:51:25 ******/
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
    DELETE FROM ARTICLE_COMMMENTS WHERE ARTICLE_ID = @Id
	DELETE FROM ARTICLE WHERE ID = @Id
END
