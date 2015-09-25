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

        public static void DrawMarkPoint(Vector3 pos, Color color, int size = 30, float thickness = 2f)
        {
            {
                var lineLU = Drawing.WorldToScreen(pos);

                lineLU.Y -= 120;
                lineLU.X -= 75;

                var lineLD = lineLU;

                lineLD.X -= size;
                lineLD.Y -= size;

                var lineRU = Drawing.WorldToScreen(pos);

                lineRU.Y -= 120;
                lineRU.X -= 75;

                var lineRD = lineRU;

                lineRD.X -= size;
                lineRD.Y += size;

                Drawing.DrawLine(lineRD, lineRU, thickness, color);
                Drawing.DrawLine(lineLD, lineLU, thickness, color);
            }

            {
                var lineLU = Drawing.WorldToScreen(pos);

                lineLU.Y -= 120;
                lineLU.X += 75;

                var lineLD = lineLU;

                lineLD.X += size;
                lineLD.Y -= size;

                var lineRU = Drawing.WorldToScreen(pos);

                lineRU.Y -= 120;
                lineRU.X += 75;

                var lineRD = lineRU;

                lineRD.X += size;
                lineRD.Y += size;

                Drawing.DrawLine(lineRD, lineRU, thickness, color);
                Drawing.DrawLine(lineLD, lineLU, thickness, color);
            }
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
