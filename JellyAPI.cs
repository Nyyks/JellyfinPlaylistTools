using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json; 

namespace JellyfinPlaylistTools;

public class JellyApi
{
    private static async Task<string> PostApiCall(string apiUrl, string[] headerContent, string requestBody)
    {
        // This Method can be called to make a post request to a specified apiUrl
        using HttpClient client = new();
        
        // Takes the headerContent array, and adds every argument into the http header
        int y = headerContent.Length;
        int i = 0;
        
        while (i != y)
        {
            client.DefaultRequestHeaders.Add(headerContent[i], headerContent[i+1]);
            i = i + 2;
        }
        
        // Takes the remaining strings and makes them compatible with a http request
        var jsonBody = $"{requestBody}";
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(apiUrl, content);
        
        // Checks if the response is successful. Prints a error or returns the apiResult
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
        // This Method can be called to make a post request to a specified apiUrl
        using HttpClient client = new();
        
        
        // Takes the headerContent array, and adds every argument into the http header
        int y = headerContent.Length;
        int i = 0;
        
        while (i != y)
        {
            client.DefaultRequestHeaders.Add(headerContent[i], headerContent[i+1]);
            i = i + 2;
        }
            
        // Makes a Get request
        var response = await client.GetAsync(apiUrl);
            
        // Checks if the response is successful. Prints a error or returns the apiResult
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
        else
        {
            Console.WriteLine($"Failed to retrieve data. Status code: {response.StatusCode}");
            return null;
        }
        
    }
    
    public static async Task<string[]> Authenticate(string[] jellyDetails)
    {
        // Gets the Accesstoken and UserId using the given username and password
        string requestBody = $"{{\"Username\":\"{jellyDetails[1]}\",\"Pw\":\"{jellyDetails[2]}\"}}";
        string apiUrl = $"{jellyDetails[0]}/Users/authenticatebyname";
        string[] headerContent = {"X-Emby-Authorization", "MediaBrowser Client=\"other\", Device=\"script\", DeviceId=\"script\", Version=\"0.0.0\""};
        string apiResult = await PostApiCall(apiUrl, headerContent, requestBody);
        dynamic jsonObject = JsonConvert.DeserializeObject(apiResult);
        string[] authInfo = new string[2];
        authInfo[0] = jsonObject["AccessToken"];
        authInfo[1] = jsonObject["SessionInfo"]["UserId"];
        return authInfo;
    }
    
    public static async Task<string[]> GetPlaylists(string[] jellyDetails, string[] authInfo)
    {
        // This makes a http request to the jellyfin server, which should return all infos about the available collections on the server
        string[] apiUrl = new string[2];
        apiUrl[0] = $"{jellyDetails[0]}/Users/{authInfo[1]}/Items";
        string[] headerContent = {"X-Emby-Authorization", $"MediaBrowser Client=\"other\", Device=\"script\", DeviceId=\"script\", Version=\"0.0.0\", Token=\"{authInfo[0]}\""};

        string apiResult = await GetApiCall(apiUrl[0], headerContent);
        
        // Extracting the Playlists Id from the http response
        JObject jsonObject = JObject.Parse(apiResult);

        var playlistInfos = jsonObject["Items"].FirstOrDefault(item => (string)item["Name"] == "Playlists");
        string parentId;

        if (playlistInfos != null)
        {
            parentId = (string)playlistInfos["Id"];
        }
        else
        {
            Console.WriteLine("ParentId for your Playlists Collection was not found. Have you enabled the Playlists Collection under (Settings => Home => Playlists => Display on home screen)?");
            return null;
        }

        
        apiUrl[1] = $"{jellyDetails[0]}/Users/{authInfo[1]}/Items?ParentId={parentId}";
        apiResult = await GetApiCall(apiUrl[0], headerContent);
        
        

        
        // Console.WriteLine(apiResult);
        
        
        string[] placeholder = { "1", "2" };
        return placeholder;

    }
}