using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

class Program
{
    private static readonly HttpClient httpClient = new HttpClient();
    private const string apiUrl = "http://localhost:26441/api/CitiesController"; // Substitua pela URL da sua API

    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Escolha uma opção:");
            Console.WriteLine("1. Mostrar todas as cidades");
            Console.WriteLine("2. Mostrar cidade por ID");
            Console.WriteLine("3. Cadastrar cidade");
            Console.WriteLine("4. Atualizar cidade");
            Console.WriteLine("5. Deletar cidade");
            Console.WriteLine("0. Sair");

            var opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    await MostrarTodasCidades();
                    break;

                case "2":
                    await MostrarCidadePorId();
                    break;

                case "3":
                    await CadastrarCidade();
                    break;

                case "4":
                    await AtualizarCidade();
                    break;

                case "5":
                    await DeletarCidade();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
        }
    }

    static async Task MostrarTodasCidades()
    {
        var cidades = await httpClient.GetFromJsonAsync<List<CityViewModel>>(apiUrl + "/GetAll");
        ImprimirCidades(cidades);
    }

    static async Task MostrarCidadePorId()
    {
        Console.Write("Digite o ID da cidade: ");
        if (Guid.TryParse(Console.ReadLine(), out Guid id))
        {
            var cidade = await httpClient.GetFromJsonAsync<CityViewModel>($"{apiUrl}/{id}/Get");
            if (cidade != null)
            {
                ImprimirCidades(new List<CityViewModel> { cidade });
            }
            else
            {
                Console.WriteLine("Cidade não encontrada.");
            }
        }
        else
        {
            Console.WriteLine("ID inválido.");
        }
    }

    static async Task CadastrarCidade()
    {
        Console.Write("Nome da cidade: ");
        var cityName = Console.ReadLine();

        Console.Write("Estado: ");
        var stateName = Console.ReadLine();

        var cidade = new CityDTO { CityName = cityName, StateName = stateName };

        var response = await httpClient.PostAsJsonAsync(apiUrl + "/Add", cidade);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Cidade cadastrada com sucesso!");
        }
        else
        {
            Console.WriteLine($"Erro ao cadastrar cidade. Status: {response.StatusCode}");
        }
    }

    static async Task AtualizarCidade()
    {
        Console.Write("ID da cidade a ser atualizada: ");
        if (Guid.TryParse(Console.ReadLine(), out Guid id))
        {
            Console.Write("Novo nome: ");
            var cityName = Console.ReadLine();

            Console.Write("Novo estado: ");
            var stateName = Console.ReadLine();

            var cidade = new CityViewModel { CityName = cityName, StateName = stateName };

            var response = await httpClient.PutAsJsonAsync($"{apiUrl}/{id}", cidade);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Cidade atualizada com sucesso!");
            }
            else
            {
                Console.WriteLine($"Erro ao atualizar cidade. Status: {response.StatusCode}");
            }
        }
        else
        {
            Console.WriteLine("ID inválido.");
        }
    }

    static async Task DeletarCidade()
    {
        Console.Write("ID da cidade a ser deletada: ");
        if (Guid.TryParse(Console.ReadLine(), out Guid id))
        {
            var response = await httpClient.DeleteAsync($"{apiUrl}/{id}/Delete");

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Cidade deletada com sucesso!");
            }
            else
            {
                Console.WriteLine($"Erro ao deletar cidade. Status: {response.StatusCode}");
            }
        }
        else
        {
            Console.WriteLine("ID inválido.");
        }
    }

    static void ImprimirCidades(List<CityViewModel> cidades)
    {
        foreach (var cidade in cidades)
        {
            Console.WriteLine($"ID: {cidade.Id}, Nome: {cidade.CityName}, Estado: {cidade.StateName}");
        }
    }
}

public class CityViewModel
{
    public Guid Id { get; set; }
    public string CityName { get; set; }
    public string StateName { get; set; }
}

public class CityDTO
{
    public string CityName { get; set; }
    public string StateName { get; set; }
}