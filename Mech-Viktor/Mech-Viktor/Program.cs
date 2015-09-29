using EloBuddy;
using EloBuddy.SDK.Events;
using Mech_Viktor.Plugin;
using System;

namespace Mech_Viktor
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
                case "viktor":
                    Viktor.Init();
                    break;
            }

        }
    }
}
