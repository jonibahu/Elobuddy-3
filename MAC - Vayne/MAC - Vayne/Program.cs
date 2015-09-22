using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;
using Vayne.Util;

namespace Vayne
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
        }

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static Spell.Ranged Q;
        public static Spell.Targeted E;
        public static Spell.Active R;

        public static List<Vector2> Points = new List<Vector2>();

        public static Menu Menu,
            ComboMenu,
            LaneClearMenu,
            DrawMenu;
        
        private static void Game_OnStart(EventArgs args)
        {
            if (!_Player.ChampionName.ToLower().Contains("vayne")) return;

            Bootstrap.Init(null);

            Q = new Spell.Skillshot(SpellSlot.Q, 360, SkillShotType.Circular);
            E = new Spell.Targeted(SpellSlot.E, 525);
            R = new Spell.Active(SpellSlot.R);

            Menu = MainMenu.AddMenu("MAC - Vayne", "vania");

            Menu.AddGroupLabel("MAC - Vayne");
            Menu.AddLabel("Version: " + "1.0");
            Menu.AddSeparator();
            Menu.AddLabel("By Mr Articuno");
            Menu.AddSeparator();
            Menu.AddLabel("Pau no seu cu Mineiro! XD");

            DrawMenu = Menu.AddSubMenu("Draw", "vaniaDraw");
            DrawMenu.AddGroupLabel("Draw");
            DrawMenu.Add("drawQ", new CheckBox("Draw Q Range", true));
            DrawMenu.Add("drawE", new CheckBox("Draw E Range", true));
            
            ComboMenu = Menu.AddSubMenu("Combo", "vaniaCombo");
            ComboMenu.AddGroupLabel("Combo");
            ComboMenu.AddLabel("Q Settings");
            ComboMenu.Add("comboQ", new CheckBox("Use Q", true));
            ComboMenu.AddLabel("Q Usage: Checked - After Auto Attack, Unchecked Before Auto Attack");
            ComboMenu.Add("comboQUsage", new CheckBox("Q Usage", true));
            ComboMenu.Add("comboQFoward", new CheckBox("Use on Target Direction", false));
            ComboMenu.AddLabel("E Settings");
            ComboMenu.Add("comboE", new CheckBox("Use E", true));
            ComboMenu.Add("comboAutoCondemn", new CheckBox("Auto Condemn Target in combo", true));
            ComboMenu.Add("comboAutoCondemnGapCloser", new CheckBox("Auto Condemn Gap Closers", true));
            ComboMenu.Add("condenmErrorMargin", new Slider("Subtract Condemn Push by: ", 20, 0, 50));
            ComboMenu.Add("autoCondemnToggle",
                new KeyBind("Auto Condemn", false, KeyBind.BindTypes.PressToggle, 'W'));
            ComboMenu.AddLabel("R Settings");
            ComboMenu.Add("comboR", new CheckBox("Use R", false));
            ComboMenu.Add("minEnemiesForR", new Slider("Min Enemies for cast R: ", 2, 1, 5));

            LaneClearMenu = Menu.AddSubMenu("Lane Clear", "vaniaClear");
            LaneClearMenu.AddGroupLabel("LaneClear");
            LaneClearMenu.AddLabel("Q Settings");
            LaneClearMenu.Add("laneQ", new CheckBox("Use Q", true));
            LaneClearMenu.Add("laneQIfCantAttack", new CheckBox("Use Q if Cant Auto Attack killable minion", true));

            Orbwalker.OnPostAttack += Events.Orbwalker_OnPostAttack;
            Orbwalker.OnPreAttack += Events.Orbwalker_OnPreAttack;
            Gapcloser.OnGapcloser += Events.Gapcloser_OnGapCloser;
            AIHeroClient.OnProcessSpellCast += Events.AIHeroClient_OnProcessSpellCast;

            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;

            Chat.Print("Mechanics Auto Carry: You have 30 seconds to comply.", Color.BlueViolet);
            Chat.Print("Version: " + "1.1.3", Color.Red);

        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (isChecked(DrawMenu, "drawQ") && Q.IsReady())
            {
                new Circle() { Color = Color.White, Radius = Q.Range }.Draw(_Player.Position);
            }
            if (isChecked(DrawMenu, "drawE"))
            {
                var condenavel = false;
                foreach (var enem in ObjectManager.Get<AIHeroClient>().Where(a => a.IsEnemy && E.IsInRange(a)).Where(a => !a.IsDead))
                {
                    var posicao = _Player.Position.Extend(enem.Position, _Player.Distance(enem) - getSliderValue(ComboMenu, "condenmErrorMargin")).To3D();
                    new Circle() { Color = Color.Blue, Radius = 60 }.Draw(_Player.Position.Extend(enem.Position, _Player.Distance(enem) + 470 - getSliderValue(ComboMenu, "condenmErrorMargin")).To3D());

                    for (int i = 0; i < 470 - getSliderValue(ComboMenu, "condenmErrorMargin"); i += 10)
                    {
                        var cPos = _Player.Position.Extend(posicao, _Player.Distance(posicao) + i).To3D();
                        if (cPos.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Wall) || cPos.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Building))
                        {
                            condenavel = true;
                            new Circle() { Color = Color.Red, Radius = 470 }.Draw(enem.Position);
                            Drawing.DrawText(Drawing.WorldToScreen(enem.Position) - new Vector2(30, 0), Color.Red, "CONDEMN THIS SON OF A BITCH !!", 15);
                            break;
                        }
                    }

                    if (!condenavel)
                    {
                        new Circle() { Color = Color.White, Radius = 470 }.Draw(enem.Position);
                    }
                }
            } 
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            var target = Globals._HeroOrbTarget;

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && target != null && target.IsValid)
            {
                if (R.IsReady() && isChecked(ComboMenu, "comboR"))
                {
                    if (_Player.CountEnemiesInRange(_Player.GetAutoAttackRange()) >= getSliderValue(ComboMenu, "minEnemiesForR"))
                    {
                        R.Cast();
                    }
                }

                if (Q.IsReady() && isChecked(ComboMenu, "comboQ") && _Player.Distance(target) > _Player.GetAutoAttackRange())
                {
                    Q.Cast(Game.CursorPos);
                }

                if (E.IsReady() && isChecked(ComboMenu, "comboAutoCondemn") && target != null && target.IsValid && E.IsInRange(target))
                {
                    foreach (var enem in ObjectManager.Get<AIHeroClient>().Where(a => a.IsEnemy && Program.E.IsInRange(a)).Where(a => !a.IsDead))
                    {
                        var posicao = Globals._Player.Position.Extend(enem.Position, Globals._Player.Distance(enem) - Program.getSliderValue(Program.ComboMenu, "condenmErrorMargin")).To3D();
                        for (int i = 0; i < 470 - Program.getSliderValue(Program.ComboMenu, "condenmErrorMargin"); i += 10)
                        {
                            var cPos = Globals._Player.Position.Extend(posicao, Globals._Player.Distance(posicao) + i).To3D();
                            if (cPos.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Wall) || cPos.ToNavMeshCell().CollFlags.HasFlag(CollisionFlags.Building))
                            {
                                Program.E.Cast(enem);
                                break;
                            }
                        }
                    }
                }

                if (E.IsReady() && isChecked(ComboMenu, "autoCondemnToggle"))
                {
                    E.Cast(target);
                    ComboMenu["autoCondemnToggle"].Cast<CheckBox>().CurrentValue = false;
                }
            }
        }

        public static bool Has2WStacks(this AIHeroClient target)
        {
            return target.Buffs.Any(bu => bu.Name == "vaynesilvereddebuff");
        }


        public static bool isChecked(Menu obj, String value)
        {
            return obj[value].Cast<CheckBox>().CurrentValue;
        }

        public static int getSliderValue(Menu obj, String value)
        {
            return obj[value].Cast<Slider>().CurrentValue;
        }
    }
}
