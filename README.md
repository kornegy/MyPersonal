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

Fields marked `// TODO:` in `SeedData.cs` are placeholders (name, phone,
LinkedIn / Telegram links, real project repos) — replace them with your details.
Leave any contact field blank and it will simply be hidden in the UI.

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
