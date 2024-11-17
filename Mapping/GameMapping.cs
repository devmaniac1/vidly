using System;
using vidly.DTOS;
using vidly.Entities;

namespace vidly.Mapping;

public static class GameMapping
{
    public static Game ToEntity(this CreateGameDTO game)
    {
        return new()
        {
            Name = game.Name,
            // Genre = dbContext.Genres.Find(game.GenreId),
            GenreId = game.GenreId,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        };
    }
    public static Game ToEntity(this UpdateGameDTO game, int id)
    {
        return new()
        {
            Id = id,
            Name = game.Name,
            // Genre = dbContext.Genres.Find(game.GenreId),
            GenreId = game.GenreId,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        };
    }

    public static GameSummaryDTO ToGameSummaryDTO(this Game game)
    {
        return new(
            game.Id,
            game.Name,
            game.Genre!.Name,
            game.Price,
            game.ReleaseDate
        );
    }
    public static GameDetailsDTO ToGameDetailsDTO(this Game game)
    {
        return new(
            game.Id,
            game.Name,
            game.GenreId,
            game.Price,
            game.ReleaseDate
        );
    }
}
