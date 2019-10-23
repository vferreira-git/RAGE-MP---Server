using RAGE;
using CEF = RAGE.Ui.HtmlWindow;
using System;
using System.Collections;
using System.Collections.Generic;

namespace gf_notification
{
    public class gf_notification : Events.Script
    {
        CEF notification = null;
        Queue<object[]> notificationQueue = new Queue<object[]>();
        public gf_notification()
        {
            RAGE.Events.Add("endnotification", EndNotification);
            RAGE.Events.Add("shownotification", ShowNotification);
        }

        private void ShowNotification(object[] args)
        {
            if (notification != null)
            {
                if (notification.Active)
                {
                    notificationQueue.Enqueue(args);
                    return;
                }
            }
            

            notification = new CEF("package://html/notification/index.html");
            notification.Active = true;
            notification.ExecuteJs("ShowNotification('" + (args.Length < 2 ? "Info" : args[0].ToString()) + "','" + (args.Length < 2 ? args[0].ToString() : args[1].ToString()) + "');");
        }

        private void EndNotification(object[] args)
        {
            notification.Active = false;
            notification.Destroy();
            notification = null;
            if (notificationQueue.Count > 0)
                ShowNotificationQueue(notificationQueue.Dequeue());
        }

        private void ShowNotificationQueue(object[] args)
        {
            notification = new CEF("package://html/notification/index.html");
            notification.Active = true;
            notification.ExecuteJs("ShowNotification('" + (args.Length < 2 ? "Info" : args[0].ToString()) + "','" + (args.Length < 2 ? args[0].ToString() : args[1].ToString()) + "');");
        }
    }
}
