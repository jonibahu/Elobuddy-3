using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using SharpDX;
using Color = System.Drawing.Color;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace BrainDotExe.Util
{
    static class Misc
    {
        #region Draws

        public static void DrawLine(Vector3 start, Vector3 end, Color color, float thickness = 2f)
        {
            Drawing.DrawLine(Drawing.WorldToScreen(start), Drawing.WorldToScreen(start), thickness, color);
        }

        public static void DrawText(Vector3 pos, Color color, string text, int size = 15)
        {
            Drawing.DrawText(Drawing.WorldToScreen(pos), color, text, size);
        }

        #endregion

        #region Util Functions

        public static bool isChecked(Menu obj, String value)
        {
            return obj[value].Cast<CheckBox>().CurrentValue;
        }

        public static int getSliderValue(Menu obj, String value)
        {
            return obj[value].Cast<Slider>().CurrentValue;
        }

        #endregion
    }
}
