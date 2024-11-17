using System;
using Microsoft.EntityFrameworkCore;
using vidly.Data;
using vidly.DTOS;
using vidly.Entities;
using vidly.Mapping;

namespace vidly.EndPoints;

public static class GamesEndPoints
{
    // private readonly static List<GameSummaryDTO> games = [
    //     new (
    //     1,
    //     "Street Fighter II",
    //     "Fighting" ,
    //     19.99M,
    //     new DateOnly(1992, 7, 15)),
    // new (
    //     2,
    //     "Final Fantasy XIV",
    //     "Roleplaying",
    //     59.99M,
    //     new DateOnly(2010, 9,30)),
    // new (
    //     3,
    //     "FIFA 23",
    //     "Sports" ,
    //     69.99M,
    //     new DateOnly(2022,9,27))
    // ];

    public static WebApplication MapGamesEndPoints(this WebApplication app)
    {
        app.MapGet("games", (GameStoreContext dbContext) =>
            dbContext.Games
            .Include(game => game.Genre)
            .Select(game => game.ToGameSummaryDTO())
            .AsNoTracking()
        );

        app.MapGet("/games/{id:int}", (int id, GameStoreContext dbContext) =>
        {

            // GameDTO? game = games.Find(game => game.Id == id);
            Game? game = dbContext.Games.Find(id);

            return game is not null ? Results.Ok(game.ToGameDetailsDTO()) : Results.NotFound();

        }).WithName("GetGame");

        app.MapPost("games", (CreateGameDTO newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();
            // game.Genre = dbContext.Genres.Find(newGame.GenreId);

            // Game game = new()
            // {
            //     Name = newGame.Name,
            //     Genre = dbContext.Genres.Find(newGame.GenreId),
            //     GenreId = newGame.GenreId,
            //     Price = newGame.Price,
            //     ReleaseDate = newGame.ReleaseDate
            // };

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            // GameDTO gameDTO = new(
            //     game.Id,
            //     game.Name,
            //     game.Genre!.Name,
            //     game.Price,
            //     game.ReleaseDate
            // );

            return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game.ToGameDetailsDTO());
        }).WithParameterValidation();

        app.MapPut("games/{id:int}", (int id, UpdateGameDTO updatedGame, GameStoreContext dbContext) =>
        {
            // var index = games.FindIndex(game => game.Id == id);
            var existingGame = dbContext.Games.Find(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            // games[index] = new GameSummaryDTO(
            //     id,
            //     updatedGame.Name,
            //     updatedGame.Genre,
            //     updatedGame.Price,
            //     updatedGame.ReleaseDate
            // );

            dbContext.Entry(existingGame).CurrentValues.SetValues(updatedGame.ToEntity(id));
            dbContext.SaveChanges();

            return Results.NoContent();
        }).WithParameterValidation();

        app.MapDelete("games/{id:int}", (int id, GameStoreContext dbContext) =>
        {

            dbContext.Games
            .Where(game => game.Id == id)
            .ExecuteDelete();

            return Results.NoContent();
            // int removedCount = games.RemoveAll(game => game.Id == id);

            // if (removedCount > 0)
            // {
            //     return Results.NoContent();
            // }
            // else
            // {
            //     return Results.NotFound("Game not found");
            // }
        });

        return app;
    }
}
