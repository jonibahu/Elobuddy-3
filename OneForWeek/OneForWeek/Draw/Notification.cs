using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK.Rendering;
using OneForWeek.Model;
using OneForWeek.Properties;
using SharpDX;

namespace OneForWeek.Draw
{
    static class Notification
    {
        static List<NotificationModel> notifications = new List<NotificationModel>();
        public static readonly TextureLoader TextureLoader = new TextureLoader();
        private static Sprite MainBar { get; set; }
        private static Text Text { get; set; }

        static Notification()
        {
            TextureLoader.Load("notification", Resources.notification);
            TextureLoader.Load("logo", Resources.logo);
        }

        public static void DrawNotification(NotificationModel notification)
        {
            notifications.Add(notification);

            Init();
            Drawing.OnEndScene += OnDraw;
        }

        private static void Init()
        {
            Text = new Text("", new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold)) { Color = System.Drawing.Color.AntiqueWhite };

            MainBar = new Sprite(() => TextureLoader["notification"]);

            AppDomain.CurrentDomain.DomainUnload += OnDomainUnload;
            AppDomain.CurrentDomain.ProcessExit += OnDomainUnload;
        }

        private static void OnDraw(EventArgs args)
        {
            if(notifications.Count == 0) return;

            var lastNotifPos = new Vector2();
            var auxNotifications = new List<NotificationModel>();

            foreach (var notificationModel in notifications)
            {
                var diffTime = (notificationModel.StartTimer + notificationModel.AnimationTimer + notificationModel.ShowTimer) - Game.Time;

                var animationEnd = notificationModel.StartTimer + notificationModel.AnimationTimer - Game.Time;

                var diffPos = 0f;

                if (animationEnd > 0)
                {
                    diffPos = 200 * animationEnd;
                }

                if (diffTime > 0)
                {
                    if (lastNotifPos.Equals(new Vector2()))
                    {
                        var pos = new Vector2(Drawing.Width - 220, y: Drawing.Height/13.5f - diffPos);
                        MainBar.Draw(pos);
                        lastNotifPos = pos;
                        if (!string.IsNullOrEmpty(notificationModel.ShowText))
                        {
                            pos.Y += 20;
                            pos.X += 30;
                            Text.Position = pos;
                            Text.TextValue = notificationModel.ShowText;
                            Text.TextAlign = Text.Align.Center;
                            Text.Draw();
                        }
                    }
                    else
                    {
                        var pos = new Vector2(lastNotifPos.X, y: lastNotifPos.Y + 70);
                        MainBar.Draw(pos);
                        lastNotifPos = pos;
                        if (!string.IsNullOrEmpty(notificationModel.ShowText))
                        {
                            pos.Y += 20;
                            pos.X += 30;
                            Text.Position = pos;
                            Text.TextValue = notificationModel.ShowText;
                            Text.TextAlign = Text.Align.Center;
                            Text.Draw();
                        }
                    }
                }
                else
                {
                    auxNotifications.Add(notificationModel);
                }
            }

            if (auxNotifications.Count > 0)
            {
                notifications = notifications.Except(auxNotifications).ToList();
            }

        }

        private static void OnDomainUnload(object sender, EventArgs e)
        {
            TextureLoader.Dispose();
        }

    }
}
