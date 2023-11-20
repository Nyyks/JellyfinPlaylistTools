using System;
using System.Threading.Tasks;

namespace JellyfinPlaylistTools
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            string[] jellyDetails = GetJellyfinDetails.GetDetails();
            string authToken = await JellyAPI.Authenticate(jellyDetails);
            string[] playlistList = await JellyAPI.GetPlaylists(jellyDetails, authToken);
        }
    }
}