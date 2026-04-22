namespace SmartHireAI.Backend.Entities;

public class Candidate
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Resume> Resumes { get; set; } = new List<Resume>();
}
