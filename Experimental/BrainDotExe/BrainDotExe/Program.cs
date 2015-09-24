using System;
using EloBuddy.SDK.Events;

namespace BrainDotExe
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
        }

        private static void Game_OnStart(EventArgs args)
        {
           
        }
    }
}
