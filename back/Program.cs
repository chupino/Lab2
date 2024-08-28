using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

//Clase Para "Enlace"
public class Link
{
    public string Text { get; set; }
    public string Href { get; set; }
}

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var app = builder.Build();

        // Endpoint para manejar la API
        app.MapGet("/api/{id:int}", async ([FromRoute] int id) =>
        {
            var links = await ExtractLinks(50);
            if (id >= 0 && id < links.Count)
            {
                var selectedLink = new List<Link> { links[id] };
                return Results.Json(selectedLink);
            }
            return Results.NotFound("Link not found.");
        });

        await app.RunAsync();
    }


    public static async Task<List<Link>> ExtractLinks(int lmt = 8)
    {
        string apikey = "LIVDSRZULELA";
        string searchTerm = "excited";
        var links = new List<Link>();
        using var client = new HttpClient();

        var response = await client.GetAsync($"https://g.tenor.com/v1/search?q={searchTerm}&key={apikey}&limit={lmt}");
        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var topGifs = JsonDocument.Parse(jsonString);

            foreach (var result in topGifs.RootElement.GetProperty("results").EnumerateArray())
            {
                var link = new Link
                {
                    Text = result.GetProperty("id").GetString(),
                    Href = result.GetProperty("media")[0].GetProperty("webm").GetProperty("preview").GetString()
                };
                links.Add(link);
            }

            var clonedLinks = new List<Link>();
            for (int i = 0; i < 10000; i++)
            {
                clonedLinks.AddRange(links);
            }

            return clonedLinks;
        }

        return links;
    }
}