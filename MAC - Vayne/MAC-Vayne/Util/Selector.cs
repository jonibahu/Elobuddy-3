using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using MAC_Vayne.Plugin;
using SharpDX;

namespace MAC_Vayne.Util
{
    public class Selector
    {
        public static Menu TargetMenu;

        public static EnumSelectorMode Mode { get; private set; }

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static void Init()
        {
            Game.OnWndProc += Game_OnWndProc;
            Drawing.OnDraw += Drawing_OnDraw;

            /*
                Menu Stuff
            */

            TargetMenu = Vayne.Menu.AddSubMenu("Advanced Target Selector ", "advTargetSelector");
            TargetMenu.AddGroupLabel("Target Selector");

            foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(t => t.IsEnemy))
            {
                var priority = HighestPriority.Contains(enemy.ChampionName) ? 4 : MedHighPriority.Contains(enemy.ChampionName) ? 3 : MedLowPriority.Contains(enemy.ChampionName) ? 2 : 1;

                TargetMenu.Add("ts" + enemy.ChampionName, new Slider(enemy.ChampionName, priority, 1, 4));
                TargetMenu.AddSeparator();
            }

            TargetMenu.Add("focusTarget", new CheckBox("Focus Selected Target", true));

            TargetMenu.AddGroupLabel("Target Selector");
            TargetMenu.Add("selectedMode", new Slider("Active Mode", (int)EnumSelectorMode.AutoPriority, (int)EnumSelectorMode.AutoPriority, (int)EnumSelectorMode.LessAttacksToKill));
            foreach (var mode in Enum.GetValues(typeof(EnumSelectorMode)))
            {
                TargetMenu.AddLabel(mode + " = " + (int)mode);
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (_target != null)
            {
                Circle.Draw(SharpDX.Color.Red, 150, _target.Position);
            }
        }

        public static AIHeroClient GetTarget(float range, DamageType type, bool forceOrbwalker = false)
        {
            return SelectTarget(range, type, forceOrbwalker);
        }

        public static AIHeroClient SelectTarget(float range, DamageType type, bool forceOrbwalker)
        {
            Mode = (EnumSelectorMode)Misc.getSliderValue(TargetMenu, "selectedMode");
            var target = new AIHeroClient();
            switch (Mode)
            {
                case EnumSelectorMode.AutoPriority:
                    target = AutoPriority(range);
                    break;
                case EnumSelectorMode.Closest:
                    target = Closest(range);
                    break;
                case EnumSelectorMode.LeastHealth:
                    target = LeastHealth(range);
                    break;
                case EnumSelectorMode.LessAttacksToKill:
                    target = LessAttacksToKill(range);
                    break;
                case EnumSelectorMode.MostAbilityPower:
                    target = MostAbilityPower(range);
                    break;
                case EnumSelectorMode.MostAttackDamage:
                    target = MostAttackDamage(range);
                    break;
                case EnumSelectorMode.NearMouse:
                    target = NearMouse(range);
                    break;
            }

            if (!Validtarget(target))
            {
                return TargetSelector.GetTarget(range, type);
            }

            if (_target != null && Misc.isChecked(TargetMenu, "focusTarget") && _target.Distance(_Player) < range)
                target = _target;

            if (Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.Combo)
            {

            }
            else
            {
                Orbwalker.ForcedTarget = target;
            }

            return target;
        }

        private static AIHeroClient _target;
        private static int _lastClick;

        public static AIHeroClient AutoPriority(float range)
        {
            var target = new AIHeroClient();
            foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(t => t.IsEnemy).Where(t => _Player.Distance(t) <= range))
            {
                if (!Validtarget(enemy)) continue;

                var attackDamage = _Player.GetAutoAttackDamage(enemy, true);
                if (Validtarget(target))
                {
                    if (enemy.Health <= attackDamage || enemy.Health <= attackDamage * 2)
                    {
                        if (Misc.getSliderValue(TargetMenu, "ts" + target.ChampionName) <
                            Misc.getSliderValue(TargetMenu, "ts" + enemy.ChampionName))
                        {
                            target = enemy;
                        }
                    }
                    else if (target.Health < enemy.Health)
                    {
                        target = enemy;
                    }
                    else
                    {
                        if (Misc.getSliderValue(TargetMenu, "ts" + target.ChampionName) < Misc.getSliderValue(TargetMenu, "ts" + enemy.ChampionName))
                        {
                            target = enemy;
                        }
                    }
                }
                else
                {
                    target = enemy;
                }
            }
            return target;
        }

