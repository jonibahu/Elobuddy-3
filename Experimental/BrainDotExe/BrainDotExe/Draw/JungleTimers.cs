using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX.Direct3D9;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using Utility = BrainDotExe.Common.Utility;
using System.IO;

namespace BrainDotExe.Draw
{
    public static class JungleTimers
    {
        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static void DrawJungleTimers_OnDraw(EventArgs args)
        {
            
        }

    }

    class Jungle
    {
        //public static Menu.MenuItemSettings JungleTimer = new Menu.MenuItemSettings(typeof(Jungle));

        private static List<JungleMob> JungleMobs = new List<JungleMob>();
        private static List<JungleCamp> JungleCamps = new List<JungleCamp>();
        private static List<Obj_AI_Minion> JungleMobList = new List<Obj_AI_Minion>();
        private static List<PlayerBossMobBuff> Enemies = new List<PlayerBossMobBuff>();
        private static readonly Utility.Map GMap = Utility.Map.GetMap();

        private int lastGameUpdateTime = 0;

        public bool IsBigMob(Obj_AI_Minion jungleBigMob)
        {
            foreach (JungleMob jungleMob in JungleMobs)
            {
                if (jungleBigMob.Name.Contains(jungleMob.Name))
                {
                    return jungleMob.Smite;
                }
            }
            return false;
        }

        public bool IsBossMob(Obj_AI_Minion jungleBossMob)
        {
            foreach (JungleMob jungleMob in JungleMobs)
            {
                if (jungleBossMob.BaseSkinName.Contains(jungleMob.Name))
                {
                    return jungleMob.Boss;
                }
            }
            return false;
        }

        public bool HasBuff(Obj_AI_Minion jungleBigMob)
        {
            foreach (JungleMob jungleMob in JungleMobs)
            {
                if (jungleBigMob.BaseSkinName.Contains(jungleMob.Name))
                {
                    return jungleMob.Buff;
                }
            }
            return false;
        }

        private JungleMob GetJungleMobByName(string name, Utility.Map.MapType mapType)
        {
            return JungleMobs.Find(jm => name.Contains(jm.Name) && jm.MapType == mapType);
        }

