using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using SharpDX;
using Color = System.Drawing.Color;

namespace BrainDotExe.Util
{
    static class Misc
    {
        public static void DrawLine(Vector3 start, Vector3 end, Color color, float thickness = 2f)
        {
            Drawing.DrawLine(Drawing.WorldToScreen(start), Drawing.WorldToScreen(start), thickness, color);
        }

        public static void DrawText(Vector3 pos, Color color, string text, int size = 15)
        {
            Drawing.DrawText(Drawing.WorldToScreen(pos), color, text, size);
        }
    }
}