        public static AIHeroClient Closest(float range)
        {
            var target = new AIHeroClient();
            foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(t => t.IsEnemy).Where(t => _Player.Distance(t) <= range).Where(Validtarget))
            {
                if (!Validtarget(target))
                {
                    target = enemy;
                }

                if (target.Distance(_Player) > enemy.Distance(_Player)) { }
                else
                {
                    target = enemy;
                }
            }
            return target;
        }

        public static AIHeroClient LeastHealth(float range)
        {
            var target = new AIHeroClient();
            foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(t => t.IsEnemy).Where(t => _Player.Distance(t) <= range).Where(Validtarget))
            {
                if (!Validtarget(target))
                {
                    target = enemy;
                    continue;
                }

                if (target.Health > enemy.Health) { }
                else
                {
                    target = enemy;
                }
            }
            return target;
        }

        public static AIHeroClient LessAttacksToKill(float range)
        {
            var target = new AIHeroClient();
            foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(t => t.IsEnemy).Where(t => _Player.Distance(t) <= range).Where(Validtarget))
            {
                if (!Validtarget(target))
                {
                    target = enemy;
                    continue;
                }

                if (QuantAaToKill(target) > QuantAaToKill(enemy)) { }
                else
                {
                    target = enemy;
                }
            }
            return target;
        }

        public static AIHeroClient MostAbilityPower(float range)
        {
            var target = new AIHeroClient();
            foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(t => t.IsEnemy).Where(t => _Player.Distance(t) <= range).Where(Validtarget))
            {
                if (!Validtarget(target))
                {
                    target = enemy;
                    continue;
                }

                if (target.FlatMagicDamageMod > enemy.FlatMagicDamageMod) { }
                else
                {
                    target = enemy;
                }
            }
            return target;
        }

        public static AIHeroClient MostAttackDamage(float range)
        {
            var target = new AIHeroClient();
            foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(t => t.IsEnemy).Where(t => _Player.Distance(t) <= range).Where(Validtarget))
            {
                if (!Validtarget(target))
                {
                    target = enemy;
                    continue;
                }

                if (target.GetAutoAttackDamage(_Player) > enemy.GetAutoAttackDamage(_Player)) { }
                else
                {
                    target = enemy;
                }
            }
            return target;
        }

        public static AIHeroClient NearMouse(float range)
        {
            var target = new AIHeroClient();
            foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(t => t.IsEnemy).Where(t => _Player.Distance(t) <= range).Where(Validtarget))
            {
                if (!Validtarget(target))
                {
                    target = enemy;
                }

                if (target.Distance(Game.CursorPos) > enemy.Distance(Game.CursorPos)) { }
                else
                {
                    target = enemy;
                }
            }
            return target;
        }

        public static double QuantAaToKill(AIHeroClient target)
        {
            return _Player.GetAutoAttackDamage(target, true) / target.Health;
        }

        public static bool Validtarget(Obj_AI_Base target)
        {
            return !(target == null || target.IsDead || target.Health <= 0 || !target.IsValidTarget() || target.IsInvulnerable);
        }

        private static void Game_OnWndProc(WndEventArgs args)
        {
            if (args.Msg != 0x202) return;
            if (_lastClick + 500 <= Environment.TickCount)
            {
                _target =
                    ObjectManager.Get<AIHeroClient>()
                        .OrderBy(a => a.Distance(ObjectManager.Player))
                        .FirstOrDefault(a => a.IsEnemy && a.Distance(Game.CursorPos) < 200);
                if (_target != null)
                {
                    _lastClick = Environment.TickCount;
                }
            }
        }

        #region Dummy Info
        public static readonly string[] HighestPriority =
            {
                "Ahri", "Anivia", "Annie", "Ashe", "Brand", "Caitlyn",
                "Cassiopeia", "Corki", "Draven", "Ezreal", "Graves", "Jinx",
                "Kalista", "Karma", "Karthus", "Katarina", "Kennen",
                "KogMaw", "Leblanc", "Lucian", "Lux", "Malzahar", "MasterYi",
                "MissFortune", "Orianna", "Quinn", "Sivir", "Syndra",
                "Talon", "Teemo", "Tristana", "TwistedFate", "Twitch",
                "Varus", "Vayne", "Veigar", "VelKoz", "Viktor", "Xerath",
                "Zed", "Ziggs"
            };

        public static readonly string[] LowestPriority =
            {
                "Alistar", "Amumu", "Blitzcrank", "Braum", "Cho'Gath",
                "Dr. Mundo", "Garen", "Gnar", "Hecarim", "Janna", "Jarvan IV",
                "Leona", "Lulu", "Malphite", "Nami", "Nasus", "Nautilus",
                "Nunu", "Olaf", "Rammus", "Renekton", "Sejuani", "Shen",
                "Shyvana", "Singed", "Sion", "Skarner", "Sona", "Soraka",
                "Taric", "Thresh", "Volibear", "Warwick", "MonkeyKing",
                "Yorick", "Zac", "Zyra"
            };

        public static readonly string[] MedHighPriority =
            {
                "Akali", "Diana", "Fiddlesticks", "Fiora", "Fizz",
                "Heimerdinger", "Jayce", "Kassadin", "Kayle", "Kha'Zix",
                "Lissandra", "Mordekaiser", "Nidalee", "Riven", "Shaco",
                "Vladimir", "Yasuo", "Zilean"
            };

        public static readonly string[] MedLowPriority =
            {
                "Aatrox", "Darius", "Elise", "Evelynn", "Galio", "Gangplank",
                "Gragas", "Irelia", "Jax", "Lee Sin", "Maokai", "Morgana",
                "Nocturne", "Pantheon", "Poppy", "Rengar", "Rumble", "Ryze",
                "Swain", "Trundle", "Tryndamere", "Udyr", "Urgot", "Vi",
                "XinZhao", "RekSai"
            };

        #endregion
    }
}
