namespace MyProjectMaybe.Domain;

public class Upload
{
    public Guid Id { get; init; }
    public UploadState State { get; set; }
    public DateTime CreatedAt { get; init; }
}
