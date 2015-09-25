using System;
using BrainDotExe.Draw;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy;
using EloBuddy.SDK.Menu.Values;

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
            Chat.Print("<font color='#fb00ff'>L</font><font color='#d12bff'>o</font><font color='#a755ff'>a</font><font color='#7d80ff'>d</font><font color='#54aaff'>i</font><font color='#2ad5ff'>n</font><font color='#00ffff'>g</font><font color='#00dbdb'> </font><font color='#00b6b6'>B</font><font color='#009292'>r</font><font color='#006d6d'>a</font><font color='#004949'>i</font><font color='#002424'>n</font><font color='#000000'>.</font><font color='#002b00'>e</font><font color='#005500'>x</font><font color='#008000'>e</font><font color='#00aa00'>.</font><font color='#00d500'>.</font><font color='#00ff00'>.</font>");

            Menu = MainMenu.AddMenu("Brain.exe", "braindotexe");
            Menu.AddSeparator();
            Menu.AddLabel("By KK2 & MrArticuno");

            DrawMenu = Menu.AddSubMenu("Draw", "brainDraw");
            DrawMenu.Add("drawDisable", new CheckBox("Turn off all drawings", false));

            LastHit.Init();
            JungleTimers.Init();

        }

        #region Variaveis

        /*
         Config
         */

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static String G_version = "1.0.0";
        public static String G_charname = _Player.ChampionName;

        /*
         Menus
         */

        public static Menu Menu,
            DrawMenu;

        #endregion
    }
}
