using TCMS.Data.Models;

namespace TCMS.Services.Interfaces;

public interface IManifestService
{
    Task<IEnumerable<Manifest>> GetAllManifestsAsync();
    Task<Manifest> GetManifestByIdAsync(int id);
    Task<Manifest> CreateManifestAsync(Manifest manifest);
    Task<bool> UpdateManifestAsync(Manifest manifest);
    Task<bool> DeleteManifestAsync(int id);


}