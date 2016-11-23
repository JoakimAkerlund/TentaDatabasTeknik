CREATE PROCEDURE [dbo].[AddTerritory](@TerritoryID int,@TerritoryDescription nchar(50), @RegionID int)

AS
BEGIN
INSERT INTO dbo.Territories
(TerritoryID, TerritoryDescription,RegionID)
Values
(@TerritoryID,@TerritoryDescription,@RegionID)
END









