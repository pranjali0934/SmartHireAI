using SmartHireAI.Backend.DTOs;
using SmartHireAI.Backend.Entities;
using SmartHireAI.Backend.Interfaces;

namespace SmartHireAI.Backend.Services;

public class ResumeService
{
    private readonly IResumeRepository _resumeRepository;
    private readonly ICandidateRepository _candidateRepository;
    private readonly IDocumentIntelligenceService _documentIntelligence;

    public ResumeService(
        IResumeRepository resumeRepository,
        ICandidateRepository candidateRepository,
        IDocumentIntelligenceService documentIntelligence)
    {
        _resumeRepository = resumeRepository;
        _candidateRepository = candidateRepository;
        _documentIntelligence = documentIntelligence;
    }

    public async Task<ResumeUploadResponse> UploadResumeAsync(
        Stream fileStream,
        string fileName,
        string candidateName,
        string candidateEmail)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        if (extension != ".pdf" && extension != ".docx" && extension != ".txt")
        {
            throw new ArgumentException("Only PDF, DOCX, and TXT files are supported.");
        }

        var candidate = await _candidateRepository.GetByEmailAsync(candidateEmail)
            ?? await _candidateRepository.AddAsync(new Candidate
            {
                Name = candidateName,
                Email = candidateEmail
            });

        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Resumes");
        Directory.CreateDirectory(uploadsPath);
        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsPath, uniqueFileName);

        await using (var fileStreamCopy = File.Create(filePath))
        {
            fileStream.Position = 0;
            await fileStream.CopyToAsync(fileStreamCopy);
        }

        fileStream.Position = 0;
        var extractedText = await _documentIntelligence.ExtractTextFromDocumentAsync(fileStream, fileName);

        var resume = await _resumeRepository.AddAsync(new Resume
        {
            CandidateId = candidate.Id,
            FileName = fileName,
            FilePath = filePath,
            ExtractedText = extractedText
        });

        return new ResumeUploadResponse
        {
            ResumeId = resume.Id,
            CandidateId = candidate.Id,
            FileName = resume.FileName,
            ExtractedText = resume.ExtractedText,
            UploadedAt = resume.UploadedAt
        };
    }

    public async Task<Resume?> GetByIdAsync(int id) =>
        await _resumeRepository.GetByIdAsync(id);

    public async Task<IEnumerable<ResumeListItemDto>> GetAllAsync()
    {
        var resumes = await _resumeRepository.GetAllAsync();
        return resumes.Select(r => new ResumeListItemDto
        {
            Id = r.Id,
            CandidateId = r.CandidateId,
            CandidateName = r.Candidate.Name,
            FileName = r.FileName,
            UploadedAt = r.UploadedAt
        });
    }
}
