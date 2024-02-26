using Microsoft.AspNetCore.Mvc;
using Cities.Api.Infrastruture.Models;
using Cities.Api.Service.Service;
using CitiesAPI.Application.ViewModel;
using Cities.Api.Service.DTO;


namespace CitiesAPI.Application.Controllers
{
    [Route("api/CitiesController")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICityService _cityService;
        public CitiesController(ICityService cityInterface)
        {
            _cityService = cityInterface;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<CityViewModel>>>> GetCities()
        {
            ServiceResponse<List<CityDTO>> serviceResponse = await _cityService.GetCities();
            List<CityViewModel> citiesViewModel = serviceResponse.Dados
                .Select(cityDTO => new CityViewModel
                {
                    Id = cityDTO.Id,
                    Name = cityDTO.Name,
                    State = cityDTO.State
                })
                .ToList();

            return Ok(new ServiceResponse<List<CityViewModel>>
            {
                Dados = citiesViewModel,
                Message = serviceResponse.Message,
                Success = serviceResponse.Success
            });
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<CityViewModel>>>> PostCity(CityViewModel newCity)
        {
            CityDTO newCityDTO = new CityDTO
            {
                Id = newCity.Id,
                Name = newCity.Name,
                State = newCity.State
            };

            ServiceResponse<List<CityDTO>> serviceResponse = await _cityService.PostCity(newCityDTO);

            List<CityViewModel> citiesViewModel = serviceResponse.Dados
                .Select(cityDTO => new CityViewModel
                {
                    Id = cityDTO.Id,
                    Name = cityDTO.Name,
                    State = cityDTO.State
                })
                .ToList();

            return Ok(new ServiceResponse<List<CityViewModel>>
            {
                Dados = citiesViewModel,
                Message = serviceResponse.Message,
                Success = serviceResponse.Success
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<CityViewModel>>>> DeleteCity(int id)
        {
            ServiceResponse<List<CityDTO>> serviceResponse = await _cityService.DeleteCity(id);

            List<CityViewModel> citiesViewModel = serviceResponse.Dados
                .Select(cityDTO => new CityViewModel
                {
                    Id = cityDTO.Id,
                    Name = cityDTO.Name,
                    State = cityDTO.State   
                })
                .ToList();

            return Ok(new ServiceResponse<List<CityViewModel>>
            {
                Dados = citiesViewModel,
                Message = serviceResponse.Message,
                Success = serviceResponse.Success
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<CityViewModel>>> GetCity(int id)
        {
            ServiceResponse<CityDTO> serviceResponse = await _cityService.GetCity(id);

            if (!serviceResponse.Success)
            {
                return BadRequest(new ServiceResponse<CityViewModel>
                {
                    Message = serviceResponse.Message,
                    Success = false
                });
            }

            CityViewModel cityViewModel = new CityViewModel
            {
                Id = serviceResponse.Dados.Id,
                Name = serviceResponse.Dados.Name,
                State = serviceResponse.Dados.State
            };

            return Ok(new ServiceResponse<CityViewModel>
            {
                Dados = cityViewModel,
                Message = serviceResponse.Message,
                Success = serviceResponse.Success
            });
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<CityViewModel>>>> PutCity(CityDTO updateCity)
        {
            ServiceResponse<List<CityDTO>> serviceResponse = await _cityService.PutCity(updateCity);

            List<CityViewModel> citiesViewModel = serviceResponse.Dados
                .Select(cityDTO => new CityViewModel
                {
                    Id = cityDTO.Id,
                    Name = cityDTO.Name,
                    State = cityDTO.State
                })
                .ToList();

            return Ok(new ServiceResponse<List<CityViewModel>>
            {
                Dados = citiesViewModel,
                Message = serviceResponse.Message,
                Success = serviceResponse.Success
            });
        }

    }
}
