using System;
using System.Drawing;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using OneForWeek.Draw;
using OneForWeek.Model;
using OneForWeek.Properties;
using SharpDX;
using Color = System.Drawing.Color;

namespace OneForWeek
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoadCompleted;
        }

        private static void OnLoadCompleted(EventArgs args)
        {
            Notification.DrawNotification(new NotificationModel(Game.Time, 5f, 1f, "I'm a toaster by Vector"));
            Notification.DrawNotification(new NotificationModel(Game.Time, 10f, 1f, "Elobuddy"));
            Notification.DrawNotification(new NotificationModel(Game.Time, 15f, 1f, null));
        }
        
    }
}
