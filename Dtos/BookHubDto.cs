namespace bookhub_api.Dtos;

public record BookResponse(int id, string Title, string Author, DateOnly PublishedDate);

public record CreateBookRequest(string Title, string Author, DateOnly PublishedDate);

public record UpdateBookRequest(string Title, string Author, DateOnly PublishedDate);
