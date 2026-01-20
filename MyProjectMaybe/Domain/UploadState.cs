namespace MyProjectMaybe.Domain;

public enum UploadState
{
    Created,
    Validating,
    Validated,
    Processing,
    Completed,
    Failed
}