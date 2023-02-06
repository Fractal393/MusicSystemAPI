select * from album order by AlbumId offset 350 Rows fetch next 10 rows only;
select * from PlaylistTrack where PlaylistId = 4;
DECLARE @sql NVARCHAR(MAX) = 'SELECT Album.ArtistId, Album.AlbumId, Track.GenreId, Track.Name\r\nFROM Track\r\nJOIN Album ON Track.AlbumId = Album.AlbumId\r\nWHERE 1=1'\r\n\r\nIF (@albumId IS NOT NULL)\r\nBEGIN\r\n    SET @sql = @sql + ' AND Album.AlbumId = @albumId'\r\nEND\r\n\r\nIF (@artistId IS NOT NULL)\r\nBEGIN\r\n    SET @sql = @sql + ' AND Album.ArtistId = @artistId'\r\nEND\r\n\r\nIF (@genreId IS NOT NULL)\r\nBEGIN\r\n    SET @sql = @sql + ' AND Track.GenreId = @genreId'\r\nEND\r\n\r\nDECLARE @params NVARCHAR(MAX) = N'@albumId INT, @artistId INT, @genreId INT'\r\nDECLARE @albumIdParam INT = COALESCE(@albumId, NULL)\r\nDECLARE @artistIdParam INT = COALESCE(@artistId, NULL)\r\nDECLARE @genreIdParam INT = COALESCE(@genreId, NULL)\r\n\r\nEXEC sp_executesql @sql, @params, @albumIdParam, @artistIdParam, @genreIdParam;\r\n
DECLARE @sql NVARCHAR(MAX) = 'SELECT Album.ArtistId, Album.AlbumId, Track.GenreId, Track.Name, PlaylistId
FROM Track
JOIN Album ON Track.AlbumId = Album.AlbumId
WHERE 1=1'

IF (@albumId IS NOT NULL)
BEGIN
    SET @sql = @sql + ' AND Album.AlbumId = @albumId'
END

IF (@artistId IS NOT NULL)
BEGIN
    SET @sql = @sql + ' AND Album.ArtistId = @artistId'
END

IF (@genreId IS NOT NULL)
BEGIN
    SET @sql = @sql + ' AND Track.GenreId = @genreId'
END

IF (@playlistId IS NOT NULL)
BEGIN
    SET @sql = @sql + ' AND Track.PlaylistId = @playlistId'
END

DECLARE @params NVARCHAR(MAX) = N'@albumId INT, @artistId INT, @genreId INT, @playlistId'
DECLARE @albumIdParam INT = COALESCE(@albumId, NULL)
DECLARE @artistIdParam INT = COALESCE(@artistId, NULL)
DECLARE @genreIdParam INT = COALESCE(@genreId, NULL)
DECLARE @playlistIdParam INT = COALESCE(@playlistId, NULL)

EXEC sp_executesql @sql, @params, @albumIdParam, @artistIdParam, @genreIdParam, @playlistIdParam;
