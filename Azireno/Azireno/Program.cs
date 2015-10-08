using Azireno.Plugin;
using Azireno.Util;
using EloBuddy;
using EloBuddy.SDK.Events;
using System;
using System.Drawing;

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
