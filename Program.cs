using System;
using System.Threading.Tasks;

namespace JellyfinPlaylistTools
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            string[] jellyDetails = GetJellyfinDetails.GetDetails();
            string[] authInfo = await JellyApi.Authenticate(jellyDetails);
            string[] playlistList = await JellyApi.GetPlaylists(jellyDetails, authInfo);
        }
    }
}