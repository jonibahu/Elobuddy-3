using System;
using Azireno.Plugin;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace Azireno
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
        }

        private static void Game_OnStart(EventArgs args)
        {
            var champion = ObjectManager.Player.ChampionName.ToLower();

            switch (champion)
            {
                case "azir":
                    new Azir().Init();
                    break;
            }

        }
    }
}
