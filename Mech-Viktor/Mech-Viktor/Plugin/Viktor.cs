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
         Misc
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

            Orbwalker.OnPostAttack += OnAfterAttack;
            Orbwalker.OnPreAttack += OnBeforeAttack;
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
            if (Misc.isChecked(DrawMenu, "drawDisable"))
                return;

            if(Misc.isChecked(DrawMenu, "drawQ"))
                new Circle() { Color = Color.Cyan, Radius = Q.Range, BorderWidth = 2f }.Draw(_Player.Position);

            if (Misc.isChecked(DrawMenu, "drawW"))
                new Circle() { Color = Color.Cyan, Radius = W.Range, BorderWidth = 2f }.Draw(_Player.Position);

            if (Misc.isChecked(DrawMenu, "drawE"))
                new Circle() { Color = Color.Cyan, Radius = E.Range, BorderWidth = 2f }.Draw(_Player.Position);

            if (Misc.isChecked(DrawMenu, "drawR"))
                new Circle() { Color = Color.Cyan, Radius = R.Range, BorderWidth = 2f }.Draw(_Player.Position);
        }

        public static void OnAfterAttack(AttackableUnit target, EventArgs args)
        {
            if (target != null && (!target.IsValid || target.IsDead))
                return;

        }

        public static void OnBeforeAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (target != null && (!target.IsValid || target.IsDead))
                return;
        }

        public static void OnProcessSpell(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe) return;

            if (!args.SData.Name.ToLower().Contains("viktorpowertransferreturn")) return;
            
            if (_Player.HasBuff("viktorqbuff"))
            {
                Core.DelayAction(Orbwalker.ResetAutoAttack, 250);
            }
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
            if (Misc.isChecked(LaneClearMenu, "laneClearE") && E.IsReady())
                PredictCastMinionE(Misc.getSliderValue(LaneClearMenu, "lcMinEnemiesForE"));

            if (Misc.isChecked(LaneClearMenu, "laneClearQ") && Q.IsReady() && Orbwalker.GetTarget().IsValid)
            {
                var target = new Obj_AI_Base(Orbwalker.GetTarget().Index, (uint)Orbwalker.GetTarget().NetworkId);
                Q.Cast(target);
            }
            
        }

        public static void OnHarass()
        {
            //  OnHarass
        }

        public static void OnCombo()
        {
            if (_target == null || !_target.IsValid)
                return;

            if (Misc.isChecked(ComboMenu, "comboR") && R.IsReady())
            {
                /*
                OverKill protection
                */
                if (DmgLib.possibleDamage(_target, false) > _target.Health)
                    return;

                if (DmgLib.possibleDamage(_target) > _target.Health && !Q.IsReady() && !E.IsReady())
                    R.Cast(_target);

            }

            if (Misc.isChecked(ComboMenu, "comboW") && W.IsReady() && _Player.Distance(_target) < W.Range - 100)
            {
                Player.CastSpell(SpellSlot.W, _Player.Position.Extend(_target, 100).To3D());
            }

            if (Misc.isChecked(ComboMenu, "comboQ") && Q.IsReady() && Q.IsInRange(_target))
            {
                Q.Cast(_target);
            }

            if (Misc.isChecked(ComboMenu, "comboE") && E.IsReady())
            {
                PredictCastE(_target);
            }
        }

        private static void PredictCastE(AIHeroClient target)
        {
            // Helpers
            bool inRange = Vector2.DistanceSquared(target.ServerPosition.To2D(), _Player.Position.To2D()) < E.Range * E.Range;
            PredictionResult prediction;
            bool spellCasted = false;

            // Positions
            Vector3 pos1, pos2;

            // Champs
            var nearChamps = (from champ in ObjectManager.Get<AIHeroClient>() where champ.IsValidTarget(maxRangeE) && target != champ select champ).ToList();
            var innerChamps = new List<AIHeroClient>();
            var outerChamps = new List<AIHeroClient>();
            foreach (var champ in nearChamps)
            {
                if (Vector2.DistanceSquared(champ.ServerPosition.To2D(), _Player.Position.To2D()) < E.Range * E.Range)
                    innerChamps.Add(champ);
                else
                    outerChamps.Add(champ);
            }

            // Minions
            var nearMinions = EntityManager.GetLaneMinions(EntityManager.UnitTeam.Enemy,_Player.Position.To2D(), maxRangeE, true);
            var innerMinions = new List<Obj_AI_Base>();
            var outerMinions = new List<Obj_AI_Base>();
            foreach (var minion in nearMinions)
            {
                if (Vector2.DistanceSquared(minion.ServerPosition.To2D(), _Player.Position.To2D()) < E.Range * E.Range)
                    innerMinions.Add(minion);
                else
                    outerMinions.Add(minion);
            }

            // Main target in close range
            if (inRange)
            {
                // Get prediction reduced speed, adjusted sourcePosition
                var speed = speedE * 0.9f;
                var from = target.ServerPosition + (Vector3.Normalize(_Player.Position - target.ServerPosition) * (lengthE * 0.1f));
                prediction = E.GetPrediction(target);
                from = _Player.Position;

                // Prediction in range, go on
                if (prediction.CastPosition.Distance(_Player.Position) < E.Range)
                    pos1 = prediction.CastPosition;
                // Prediction not in range, use exact position
                else
                {
                    pos1 = target.ServerPosition;
                    E.Speed = speedE;
                }

                // Set new sourcePosition
                from = pos1;
                var castedFrom = pos1;

                // Set new range
                var range = lengthE;

                // Get next target
                if (nearChamps.Count > 0)
                {
                    // Get best champion around
                    var closeToPrediction = new List<AIHeroClient>();
                    foreach (var enemy in nearChamps)
                    {
                        // Get prediction
                        prediction = E.GetPrediction(enemy);
                        // Validate target
                        if (prediction.HitChance >= HitChance.High && Vector2.DistanceSquared(pos1.To2D(), prediction.CastPosition.To2D()) < (E.Range * E.Range) * 0.8)
                            closeToPrediction.Add(enemy);
                    }

                    // Champ found
                    if (closeToPrediction.Count > 0)
                    {
                        // Sort table by health DEC
                        if (closeToPrediction.Count > 1)
                            closeToPrediction.Sort((enemy1, enemy2) => enemy2.Health.CompareTo(enemy1.Health));

                        // Set destination
                        prediction = E.GetPrediction(closeToPrediction[0]);
                        pos2 = prediction.CastPosition;

                        // Cast spell
                        CastE(pos1, pos2);
                        spellCasted = true;
                    }
                }

                // Spell not casted
                if (!spellCasted)
                    CastE(pos1, E.GetPrediction(target).CastPosition);

                // Reset spell
                speed = speedE;
                range = rangeE;
                from = _Player.Position;
                castedFrom = _Player.Position;
            }

            // Main target in extended range
            else
            {
                // Radius of the start point to search enemies in
                float startPointRadius = 150;

                // Get initial start point at the border of cast radius
                Vector3 startPoint = _Player.Position + Vector3.Normalize(target.ServerPosition - _Player.Position) * rangeE;

                // Potential start from postitions
                var targets = (from champ in nearChamps where Vector2.DistanceSquared(champ.ServerPosition.To2D(), startPoint.To2D()) < startPointRadius * startPointRadius && Vector2.DistanceSquared(_Player.Position.To2D(), champ.ServerPosition.To2D()) < rangeE * rangeE select champ).ToList();
                if (targets.Count > 0)
                {
                    // Sort table by health DEC
                    if (targets.Count > 1)
                        targets.Sort((enemy1, enemy2) => enemy2.Health.CompareTo(enemy1.Health));

                    // Set target
                    pos1 = targets[0].ServerPosition;
                }
                else
                {
                    var minionTargets = (from minion in nearMinions where Vector2.DistanceSquared(minion.ServerPosition.To2D(), startPoint.To2D()) < startPointRadius * startPointRadius && Vector2.DistanceSquared(_Player.Position.To2D(), minion.ServerPosition.To2D()) < rangeE * rangeE select minion).ToList();
                    if (minionTargets.Count > 0)
                    {
                        // Sort table by health DEC
                        if (minionTargets.Count > 1)
                            minionTargets.Sort((enemy1, enemy2) => enemy2.Health.CompareTo(enemy1.Health));

                        // Set target
                        pos1 = minionTargets[0].ServerPosition;
                    }
                    else
                        // Just the regular, calculated start pos
                        pos1 = startPoint;
                }

                // Predict target position
                var from = pos1;
                var range = lengthE;
                var castedfrom = pos1;
                prediction = E.GetPrediction(target);

                // Cast the E
                if (prediction.HitChance >= HitChance.High)
                    CastE(pos1, prediction.CastPosition);

                // Reset spell
                range = rangeE;
                from = _Player.Position;
                castedfrom   = _Player.Position;
            }

        }

        public static void CastE(Vector3 pos1, Vector3 pos2)
        {
            Player.CastSpell(SpellSlot.E, pos1, pos2);
        }

        private static bool PredictCastMinionE(int requiredHitNumber = -1)
        {
            int hitNum = 0;
            Vector2 startPos = new Vector2(0, 0);
            Vector2 endPos = new Vector2(0, 0);
            foreach (var minion in EntityManager.GetLaneMinions(EntityManager.UnitTeam.Enemy,_Player.Position.To2D(), rangeE))
            {
                var farmLocation = GetBestLaserFarmLocation(minion.Position.To2D(), (from mnion in EntityManager.GetLaneMinions(EntityManager.UnitTeam.Enemy, minion.Position.To2D(), lengthE) select mnion.Position.To2D()).ToList<Vector2>(), E.Width, lengthE);
                if (farmLocation.MinionsHit > hitNum)
                {
                    hitNum = farmLocation.MinionsHit;
                    startPos = minion.Position.To2D();
                    endPos = farmLocation.Position;
                    CastE(startPos.To3D(), endPos.To3D());
                    return true;
                }
            }

            if (startPos.X != 0 && startPos.Y != 0)
                return true;

            return false;
        }

        public static FarmLocation GetBestLaserFarmLocation(Vector2 sourcepos, List<Vector2> minionPositions, float width, float range)
        {
            var result = new Vector2();
            var minionCount = 0;
            var startPos = sourcepos;

            var max = minionPositions.Count;
            for (var i = 0; i < max; i++)
            {
                for (var j = 0; j < max; j++)
                {
                    if (minionPositions[j] != minionPositions[i])
                    {
                        minionPositions.Add((minionPositions[j] + minionPositions[i]) / 2);
                    }
                }
            }

            foreach (var pos in minionPositions)
            {
                if (pos.Distance(startPos, true) <= range * range)
                {
                    var endPos = startPos + range * (pos - startPos).Normalized();

                    var count =
                        minionPositions.Count(pos2 => pos2.Distance(startPos, endPos, true, true) <= width * width);

                    if (count >= minionCount)
                    {
                        result = endPos;
                        minionCount = count;
                    }
                }
            }

            return new FarmLocation(result, minionCount);
        }

        public struct FarmLocation
        {
            public int MinionsHit;
            public Vector2 Position;

            public FarmLocation(Vector2 position, int minionsHit)
            {
                Position = position;
                MinionsHit = minionsHit;
            }
        }

        public static void OnGameUpdate(EventArgs args)
        {
            _target = TargetSelector.GetTarget(1400, DamageType.Magical);
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    OnCombo();
                    break;
                case Orbwalker.ActiveModes.LaneClear:
                    OnLaneClear();
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
