using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK.Events;
using MAC_Jinx.Plugin;

namespace MAC_Jinx
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnGameStart;
        }

        private static void OnGameStart(EventArgs args)
        {
            if (!Player.Instance.ChampionName.Equals("Jinx")) return;

            new Jinx().Init();
        }
    }
}
