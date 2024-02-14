
using TCMS.Data.Models;

namespace TCMS.Services.Interfaces;

public interface IDrugTestService
{
    Task<IEnumerable<DrugAndAlcoholTest>> GetAllAsync();
    Task<DrugAndAlcoholTest> GetByIdAsync(int id);
    Task<DrugAndAlcoholTest> CreateAsync(DrugAndAlcoholTest drugAndAlcoholTest);
    Task<bool> UpdateAsync(DrugAndAlcoholTest drugAndAlcoholTest);
    Task<bool> DeleteAsync(int id);
}