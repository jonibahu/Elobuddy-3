using System.Drawing;

namespace OneForWeek.Model
{
    class NotificationModel
    {
        public float StartTimer { get; set; }
        public float ShowTimer { get; set; }
        public float AnimationTimer { get; set; }
        public string ShowText { get; set; }

        public NotificationModel(float startTimer, float showTimer, float animationTimer)
        {
            StartTimer = startTimer;
            ShowTimer = showTimer;
            AnimationTimer = animationTimer;
        }

        public NotificationModel(float startTimer, float showTimer, float animationTimer, string showText)
        {
            StartTimer = startTimer;
            ShowTimer = showTimer;
            AnimationTimer = animationTimer;
            ShowText = showText;
        }
    }
}
