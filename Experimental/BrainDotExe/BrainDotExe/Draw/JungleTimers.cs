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
using EloBuddy.SDK.Menu;
using BrainDotExe.Util;

namespace BrainDotExe.Draw
{
    public static class JungleTimers
    {
        public static Menu JungleTimersMenu;

        public static Font MinimapText;

        public static Font MapText;

        public static Utility.Map.MapType MapType { get; set; }

        public static Jungle.Camp DragonCamp;
        public static Jungle.Camp BaronCamp;

        public static List<int> OnAttackList;
        public static List<int> MissileHitList;
        public static List<int[]> OnCreateGrompList;
        public static List<int[]> OnCreateCampIconList;
        public static List<int[]> PossibleBaronList;
        public static List<int> PossibleDragonList;

        public static float ClockTimeAdjust;
        public static int UpdateTick;

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static void Init()
        {
            JungleTimersMenu = Program.Menu.AddSubMenu("Jungle Timers ", "jungleTimersDraw");
            JungleTimersMenu.AddGroupLabel("Jungle Timers");
            JungleTimersMenu.Add("drawjungleTime", new CheckBox("Jungle times in minimap", true));

            MinimapText = new Font(Drawing.Direct3DDevice,
                        new FontDescription
                        {
                            FaceName = "Calibri",
                            Height = 12,
                            OutputPrecision = FontPrecision.Default,
                            Quality = FontQuality.Default
                        });

            MapText = new Font(Drawing.Direct3DDevice,
                    new FontDescription
                    {
                        FaceName = "Calibri",
                        Height = 12,
                        OutputPrecision = FontPrecision.Default,
                        Quality = FontQuality.Default
                    });

            #region Set Dragon/Baron Camp
            foreach (var camp in Jungle.Camps.Where(camp => camp.MapType.ToString() == "SummonersRift"))
            {
                if (camp.Name == "Dragon")
                {
                    DragonCamp = camp;
                }
                else if (camp.Name == "Baron")
                {
                    BaronCamp = camp;
                }
            }
            #endregion

            #region
            foreach (Obj_AI_Minion minion in ObjectManager.Get<Obj_AI_Minion>().Where(x => x.Name.Contains("SRU_") || x.Name.Contains("Sru_")))
            {
                foreach (var camp in Jungle.Camps.Where(camp => camp.MapType.ToString() == Game.MapId.ToString()))
                {
                    foreach (var mob in camp.Mobs)
                    {
                        if (mob.Name.Contains(minion.Name) && !minion.IsDead && mob.NetworkId != minion.NetworkId)
                        {
                            mob.NetworkId = minion.NetworkId;

                            mob.LastChangeOnState = Environment.TickCount;
                            mob.Unit = minion;

                            if (!camp.IsRanged && camp.Mobs.Count > 1)
                            {
                                mob.State = 6;
                            }
                            else
                            {
                                mob.State = 5;
                            }

                            if (camp.Mobs.Count == 1)
                            {
                                camp.State = mob.State;
                                camp.LastChangeOnState = mob.LastChangeOnState;
                            }
                        }
                    }
                }
            }
            #endregion

            Drawing.OnEndScene += JungleTimers_OnEndScene;
            GameObject.OnCreate += GameObjectOnCreate;
            GameObject.OnDelete += GameObjectOnDelete;
            Game.OnUpdate += OnGameUpdate;
        }

        public static void GameObjectOnCreate(GameObject sender, EventArgs args)
        {
            if (!(sender is Obj_AI_Minion) || sender.Team != GameObjectTeam.Neutral)
            {
                return;
            }

            var minion = (Obj_AI_Minion)sender;

            foreach (var camp in Jungle.Camps.Where(camp => camp.MapType.ToString() == Game.MapId.ToString()))
            {
                //Do Stuff for each camp

                foreach (var mob in camp.Mobs.Where(mob => mob.Name == minion.Name))
                {
                    //Do Stuff for each mob in a camp

                    mob.NetworkId = minion.NetworkId;
                    mob.LastChangeOnState = Environment.TickCount;
                    mob.Unit = minion;
                    if (!minion.IsDead)
                    {
                        mob.State = 3;
                        mob.JustDied = false;
                    }
                    else
                    {
                        mob.State = 7;
                        mob.JustDied = true;
                    }

                    if (camp.Mobs.Count == 1)
                    {
                        camp.State = mob.State;
                        camp.LastChangeOnState = mob.LastChangeOnState;
                    }

                    if (mob.Name.Contains("Baron") && PossibleBaronList.Count >= 1)
                    {
                        try
                        {
                            PossibleBaronList.Clear();
                        }
                        catch (Exception)
                        {
                            //ignored
                        }
                    }
                }
            }
        }

