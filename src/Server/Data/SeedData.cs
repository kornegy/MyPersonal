using MyPersonal.Shared;

namespace MyPersonal.Server.Data;

/// <summary>
/// Initial content for the site. This is the ONE place to edit copy —
/// change the values below, delete app.db, and restart to reseed.
/// </summary>
public static class SeedData
{
    public static void EnsureSeeded(AppDbContext db)
    {
        db.Database.EnsureCreated();

        if (!db.Stats.Any(s => s.Key == "visits"))
            db.Stats.Add(new SiteStat { Key = "visits", Value = 0 });

        if (!db.Profiles.Any())
        {
            db.Profiles.Add(new Profile
            {
                FullName = "Nazar Koval",
                Title = "Junior Software Engineer",
                Tagline = "3rd-year Informatics student building full-stack .NET applications — bringing clean code, fast learning and fresh energy to a team.",
                About =
                    "I am an active 3rd-year undergraduate student of Informatics at VŠB – Technical University of Ostrava. " +
                    "My expertise includes programming with Python, C# (ASP.NET), PL/SQL and relational databases. " +
                    "I also work confidently with modern AI tools — using LLMs and AI assistants to write code faster, " +
                    "solve problems and build AI-powered features into applications. " +
                    "During my studies I have developed several projects, which you can explore below. " +
                    "I am a quick learner, responsible, and enjoy working in a team. Currently I am looking to apply my " +
                    "skills in a practical setting — through an internship, traineeship, part-time position, or other collaboration.",
                Location = "Ostrava, Czech Republic",
                PhotoUrl = "img/nazar.jpg",
                AvailabilityNote = "Open to internships, traineeships & junior roles",
                Email = "nazark.2810@gmail.com",
                Phone = "+420 703 031 998",
                GitHubUrl = "https://github.com/kornegy",
                LinkedInUrl = "https://www.linkedin.com/in/nazar-koval-a2979335a/",
                TelegramUrl = "https://t.me/kornegyNN",
                ResumeUrl = "files/Nazar_Koval_CV.pdf",
            });
        }

        if (!db.Services.Any())
        {
            db.Services.AddRange(
                new ServiceOffering { SortOrder = 1, Icon = "bi-window-stack",
                    Title = "Full-Stack .NET Web Apps",
                    Description = "End-to-end web applications on ASP.NET Core MVC and Blazor WebAssembly — dynamic UI, authentication, and admin panels." },
                new ServiceOffering { SortOrder = 2, Icon = "bi-hdd-network",
                    Title = "REST APIs & Backend",
                    Description = "Clean, well-structured REST APIs with .NET 8, Entity Framework Core, external API integration and structured logging (Serilog)." },
                new ServiceOffering { SortOrder = 3, Icon = "bi-database",
                    Title = "Databases & SQL",
                    Description = "Relational database design and PL/SQL — normalized schemas, integrity constraints and ER modelling in MS SQL Server and Oracle." },
                new ServiceOffering { SortOrder = 4, Icon = "bi-code-square",
                    Title = "Desktop & Scripting",
                    Description = "GUI desktop tools in Python (Tkinter) and low-level programming in C — solid fundamentals in logic and algorithms." }
            );
        }

        if (!db.Skills.Any())
        {
            db.Skills.AddRange(
                new SkillCategory { SortOrder = 1, Name = "Programming",
                    Items = "C#, Python, PL/SQL, SQL, C" },
                new SkillCategory { SortOrder = 2, Name = "Backend",
                    Items = "ASP.NET Core MVC, ASP.NET Core Web API, .NET 8, Entity Framework Core, REST API, Serilog, Razor" },
                new SkillCategory { SortOrder = 3, Name = "Frontend",
                    Items = "Blazor WebAssembly, Bootstrap, HTML, CSS" },
                new SkillCategory { SortOrder = 4, Name = "Databases",
                    Items = "MS SQL Server, Oracle, PL/SQL, Relational Design, ER Diagrams" },
                new SkillCategory { SortOrder = 5, Name = "AI & Tools",
                    Items = "LLMs, Prompt Engineering, AI-Assisted Development, Git, GitHub, OOP, Software Architecture" },
                new SkillCategory { SortOrder = 6, Name = "Spoken Languages",
                    Items = "English (B2), Czech (B2), Russian (C2), Ukrainian (C2), French (A1)" }
            );
        }

        if (!db.Experience.Any())
        {
            db.Experience.AddRange(
                new ExperienceItem { SortOrder = 1,
                    Role = "BSc in Informatics",
                    Company = "VŠB – Technical University of Ostrava",
                    Period = "2023 — Present",
                    Summary = "Third-year undergraduate focused on software engineering, databases and full-stack development.",
                    Highlights =
                        "Hands-on coursework in C#, Python, PL/SQL and relational databases.\n" +
                        "Built multiple full-stack and database projects (see below).\n" +
                        "Team-based, project-driven learning with an emphasis on clean architecture." },
                new ExperienceItem { SortOrder = 2,
                    Role = "Czech Language Courses",
                    Company = "VŠB – Technical University of Ostrava",
                    Period = "2023",
                    Summary = "Intensive preparatory Czech-language programme.",
                    Highlights = "Reached B2 proficiency in Czech." }
            );
        }

        if (!db.Projects.Any())
        {
            db.Projects.AddRange(
                new Project { SortOrder = 1, Featured = true,
                    Title = "E-Commerce Web System",
                    Description = "Web system built on ASP.NET Core MVC with a dynamic catalog, an interactive ordering process, a full administrative panel and user authentication.",
                    TechStack = "C#, ASP.NET Core MVC, Razor, Bootstrap, JavaScript, JSON",
                    RepoUrl = "https://github.com/kornegy" },
                new Project { SortOrder = 2, Featured = true,
                    Title = "Employee Record System",
                    Description = "ASP.NET Core Web API + Blazor WebAssembly app with JSON data import, automatic relational mapping, IP-based geolocation and advanced Serilog logging.",
                    TechStack = "C#, .NET 8, Blazor WASM, EF Core, REST API, Serilog, MS SQL Server",
                    RepoUrl = "https://github.com/kornegy" },
                new Project { SortOrder = 3, Featured = true,
                    Title = "Inventory Manager (Desktop)",
                    Description = "Python/Tkinter desktop application for simple inventory management, with a graphical interface and application logic beyond the console.",
                    TechStack = "Python, Tkinter, GUI",
                    RepoUrl = "https://github.com/kornegy" },
                new Project { SortOrder = 4, Featured = false,
                    Title = "Tournament Database",
                    Description = "Relational database for sports-tournament management — players, teams, matches, tournaments, referees and locations — with normalization and integrity constraints.",
                    TechStack = "MS SQL Server, Oracle Data Modeler, ER Diagrams",
                    RepoUrl = "https://github.com/kornegy" },
                new Project { SortOrder = 5, Featured = false,
                    Title = "Console Game in C",
                    Description = "A small Bulánci-inspired console game written in C, focused on core game logic, loops, conditionals and user input.",
                    TechStack = "C, Algorithms, Console",
                    RepoUrl = "https://github.com/kornegy" }
            );
        }

        db.SaveChanges();
    }
}
