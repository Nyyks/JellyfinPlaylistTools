namespace JellyfinPlaylistTools;

using System.Net.Http.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json; 

public class JellyAPI
{
    private static async Task<string> PostApiCall(string apiUrl, string[] headerContent, string requestBody)
    {
        using HttpClient client = new();

        int y = headerContent.Length;
        int i = 0;
        
        while (i != y)
        {
            client.DefaultRequestHeaders.Add(headerContent[i], headerContent[i+1]);
            i = i + 2;
        }

        var jsonBody = $"{requestBody}";
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(apiUrl, content);
        
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            dynamic jsonResponse = JsonConvert.DeserializeObject(responseContent);
            string apiResult = jsonResponse.ToString();
            return apiResult;
        }
        else
        {
            Console.WriteLine("API failure");
            Console.WriteLine(response);
            return null;
        }
    }

    private static async Task<string> GetApiCall(string apiUrl, string[] headerContent)
    {
        using HttpClient client = new();
        {
            var response = await client.GetAsync(apiUrl);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);
                return responseContent;
            }
            else
            {
                Console.WriteLine($"Failed to retrieve data. Status code: {response.StatusCode}");
                return null;
            }
        }
    }
    
    public static async Task<string> Authenticate(string[] jellyDetails)
    {
        string requestBody = $"{{\"Username\":\"{jellyDetails[1]}\",\"Pw\":\"{jellyDetails[2]}\"}}";
        string apiUrl = $"{jellyDetails[0]}/Users/authenticatebyname";
        string[] headerContent = {"X-Emby-Authorization", "MediaBrowser Client=\"other\", Device=\"script\", DeviceId=\"script\", Version=\"0.0.0\""};
        string apiResult = await PostApiCall(apiUrl, headerContent, requestBody);
        dynamic jsonObject = JsonConvert.DeserializeObject(apiResult);
        string authToken = jsonObject["AccessToken"];
        return authToken;
    }
    
    public static async Task<string[]> GetPlaylists(string[] jellyDetails, string authToken)
    {
        string requestBody = $"";
        string apiUrl = $"{jellyDetails[0]}/playlists";
        string[] headerContent = {"X-Emby-Authorization", authToken};
        
        string apiResult = await GetApiCall(apiUrl, headerContent);
        Console.WriteLine(apiResult);
        
        
        string[] placeholder = { "1", "2" };
        return placeholder;

    }
}