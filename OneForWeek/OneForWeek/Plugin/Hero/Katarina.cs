using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using OneForWeek.Draw.Notifications;
using OneForWeek.Model.Notification;

namespace OneForWeek.Plugin.Hero
{
    class Katarina : PluginModel, IChampion
    {
        public static Spell.Targeted Q;
        public static Spell.Skillshot W;
        public static Spell.Targeted E;
        public static Spell.Skillshot R;

        public Katarina()
        {
            Init();
        }

        public void Init()
        {
            Notification.DrawNotification(new NotificationModel(Game.Time, 20f, 1f, ObjectManager.Player.ChampionName + " injected have a good game.", Color.DeepSkyBlue));
            InitVariables();
        }

        public void InitVariables()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 675);
            W = new Spell.Skillshot(SpellSlot.W, 375, SkillShotType.Circular, 0, int.MaxValue, 375);
            E = new Spell.Targeted(SpellSlot.E, 700);
            R = new Spell.Skillshot(SpellSlot.R, 550, SkillShotType.Circular, 250, 1500, 140);
            InitMenu();
        }

        public void InitMenu()
        {
            Menu = MainMenu.AddMenu("MAC " + GCharname, GCharname);

            Menu.AddLabel("Version: " + GVersion);
            Menu.AddSeparator();
            Menu.AddLabel("By Vector");

            DrawMenu = Menu.AddSubMenu("Draw - " + GCharname, GCharname + "Draw");
            DrawMenu.AddGroupLabel("Draw");
            DrawMenu.Add("drawDisable", new CheckBox("Turn off all drawings", true));
            DrawMenu.Add("drawQ", new CheckBox("Draw Q Range", true));
            DrawMenu.Add("drawW", new CheckBox("Draw W Range", true));
            DrawMenu.Add("drawE", new CheckBox("Draw E Range", true));
            DrawMenu.Add("drawR", new CheckBox("Draw R Range", true));

            ComboMenu = Menu.AddSubMenu("Combo - " + GCharname, GCharname + "Combo");
            ComboMenu.AddGroupLabel("Combo");
            ComboMenu.Add("comboQ", new CheckBox("Use Q", true));
            ComboMenu.Add("comboW", new CheckBox("Use W", true));
            ComboMenu.Add("comboE", new CheckBox("Use E", true));
            ComboMenu.Add("comboR", new CheckBox("Use R", true));
        }

        public void OnCombo()
        {
            
        }

        public void OnHarass()
        {
            
        }

        public void OnLaneClear()
        {
            
        }

        public void OnFlee()
        {
            
        }

        public void OnGameUpdate(EventArgs args)
        {
            
        }

        public void OnDraw(EventArgs args)
        {
            
        }

        public void OnAfterAttack(AttackableUnit target, EventArgs args)
        {
            
        }

        public void OnPossibleToInterrupt(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs interruptableSpellEventArgs)
        {
            
        }

        public void OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            
        }

        public void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            
        }

        public void GameObjectOnCreate(GameObject sender, EventArgs args)
        {
            
        }

        public void GameObjectOnDelete(GameObject sender, EventArgs args)
        {
            
        }
    }
}
