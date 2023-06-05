using AutoMapper;
namespace SavourySolutions.Services.Mapping;

public interface IHaveCustomMappings
{
    void CreateMappings(IProfileExpression configuration);
}
