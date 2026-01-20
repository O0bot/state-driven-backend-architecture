using System.Collections.Concurrent;
using MyProjectMaybe.Domain;

namespace MyProjectMaybe.Infrastructure;

public class InMemoryUploadStore : IUploadStore
{
    private readonly ConcurrentDictionary<Guid, Upload> _uploads = new();

    public Upload Create()
    {
        var upload = new Upload
        {
            Id = Guid.NewGuid(),
            State = UploadState.Created,
            CreatedAt = DateTime.UtcNow
        };

        _uploads[upload.Id] = upload;
        return upload;
    }

    public Upload? Get(Guid id)
        => _uploads.TryGetValue(id, out var upload) ? upload : null;

    public IEnumerable<Upload> GetAll()
        => _uploads.Values;
}
