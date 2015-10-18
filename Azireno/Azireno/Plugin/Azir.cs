using System;
using System.Collections.Generic;
using System.Linq;
using Azireno.Modes;
using Azireno.Util;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace Azireno.Plugin
{
    class Azir : ModeModel, Champion
    {
        static Combo combo = new Combo();
        static Flee flee = new Flee();
        static Harass harass = new Harass();
        static LaneClear laneClear = new LaneClear();

        public static AIHeroClient selectedtarget;
        public static Obj_AI_Base selectedToTarget;

        private static int _lastClick;
        public static bool IsSelected;

        public static List<Obj_AI_Minion> AzirSoldiers = new List<Obj_AI_Minion>();
        
        public void GameObjectOnCreate(GameObject sender, EventArgs args){}

        public void GameObjectOnDelete(GameObject sender, EventArgs args) { }

        public void OnAfterAttack(AttackableUnit target, EventArgs args) { }

        public void OnCombo()
        {
            combo.Execute();
        }

        public void OnLaneClear()
        {
            laneClear.Execute();
        }

        public void OnHarass()
        {
            harass.Execute();
        }

        public void OnFlee()
        {
            if(E.State == SpellState.Ready || E.State == SpellState.Surpressed)
                flee.Execute();
        }

        public void OnDraw(EventArgs args)
        {
            if (Misc.isChecked(DrawMenu, "drawDisable"))
                return;

            if (Misc.isChecked(DrawMenu, "drawQ"))
                Circle.Draw(Q.State == SpellState.Surpressed || Q.State == SpellState.Ready ? Color.Blue : Color.Red, Q.Range, Player.Instance.Position);

            if (Misc.isChecked(DrawMenu, "drawW"))
                Circle.Draw(W.IsReady() ? Color.Blue : Color.Red, W.Range, Player.Instance.Position);

            if (Misc.isChecked(DrawMenu, "drawE"))
                Circle.Draw(E.State == SpellState.Surpressed || E.State == SpellState.Ready ? Color.Blue : Color.Red, E.Range, Player.Instance.Position);

            if (Misc.isChecked(DrawMenu, "drawR"))
                Circle.Draw(R.IsReady() ? Color.Blue : Color.Red, R.Range, Player.Instance.Position);

            if (Azir.AzirSoldiers.Count > 0 && Misc.isChecked(DrawMenu, "drawSoldierRange"))
            {
                foreach (var validAzirSoldier in Azir.AzirSoldiers)
                {
                    Circle.Draw(Color.White, Orbwalker.AzirSoldierAutoAttackRange, validAzirSoldier.Position);
                }
            }

            if (selectedtarget != null)
            {
                new Circle
                {
                    Color = System.Drawing.Color.Red,
                    Radius = 150
                }.Draw(selectedtarget.Position);
            }

            if (selectedToTarget != null)
            {
                new Circle
                {
                    Color = System.Drawing.Color.Blue,
                    Radius = 150
                }.Draw(selectedToTarget.Position);
            }

        }

        private static void Game_OnWndProc(WndEventArgs args)
        {
            if (args.Msg != 0x202) return;
            if (_lastClick + 500 <= Environment.TickCount)
            {
                var selected =
                    ObjectManager.Get<Obj_AI_Base>()
                        .OrderBy(a => a.Distance(ObjectManager.Player))
                        .FirstOrDefault(a =>a.Distance(Game.CursorPos) < 200);

                if(selected == null) return;

                if (selected.IsEnemy && selected.Type == _Player.Type)
                {
                    selectedtarget = (AIHeroClient) selected;
                }
                else
                {
                    selectedToTarget = selected;
                }

                if (selectedtarget != null)
                {
                    _lastClick = Environment.TickCount;
                }
            }
        }

        public void OnGameUpdate(EventArgs args)
        {
            if (Misc.isKeyActive(ComboMenu, "keyBindInsec"))//&& selectedtarget != null && selectedToTarget != null
            {
                new Brain().InsecTarget(selectedtarget, selectedToTarget);
            }

            _target = TargetSelector.GetTarget(1100, DamageType.Magical);
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

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                OnLaneClear();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)){}

        }

        public void OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (R.IsReady() && (sender.IsAttackingPlayer || _Player.Distance(e.End) < 70) && Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Flee && Misc.isChecked(MiscMenu, "allowAntiGapCloser"))
            {
                Player.CastSpell(SpellSlot.R, sender);
            }
        }

        public void OnPossibleToInterrupt(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs interruptableSpellEventArgs)
        {
            if (R.IsReady() && R.IsInRange(sender) && interruptableSpellEventArgs.DangerLevel == DangerLevel.High && Misc.isChecked(MiscMenu, "allowInterrupt"))
                R.Cast(sender);
        }

        public void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args) { }

        public void Init()
        {
            Bootstrap.Init(null);

            Console.WriteLine("Azir injected! - By MrArticuno");

            InitVariables();
            Orbwalker.OnPostAttack += OnAfterAttack;
            Gapcloser.OnGapcloser += OnGapCloser;
            Interrupter.OnInterruptableSpell += OnPossibleToInterrupt;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;

            Game.OnUpdate += OnGameUpdate;
            Game.OnWndProc += Game_OnWndProc;
            Drawing.OnDraw += OnDraw;

            GameObject.OnCreate += OnCreateBase;

        }

        private static void OnCreateBase(GameObject sender, EventArgs args)
        {
            if (sender.Name == "AzirSoldier" && AzirSoldiers.Count <= 2 && sender.Team == _Player.Team)
            {
                AzirSoldiers.Add((Obj_AI_Minion) sender);
            }

            if (sender.Name == "Azir_Base_W_Soldier_Dissipate.troy" && AzirSoldiers.Count > 0)
            {
                AzirSoldiers.Remove(AzirSoldiers.Aggregate((curMin, x) => (curMin == null || x.Distance(sender.Position) < curMin.Distance(sender.Position) ? x : curMin)));
            }

        }

        public void InitVariables()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear);
            W = new Spell.Skillshot(SpellSlot.W, 450, SkillShotType.Circular);
            E = new Spell.Active(SpellSlot.E, 1100);
            R = new Spell.Skillshot(SpellSlot.R, 250, SkillShotType.Linear);
            InitMenu();

            if(_Player.Spellbook.GetSpell(SpellSlot.W).Level - 1 == 0)
                _Player.Spellbook.LevelSpell(SpellSlot.W); // Azir cannot user other skill lvl 1 so dur
        }

        public void InitMenu()
        {
            Menu = MainMenu.AddMenu("SAND GOD " + G_charname, "azir");

            Menu.AddLabel("Version: " + G_version);
            Menu.AddSeparator();
            Menu.AddLabel("By Mr Articuno");

            DrawMenu = Menu.AddSubMenu("Draw - " + G_charname, "azirDraw");
            DrawMenu.AddGroupLabel("Draw");
            DrawMenu.Add("drawDisable", new CheckBox("Turn off all drawings", true));
            DrawMenu.Add("drawQ", new CheckBox("Draw Q Range", true));
            DrawMenu.Add("drawW", new CheckBox("Draw W Range", true));
            DrawMenu.Add("drawE", new CheckBox("Draw E Range", true));
            DrawMenu.Add("drawR", new CheckBox("Draw R Range", true));
            DrawMenu.Add("drawSoldierRange", new CheckBox("Draw Soldier Range", true));

            ComboMenu = Menu.AddSubMenu("Combo - " + G_charname, "azirCombo");
            ComboMenu.AddGroupLabel("Combo");
            ComboMenu.Add("keyBindInsec", new KeyBind("Insec", false, KeyBind.BindTypes.HoldActive));
            ComboMenu.Add("insecFlashForce", new CheckBox("Force Flash", true));
            ComboMenu.Add("comboSoldiers", new CheckBox("Auto Pilot Soldiers", true));
            ComboMenu.Add("finisherE", new CheckBox("Allow E to finish enemy", true));
            ComboMenu.Add("finisherR", new CheckBox("Allow R to finish enemy", true));

            LaneClearMenu = Menu.AddSubMenu("Lane Clear - " + G_charname, "azirLaneClear");
            LaneClearMenu.AddGroupLabel("Lane Clear");
            LaneClearMenu.Add("lcSoldiers", new CheckBox("Auto Pilot Soldiers", true));

            HarassMenu = Menu.AddSubMenu("Harass - " + G_charname, "azirHarass");
            HarassMenu.AddGroupLabel("Harass");
            HarassMenu.Add("hsSoldiers", new CheckBox("Auto Pilot Soldiers", true));

            MiscMenu = Menu.AddSubMenu("Misc - " + G_charname, "azirMisc");
            MiscMenu.AddGroupLabel("Misc");
            MiscMenu.Add("allowAntiGapCloser", new CheckBox("Ultimante on Anti Gap Closers", true));
            MiscMenu.Add("allowInterrupt", new CheckBox("Ultimante on Dangerous Spells", true));
        }
    }
}
