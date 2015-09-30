using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Mech_Viktor.Misc;
using Mech_Viktor.Plugin;
using SharpDX;

namespace Mech_Viktor.Util
{
    public static class AdvancedTargetSelector
    {
        #region Static Fields

        /// <summary>
        ///     Champions that should be prioritized first. (1)
        /// </summary>
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

        /// <summary>
        ///     Champions that should be prioritized fourth(last). (4)
        /// </summary>
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

        /// <summary>
        ///     Champions that should be prioritized second. (2)
        /// </summary>
        public static readonly string[] MedHighPriority =
            {
                "Akali", "Diana", "Fiddlesticks", "Fiora", "Fizz",
                "Heimerdinger", "Jayce", "Kassadin", "Kayle", "Kha'Zix",
                "Lissandra", "Mordekaiser", "Nidalee", "Riven", "Shaco",
                "Vladimir", "Yasuo", "Zilean"
            };

        /// <summary>
        ///     Champions that should be prioritized third (3)
        /// </summary>
        public static readonly string[] MedLowPriority =
            {
                "Aatrox", "Darius", "Elise", "Evelynn", "Galio", "Gangplank",
                "Gragas", "Irelia", "Jax", "Lee Sin", "Maokai", "Morgana",
                "Nocturne", "Pantheon", "Poppy", "Rengar", "Rumble", "Ryze",
                "Swain", "Trundle", "Tryndamere", "Udyr", "Urgot", "Vi",
                "XinZhao", "RekSai"
            };

        /// <summary>
        ///     The atsMenu handle.
        /// </summary>
        private static Menu atsMenu;

        #endregion

        public static void Init()
        {

            atsMenu = Viktor.Menu.AddSubMenu("Advanced Target Selector ", "advTargetSelector");
            atsMenu.AddGroupLabel("Target Selector");

            foreach (var enemy in ObjectManager.Get<AIHeroClient>().Where(t => t.IsEnemy))
            {
                var priority = HighestPriority.Any(t => t == enemy.ChampionName) ? 1: MedHighPriority.Any(t => t == enemy.ChampionName) ? 2: MedLowPriority.Any(t => t == enemy.ChampionName) ? 3 : 4;
                atsMenu.Add("ts" + enemy.ChampionName, new Slider(enemy.ChampionName, priority, 1, 5));
                atsMenu.AddSeparator();
            }

            atsMenu.Add("focusTarget", new CheckBox("Focus Selected Target", true));
            atsMenu.AddGroupLabel("Mode");
            atsMenu.Add("selectedMode", new Slider("Active Mode", (int)AdvTargetSelectorMode.AutoPriority, (int)AdvTargetSelectorMode.LessAttacksToKill, (int)AdvTargetSelectorMode.LeastHealth));

            Game.OnUpdate += OnStateChanged;

        }

        private static void OnStateChanged(EventArgs args)
        {
            if (Mode == (AdvTargetSelectorMode) Misc.getSliderValue(atsMenu, "selectedMode")) return;

            Mode = (AdvTargetSelectorMode)Misc.getSliderValue(atsMenu, "selectedMode");
        }


        public static AdvTargetSelectorMode Mode { get; private set; }

        public static AIHeroClient SelectedTarget { get; set; }

        public static float GetPriority(AIHeroClient hero)
        {
            var p = 1;
            try
            {
                p = Util.Misc.getSliderValue(atsMenu, "ts" + hero.ChampionName);
            }
            catch (Exception)
            {
                // Ignored.
            }

            switch (p)
            {
                case 2:
                    return 1.5f;
                case 3:
                    return 1.75f;
                case 4:
                    return 2f;
                case 5:
                    return 2.5f;
                default:
                    return 1f;
            }
        }

        public static AIHeroClient GetSelectedTarget(float range = -1f)
        {
            return SelectedTarget != null && SelectedTarget.IsValidTarget(range < 0 ? float.MaxValue : range)
                       ? SelectedTarget
                       : null;
        }

        public static AIHeroClient GetTarget(
            float range = -1f,
            DamageType damage = DamageType.Physical,
            IEnumerable<AIHeroClient> ignoredChamps = null,
            Vector3? rangeCheckFrom = null)
        {
            if (Util.Misc.isChecked(atsMenu, "focusTarget") && SelectedTarget != null
                && SelectedTarget.IsValidTarget(range < 0 ? SelectedTarget.GetAutoAttackRange() : range))
            {
                return SelectedTarget;
            }

            var targets =
                ObjectManager.Get<AIHeroClient>()
                .Where(t => t.IsEnemy)
                .Where(t => t.IsValidTarget(range < 0 ? t.GetAutoAttackRange() : range))
                    .ToArray();

            if (!targets.Any())
            {
                return null;
            }

            if (ignoredChamps != null)
            {
                targets = targets.Where(t => ignoredChamps.All(i => t.NetworkId == i.NetworkId)).ToArray();
            }

            var excludedTargets = targets.Where(t => t.IsInvulnerable);

            return targets.Any()
                       ? GetTarget(targets, damage, rangeCheckFrom)
                       : GetTarget(excludedTargets, damage, rangeCheckFrom);
        }

