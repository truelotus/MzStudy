USE [Article]
GO
/****** Object:  StoredProcedure [dbo].[SP_SelectArticle]    Script Date: 10/16/2014 08:47:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_SelectArticle] 
	-- Add the parameters for the stored procedure here
	@Id varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM ARTICLE_INFO WHERE ID = @Id
END
