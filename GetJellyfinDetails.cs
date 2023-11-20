using System;
using System.Linq;

namespace JellyfinPlaylistTools;

public class GetJellyfinDetails
{
    public static string[] GetDetails()
    {
        string[] jellyDetails = new string[3];
        
        Console.WriteLine("Is your Jellyfin-Server using ssl? y/n");
        string ssl = Console.ReadLine();
        string prefix;
        if (ssl.ToLower() == "y")
        {
            prefix = "https://";
        }
        else
        {
            prefix = "http://";
        }
        Console.WriteLine("Please input your Jellyfin URL:");
        jellyDetails[0] = Console.ReadLine();
        if (!jellyDetails[0].Contains("http") || !jellyDetails[0].Contains("https"))
        {
            jellyDetails[0] = prefix + jellyDetails[0];
        }
        
        Console.WriteLine("Input your username:");
        jellyDetails[1] = Console.ReadLine();
        Console.WriteLine("Input your password:");
        jellyDetails[2] = Console.ReadLine();
        Console.Clear();
        return jellyDetails;
    }
}