        public void Init()
        {
            JungleMobs.Add(new JungleMob("SRU_Baron12.1.1", null, true, true, true, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_Dragon6.1.1", null, true, false, true, Utility.Map.MapType.SummonersRift));

            JungleMobs.Add(new JungleMob("SRU_Blue1.1.1", null, true, true, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_Murkwolf2.1.1", null, true, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_Razorbeak3.1.1", null, true, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_Red4.1.1", null, true, true, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_Krug5.1.2", null, true, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_Gromp13.1.1", null, true, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_Crab15.1.1", null, true, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_RedMini4.1.2", null, false, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_RedMini4.1.3", null, false, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_MurkwolfMini2.1.2", null, false, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_MurkwolfMini2.1.3", null, false, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_RazorbeakMini3.1.2", null, false, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_RazorbeakMini3.1.3", null, false, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_RazorbeakMini3.1.4", null, false, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_KrugMini5.1.1", null, false, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_BlueMini1.1.2", null, false, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_BlueMini21.1.3", null, false, false, false, Utility.Map.MapType.SummonersRift));

            JungleMobs.Add(new JungleMob("SRU_Blue7.1.1", null, true, true, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_Murkwolf8.1.1", null, true, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_Razorbeak9.1.1", null, true, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_Red10.1.1", null, true, true, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_Krug11.1.2", null, true, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_Gromp14.1.1", null, true, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_Crab16.1.1", null, true, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_RedMini10.1.2", null, false, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_RedMini10.1.3", null, false, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_MurkwolfMini8.1.2", null, false, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_MurkwolfMini8.1.3", null, false, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_RazorbeakMini9.1.2", null, false, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_RazorbeakMini9.1.3", null, false, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_RazorbeakMini9.1.4", null, false, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_KrugMini11.1.1", null, false, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_BlueMini7.1.2", null, false, false, false, Utility.Map.MapType.SummonersRift));
            JungleMobs.Add(new JungleMob("SRU_BlueMini27.1.3", null, false, false, false, Utility.Map.MapType.SummonersRift));

            //Twisted Treeline
            JungleMobs.Add(new JungleMob("TT_Spiderboss8.1.1", null, true, true, true, Utility.Map.MapType.TwistedTreeline));
            JungleMobs.Add(new JungleMob("TT_Relic7.1.1", null, false, false, false, Utility.Map.MapType.TwistedTreeline));

            JungleMobs.Add(new JungleMob("TT_NWraith1.1.1", null, false, false, false, Utility.Map.MapType.TwistedTreeline));
            JungleMobs.Add(new JungleMob("TT_NWraith21.1.2", null, false, false, false, Utility.Map.MapType.TwistedTreeline));
            JungleMobs.Add(new JungleMob("TT_NWraith21.1.3", null, false, false, false, Utility.Map.MapType.TwistedTreeline));
            JungleMobs.Add(new JungleMob("TT_NGolem2.1.1", null, false, false, false, Utility.Map.MapType.TwistedTreeline));
            JungleMobs.Add(new JungleMob("TT_NGolem22.1.2", null, false, false, false, Utility.Map.MapType.TwistedTreeline));
            JungleMobs.Add(new JungleMob("TT_NWolf3.1.1", null, false, false, false, Utility.Map.MapType.TwistedTreeline));
            JungleMobs.Add(new JungleMob("TT_NWolf23.1.2", null, false, false, false, Utility.Map.MapType.TwistedTreeline));
            JungleMobs.Add(new JungleMob("TT_NWolf23.1.3", null, false, false, false, Utility.Map.MapType.TwistedTreeline));

            JungleMobs.Add(new JungleMob("TT_NWraith4.1.1", null, false, false, false, Utility.Map.MapType.TwistedTreeline));
            JungleMobs.Add(new JungleMob("TT_NWraith24.1.2", null, false, false, false, Utility.Map.MapType.TwistedTreeline));
            JungleMobs.Add(new JungleMob("TT_NWraith24.1.3", null, false, false, false, Utility.Map.MapType.TwistedTreeline));
            JungleMobs.Add(new JungleMob("TT_NGolem5.1.1", null, false, false, false, Utility.Map.MapType.TwistedTreeline));
            JungleMobs.Add(new JungleMob("TT_NGolem25.1.2", null, false, false, false, Utility.Map.MapType.TwistedTreeline));
            JungleMobs.Add(new JungleMob("TT_NWolf6.1.1", null, false, false, false, Utility.Map.MapType.TwistedTreeline));
            JungleMobs.Add(new JungleMob("TT_NWolf26.1.2", null, false, false, false, Utility.Map.MapType.TwistedTreeline));
            JungleMobs.Add(new JungleMob("TT_NWolf26.1.3", null, false, false, false, Utility.Map.MapType.TwistedTreeline));


            JungleCamps.Add(new JungleCamp("blue", GameObjectTeam.Order, 1, 115, 300, Utility.Map.MapType.SummonersRift,
                new Vector3(3791, 7901, 52), new Vector3(3641.904f, 8098.774f, 50.87435f),
                new[]
                {
                    GetJungleMobByName("SRU_Blue1.1.1", Utility.Map.MapType.SummonersRift),
                    GetJungleMobByName("SRU_BlueMini1.1.2", Utility.Map.MapType.SummonersRift),
                    GetJungleMobByName("SRU_BlueMini21.1.3", Utility.Map.MapType.SummonersRift)
                }));
            JungleCamps.Add(new JungleCamp("wolves", GameObjectTeam.Order, 2, 115, 100, Utility.Map.MapType.SummonersRift,
                new Vector3(3826, 6481, 52), new Vector3(3793.65f, 6580.254f, 52.46237f),
                new[]
                {
                    GetJungleMobByName("SRU_Murkwolf2.1.1", Utility.Map.MapType.SummonersRift),
                    GetJungleMobByName("SRU_MurkwolfMini2.1.2", Utility.Map.MapType.SummonersRift),
                    GetJungleMobByName("SRU_MurkwolfMini2.1.3", Utility.Map.MapType.SummonersRift)
                }));
            JungleCamps.Add(new JungleCamp("wraiths", GameObjectTeam.Order, 3, 115, 100, Utility.Map.MapType.SummonersRift,
                new Vector3(6981, 5460, 53), new Vector3(7030.898f, 5567.907f, 57.33538f),
                new[]
                {
                    GetJungleMobByName("SRU_Razorbeak3.1.1", Utility.Map.MapType.SummonersRift),
                    GetJungleMobByName("SRU_RazorbeakMini3.1.2", Utility.Map.MapType.SummonersRift),
                    GetJungleMobByName("SRU_RazorbeakMini3.1.3", Utility.Map.MapType.SummonersRift),
                    GetJungleMobByName("SRU_RazorbeakMini3.1.4", Utility.Map.MapType.SummonersRift)
                }));
            JungleCamps.Add(new JungleCamp("red", GameObjectTeam.Order, 4, 115, 300, Utility.Map.MapType.SummonersRift,
                new Vector3(7752, 4010, 54), new Vector3(7739.046f, 4100.004f, 54.0293f),
                new[]
                {
                    GetJungleMobByName("SRU_Red4.1.1", Utility.Map.MapType.SummonersRift),
                    GetJungleMobByName("SRU_RedMini4.1.2", Utility.Map.MapType.SummonersRift),
                    GetJungleMobByName("SRU_RedMini4.1.3", Utility.Map.MapType.SummonersRift)
                }));
            JungleCamps.Add(new JungleCamp("golems", GameObjectTeam.Order, 5, 115, 100, Utility.Map.MapType.SummonersRift,
                new Vector3(8422f, 2654f, 50.67624f), new Vector3(8295.447f, 2783.954f, 51.13f),
                new[]
                {
                    GetJungleMobByName("SRU_Krug5.1.2", Utility.Map.MapType.SummonersRift),
                    GetJungleMobByName("SRU_KrugMini5.1.1", Utility.Map.MapType.SummonersRift)
                }));
            JungleCamps.Add(new JungleCamp("wight", GameObjectTeam.Order, 13, 115, 100, Utility.Map.MapType.SummonersRift,
                new Vector3(2147, 8456, 51), new Vector3(2175.026f, 8554.33f, 51.77746f),
                new[] { GetJungleMobByName("SRU_Gromp13.1.1", Utility.Map.MapType.SummonersRift) }));
            JungleCamps.Add(new JungleCamp("blue", GameObjectTeam.Chaos, 7, 115, 300, Utility.Map.MapType.SummonersRift,
                new Vector3(10989, 6944, 51), new Vector3(11077.46f, 7086.427f, 51.72424f),
                new[]
                {
                    GetJungleMobByName("SRU_Blue7.1.1", Utility.Map.MapType.SummonersRift),
                    GetJungleMobByName("SRU_BlueMini7.1.2", Utility.Map.MapType.SummonersRift),
                    GetJungleMobByName("SRU_BlueMini27.1.3", Utility.Map.MapType.SummonersRift)
                }));
            JungleCamps.Add(new JungleCamp("wolves", GameObjectTeam.Chaos, 8, 115, 100, Utility.Map.MapType.SummonersRift,
                new Vector3(10991, 8348, 62), new Vector3(11128.04f, 8503.731f, 59.90768f),
                new[]
                {
                    GetJungleMobByName("SRU_Murkwolf8.1.1", Utility.Map.MapType.SummonersRift),
                    GetJungleMobByName("SRU_MurkwolfMini8.1.2", Utility.Map.MapType.SummonersRift),
                    GetJungleMobByName("SRU_MurkwolfMini8.1.3", Utility.Map.MapType.SummonersRift)
                }));
            JungleCamps.Add(new JungleCamp("wraiths", GameObjectTeam.Chaos, 9, 115, 100,
                Utility.Map.MapType.SummonersRift, new Vector3(7884, 9466, 52), new Vector3(7789.627f, 9819.764f, 51.45492f),
                new[]
                {
                    GetJungleMobByName("SRU_Razorbeak9.1.1", Utility.Map.MapType.SummonersRift),
                    GetJungleMobByName("SRU_RazorbeakMini9.1.2", Utility.Map.MapType.SummonersRift),
                    GetJungleMobByName("SRU_RazorbeakMini9.1.3", Utility.Map.MapType.SummonersRift),
                    GetJungleMobByName("SRU_RazorbeakMini9.1.4", Utility.Map.MapType.SummonersRift)
                }));
            JungleCamps.Add(new JungleCamp("red", GameObjectTeam.Chaos, 10, 115, 300, Utility.Map.MapType.SummonersRift,
                new Vector3(7106, 10930, 56), new Vector3(7164.198f, 11113.5f, 1093.54f),
                new[]
                {
                    GetJungleMobByName("SRU_Red10.1.1", Utility.Map.MapType.SummonersRift),
                    GetJungleMobByName("SRU_RedMini10.1.2", Utility.Map.MapType.SummonersRift),
                    GetJungleMobByName("SRU_RedMini10.1.3", Utility.Map.MapType.SummonersRift)
                }));
            JungleCamps.Add(new JungleCamp("golems", GameObjectTeam.Chaos, 11, 115, 100,
                Utility.Map.MapType.SummonersRift, new Vector3(6476, 12268, 56), new Vector3(6508.562f, 12127.83f, 1185.667f),
                new[]
                {
                    GetJungleMobByName("SRU_Krug11.1.2", Utility.Map.MapType.SummonersRift),
                    GetJungleMobByName("SRU_KrugMini11.1.1", Utility.Map.MapType.SummonersRift)
                }));
            JungleCamps.Add(new JungleCamp("wight", GameObjectTeam.Chaos, 14, 115, 100, Utility.Map.MapType.SummonersRift,
                new Vector3(12668, 6360, 51), new Vector3(12671.58f, 6617.756f, 1118.074f),
                new[] { GetJungleMobByName("SRU_Gromp14.1.1", Utility.Map.MapType.SummonersRift) }));
            JungleCamps.Add(new JungleCamp("crab", GameObjectTeam.Neutral, 15, 2 * 60 + 30, 180, Utility.Map.MapType.SummonersRift,
                new Vector3(10586, 5114, -62), new Vector3(10501, 5159, -62),
                new[] { GetJungleMobByName("SRU_Crab15.1.1", Utility.Map.MapType.SummonersRift) }));
            JungleCamps.Add(new JungleCamp("crab", GameObjectTeam.Neutral, 16, 2 * 60 + 30, 180, Utility.Map.MapType.SummonersRift,
                new Vector3(4402, 9589, -66), new Vector3(4274, 9696, -68),
                new[] { GetJungleMobByName("SRU_Crab16.1.1", Utility.Map.MapType.SummonersRift) }));
            JungleCamps.Add(new JungleCamp("dragon", GameObjectTeam.Neutral, 6, 2 * 60 + 30, 360,
                Utility.Map.MapType.SummonersRift, new Vector3(10116, 4438, -71), new Vector3(10109.18f, 4850.93f, 1032.274f),
                new[] { GetJungleMobByName("SRU_Dragon6.1.1", Utility.Map.MapType.SummonersRift) }));
            JungleCamps.Add(new JungleCamp("nashor", GameObjectTeam.Neutral, 12, 20 * 60, 420,
                Utility.Map.MapType.SummonersRift, new Vector3(4940, 10406, -71), new Vector3(4951.034f, 10831.035f, 1027.482f),
                new[] { GetJungleMobByName("SRU_Baron12.1.1", Utility.Map.MapType.SummonersRift) }));

            JungleCamps.Add(new JungleCamp("wraiths", GameObjectTeam.Order, 1, 100, 75,
                Utility.Map.MapType.TwistedTreeline, new Vector3(4270, 5871, -106), new Vector3(4414, 5774, 60),
                new[]
                {
                    GetJungleMobByName("TT_NWraith1.1.1", Utility.Map.MapType.TwistedTreeline),
                    GetJungleMobByName("TT_NWraith21.1.2", Utility.Map.MapType.TwistedTreeline),
                    GetJungleMobByName("TT_NWraith21.1.3", Utility.Map.MapType.TwistedTreeline)
                }));
            JungleCamps.Add(new JungleCamp("golems", GameObjectTeam.Order, 2, 100, 75,
                Utility.Map.MapType.TwistedTreeline, new Vector3(5034, 7929, -107), new Vector3(5088, 8065, 60),
                new[]
                {
                    GetJungleMobByName("TT_NGolem2.1.1", Utility.Map.MapType.TwistedTreeline),
                    GetJungleMobByName("TT_NGolem22.1.2", Utility.Map.MapType.TwistedTreeline)
                }));
            JungleCamps.Add(new JungleCamp("wolves", GameObjectTeam.Order, 3, 100, 75,
                Utility.Map.MapType.TwistedTreeline, new Vector3(6014, 6183, -98), new Vector3(6148, 5993, 60),
                new[]
                {
                    GetJungleMobByName("TT_NWolf3.1.1", Utility.Map.MapType.TwistedTreeline),
                    GetJungleMobByName("TT_NWolf23.1.2", Utility.Map.MapType.TwistedTreeline),
                    GetJungleMobByName("TT_NWolf23.1.3", Utility.Map.MapType.TwistedTreeline)
                }));
            JungleCamps.Add(new JungleCamp("wraiths", GameObjectTeam.Chaos, 4, 100, 75,
                Utility.Map.MapType.TwistedTreeline, new Vector3(11022, 5815, -107), new Vector3(11008, 5775, 60),
                new[]
                {
                    GetJungleMobByName("TT_NWraith4.1.1", Utility.Map.MapType.TwistedTreeline),
                    GetJungleMobByName("TT_NWraith24.1.2", Utility.Map.MapType.TwistedTreeline),
                    GetJungleMobByName("TT_NWraith24.1.3", Utility.Map.MapType.TwistedTreeline)
                }));
            JungleCamps.Add(new JungleCamp("golems", GameObjectTeam.Chaos, 5, 100, 75,
                Utility.Map.MapType.TwistedTreeline, new Vector3(10332, 7925, -108), new Vector3(10341, 8084, 60),
                new[]
                {
                    GetJungleMobByName("TT_NGolem5.1.1", Utility.Map.MapType.TwistedTreeline),
                    GetJungleMobByName("TT_NGolem25.1.2", Utility.Map.MapType.TwistedTreeline)
                }));
            JungleCamps.Add(new JungleCamp("wolves", GameObjectTeam.Chaos, 6, 100, 75,
                Utility.Map.MapType.TwistedTreeline, new Vector3(9394, 6085, -95), new Vector3(9239, 6022, 60),
                new[]
                {
                    GetJungleMobByName("TT_NWolf6.1.1", Utility.Map.MapType.TwistedTreeline),
                    GetJungleMobByName("TT_NWolf26.1.2", Utility.Map.MapType.TwistedTreeline),
                    GetJungleMobByName("TT_NWolf26.1.3", Utility.Map.MapType.TwistedTreeline)
                }));
            JungleCamps.Add(new JungleCamp("heal", GameObjectTeam.Neutral, 7, 115, 90,
                Utility.Map.MapType.TwistedTreeline, new Vector3(7712, 6713, -69), new Vector3(7711, 6722, 60),
                new[] { GetJungleMobByName("TT_Relic7.1.1", Utility.Map.MapType.TwistedTreeline) }));
            JungleCamps.Add(new JungleCamp("vilemaw", GameObjectTeam.Neutral, 8, 10 * 60, 300,
                Utility.Map.MapType.TwistedTreeline, new Vector3(7726, 9937, -79), new Vector3(7711, 10080, 60),
                new[] { GetJungleMobByName("TT_Spiderboss8.1.1", Utility.Map.MapType.TwistedTreeline) }));


            foreach (GameObject objAiBase in ObjectManager.Get<GameObject>())
            {
                //Obj_AI_Base_OnCreate(objAiBase, new EventArgs());
            }

            foreach (var hero in ObjectManager.Get<AIHeroClient>())
            {
                if (!hero.IsEnemy)
                    continue;

                Enemies.Add(new PlayerBossMobBuff(hero));
            }

        }

        public class JungleCamp
        {
            
            public JungleCamp(String name, GameObjectTeam team, int campId, int spawnTime, int respawnTime,
                Utility.Map.MapType mapType, Vector3 mapPosition, Vector3 minimapPosition, JungleMob[] creeps)
            {
                
            }

        }

        public class JungleMob
        {
            public bool Boss;
            public bool Buff;
            public Utility.Map.MapType MapType;
            public String Name;
            public Obj_AI_Minion Obj;
            public bool Smite;

            public JungleMob(string name, Obj_AI_Minion obj, bool smite, bool buff, bool boss,
                Utility.Map.MapType mapType)
            {
                Name = name;
                Obj = obj;
                Smite = smite;
                Buff = buff;
                Boss = boss;
                MapType = mapType;
            }
        }

        public class PlayerBossMobBuff
        {
            public int DragonBuff = 0;
            public bool NashorBuff = false;
            public AIHeroClient Hero;

            public PlayerBossMobBuff(AIHeroClient hero)
            {
                Hero = hero;
            }

            public int GetDragonStacks()
            {
                var buff = Hero.Buffs.FirstOrDefault(x => x.Name == "s5test_dragonslayerbuff");
                if (buff == null)
                    return 0;
                else
                    return buff.Count;
            }
            public bool HasNashorBuff()
            {
                return Hero.Buffs.Any(x => x.Name == "exaltedwithbaronnashor");
            }
        }
    }
}
