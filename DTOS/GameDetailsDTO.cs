namespace vidly.DTOS;

public record class GameDetailsDTO(
    int Id,
    string Name,
    int GenreId,
    decimal Price,
    DateOnly ReleaseDate
);
