USE [Article]
GO
/****** Object:  StoredProcedure [dbo].[SP_SelectArticleAllComment]    Script Date: 10/24/2014 16:34:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_SelectArticleAllComment] 
	@Article_id varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT * FROM ARTICLE_COMMENTS where ARTICLE_ID = @Article_id ORDER BY DATE DESC
END
