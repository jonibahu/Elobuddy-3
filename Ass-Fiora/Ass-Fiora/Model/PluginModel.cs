using System;
using System.Drawing;
using Ass_Fiora.Controller;
using Ass_Fiora.Model.Enum;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using OneForWeek.Draw.Notifications;
using OneForWeek.Model.Notification;

namespace Ass_Fiora.Model
{
    public abstract class PluginModel : IChampion
    {
        #region Global Variables

        /*
         Config
         */

        public static readonly string GVersion = "1.0.1";
        public static readonly string GCharname = Player.Instance.ChampionName;
        public static EnumModeManager ActiveMode;

        /*
         Menus
         */

        public static Menu Menu,
            ComboMenu,
            LaneClearMenu,
            JungleClearMenu,
            HarassMenu,
            LastHitMenu,
            MiscMenu,
            DrawMenu;

        /*
        Scope Propreties
        */

        public static Obj_AI_Base TargetBase;

        public static Spell.Skillshot Q { get; set; }
        public static Spell.Skillshot W { get; set; }
        public static Spell.Active E { get; set; }
        public static Spell.Targeted R { get; set; }

        /*
         Misc
         */

        protected PluginModel()
        {
            Notification.DrawNotification(new NotificationModel(Game.Time, 0.5f, 1f, Player.Instance.ChampionName + " addon loading...", Color.White));
        }

        #endregion

        #region Virtual Methods

        public virtual void Init()
        {
            throw new NotImplementedException();
        }

        public virtual void InitVariables()
        {
            throw new NotImplementedException();
        }

        public virtual void InitMenu()
        {
            throw new NotImplementedException();
        }

        public virtual void InitEvents()
        {
            throw new NotImplementedException();
        }

        public virtual void OnCombo()
        {
            throw new NotImplementedException();
        }

        public virtual void OnHarass()
        {
            throw new NotImplementedException();
        }

        public virtual void OnLaneClear()
        {
            throw new NotImplementedException();
        }

        public virtual void OnFlee()
        {
            throw new NotImplementedException();
        }

        public virtual void OnGameUpdate(EventArgs args)
        {
            throw new NotImplementedException();
        }

        public virtual void OnDraw(EventArgs args)
        {
            throw new NotImplementedException();
        }

        public virtual void OnAfterAttack(AttackableUnit target, EventArgs args)
        {
            throw new NotImplementedException();
        }

        public virtual void OnPossibleToInterrupt(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs interruptableSpellEventArgs)
        {
            throw new NotImplementedException();
        }

        public virtual void OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            throw new NotImplementedException();
        }

        public virtual void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            throw new NotImplementedException();
        }

        public virtual void GameObjectOnCreate(GameObject sender, EventArgs args)
        {
            throw new NotImplementedException();
        }

        public virtual void GameObjectOnDelete(GameObject sender, EventArgs args)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