        public static void GameObjectOnDelete(GameObject sender, EventArgs args)
        {
            if (!(sender is Obj_AI_Minion) || sender.Team != GameObjectTeam.Neutral)
            {
                return;
            }

            var minion = (Obj_AI_Minion)sender;

            foreach (var camp in Jungle.Camps.Where(camp => camp.MapType.ToString() == Game.MapId.ToString()))
            {
                //Do Stuff for each camp

                foreach (var mob in camp.Mobs.Where(mob => mob.Name == minion.Name))
                {
                    //Do Stuff for each mob in a camp

                    mob.LastChangeOnState = Environment.TickCount - 3000;
                    mob.Unit = null;
                    if (mob.State != 7)
                    {
                        mob.State = 7;
                        mob.JustDied = true;
                    }

                    if (camp.Mobs.Count == 1)
                    {
                        camp.State = mob.State;
                        camp.LastChangeOnState = mob.LastChangeOnState;
                    }
                }
            }
        }

        public static void OnGameUpdate(EventArgs args)
        {
            if (Environment.TickCount > UpdateTick )
            {
                var enemy = HeroManager.Enemies.FirstOrDefault(x => x.IsValidTarget());

                foreach (var camp in Jungle.Camps.Where(camp => camp.MapType.ToString() == Game.MapId.ToString()))
                {
                    #region Update States

                    int mobCount = 0;
                    bool firstMob = true;
                    int visibleMobsCount = 0;
                    int rangedMobsCount = 0;
                    int deadRangedMobsCount = 0;

                    foreach (var mob in camp.Mobs)
                    {
                        //Do Stuff for each mob in a camp

                        try
                        {
                            if (mob.Unit != null && mob.Unit.IsVisible)
                            {
                                visibleMobsCount++;
                            }
                        }
                        catch (Exception)
                        {
                            //ignored
                        }


                        if (mob.IsRanged)
                        {
                            rangedMobsCount++;

                            if (mob.JustDied)
                            {
                                deadRangedMobsCount++;
                            }
                        }

                        bool visible = false;

                        mobCount += 1;

                        int guessedTimetoDead = 3000;

                        if (camp.Name == "Dragon")
                        {
                            if (Game.Time - ClockTimeAdjust < 420f) guessedTimetoDead = 60000;
                            else if (Game.Time - ClockTimeAdjust < 820f) guessedTimetoDead = 40000;
                            else guessedTimetoDead = 15000;
                        }

                        if (camp.Name == "Baron")
                        {
                            guessedTimetoDead = 5000;
                        }


                        switch (mob.State)
                        {
                            case 1:
                                if ((Environment.TickCount - mob.LastChangeOnState) >= guessedTimetoDead && camp.Name != "Crab")
                                {
                                    if (camp.Name == "Dragon")
                                    {
                                        try
                                        {
                                            if (mob.Unit != null && !mob.Unit.IsVisible && enemy == null)
                                            {
                                                mob.State = 4;
                                                mob.LastChangeOnState = Environment.TickCount - 2000;
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            //ignored
                                        }
                                    }
                                    else if (camp.Name == "Baron")
                                    {
                                        mob.State = 5;
                                        mob.LastChangeOnState = Environment.TickCount - 2000;
                                    }
                                    else
                                    {
                                        mob.State = 4;
                                        mob.LastChangeOnState = Environment.TickCount - 2000;
                                    }
                                }

                                if ((Environment.TickCount - mob.LastChangeOnState >= 10000 && camp.Name == "Crab"))
                                {
                                    mob.State = 3;
                                    mob.LastChangeOnState = Environment.TickCount;
                                }
                                break;
                            case 2:
                                if (Environment.TickCount - mob.LastChangeOnState >= 4000)
                                {
                                    if (!camp.IsRanged && camp.Mobs.Count > 1)
                                    {
                                        mob.State = 6;
                                    }
                                    else
                                    {
                                        mob.State = 5;
                                    }
                                    mob.LastChangeOnState = Environment.TickCount;
                                }
                                break;
                            case 4:
                                if (Environment.TickCount - mob.LastChangeOnState >= 5000)
                                {
                                    mob.State = 7;
                                    mob.JustDied = true;
                                }
                                break;
                            case 5:
                                if (Environment.TickCount - mob.LastChangeOnState >= 45000)
                                {
                                    mob.State = 6;
                                }
                                if (mob.Unit != null && mob.Unit.IsVisible && !mob.Unit.IsDead)
                                {
                                    mob.State = 3;
                                }
                                break;
                            case 6:
                                if (mob.Unit != null && mob.Unit.IsVisible && !mob.Unit.IsDead)
                                {
                                    mob.State = 3;
                                }
                                break;
                            default:
                                break;
                        }

                        if (mob.Unit != null && mob.Unit.IsVisible && !mob.Unit.IsDead)
                        {
                            visible = true;
                        }

                        if ((mob.State == 7 || mob.State == 4) && visible) //check again
                        {
                            mob.State = 3;
                            mob.LastChangeOnState = Environment.TickCount;
                            mob.JustDied = false;
                        }

                        if (camp.Mobs.Count == 1)
                        {
                            camp.State = mob.State;
                            camp.LastChangeOnState = mob.LastChangeOnState;
                        }

                        if (camp.IsRanged && camp.Mobs.Count > 1 && mob.State > 0)
                        {
                            if (visible)
                            {
                                if (firstMob)
                                {
                                    camp.State = mob.State;
                                    camp.LastChangeOnState = mob.LastChangeOnState;
                                    firstMob = false;
                                }
                                else if (!firstMob)
                                {
                                    if (mob.State < camp.State)
                                    {
                                        camp.State = mob.State;
                                    }
                                    if (mob.LastChangeOnState > camp.LastChangeOnState)
                                    {
                                        camp.LastChangeOnState = mob.LastChangeOnState;
                                    }
                                }

                                if (!mob.IsRanged)
                                {
                                    camp.LastChangeOnState = Environment.TickCount;
                                    camp.RespawnTime = (camp.LastChangeOnState + camp.RespawnTimer * 1000);
                                }
                            }
                            else
                            {
                                if (firstMob)
                                {
                                    if (mob.IsRanged)
                                    {
                                        camp.State = mob.State;
                                        firstMob = false;
                                    }
                                    camp.LastChangeOnState = mob.LastChangeOnState;
                                }
                                else if (!firstMob)
                                {
                                    if (mob.State < camp.State && mob.IsRanged)
                                    {
                                        camp.State = mob.State;
                                    }
                                    if (mob.LastChangeOnState > camp.LastChangeOnState)
                                    {
                                        camp.LastChangeOnState = mob.LastChangeOnState;
                                    }
                                }
                            }
                        }
                        else if (!camp.IsRanged && camp.Mobs.Count > 1 && mob.State > 0)
                        {
                            if (firstMob)
                            {
                                camp.State = mob.State;
                                camp.LastChangeOnState = mob.LastChangeOnState;
                                firstMob = false;
                            }
                            else
                            {
                                if (mob.State < camp.State)
                                {
                                    camp.State = mob.State;
                                }
                                if (mob.LastChangeOnState > camp.LastChangeOnState)
                                {
                                    camp.LastChangeOnState = mob.LastChangeOnState;
                                }
                            }
                            if (visible)
                            {
                                camp.LastChangeOnState = Environment.TickCount;
                                camp.RespawnTime = (camp.LastChangeOnState + camp.RespawnTimer * 1000);
                            }
                        }

                        if (visible && camp.RespawnTime > Environment.TickCount)
                        {
                            camp.RespawnTime = (Environment.TickCount + camp.RespawnTimer * 1000);
                        }
                    }


                    //Do Stuff for each camp

                    if (camp.State == 7)
                    {
                        int mobsJustDiedCount = 0;

                        for (int i = 0; i < mobCount; i++)
                        {
                            try
                            {
                                if (camp.Mobs[i].JustDied)
                                {
                                    mobsJustDiedCount++;
                                }
                            }
                            catch (Exception)
                            {
                                //ignored
                            }

                        }

                        if (mobsJustDiedCount == mobCount)
                        {
                            camp.RespawnTime = (camp.LastChangeOnState + camp.RespawnTimer * 1000);

                            for (int i = 0; i < mobCount; i++)
                            {
                                try
                                {
                                    camp.Mobs[i].JustDied = false;
                                }
                                catch (Exception)
                                {
                                    //ignored
                                }
                            }
                        }
                    }

                    if (camp.IsRanged && visibleMobsCount == 0 && rangedMobsCount == deadRangedMobsCount)
                    {
                        camp.RespawnTime = (camp.LastChangeOnState + camp.RespawnTimer * 1000);

                        for (int i = 0; i < mobCount; i++)
                        {
                            try
                            {
                                camp.Mobs[i].JustDied = false;
                            }
                            catch (Exception)
                            {
                                //ignored
                            }

                        }
                    }

                    if (camp.Name == "Baron" && PossibleBaronList.Count >= 1 && camp.State >= 1 && camp.State <= 3)
                    {
                        try
                        {
                            PossibleBaronList.Clear();
                        }
                        catch (Exception)
                        {
                            //ignored
                        }
                    }

                    #endregion

                    #region Timers

                    if (camp.RespawnTime > Environment.TickCount && camp.State == 7)
                    {
                        var timespan = TimeSpan.FromSeconds((camp.RespawnTime - Environment.TickCount) / 1000f);

                        bool format = false;

                        if (camp.Position.IsOnScreen())
                        {

                            camp.Timer.TextOnMap = string.Format(format ? "{1}" : "{0}:{1:00}", (int)timespan.TotalMinutes,
                                format ? (int)timespan.TotalSeconds : timespan.Seconds);
                        }

                        camp.Timer.TextOnMinimap = string.Format(format ? "{1}" : "{0}:{1:00}",
                            (int)timespan.TotalMinutes,
                            format ? (int)timespan.TotalSeconds : timespan.Seconds);

                        var textrect = MapText.MeasureText(null, camp.Timer.TextOnMinimap, FontDrawFlags.Center);

                        camp.Timer.MinimapPosition = new Vector2((int)(camp.MinimapPosition.X - textrect.Width / 2f), (int)(camp.MinimapPosition.Y - textrect.Height / 2f));
                        
                    }

                    if (camp.Position.IsOnScreen())
                    {
                        var textrect = MapText.MeasureText(null, camp.Timer.TextOnMap, FontDrawFlags.Center);

                        camp.Timer.Position = new Vector3((int)(camp.Position.X - textrect.Width / 2f), (int)(camp.Position.Y - textrect.Height / 2f), 0);
                    }

                    #endregion

                }

                UpdateTick = Environment.TickCount;
            }
        }

        private static void JungleTimers_OnEndScene(EventArgs args)
        {
            if (Misc.isChecked(Program.DrawMenu, "drawDisable")) return;

            if (!Misc.isChecked(JungleTimersMenu, "drawjungleTime"))
                return;

            foreach (var camp in Jungle.Camps.Where(camp => camp.MapType.ToString() == Game.MapId.ToString()))
            {

                #region Timers

                if (camp.RespawnTime > Environment.TickCount && camp.State == 7)
                {
                    if (camp.Position.IsOnScreen())
                    {
                        try
                        {
                            var pos = Drawing.WorldToScreen(camp.Timer.Position);
                            MapText.DrawText(null, camp.Timer.TextOnMap, (int)pos.X, (int)pos.Y, Color.White);
                        }
                        catch (Exception)
                        {
                            //ingore
                        }
                    }
                        try
                        {
                            MinimapText.DrawText(null, camp.Timer.TextOnMinimap, (int)camp.Timer.MinimapPosition.X, (int)camp.Timer.MinimapPosition.Y, Color.White);
                        }
                        catch (Exception)
                        {
                            //ingore
                        }
                }

                #endregion
            }
        }

    }

