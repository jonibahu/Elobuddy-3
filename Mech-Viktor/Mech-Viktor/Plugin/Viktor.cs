using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Color = System.Drawing.Color;
using Mech_Viktor.Util;
using System.Linq;
using SharpDX;
using System.Collections.Generic;
using EloBuddy.SDK.Rendering;

// ReSharper disable All

namespace Mech_Viktor.Plugin
{
    static class Viktor
    {

        #region Global Variables

        /*
         Config
         */

        public static String G_version = "1.0.0";
        public static String G_charname = _Player.ChampionName;

        /*
         Spells
         */
        public static Spell.Targeted Q;
        public static Spell.Skillshot W;
        public static Spell.Skillshot E;
        public static Spell.Skillshot R;

        private static readonly int maxRangeE = 1225;
        private static readonly int lengthE = 700;
        private static readonly int speedE = 1200;
        private static readonly int rangeE = 525;

        /*
         Menus
         */

        public static Menu Menu,
            ComboMenu,
            LaneClearMenu,
            HarassMenu,
            GapCloserMenu,
            KSMenu,
            DrawMenu;

        /*
         Util.Misc
         */

        public static AIHeroClient _target;

        #endregion

        #region Initialization

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static void Init()
        {
            Bootstrap.Init(null);

            Chat.Print("Mechanics: Loading...", Color.BlueViolet);

            InitVariables();

            Gapcloser.OnGapcloser += OnGapCloser;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpell;

            Game.OnUpdate += OnGameUpdate;
            Drawing.OnDraw += OnDraw;

            Chat.Print("Mechanics Viktor: Loaded, Version" + G_version + " have a nice game.", Color.Red);
        }

        public static void InitVariables()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Skillshot(SpellSlot.W, 700, SkillShotType.Circular, 500, int.MaxValue, 300);
            W.AllowedCollisionCount = int.MaxValue;
            E = new Spell.Skillshot(SpellSlot.E, (uint)rangeE, SkillShotType.Linear, 0, speedE, 80);
            E.AllowedCollisionCount = int.MaxValue;
            R = new Spell.Skillshot(SpellSlot.R, 700, SkillShotType.Circular, 250, int.MaxValue, 450);
            R.AllowedCollisionCount = int.MaxValue;
            InitMenu();
        }

        public static void InitMenu()
        {
            Menu = MainMenu.AddMenu("Mechanics - " + G_charname, "viktor");

            Menu.AddGroupLabel("MAC - " + G_charname);
            Menu.AddLabel("Version: " + G_version);
            Menu.AddSeparator();
            Menu.AddLabel("By Mr Articuno");

            DrawMenu = Menu.AddSubMenu("Draw - " + G_charname, "viktorDraw");
            DrawMenu.AddGroupLabel("Draw");
            DrawMenu.Add("drawDisable", new CheckBox("Turn off all drawings", false));
            DrawMenu.Add("drawNameLine", new CheckBox("Show names on line", true));
            DrawMenu.Add("drawAARange", new CheckBox("Draw Auto Attack Range", true));
            DrawMenu.Add("drawQ", new CheckBox("Draw Q Range", true));
            DrawMenu.Add("drawW", new CheckBox("Draw W Range", true));
            DrawMenu.Add("drawE", new CheckBox("Draw E Range", true));
            DrawMenu.Add("drawR", new CheckBox("Draw R Range", true));
            DrawMenu.Add("drawCondemnPos", new CheckBox("Draw Condemn Position", true));

            ComboMenu = Menu.AddSubMenu("Combo - " + G_charname, "viktorCombo");
            ComboMenu.AddGroupLabel("Combo");
            ComboMenu.Add("comboQ", new CheckBox("Allow Q usage in combo", true));
            ComboMenu.Add("comboW", new CheckBox("Allow W usage in combo", true));
            ComboMenu.Add("comboE", new CheckBox("Allow E usage in combo", true));
            ComboMenu.Add("comboR", new CheckBox("Allow R usage in combo", true));
            ComboMenu.AddGroupLabel("R Settings");
            ComboMenu.Add("rsMinEnemiesForR", new Slider("Min Enemies for cast R: ", 1, 1, 5));

            LaneClearMenu = Menu.AddSubMenu("Lane Clear - " + G_charname, "viktorLaneClear");
            LaneClearMenu.AddGroupLabel("Lane Clear");
            LaneClearMenu.Add("laneClearQ", new CheckBox("Allow Q usage in LaneClear", true));
            LaneClearMenu.Add("laneClearE", new CheckBox("Allow E usage in LaneClear", true));
            LaneClearMenu.Add("lcMinEnemiesForE", new Slider("Min Enemies for cast E: ", 2, 1, 6));

            KSMenu = Menu.AddSubMenu("KS - " + G_charname, "viktorKillSteal");
            KSMenu.AddGroupLabel("Kill Steal");
            KSMenu.Add("ksQ", new CheckBox("Use Q if killable", false));
            KSMenu.Add("ksE", new CheckBox("Use E if killable", false));
            KSMenu.Add("ksR", new CheckBox("Use R if killable", false));
        }

