USE [Article]
GO
/****** Object:  StoredProcedure [dbo].[SP_SelectCountArticles]    Script Date: 10/19/2014 19:29:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SP_SelectCountArticles]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT COUNT(*) FROM ARTICLE_INFORMATION
END
