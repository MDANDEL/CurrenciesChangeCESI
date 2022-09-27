namespace ApiCESIProject
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    
    class Program
    {
        static async Task Main(string[] args)
        {
            string json = File.ReadAllText("C:\\Users\\mbb805\\source\\repos\\ApiCESIProject\\ApiCESIProject\\Settings.json");
            Settings settings = JsonSerializer.Deserialize<Settings>(json);
            
            Console.WriteLine("Voici le taux de change du dollar US");
            using var client = new HttpClient();
            using HttpResponseMessage response = await client.GetAsync(settings!.ApiLink);
            response.EnsureSuccessStatusCode();
            var result = response.Content.ReadAsStringAsync().Result;
            WriteCurrencyChange(result);
            Console.WriteLine(result);
            Console.ReadKey();
        }

        private static void WriteCurrencyChange(string result)
        {
            var directory = Directory.GetCurrentDirectory();
            var projectPath = Path.GetFullPath(Path.Combine(directory, @"..\..\..\..\"));

            ResultAPI resultApi = JsonSerializer.Deserialize<ResultAPI>(result);
            foreach (var currency in resultApi!.conversion_rates.GetType().GetProperties())
            {
                var writingData = $"{currency.Name} = {currency.GetValue(resultApi.conversion_rates)}";
                Console.WriteLine(writingData);
                File.WriteAllText($@"{projectPath}\{currency.Name}.txt", writingData);
            }
        }
    }
}