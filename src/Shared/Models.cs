using System.ComponentModel.DataAnnotations;

namespace MyPersonal.Shared;

/// <summary>
/// Core identity and contact information. A single row lives in the database
/// so all copy can be edited without redeploying the client.
/// </summary>
public class Profile
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Title { get; set; } = "";
    public string Tagline { get; set; } = "";
    public string About { get; set; } = "";
    public string Location { get; set; } = "";
    public string AvailabilityNote { get; set; } = "";

    // Contact links — leave any blank to hide it in the UI.
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public string GitHubUrl { get; set; } = "";
    public string LinkedInUrl { get; set; } = "";
    public string TelegramUrl { get; set; } = "";
    /// <summary>Relative URL to the downloadable résumé (PDF). Blank hides the button.</summary>
    public string ResumeUrl { get; set; } = "";
    /// <summary>Relative URL to the profile photo. Blank falls back to initials.</summary>
    public string PhotoUrl { get; set; } = "";
}

/// <summary>A featured portfolio project.</summary>
public class Project
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    /// <summary>Comma-separated technologies, e.g. "C#, ASP.NET, PyTorch".</summary>
    public string TechStack { get; set; } = "";
    public string RepoUrl { get; set; } = "";
    public string LiveUrl { get; set; } = "";
    public bool Featured { get; set; }
    public int SortOrder { get; set; }

    public IEnumerable<string> Technologies() =>
        TechStack.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}

/// <summary>A single role in the work-experience timeline.</summary>
public class ExperienceItem
{
    public int Id { get; set; }
    public string Role { get; set; } = "";
    public string Company { get; set; } = "";
    public string Period { get; set; } = "";
    public string Summary { get; set; } = "";
    /// <summary>Newline-separated achievement bullets.</summary>
    public string Highlights { get; set; } = "";
    public int SortOrder { get; set; }

    public IEnumerable<string> HighlightLines() =>
        Highlights.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}

/// <summary>A group of related technologies shown in the skills grid.</summary>
public class SkillCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    /// <summary>Comma-separated skill names.</summary>
    public string Items { get; set; } = "";
    public int SortOrder { get; set; }

    public IEnumerable<string> ItemList() =>
        Items.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}

/// <summary>Something concrete the engineer offers a client.</summary>
public class ServiceOffering
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    /// <summary>Bootstrap-Icons class name, e.g. "bi-cpu".</summary>
    public string Icon { get; set; } = "bi-stars";
    public int SortOrder { get; set; }
}

/// <summary>A message submitted through the contact form and stored in SQL.</summary>
public class ContactMessage
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Name { get; set; } = "";

    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = "";

    [Required, StringLength(4000, MinimumLength = 5)]
    public string Message { get; set; } = "";

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Single aggregate payload the client fetches on load, so the whole page
/// renders from one round-trip.
/// </summary>
public class SiteData
{
    public Profile Profile { get; set; } = new();
    public List<ServiceOffering> Services { get; set; } = new();
    public List<SkillCategory> Skills { get; set; } = new();
    public List<ExperienceItem> Experience { get; set; } = new();
    public List<Project> Projects { get; set; } = new();
    public long Visits { get; set; }
}
