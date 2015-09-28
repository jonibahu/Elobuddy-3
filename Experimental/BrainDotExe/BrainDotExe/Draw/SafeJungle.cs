using BrainDotExe.Common;
using BrainDotExe.Util;
using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using System;
using Circle = BrainDotExe.Common.Render.Circle;
using Color = System.Drawing.Color;

namespace BrainDotExe.Draw
{
    class SafeJungle
    {
        public static Menu SafeJungleMenu;

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static void Init()
        {
            SafeJungleMenu = Program.Menu.AddSubMenu("Safe Jungle ", "attackRangeDraw");
            SafeJungleMenu.AddGroupLabel("Safe Jungle");
            SafeJungleMenu.Add("drawSpots", new KeyBind("Draw Jungle Spots", false, KeyBind.BindTypes.PressToggle, 'K'));

            Drawing.OnDraw += AttackRange_OnDraw;
        }

        public static void AttackRange_OnDraw(EventArgs args)
        {
            if (Misc.isChecked(Program.DrawMenu, "drawDisable")) return;

            if (Misc.isChecked(SafeJungleMenu, "drawSpots"))
            {
                DrawJunglePosition();
            }
        }

        public static void DrawJunglePosition()
        {
            if (Game.MapId == (GameMapId)11)
            {
                const float CircleRange = 100f;

                /*
                Order Team
                */
                Circle.DrawCircle(
                    new Vector3(7461.018f, 3253.575f, 52.57141f),
                    CircleRange,
                    Color.Blue);

                Circle.DrawCircle(
                    new Vector3(3511.601f, 8745.617f, 52.57141f),
                    CircleRange,
                    Color.Blue);
                
                Circle.DrawCircle(
                    new Vector3(7462.053f, 2489.813f, 52.57141f),
                    CircleRange,
                    Color.Blue);
                
                Circle.DrawCircle(
                    new Vector3(3144.897f, 7106.449f, 51.89026f),
                    CircleRange,
                    Color.Blue);
                
                Circle.DrawCircle(
                    new Vector3(7770.341f, 5061.238f, 49.26587f),
                    CircleRange,
                    Color.Blue);
                

                Circle.DrawCircle(
                    new Vector3(10930.93f, 5405.83f, -68.72192f),
                    CircleRange,
                    Color.Yellow);
                // Dragon

                /*
                Chaos Team
                */
                Circle.DrawCircle(
                    new Vector3(7326.056f, 11643.01f, 50.21985f),
                    CircleRange,
                    Color.Red);
                //red
                Circle.DrawCircle(
                    new Vector3(11417.6f, 6216.028f, 51.00244f),
                    CircleRange,
                    Color.Red);
                //blue
                Circle.DrawCircle(
                    new Vector3(7368.408f, 12488.37f, 56.47668f),
                    CircleRange,
                    Color.Red);
                //golems
                Circle.DrawCircle(
                    new Vector3(10342.77f, 8896.083f, 51.72742f),
                    CircleRange,
                    Color.Red);
                //wolfs
                Circle.DrawCircle(
                    new Vector3(7001.741f, 9915.717f, 54.02466f),
                    CircleRange,
                    Color.Red);
                //birds
            }
        }
    }
}
