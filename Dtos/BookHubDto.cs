namespace bookhub_api.Dtos;

public record BookResponse(int id, string Title, string Author, DateOnly PublishedDate);

public record CreateBookRequest(string Title, string Author, DateOnly PublishedDate);

public record UpdateBookRequest(string Title, string Author, DateOnly PublishedDate);

public record RegisterRequest(string Username, string Password);

public record UserResponse(int id, string Username);

public record LoginRequest(string Username, string Password);

public record TokenResponse(string Token);

public record QuoteResponse(int id, string Text, string Author, DateTime CreatedAt);

public record CreateQuoteRequest(string Text, string Author);

public record UpdateQuoteRequest(string Text, string Author);
