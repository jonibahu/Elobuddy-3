using EloBuddy;
using EloBuddy.SDK.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    //Vayne.Init();
                    break;
            }

        }
    }
}
