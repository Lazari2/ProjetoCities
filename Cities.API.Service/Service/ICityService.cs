using Cities.Api.Infrastruture.Models;
using Cities.Api.Service.DTO;

namespace Cities.Api.Service.Service
{
    public interface ICityService
    {
        Task<ServiceResponse<List<CityDTO>>> GetCities();
        Task<ServiceResponse<List<CityDTO>>> PostCity(CityDTO newCity);
        Task<ServiceResponse<CityDTO>> GetCity(int Id);
        Task<ServiceResponse<List<CityDTO>>> PutCity(CityDTO updateCity);
        Task<ServiceResponse<List<CityDTO>>> DeleteCity(int Id);
    }
}