    using Cities.Api.Infrastruture.Models;
using Cities.Api.Service.DTO;
using Microsoft.EntityFrameworkCore;

namespace Cities.Api.Service.Service
{
    public class CityService : ICityService
    {
        private readonly CityContext _context;

        public CityService(CityContext cityContext)
        {
            _context = cityContext;
        }

        public async Task<ServiceResponse<List<CityDTO>>> DeleteCity(int id)
        {
            ServiceResponse<List<CityDTO>> serviceResponse = new ServiceResponse<List<CityDTO>>();

            try
            {
                City city = await _context.Cities
                            .FirstOrDefaultAsync(x => x.Id == id);

                if (city == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Message = "City Not Found";
                    serviceResponse.Success = false;
                }
                else
                {
                    _context.Cities.Remove(city);
                    await _context.SaveChangesAsync();

                    serviceResponse.Dados = _context.Cities
                        .Select(c => new CityDTO
                        {
                            Id = c.Id,
                            Name = c.Name,
                            State = c.State
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }


        public async Task<ServiceResponse<List<CityDTO>>> GetCities()
        {
            ServiceResponse<List<CityDTO>> serviceResponse = new ServiceResponse<List<CityDTO>>();

            try
            {
                List<CityDTO> citiesDTO = await _context.Cities
                    .Select(city => new CityDTO
                    {
                        Id = city.Id,
                        Name = city.Name,
                        State = city.State
                    })
                    .ToListAsync();

                serviceResponse.Dados = citiesDTO;

                if (serviceResponse.Dados.Count == 0)
                {
                    serviceResponse.Message = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<CityDTO>> GetCity(int id)
        {
            ServiceResponse<CityDTO> serviceResponse = new ServiceResponse<CityDTO>();

            try
            {
             CityDTO city = await _context.Cities
                            .Where(x => x.Id == id)
                            .Select(c => new CityDTO
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    State = c.State
                                })
                            .FirstOrDefaultAsync();

                if (city == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Message = "City not Found";
                    serviceResponse.Success = false;
                }
                else
                {
                    serviceResponse.Dados = city;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<CityDTO>>> PostCity(CityDTO newCity)
        {
            ServiceResponse<List<CityDTO>> serviceResponse = new ServiceResponse<List<CityDTO>>();

            try
            {
                if (newCity == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Message = "Invalid data";
                    serviceResponse.Success = false;
                    return serviceResponse;
                }

                if (_context.Cities.Any(c => c.Id == newCity.Id))
                {
                    serviceResponse.Message = $"ID {newCity.Id} already exists";
                    serviceResponse.Success = false;
                    return serviceResponse;
                }

                _context.Add(new City
                {
                    Id = newCity.Id,
                    Name = newCity.Name,
                    State = newCity.State   
                });

                await _context.SaveChangesAsync();

                serviceResponse.Dados = _context.Cities
                    .Select(c => new CityDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        State = c.State
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<CityDTO>>> PutCity(CityDTO updateCity)
        {
            ServiceResponse<List<CityDTO>> serviceResponse = new ServiceResponse<List<CityDTO>>();

            try
            {
                City city = await _context.Cities
                    .Where(x => x.Id == updateCity.Id)
                    .FirstOrDefaultAsync();

                if (city == null)
                {
                    serviceResponse.Dados = null;
                    serviceResponse.Message = "City not found";
                    serviceResponse.Success = false;
                }
                else
                {
                    city.Name = updateCity.Name;
                    city.State = updateCity.State;

                    await _context.SaveChangesAsync();

                    serviceResponse.Dados = _context.Cities
                        .Select(c => new CityDTO
                        {
                            Id = c.Id,
                            Name = c.Name,
                            State = c.State
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Message = ex.Message;
                serviceResponse.Success = false;
            }

            return serviceResponse;
        }

        
    }
}