    public class Jungle
    {
        public static List<Camp> Camps;

        static Jungle()
        {
            try
            {
                Camps = new List<Camp>
                {
                    // Order: Blue
                    new Camp("Blue",
                        115, 300, new Vector3(3872f, 7900f, 51f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("SRU_Blue1.1.1"),
                                new Mob("SRU_BlueMini1.1.2", true),
                                new Mob("SRU_BlueMini21.1.3", true)
                            }),
                        Utility.Map.MapType.SummonersRift,
                        GameObjectTeam.Order,
                        Color.Cyan, new Timers(new Vector3(0,0,0),new Vector2(0,0)), true),
                    //Order: Wolves
                    new Camp("Wolves",
                        115, 100, new Vector3(3825f, 6491f, 52f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("SRU_Murkwolf2.1.1"),
                                new Mob("SRU_MurkwolfMini2.1.2"),
                                new Mob("SRU_MurkwolfMini2.1.3")
                            }),
                        Utility.Map.MapType.SummonersRift,
                        GameObjectTeam.Order,
                        Color.White, new Timers(new Vector3(0,0,0),new Vector2(0,0))),
                    //Order: Raptor
                    new Camp("Raptor",
                        115, 100, new Vector3(6954f, 5458f, 53f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("SRU_Razorbeak3.1.1", true),
                                new Mob("SRU_RazorbeakMini3.1.2"),
                                new Mob("SRU_RazorbeakMini3.1.3"),
                                new Mob("SRU_RazorbeakMini3.1.4")
                            }),
                        Utility.Map.MapType.SummonersRift,
                        GameObjectTeam.Order,
                        Color.Salmon, new Timers(new Vector3(0,0,0),new Vector2(0,0)), true),
                    //Order: Red
                    new Camp("Red",
                        115, 300, new Vector3(7862f, 4111f, 54f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("SRU_Red4.1.1"),
                                new Mob("SRU_RedMini4.1.2", true),
                                new Mob("SRU_RedMini4.1.3", true)
                            }),
                        Utility.Map.MapType.SummonersRift,
                        GameObjectTeam.Order,
                        Color.Red, new Timers(new Vector3(0,0,0),new Vector2(0,0)), true),
                        
