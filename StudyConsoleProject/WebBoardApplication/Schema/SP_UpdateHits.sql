-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		������
-- Create date: 2014/10/17
-- Description: �ش� �Խù��� ��ȸ���� ������Ʈ �մϴ�.
-- =============================================
CREATE PROCEDURE SP_UpdateHits
	-- Add the parameters for the stored procedure here
	@Id varchar(50)
AS
BEGIN
	UPDATE ARTICLE_INFORMATION SET HITS = HITS+1  WHERE ID = @Id
END
GO
