using Microsoft.EntityFrameworkCore;
using Cities.Api.Infrastruture.Models;
using Cities.Api.Service.Service;
using Microsoft.Extensions.DependencyInjection;
using Cities.Api.Service.DTO;

[TestClass]
public class CityServiceTests
{

    [TestMethod]
    public async Task DeleteCity_CityExists_ReturnsCorrectData()
    {
        var cityId = 1;
        var cityName = "TestCity";
        var cityState = "TS";
        var city = new City { Id = cityId, Name = cityName, State = cityState };

        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var options = new DbContextOptionsBuilder<CityContext>()
            .UseInternalServiceProvider(serviceProvider)
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using (var context = new CityContext(options))
        {
            context.Cities.Add(city);
            context.SaveChanges();
        }

        using (var context = new CityContext(options))
        {
            var cityService = new CityService(context);

            var result = await cityService.DeleteCity(cityId);

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Dados);
            Assert.IsTrue(result.Message == string.Empty);

            var deletedCity = result.Dados.Find(c => c.Id == cityId);
            Assert.IsNull(deletedCity);
        }
    }


    [TestMethod]
    public async Task DeleteCity_CityDoesNotExist_ReturnsCityNotFound()
    {
        var cityId = 1;

        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var options = new DbContextOptionsBuilder<CityContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .UseInternalServiceProvider(serviceProvider)
            .Options;

        using (var context = new CityContext(options))
        {
            var cityService = new CityService(context);

            var result = await cityService.DeleteCity(cityId);

            Assert.IsFalse(result.Success);
            Assert.IsNull(result.Dados);
            Assert.IsTrue(result.Message == "City Not Found", $"Expected 'City Not Found', but got '{result.Message}'");
        }
    }

    [TestMethod]
    public async Task PostCity_ReturnsCorrectData()
    {
        var newCity = new CityDTO { Id = 1, Name = "NewCity", State = "NS" };

        var options = new DbContextOptionsBuilder<CityContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using (var context = new CityContext(options))
        {
            var cityService = new CityService(context);

            var result = await cityService.PostCity(newCity);

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Dados);
            Assert.IsTrue(result.Message == string.Empty);

            var addedCity = result.Dados.Find(c => c.Id == newCity.Id);
            Assert.IsNotNull(addedCity);
            Assert.IsTrue(addedCity.Name == newCity.Name && addedCity.State == newCity.State);
        }
    }

    [TestMethod]
    public async Task GetCities_ReturnsCities()
    {
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var options = new DbContextOptionsBuilder<CityContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .UseInternalServiceProvider(serviceProvider)
            .Options;

        using (var context = new CityContext(options))
        {    
            context.Cities.AddRange(new List<City>
        {
            new City { Id = 1, Name = "City1", State = "State1" },
            new City { Id = 2, Name = "City2", State = "State2" },
            new City { Id = 3, Name = "City3", State = "State3" }
        });
            context.SaveChanges();
        }

        using (var context = new CityContext(options))
        {
            var cityService = new CityService(context);

            var result = await cityService.GetCities();

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Dados);
            Assert.IsTrue(result.Dados.Count > 0);
            Assert.IsTrue(string.IsNullOrEmpty(result.Message));
        }
    }

    [TestMethod]
    public async Task PutCity_ReturnsCorrectData()
    {
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var options = new DbContextOptionsBuilder<CityContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .UseInternalServiceProvider(serviceProvider)
            .Options;

        using (var context = new CityContext(options))
        {
            context.Cities.Add(new City { Id = 1, Name = "City1", State = "State1" });
            context.SaveChanges();
        }

        using (var context = new CityContext(options))
        {
            var cityService = new CityService(context);

            var updateCity = new CityDTO { Id = 1, Name = "UpdatedCity", State = "UpdatedState" };

            var result = await cityService.PutCity(updateCity);

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Dados);
            Assert.IsTrue(result.Dados.Count > 0);
            Assert.IsTrue(string.IsNullOrEmpty(result.Message));

            var updatedCity = result.Dados.Find(c => c.Id == updateCity.Id);
            Assert.IsNotNull(updatedCity);
            Assert.AreEqual(updateCity.Name, updatedCity.Name);
            Assert.AreEqual(updateCity.State, updatedCity.State);
        }
    }

    [TestMethod]
    public async Task GetCity_ReturnsCorrectData()
    {
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var options = new DbContextOptionsBuilder<CityContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .UseInternalServiceProvider(serviceProvider)
            .Options;

        using (var context = new CityContext(options))
        {
            context.Cities.Add(new City { Id = 1, Name = "City1", State = "State1" });
            context.SaveChanges();
        }

        using (var context = new CityContext(options))
        {
            var cityService = new CityService(context);

            var resultExistingCity = await cityService.GetCity(1);
            var resultNonExistingCity = await cityService.GetCity(2);

            Assert.IsTrue(resultExistingCity.Success);
            Assert.IsNotNull(resultExistingCity.Dados);
            Assert.AreEqual(1, resultExistingCity.Dados.Id);
            Assert.AreEqual("City1", resultExistingCity.Dados.Name);
            Assert.AreEqual("State1", resultExistingCity.Dados.State);
            Assert.IsTrue(string.IsNullOrEmpty(resultExistingCity.Message));

            Assert.IsFalse(resultNonExistingCity.Success);
            Assert.IsNull(resultNonExistingCity.Dados);
            Assert.IsTrue(resultNonExistingCity.Message == "City not Found");
        }
    }




}