        public static AIHeroClient GetTarget(
            IEnumerable<AIHeroClient> targets,
            DamageType damageType = DamageType.Physical,
            Vector3? rangeCheckFrom = null)
        {
            switch (Mode)
            {
                case AdvTargetSelectorMode.LessAttacksToKill:
                    return targets.MinOrDefault(
                        t =>
                        {
                            var attackDamage = ObjectManager.Player.GetAutoAttackDamage(t, true);
                            var damage = t.Health / attackDamage > 0 ? attackDamage : 1;
                            try
                            {
                                return damage * Util.Misc.getSliderValue(atsMenu, "ts" + t.ChampionName);
                            }
                            catch (Exception)
                            {
                                return damage;
                            }
                        });
                case AdvTargetSelectorMode.MostAbilityPower:
                    return targets.MaxOrDefault(t => t.TotalMagicalDamage);
                case AdvTargetSelectorMode.MostAttackDamage:
                    return targets.MaxOrDefault(t => t.TotalAttackDamage);
                case AdvTargetSelectorMode.Closest:
                    return
                        targets.MinOrDefault(
                            hero => (rangeCheckFrom ?? ObjectManager.Player.ServerPosition).DistanceSquared(hero.Position));
                case AdvTargetSelectorMode.NearMouse:
                    return targets.Find(t => t.DistanceSquared(Game.CursorPos) < 22500);
                case AdvTargetSelectorMode.AutoPriority:
                    return
                        targets.MaxOrDefault(
                            hero =>
                            ObjectManager.Player.CalculateDamage(hero, damageType, 100) / (1 + hero.Health)
                            * GetPriority(hero));
                case AdvTargetSelectorMode.LeastHealth:
                    return targets.MinOrDefault(t => t.Health);
            }

            return null;
        }

        #region mimimi stuff

        public static float DistanceSquared(this Vector3 vector3, Vector3 toVector3)
        {
            return Vector3.DistanceSquared(vector3, toVector3);
        }

        public static float DistanceSquared(this Vector3 vector3, Vector2 toVector2)
        {
            return Vector3.DistanceSquared(vector3, toVector2.To3D());
        }


        public static T MinOrDefault<T, TR>(this IEnumerable<T> container, Func<T, TR> comparer) where TR : IComparable
        {
            var enumerator = container.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return default(T);
            }

            var minElem = enumerator.Current;
            var minVal = comparer(minElem);

            while (enumerator.MoveNext())
            {
                var currVal = comparer(enumerator.Current);

                if (currVal.CompareTo(minVal) >= 0)
                {
                    continue;
                }

                minVal = currVal;
                minElem = enumerator.Current;
            }

            return minElem;
        }

        public static T MaxOrDefault<T, TR>(this IEnumerable<T> container, Func<T, TR> comparer) where TR : IComparable
        {
            var enumerator = container.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return default(T);
            }

            var maxElem = enumerator.Current;
            var maxVal = comparer(maxElem);

            while (enumerator.MoveNext())
            {
                var currVal = comparer(enumerator.Current);

                if (currVal.CompareTo(maxVal) <= 0)
                {
                    continue;
                }

                maxVal = currVal;
                maxElem = enumerator.Current;
            }

            return maxElem;
        }

        public static TSource Find<TSource>(this IEnumerable<TSource> source, Predicate<TSource> match)
        {
            return (source as List<TSource> ?? source.ToList()).Find(match);
        }

        public static float DistanceSquared(this GameObject source, GameObject target)
        {
            return source.Position.DistanceSquared(target.Position);
        }

        public static float DistanceSquared(this GameObject source, Vector3 position)
        {
            return source.Position.DistanceSquared(position);
        }

        public static float DistanceSquared(this GameObject source, Vector2 position)
        {
            return source.Position.DistanceSquared(position);
        }

        public static float DistanceSquared(this Obj_AI_Base source, Vector3 position)
        {
            return source.ServerPosition.DistanceSquared(position);
        }

        public static float DistanceSquared(this Obj_AI_Base source, Vector2 position)
        {
            return source.ServerPosition.DistanceSquared(position);
        }

        public static float DistanceSquared(this Obj_AI_Base source, Obj_AI_Base target)
        {
            return source.ServerPosition.DistanceSquared(target.ServerPosition);
        }

        public static double CalculateDamage(
            this Obj_AI_Base source,
            Obj_AI_Base target,
            DamageType damageType,
            float amount,
            float forceDamageModifier = 0f)
        {
            var damage = 0d;
            switch (damageType)
            {
                case DamageType.Magical:
                    damage = source.CalculateDamageOnUnit(target, damageType, amount);
                    break;
                case DamageType.Physical:
                    damage = source.CalculateDamageOnUnit(target, damageType, amount);
                    break;
                case DamageType.True:
                    damage = amount;
                    break;
            }

            return Math.Max(damage, 0d);
        }

        #endregion
    }
}
