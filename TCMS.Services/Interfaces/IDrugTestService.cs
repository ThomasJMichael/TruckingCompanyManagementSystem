
using TCMS.Common.DTOs.DrugAndAlcohol;
using TCMS.Data.Models;

namespace TCMS.Services.Interfaces;

public interface IDrugTestService
{
    Task<IEnumerable<DrugAndAlcoholTestDto>> GetAllAsync();
    Task<DrugAndAlcoholTestDto> GetByIdAsync(int id);
    Task<DrugAndAlcoholTestDto> CreateAsync(DrugAndAlcoholTest drugAndAlcoholTest);
    Task<bool> UpdateAsync(DrugAndAlcoholTestDto drugAndAlcoholTest);
    Task<bool> DeleteAsync(int id);
}