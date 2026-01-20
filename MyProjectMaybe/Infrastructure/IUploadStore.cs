using MyProjectMaybe.Domain;

namespace MyProjectMaybe.Infrastructure
{
    public interface IUploadStore
    {
        Upload Create();
        Upload? Get(Guid id);
        IEnumerable<Upload> GetAll();
    }
}
