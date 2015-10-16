using System.Drawing;
using EloBuddy;
using EloBuddy.SDK.Menu;
using OneForWeek.Draw.Notifications;
using OneForWeek.Model.Notification;

namespace OneForWeek.Plugin
{
    abstract class PluginModel
    {
        #region Global Variables

        /*
         Config
         */

        public static readonly string GVersion = "1.0.1";
        public static readonly string GCharname = Player.ChampionName;

        /*
         Menus
         */

        public static Menu Menu,
            ComboMenu,
            LaneClearMenu,
            HarassMenu,
            MiscMenu,
            DrawMenu;

        protected PluginModel()
        {
            Notification.DrawNotification(new NotificationModel(Game.Time, 0.5f, 1f, ObjectManager.Player.ChampionName + " injeting...", Color.White));
        }

        public void PluginLoaded()
        {
            Chat.Print(ObjectManager.Player.ChampionName + " Plugin Loaded!");
        }

        /*
         Misc
         */

        public static AIHeroClient Target;

        public static AIHeroClient Player
        {
            get { return ObjectManager.Player; }
        }

        #endregion
    }
}
