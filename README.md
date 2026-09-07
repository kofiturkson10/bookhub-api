[bookhub API – README-svenska.md](https://github.com/user-attachments/files/31904595/bookhub.API.README-svenska.md)[bookhub-api-README.md](https://github.com/user-attachments/files/31904395/bookhub-api-README.md)
# BookHub API

Backend-delen för **BookHub**, en fullstack CRUD-applikation som byggts som ett LIA-projekt (lärande i arbete) med fokus på molnutveckling. Detta repository innehåller .NET 9 Web API:t som tillhandahåller data för böcker och citat samt hanterar JWT-baserad autentisering.

Frontend-repository: [bookhub-client](https://github.com/kofiturkson10/bookhub-client)
Live-app: https://wonderful-island-02ad5290f.7.azurestaticapps.net

## Teknikstack

- **.NET 9** Web API (C#)
- **Entity Framework Core** för dataåtkomst och migreringar
- **SQL Server LocalDB** lokalt / **Azure SQL Database** i produktion
- **JWT**-autentisering (`System.IdentityModel.Tokens.Jwt`, `Microsoft.AspNetCore.Authentication.JwtBearer`), implementerad från grunden
- **PasswordHasher<T>** för lösenordshashning (valdes framför fullständig ASP.NET Core Identity för att hålla autentiseringslösningen enkel)

## Förutsättningar

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server LocalDB (installeras med Visual Studio eller via installationsprogrammet för SQL Server Express)
- EF Core Tools — antingen Package Manager Console-kommandona (inkluderas med Visual Studio) eller CLI-verktyget:
  ```bash
  dotnet tool install --global dotnet-ef
  ```

## Köra lokalt

1. **Klona och återställ**
   ```bash
   git clone https://github.com/kofiturkson10/bookhub-api.git
   cd bookhub-api
   dotnet restore
   ```

2. **Konfigurera hemligheter** (se [Konfiguration](#konfiguration) nedan) — ange JWT-signeringsnyckeln och, vid behov, anslutningssträngen.

3. **Tillämpa databasmigreringarna** för att skapa den lokala databasen. Använd det alternativ som passar ditt arbetsflöde:

   Package Manager Console (Visual Studio):
   ```powershell
   Update-Database
   ```

   .NET CLI:
   ```bash
   dotnet ef database update
   ```

4. **Kör API:t**
   ```bash
   dotnet run
   ```

   API:t startar som standard på `https://localhost:7000` (se `launchSettings.json`). Endpoints finns under `/api`, t.ex. `https://localhost:7000/api/books`.

## Konfiguration

Konfigurationsvärden läses från `appsettings.json`, medan hemligheter hålls utanför källkoden. Lokalt anges JWT-inställningarna via **user-secrets** (committa aldrig signeringsnycklar):

```json
{
  "Jwt": {
    "Key": "<en signeringsnyckel med minst 32 tecken>",
    "Issuer": "<utfärdare>",
    "Audience": "<målgrupp>"
  }
}
```

Den lokala standardanslutningssträngen pekar på LocalDB. Åsidosätt den i `appsettings.Development.json` eller user-secrets om namnet på din instans skiljer sig.

I produktion (Azure App Service) anges samma värden som **App Settings** med dubbla understreck som avgränsare, vilket motsvarar `Jwt:`-hierarkin:

```
Jwt__Key
Jwt__Issuer
Jwt__Audience
```

## API-översikt

Alla endpoints för böcker och citat kräver en giltig JWT (`Authorization: Bearer <token>`). Citat är kopplade till respektive användare.

| Metod  | Endpoint              | Beskrivning                    | Autentisering |
|--------|-----------------------|-------------------------------|------|
| POST   | `/api/auth/register`  | Registrera en ny användare    | Nej  |
| POST   | `/api/auth/login`     | Logga in, returnerar en JWT   | Nej  |
| GET    | `/api/books`          | Lista alla böcker             | Ja   |
| GET    | `/api/books/{id}`     | Hämta en enskild bok          | Ja   |
| POST   | `/api/books`          | Skapa en bok                  | Ja   |
| PUT    | `/api/books/{id}`     | Uppdatera en bok              | Ja   |
| DELETE | `/api/books/{id}`     | Ta bort en bok                | Ja   |
| GET    | `/api/quotes`         | Lista den aktuella användarens citat| Ja   |
| GET    | `/api/quotes/{id}`    | Hämta ett enskilt citat       | Ja   |
| POST   | `/api/quotes`         | Skapa ett citat               | Ja   |
| PUT    | `/api/quotes/{id}`    | Uppdatera ett citat           | Ja   |
| DELETE | `/api/quotes/{id}`    | Ta bort ett citat             | Ja   |

> Endpoint-routes återspeglar de nuvarande controllers; justera namnen på auth-routes ovan om dina skiljer sig.

## Deployment

API:t är deployat till **Azure App Service** via **GitHub Actions**. En push till `main` triggar workflow-filen i `.github/workflows/`, som bygger och publicerar applikationen och deployar den med hjälp av OIDC (federerade autentiseringsuppgifter) — utan långlivade publish-profile-hemligheter.

Produktion kräver App Settings `Jwt__Key`, `Jwt__Issuer` och `Jwt__Audience` (se [Konfiguration](#konfiguration)) samt en tillgänglig anslutningssträng till databasen.
