using System.Net;
using System.Net.Http.Json;
using CitiesAPI.Application.ViewModel;
using Newtonsoft.Json;

class Program
{
    private const string BaseUrl = "http://localhost:5297/api/CitiesController/"; 

    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("\nEscolha uma opção:");
            Console.WriteLine("1. Cities List");
            Console.WriteLine("2. Search City By Id");
            Console.WriteLine("3. Add New City");
            Console.WriteLine("4. Update City");
            Console.WriteLine("5. Delete City");
            Console.WriteLine("0. Exit");

            if (int.TryParse(Console.ReadLine(), out var choice))
            {
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("\n");
                        await GetCities();
                        break;
                    case 2:
                        Console.WriteLine("\n");
                        await GetCityById();
                        break;
                    case 3:
                        Console.WriteLine("\n");
                        await AddCity();
                        break;
                    case 4:
                        Console.WriteLine("\n");
                        await UpdateCity();
                        break;
                    case 5:
                        Console.WriteLine("\n");
                        await DeleteCity();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid Option. Try Again.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid Option. Try Again.");
            }
        }
    }

    private static async Task GetCities()
    {
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetStringAsync($"http://localhost:5297/api/CitiesController");
            var cities = JsonConvert.DeserializeObject<ServiceResponse<List<CityViewModel>>>(response);

            foreach (var city in cities.Dados)
            {
                Console.WriteLine($"ID: {city.Id}, Name: {city.Name}, State: {city.State}");
            }
        }
    }

    private static async Task GetCityById()
    {
        Console.Write("Enter the city Id: ");
        if (int.TryParse(Console.ReadLine(), out var cityId))
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync($"{BaseUrl}{cityId}");

                if (!string.IsNullOrEmpty(response))
                {
                    var city = JsonConvert.DeserializeObject<ServiceResponse<CityViewModel>>(response);

                    if (city.Dados != null)
                    {
                        Console.WriteLine($"ID: {city.Dados.Id}, Name: {city.Dados.Name}, State: {city.Dados.State}");
                    }
                    else
                    {
                        Console.WriteLine("City not found.");
                    }
                }
                else
                {
                    Console.WriteLine("Empty response received.");
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid Id. ");
        }
    }




    private static async Task AddCity()
    {
        Console.Write("Enter the City Name: ");
        var cityName = Console.ReadLine();
        Console.Write("Enter the City State: ");
        var cityState = Console.ReadLine();

        var newCity = new CityViewModel { Name = cityName, State = cityState };

        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.PostAsJsonAsync($"{BaseUrl}", newCity);
            var result = await response.Content.ReadAsStringAsync();
            var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<List<CityViewModel>>>(result);

            if (serviceResponse.Success)
            {
                Console.WriteLine("City Added");
            }
            else
            {
                Console.WriteLine($"Error: {serviceResponse.Message}");
            }
        }
    }

    private static async Task UpdateCity()
    {
        Console.Write("Enter the City Id: ");
        if (int.TryParse(Console.ReadLine(), out var cityId))
        {
            Console.Write("Type the new City Name: ");
            var cityName = Console.ReadLine();
            Console.Write("Type the new City State: ");
            var cityState = Console.ReadLine();

            var updatedCity = new CityViewModel { Id = cityId, Name = cityName, State = cityState };

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PutAsJsonAsync($"{BaseUrl}", updatedCity);
                var result = await response.Content.ReadAsStringAsync();
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<List<CityViewModel>>>(result);

                if (serviceResponse.Success)
                {
                    Console.WriteLine("City Updated");
                }
                else
                {
                    Console.WriteLine($"Error: {serviceResponse.Message}");
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid Id.");
        }
    }

    private static async Task DeleteCity()
    {
        Console.Write("Enter the Id of the City you want to delete: ");
        if (int.TryParse(Console.ReadLine(), out var cityId))
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync($"{BaseUrl}{cityId}");
                var result = await response.Content.ReadAsStringAsync();
                var serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<List<CityViewModel>>>(result);

                if (serviceResponse.Success)
                {
                    Console.WriteLine("City deleted.");
                }
                else
                {
                    Console.WriteLine($"Error: {serviceResponse.Message}");
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid Id.");
        }
    }
}
