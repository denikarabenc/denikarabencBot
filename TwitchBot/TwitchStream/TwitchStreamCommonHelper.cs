using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TwitchBot.TwitchStream.Json;

namespace TwitchBot.TwitchStream
{
    public static class TwitchStreamCommonHelper
    {
        //public static List<string> GetTwitchGames(int numberOfGames, int offset)
        //{
        //    List<string> listOfTwitchGames = new List<string>();

        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/games/top?limit=100");
        //    request.Method = "GET";
        //    request.Headers["Authorization"] = $"OAuth agjzfjjarinmxy46lc9zzae9r4e967"; //vv0yeswj1kpcmyvi381006bl7rxaj4
        //    request.ContentType = "application/json";
        //    request.Accept = $"application/vnd.twitchtv.v3+json";

        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
          
        //    using (var reader = new StreamReader(response.GetResponseStream()))
        //    {
        //        string jsonString = reader.ReadToEnd();              
        //    }

        //    return listOfTwitchGames;
        //}

        
        //public static List<string> GetAllTwitchGames() //obsolette
        //{
        //    HashSet<string> listOfTwitchGames = new HashSet<string>();

        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/games/top?limit=100&offset=20");
        //    request.Method = "GET";
        //    request.Headers["Authorization"] = $"OAuth agjzfjjarinmxy46lc9zzae9r4e967"; //vv0yeswj1kpcmyvi381006bl7rxaj4
        //    request.ContentType = "application/json";
        //    request.Accept = $"application/vnd.twitchtv.v3+json";

        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        //    TwitchGamesJsonRootObject jsonResult;
        //    using (var reader = new StreamReader(response.GetResponseStream()))
        //    {
        //        string jsonString = reader.ReadToEnd();
        //        jsonResult = JsonConvert.DeserializeObject<TwitchGamesJsonRootObject>(jsonString);
        //    }

        //    foreach (TwitchTopGamesInfo tgi in jsonResult.Top)
        //    {
        //        listOfTwitchGames.Add(tgi.Game.Name);
        //    }

        //    for (int i = 100; i <= jsonResult?.Total; i += 100)
        //    {               
        //        System.Threading.Thread.Sleep(1000);
        //        HttpWebRequest pageRequest = (HttpWebRequest)WebRequest.Create("https://api.twitch.tv/kraken/games/top?limit=100&offset=" + i.ToString()); //TODO dobija se bad request za preko 600 offset. Ako se koristi i+=90 onda opet ne radi.
        //        request.Method = "GET";
        //        request.Headers["Authorization"] = $"OAuth agjzfjjarinmxy46lc9zzae9r4e967"; //vv0yeswj1kpcmyvi381006bl7rxaj4
        //        request.ContentType = "application/json";
        //        request.Accept = $"application/vnd.twitchtv.v3+json";

        //        HttpWebResponse pageResponse = (HttpWebResponse)pageRequest.GetResponse();

        //        TwitchGamesJsonRootObject pageJsonResult;
        //        using (var reader = new StreamReader(pageResponse.GetResponseStream()))
        //        {
        //            string jsonString = reader.ReadToEnd();
        //            pageJsonResult = JsonConvert.DeserializeObject<TwitchGamesJsonRootObject>(jsonString);
        //        }

        //        foreach (TwitchTopGamesInfo tgi in pageJsonResult.Top)
        //        {
        //            listOfTwitchGames.Add(tgi.Game.Name);
        //        }
        //    }

        //    return listOfTwitchGames.ToList();
        //}
    }
}
