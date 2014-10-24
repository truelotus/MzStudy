USE [Article]
GO
/****** Object:  StoredProcedure [dbo].[SP_SelectCountComments]    Script Date: 10/24/2014 16:34:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_SelectCountComments] 
	@Article_Id varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT COUNT(*) FROM ARTICLE_COMMENTS WHERE ARTICLE_ID = @Article_Id
END
