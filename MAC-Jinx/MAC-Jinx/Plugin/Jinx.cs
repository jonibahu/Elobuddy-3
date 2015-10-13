using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using MAC_Jinx.Mode;
using MAC_Jinx.Util;
using SharpDX;

namespace MAC_Jinx.Plugin
{
    class Jinx : ModeModel, IChampion
    {
        static Combo combo = new Combo();
        static Harass harass = new Harass();
        static LaneClear laneClear = new LaneClear();

        public void Init()
        {
            Bootstrap.Init(null);

            Console.WriteLine("Jinx injected! - By MrArticuno");

            InitVariables();
            Orbwalker.OnPostAttack += OnAfterAttack;
            Gapcloser.OnGapcloser += OnGapCloser;
            Interrupter.OnInterruptableSpell += OnPossibleToInterrupt;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;

            Game.OnUpdate += OnGameUpdate;
            Obj_AI_Base.OnLevelUp += OnLevelUp;
            Drawing.OnDraw += OnDraw;
        }

        private static void OnLevelUp(Obj_AI_Base sender, Obj_AI_BaseLevelUpEventArgs args)
        {
            Q = new Spell.Active(SpellSlot.Q, (uint)(75 + (Q.Level * 25)));
        }

        public void InitVariables()
        {
            Q = new Spell.Active(SpellSlot.Q, 75);
            W = new Spell.Skillshot(SpellSlot.W, 1450, SkillShotType.Linear);
            E = new Spell.Skillshot(SpellSlot.E, 900, SkillShotType.Circular, spellWidth: 50);
            R = new Spell.Skillshot(SpellSlot.R, int.MaxValue, SkillShotType.Linear);
            InitMenu();
        }

        public void InitMenu()
        {
            Menu = MainMenu.AddMenu("MAC " + G_charname, "jinx");

            Menu.AddLabel("Version: " + G_version);
            Menu.AddSeparator();
            Menu.AddLabel("By Mr Articuno");

            DrawMenu = Menu.AddSubMenu("Draw - " + G_charname, "jinxDraw");
            DrawMenu.AddGroupLabel("Draw");
            DrawMenu.Add("drawDisable", new CheckBox("Turn off all drawings", true));
            DrawMenu.Add("drawQ", new CheckBox("Draw Q Range", true));
            DrawMenu.Add("drawW", new CheckBox("Draw W Range", true));
            DrawMenu.Add("drawE", new CheckBox("Draw E Range", true));

            ComboMenu = Menu.AddSubMenu("Combo - " + G_charname, "jinxCombo");
            ComboMenu.AddGroupLabel("Combo");
            ComboMenu.Add("comboQ", new CheckBox("Auto Switch Q", true));
            ComboMenu.Add("comboW", new CheckBox("Use W", true));
            ComboMenu.Add("comboE", new CheckBox("Use E", true));
            ComboMenu.Add("comboR", new CheckBox("Use R", true));
            ComboMenu.Add("minRangeR", new Slider("Min Range to cast R", 1700, 500, 5000));

            LaneClearMenu = Menu.AddSubMenu("Lane Clear - " + G_charname, "jinxLaneClear");
            LaneClearMenu.AddGroupLabel("Lane Clear");
            LaneClearMenu.Add("lcQ", new CheckBox("Auto Switch Q", true));
            LaneClearMenu.Add("minMinionsForSwitch", new Slider("Min Minions to Use Rocket", 2, 1, 10));

            HarassMenu = Menu.AddSubMenu("Harass - " + G_charname, "jinxHarass");
            HarassMenu.AddGroupLabel("Harass");
            HarassMenu.Add("hsQ", new CheckBox("Auto Switch Q", true));
            HarassMenu.Add("hsW", new CheckBox("Harass With W", true));

            MiscMenu = Menu.AddSubMenu("Misc - " + G_charname, "jinxMisc");
            MiscMenu.AddGroupLabel("Misc");
            MiscMenu.Add("allowAntiGapCloser", new CheckBox("Cast E on Gap Closers", true));
            MiscMenu.Add("allowEImmobile", new CheckBox("Auto E on immobile", false));
        }

        public void OnCombo()
        {
            combo.Execute();
        }

        public void OnHarass()
        {
            harass.Execute();
        }

        public void OnLaneClear()
        {
            laneClear.Execute();
        }

        public void OnFlee() { }

        public void OnGameUpdate(EventArgs args)
        {
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    OnCombo();
                    break;
                case Orbwalker.ActiveModes.Flee:
                    OnFlee();
                    break;
                case Orbwalker.ActiveModes.Harass:
                    OnHarass();
                    break;
            }

            if (!Misc.IsChecked(MiscMenu, "allowEImmobile")) return;

            foreach (var aiHeroClient in EntityManager.Heroes.Enemies.Where(aiHeroClient => E.IsInRange(aiHeroClient) && !aiHeroClient.CanMove))
            {
                E.Cast(aiHeroClient.ServerPosition);
            }
        }

        public void OnDraw(EventArgs args)
        {
            if (Misc.IsChecked(DrawMenu, "drawDisable"))
                return;

            if (Misc.IsChecked(DrawMenu,"drawQ"))
                Circle.Draw(Q.IsReady() ? Color.Blue : Color.Red, _Player.GetAutoAttackRange() + Q.Range, Player.Instance.Position);

            if (Misc.IsChecked(DrawMenu, "drawW"))
                Circle.Draw(W.IsReady() ? Color.Blue : Color.Red, W.Range, Player.Instance.Position);

            if (Misc.IsChecked(DrawMenu, "drawE"))
                Circle.Draw(E.IsReady() ? Color.Blue : Color.Red, E.Range, Player.Instance.Position);
        }

        public void OnAfterAttack(AttackableUnit target, EventArgs args)
        {

        }

        public void OnPossibleToInterrupt(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs interruptableSpellEventArgs)
        {

        }

        public void OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (Misc.IsChecked(MiscMenu, "allowAntiGapCloser") && sender.IsEnemy && e.End.Distance(_Player) < 50)
            {
                E.Cast(e.End);
            }
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
