# MyPersonal — Software Engineer portfolio

A lightweight, fast, full-stack personal portfolio / business-card site for a
**Software Engineer**. One clean landing page that tells a potential client
who I am, what I've built, the technologies I use, and how to reach me.

Built to be simple, quick to load, and easy to keep up to date — all the content
lives in a SQL database and is served through a small API, so nothing is
hard-coded in the UI.

## Tech stack

| Layer      | Technology                                             |
|------------|--------------------------------------------------------|
| Backend    | **C# / ASP.NET Core** Minimal APIs (.NET 8)            |
| Database   | **SQL** via Entity Framework Core + **SQLite** (zero-config, file-based) |
| Frontend   | **Blazor WebAssembly** + **Bootstrap 5**               |
| Icons/Font | Bootstrap Icons, Inter (single font, 2–3 colour palette) |

The ASP.NET Core server also **hosts** the compiled Blazor WebAssembly client,
so the whole thing ships as a single deployable app.

## Architecture

```
MyPersonal.sln
├── src/Shared    # DTOs / domain models shared by client & server
├── src/Server    # ASP.NET Core API + EF Core (SQLite) + hosts the WASM client
│   ├── Data/AppDbContext.cs   # EF Core context
│   └── Data/SeedData.cs       # ← ALL site content is seeded here
└── src/Client    # Blazor WebAssembly single-page site
    ├── Pages/Home.razor        # the one landing page (all sections)
    ├── Components/ContactForm.razor
    └── wwwroot/css/app.css      # design system (theme, colours, layout)
```

### How it works

1. On startup the server creates the SQLite database and seeds it from
   `SeedData.cs` (only if empty).
2. The Blazor client loads and makes **one** request to `GET /api/site`, which
   returns everything the page needs (profile, services, skills, experience,
   projects, visit count).
3. The contact form posts to `POST /api/contact`, which validates and stores the
   message in SQL.
4. `POST /api/visit` bumps a simple visit counter shown in the hero.

### API endpoints

| Method | Route          | Purpose                                        |
|--------|----------------|------------------------------------------------|
| GET    | `/api/site`    | All page content in one payload                |
| POST   | `/api/visit`   | Increment + return the visit counter           |
| POST   | `/api/contact` | Validate & store a contact message             |

## Running locally

Requires the **.NET 8 SDK**.

```bash
dotnet run --project src/Server
```

Then open the URL printed in the console (e.g. `http://localhost:5296`).
The SQLite file `app.db` is created and seeded automatically on first run.

## Editing your content

All copy lives in **one place** — `src/Server/Data/SeedData.cs`. Edit the
profile, services, skills, experience and projects there. To re-apply changes:

```bash
rm src/Server/app.db     # drop the local database
dotnet run --project src/Server   # it will be recreated and reseeded
```

Leave any contact field blank and it will simply be hidden in the UI.

## Profile photo

The hero avatar uses `Profile.PhotoUrl` (set in `SeedData.cs`). Drop your photo at
`src/Client/wwwroot/img/nazar.jpg` (roughly square works best — it's cropped to a
rounded square). A placeholder is included; just overwrite that file with your own.
Set `PhotoUrl` to `""` to fall back to your initials instead of a photo.

## Contact-form email

When a visitor submits the contact form the message is stored in SQL **and**
emailed to you. SMTP settings live in the `Email` section of
`src/Server/appsettings.json` — everything except the **password**, which must be
supplied as a secret and never committed.

Using a Gmail account (recommended):

1. Enable **2-Step Verification** on the Google account.
2. Create an **App Password**: Google Account → Security → App passwords →
   generate one for "Mail". You get a 16-character code.
3. Provide it to the app as an environment variable (note the double underscore):

   ```bash
   export Email__Password="your-16-char-app-password"   # Linux/macOS
   dotnet run --project src/Server
   ```
   ```powershell
   $env:Email__Password="your-16-char-app-password"     # Windows PowerShell
   dotnet run --project src/Server
   ```

   For local development you can instead use user-secrets:
   ```bash
   cd src/Server
   dotnet user-secrets init
   dotnet user-secrets set "Email:Password" "your-16-char-app-password"
   ```

If no password is configured, the message is still saved and the site falls back
to opening the visitor's own email client — but nothing is delivered to your inbox
automatically until the App Password is set.

## Design

- **Palette:** neutral base + two accents (indigo `#6366f1` → cyan `#22d3ee`).
- **Font:** a single typeface (Inter) across the whole site.
- **Themes:** dark & light with a toggle; the choice is remembered in
  `localStorage` and applied before first paint (no flash).
- **Motion:** subtle reveal-on-scroll (disabled for `prefers-reduced-motion`,
  and content stays fully visible if JavaScript is unavailable).
- Fully responsive down to mobile.

## Building for production

```bash
dotnet publish src/Server -c Release -o ./publish
```

The `./publish` folder contains the self-contained server + client, ready to
deploy to any host that runs .NET 8 (Azure App Service, a Linux VM, a container,
etc.).