                    //Order: Krug
                    new Camp("Krug",
                        115, 100, new Vector3(8381f, 2711f, 51f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("SRU_Krug5.1.2"),
                                new Mob("SRU_KrugMini5.1.1")
                            }),
                        Utility.Map.MapType.SummonersRift,
                        GameObjectTeam.Order,
                        Color.White, new Timers(new Vector3(0,0,0),new Vector2(0,0))),
                    //Order: Gromp
                    new Camp("Gromp",
                        115, 100, new Vector3(2091f, 8428f, 52f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("SRU_Gromp13.1.1", true)
                            }),
                        Utility.Map.MapType.SummonersRift,
                        GameObjectTeam.Order,
                        Color.Green, new Timers(new Vector3(0,0,0),new Vector2(0,0)), true),
                    //Chaos: Blue
                    new Camp("Blue",
                        115, 300, new Vector3(10930f, 6992f, 52f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("SRU_Blue7.1.1"),
                                new Mob("SRU_BlueMini7.1.2", true),
                                new Mob("SRU_BlueMini27.1.3", true)
                            }),
                        Utility.Map.MapType.SummonersRift,
                        GameObjectTeam.Chaos,
                        Color.Cyan, new Timers(new Vector3(0,0,0),new Vector2(0,0)), true),
                    //Chaos: Wolves
                    new Camp("Wolves",
                        115, 100, new Vector3(10957f, 8350f, 62f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("SRU_Murkwolf8.1.1"),
                                new Mob("SRU_MurkwolfMini8.1.2"),
                                new Mob("SRU_MurkwolfMini8.1.3")
                            }),
                        Utility.Map.MapType.SummonersRift,
                        GameObjectTeam.Chaos,
                        Color.White, new Timers(new Vector3(0,0,0),new Vector2(0,0))),
                    //Chaos: Raptor
                    new Camp("Raptor",
                        115, 100, new Vector3(7857f, 9471f, 52f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("SRU_Razorbeak9.1.1", true),
                                new Mob("SRU_RazorbeakMini9.1.2"),
                                new Mob("SRU_RazorbeakMini9.1.3"),
                                new Mob("SRU_RazorbeakMini9.1.4")
                            }),
                        Utility.Map.MapType.SummonersRift,
                        GameObjectTeam.Chaos,
                        Color.Salmon, new Timers(new Vector3(0,0,0),new Vector2(0,0)), true),
                    //Chaos: Red
                    new Camp("Red",
                        115, 300, new Vector3(7017f, 10775f, 56f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("SRU_Red10.1.1"),
                                new Mob("SRU_RedMini10.1.2", true),
                                new Mob("SRU_RedMini10.1.3", true)
                            }),
                        Utility.Map.MapType.SummonersRift,
                        GameObjectTeam.Chaos,
                        Color.Red, new Timers(new Vector3(0,0,0),new Vector2(0,0)), true),
                    //Chaos: Krug
                    new Camp("Krug",
                        115, 100, new Vector3(6449f, 12117f, 56f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("SRU_Krug11.1.2"),
                                new Mob("SRU_KrugMini11.1.1")
                            }),
                        Utility.Map.MapType.SummonersRift,
                        GameObjectTeam.Chaos,
                        Color.White, new Timers(new Vector3(0,0,0),new Vector2(0,0))),
                    //Chaos: Gromp
                    new Camp("Gromp",
                        115, 100, new Vector3(12703f, 6444f, 52f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("SRU_Gromp14.1.1", true)
                            }),
                        Utility.Map.MapType.SummonersRift,
                        GameObjectTeam.Chaos,
                        Color.Green, new Timers(new Vector3(0,0,0),new Vector2(0,0)), true),
                    //Neutral: Dragon
                    new Camp("Dragon",
                        150, 360, new Vector3(9866f, 4414f, -71f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("SRU_Dragon6.1.1")
                            }),
                        Utility.Map.MapType.SummonersRift,
                        GameObjectTeam.Neutral,
                        Color.Orange, new Timers(new Vector3(0,0,0),new Vector2(0,0))),
                    //Neutral: Baron
                    new Camp("Baron",
                        120, 420, new Vector3(5007f, 10471f, -71f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("SRU_Baron12.1.1", true, null, 0)
                            }),
                        Utility.Map.MapType.SummonersRift,
                        GameObjectTeam.Neutral,
                        Color.DarkOrchid, new Timers(new Vector3(0,0,0),new Vector2(0,0)), true,  8),
                    //Dragon: Crab
                    new Camp("Crab",
                        150, 180, new Vector3(10508f, 5271f, -62f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("Sru_Crab15.1.1")
                            }),
                        Utility.Map.MapType.SummonersRift,
                        GameObjectTeam.Neutral,
                        Color.PaleGreen, new Timers(new Vector3(0,0,0),new Vector2(0,0))),
                    //Baron: Crab
                    new Camp("Crab",
                        150, 180, new Vector3(4418f, 9664f, -69f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("Sru_Crab16.1.1")
                            }),
                        Utility.Map.MapType.SummonersRift,
                        GameObjectTeam.Neutral,
                        Color.PaleGreen, new Timers(new Vector3(0,0,0),new Vector2(0,0))),
                    //Order: Wraiths
                    new Camp("Wraiths",
                        95, 75, new Vector3(4373f, 5843f, -107f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("TT_NWraith1.1.1", true),
                                new Mob("TT_NWraith21.1.2", true),
                                new Mob("TT_NWraith21.1.3", true)
                            }),
                        Utility.Map.MapType.TwistedTreeline,
                        GameObjectTeam.Order,
                        Color.White, new Timers(new Vector3(0,0,0),new Vector2(0,0)), true),
                    //Order: Golems
                    new Camp("Golems",
                        95, 75, new Vector3(5107f, 7986f, -108f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("TT_NGolem2.1.1"),
                                new Mob("TT_NGolem22.1.2")
                            }),
                        Utility.Map.MapType.TwistedTreeline,
                        GameObjectTeam.Order,
                        Color.White, new Timers(new Vector3(0,0,0),new Vector2(0,0))),
                    //Order: Wolves
                    new Camp("Wolves",
                        95, 75, new Vector3(6078f, 6094f, -99f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("TT_NWolf3.1.1"),
                                new Mob("TT_NWolf23.1.2"),
                                new Mob("TT_NWolf23.1.3")
                            }),
                         Utility.Map.MapType.TwistedTreeline,
                         GameObjectTeam.Order,
                        Color.White, new Timers(new Vector3(0,0,0),new Vector2(0,0))),
                    //Chaos: Wraiths
                    new Camp("Wraiths",
                        95, 75, new Vector3(11026f, 5806f, -107f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("TT_NWraith4.1.1", true),
                                new Mob("TT_NWraith24.1.2", true),
                                new Mob("TT_NWraith24.1.3", true)
                            }),
                        Utility.Map.MapType.TwistedTreeline,
                        GameObjectTeam.Chaos,
                        Color.White, new Timers(new Vector3(0,0,0),new Vector2(0,0)), true),
                    //Chaos: Golems
                    new Camp("Golems",
                        95, 75, new Vector3(10277f, 8038f, -109f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("TT_NGolem5.1.1"),
                                new Mob("TT_NGolem25.1.2")
                            }),
                        Utility.Map.MapType.TwistedTreeline,
                        GameObjectTeam.Chaos,
                        Color.White, new Timers(new Vector3(0,0,0),new Vector2(0,0))),
                    //Chaos: Wolves
                    new Camp("Wolves",
                        95, 75, new Vector3(9294f, 6085f, -97f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("TT_NWolf6.1.1"),
                                new Mob("TT_NWolf26.1.2"),
                                new Mob("TT_NWolf26.1.3") }),
                         Utility.Map.MapType.TwistedTreeline,
                         GameObjectTeam.Chaos,
                        Color.White, new Timers(new Vector3(0,0,0),new Vector2(0,0))),
                    //Neutral: Spider
                    new Camp("Spider",
                        600, 360, new Vector3(7738f, 10080f, -62f),
                        new List<Mob>(
                            new[]
                            {
                                new Mob("TT_Spiderboss8.1.1")
                            }),
                        Utility.Map.MapType.TwistedTreeline,
                        GameObjectTeam.Neutral,
                        Color.DarkOrchid, new Timers(new Vector3(0,0,0),new Vector2(0,0)), true)
                };
            }
            catch (Exception)
            {
                Camps = new List<Camp>();
            }
        }

        public class Camp
        {
            public Camp(string name,
                float spawnTime,
                int respawnTimer,
                Vector3 position,
                List<Mob> mobs,
                Utility.Map.MapType mapType,
                GameObjectTeam team,
                Color colour,
                Timers timer,
                bool isRanged = false,
                int state = 0,
                int respawnTime = 0,
                int lastChangeOnState = 0,
                bool shouldping = true,
                int lastPing = 0)
            {
                Name = name;
                SpawnTime = spawnTime;
                RespawnTimer = respawnTimer;
                Position = position;
                MapPosition = Drawing.WorldToScreen(Position);
                MinimapPosition = Drawing.WorldToMinimap(Position);
                Mobs = mobs;
                MapType = mapType;
                Team = team;
                Colour = colour;
                IsRanged = isRanged;
                State = state;
                RespawnTime = respawnTime;
                LastChangeOnState = lastChangeOnState;
                Timer = timer;
                ShouldPing = shouldping;
                LastPing = lastPing;
            }

            public string Name { get; set; }
            public float SpawnTime { get; set; }
            public int RespawnTimer { get; set; }
            public Vector3 Position { get; set; }
            public Vector2 MinimapPosition { get; set; }
            public Vector2 MapPosition { get; set; }
            public List<Mob> Mobs { get; set; }
            public Utility.Map.MapType MapType { get; set; }
            public GameObjectTeam Team { get; set; }
            public Color Colour { get; set; }
            public bool IsRanged { get; set; }
            public int State { get; set; }
            public int RespawnTime { get; set; }
            public int LastChangeOnState { get; set; }
            public Timers Timer { get; set; }
            public bool ShouldPing { get; set; }
            public int LastPing { get; set; }
        }

        public class Mob
        {
            public Mob(string name, bool isRanged = false, Obj_AI_Minion unit = null, int state = 0, int networkId = 0, int lastChangeOnState = 0, bool justDied = false)
            {
                Name = name;
                IsRanged = isRanged;
                Unit = unit;
                State = state;
                NetworkId = networkId;
                LastChangeOnState = lastChangeOnState;
                JustDied = justDied;
            }

            public Obj_AI_Minion Unit { get; set; }
            public string Name { get; set; }
            public bool IsRanged { get; set; }
            public int State { get; set; }
            public int NetworkId { get; set; }
            public int LastChangeOnState { get; set; }
            public bool JustDied { get; set; }
        }

        public class Timers
        {
            public Timers(Vector3 position, Vector2 minimapPosition, string textOnMap = "", string textOnMinimap = "")
            {
                TextOnMap = textOnMap;
                TextOnMinimap = textOnMinimap;
                Position = position;
                MinimapPosition = minimapPosition;
            }

            public string TextOnMap { get; set; }
            public string TextOnMinimap { get; set; }
            public Vector3 Position { get; set; }
            public Vector2 MinimapPosition { get; set; }
        }
    }
}
