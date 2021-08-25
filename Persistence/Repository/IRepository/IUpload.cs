using Microsoft.WindowsAzure.Storage.Blob;

namespace Persistence.Repository.IRepository
{
    public interface IUpload
    {
        CloudBlobContainer GetBlobContainer(string connectionString);
    }
}