        #endregion

        public static void OnDraw(EventArgs args)
        {
            if (Util.Misc.isChecked(DrawMenu, "drawDisable"))
                return;

            if(Util.Misc.isChecked(DrawMenu, "drawQ"))
                new Circle() { Color = Color.Cyan, Radius = Q.Range, BorderWidth = 2f }.Draw(_Player.Position);

            if (Util.Misc.isChecked(DrawMenu, "drawW"))
                new Circle() { Color = Color.Cyan, Radius = W.Range, BorderWidth = 2f }.Draw(_Player.Position);

            if (Util.Misc.isChecked(DrawMenu, "drawE"))
                new Circle() { Color = Color.Cyan, Radius = E.Range, BorderWidth = 2f }.Draw(_Player.Position);

            if (Util.Misc.isChecked(DrawMenu, "drawR"))
                new Circle() { Color = Color.Cyan, Radius = R.Range, BorderWidth = 2f }.Draw(_Player.Position);
        }

        public static void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;

            if (args.SData.Name.ToLower().Contains("viktorpowertransferreturn"))
                Core.DelayAction(Orbwalker.ResetAutoAttack, 250);

        }

        public static void OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            //  OnGapClose
        }

        public static void OnPossibleToInterrupt()
        {
            //  OnPossibleToInterrupt
        }

        public static void OnLasthit()
        {
            //  OnLasthit
        }

        public static void OnLaneClear()
        {
            if (Util.Misc.isChecked(LaneClearMenu, "laneClearE") && E.IsReady())
            {
                var firstMinion = ObjectManager.Get<Obj_AI_Minion>().Where(a => a.IsEnemy).Where(a => !a.IsDead).Where(a => a.Distance(_Player) < maxRangeE).FirstOrDefault();
                var lasttMinion = ObjectManager.Get<Obj_AI_Minion>().Where(a => a.IsEnemy).Where(a => !a.IsDead).Where(a => a.Distance(_Player) < E.Range).LastOrDefault();

                if(firstMinion == null || lasttMinion == null) return;

                CastE(firstMinion.Position, lasttMinion.Position);
            }

            if (Util.Misc.isChecked(LaneClearMenu, "laneClearQ") && Q.IsReady())
            {
                var minions = ObjectManager.Get<Obj_AI_Minion>()
                    .Where(a => a.IsEnemy)
                    .Where(a => Q.IsInRange(a))
                    .Where(a => DmgLib.Q(a) > a.Health);
                foreach (var minion in minions)
                {
                    if (!Orbwalker.CanAutoAttack)
                        Q.Cast(minion);
                }
            }
            
        }

        public static void OnHarass()
        {
            //  OnHarass
        }

        public static void OnCombo()
        {
            _target = TargetSelector.GetTarget(1225f, DamageType.Magical);
            if (_target == null || !_target.IsValid)
                return;
            
            if (Util.Misc.isChecked(ComboMenu, "comboR") && R.IsReady())
            {
                /*
                OverKill protection
                */
                if (DmgLib.possibleDamage(_target, false) > _target.Health)
                    return;

                if (DmgLib.possibleDamage(_target) > _target.Health && !Q.IsReady() && !E.IsReady())
                    R.Cast(_target);

            }

            if (Util.Misc.isChecked(ComboMenu, "comboW") && W.IsReady() && _Player.Distance(_target) < W.Range - 100)
            {
                Player.CastSpell(SpellSlot.W, _Player.Position.Extend(_target, W.Range - 100).To3D());
            }

            if (Util.Misc.isChecked(ComboMenu, "comboQ") && Q.IsReady() && Q.IsInRange(_target))
            {
                Q.Cast(_target);
            }

            if (Util.Misc.isChecked(ComboMenu, "comboE") && E.IsReady())
            {
                PredictCastE(_target);
            }
        }

        private static void PredictCastE(AIHeroClient target)
        {
            if(target.Distance(_Player)> maxRangeE) return;

            var posInicial = target.Position;
            if (E.IsInRange(target))
            {
                posInicial = posInicial.Extend(target.Position, 100).To3D();

                var pred = E.GetPrediction(target);

                if (pred.HitChance == HitChance.High)
                {
                    CastE(posInicial, pred.UnitPosition);
                }
            }
            else if(target.Distance(_Player) < maxRangeE)
            {
                var maxPosition = target.Position.Extend(_Player, _Player.Distance(target) - rangeE);

                var pred = new PredictionResult(maxPosition.To3D(), target.Position, 70, null, Int32.MaxValue);
                
                if(pred.HitChance >= HitChance.Medium)
                    CastE(target.Position, maxPosition.Extend(_Player.Position.To2D(), 30).To3D());

            }
        }

        public static void CastE(Vector3 pos1, Vector3 pos2)
        {
            Player.CastSpell(SpellSlot.E, pos1, pos2);
        }

        public static void KS()
        {
            if (Util.Misc.isChecked(KSMenu, "ksQ") && Q.IsReady())
            {
                var target = ObjectManager.Get<AIHeroClient>()
                    .Where(a => !a.IsDead)
                    .Where(a => a.IsEnemy)
                    .Where(a => a.IsValidTarget())
                    .Where(a => Q.IsInRange(a))
                    .Where(a => DmgLib.Q(a) > a.Health + 50)
                    .FirstOrDefault();

                if(target == null ) return;

                Q.Cast(target);
            }
            if (Util.Misc.isChecked(KSMenu, "ksE") && E.IsReady())
            {
                var target = ObjectManager.Get<AIHeroClient>()
                    .Where(a => !a.IsDead)
                    .Where(a => a.IsEnemy)
                    .Where(a => a.IsValidTarget())
                    .Where(a => a.Distance(_Player) < maxRangeE)
                    .Where(a => DmgLib.E(a) > a.Health + 50)
                    .FirstOrDefault();

                if (target == null) return;

                PredictCastE(target);
            }

            if (Util.Misc.isChecked(KSMenu, "ksR") && R.IsReady())
            {
                var target = ObjectManager.Get<AIHeroClient>()
                    .Where(a => !a.IsDead)
                    .Where(a => a.IsEnemy)
                    .Where(a => a.IsValidTarget())
                    .Where(a => a.Distance(_Player) < R.Range)
                    .Where(a => DmgLib.R(a) > a.Health + 50)
                    .FirstOrDefault();

                if (target == null) return;

                R.Cast(target);
            }
        }

        public static void OnGameUpdate(EventArgs args)
        {
            KS();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                OnLaneClear();
            }

            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    OnCombo();
                    break;
                case Orbwalker.ActiveModes.LastHit:
                    OnLasthit();
                    break;
                case Orbwalker.ActiveModes.Harass:
                    OnHarass();
                    break;
            }
        }
    }
}
