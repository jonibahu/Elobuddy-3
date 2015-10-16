using System.Drawing;
using System.Text.RegularExpressions;

namespace OneForWeek.Model.Notification
{
    public class NotificationModel
    {
        public float StartTimer { get; set; }
        public float ShowTimer { get; set; }
        public float AnimationTimer { get; set; }
        public string ShowText { get; set; }
        public Color Color { get; set; }

        public NotificationModel(float startTimer, float showTimer, float animationTimer, string showText, Color color)
        {
            StartTimer = startTimer;
            ShowTimer = showTimer;
            AnimationTimer = animationTimer;
            var value = Regex.Replace(showText, ".{23}", "$0\n");
            ShowText = value;
            Color = color;
        }

    }
}
