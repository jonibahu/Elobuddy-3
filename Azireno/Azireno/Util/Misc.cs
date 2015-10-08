using EloBuddy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azireno.Util
{
    static class Misc
    {

        public static void ChatText(string text, Color color)
        {
            Chat.Print("<font color=\"" + HexConverter(Color.Cyan) + "\">Mechanics Azir: </font>" + "<font color=\"" +HexConverter(color)+"\">"+ text +"</font>");
        }

        private static string HexConverter(Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

    }
}
