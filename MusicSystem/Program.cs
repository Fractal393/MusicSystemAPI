using MusicSystem.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<AlbumService>()
    .AddSingleton<SongByNameService>()
    .AddSingleton<PlaylistService>()
    .AddSingleton<PreviousPurchasesService>()
    .AddSingleton<FilterService>()
    .AddSingleton<AddAlbumService>()
    .AddSingleton<DeleteAlbumService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

/*
 The customer needs to be able to 

Search songs by name
View Songs in an Album
View Songs in a Playlist
Filter search by Genre, Album, Artist
see their previous purchases

*/
