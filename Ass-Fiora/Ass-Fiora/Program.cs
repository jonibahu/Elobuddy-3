using System;
using EloBuddy;
using EloBuddy.SDK.Events;
using Champion = Ass_Fiora.Model.Champion;

namespace Ass_Fiora
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += GameLoaded;
        }

        private static void GameLoaded(EventArgs args)
        {
            if (Player.Instance.ChampionName == "Fiora")
            {
                new Champion().Init();
            }
        }
    }
}
