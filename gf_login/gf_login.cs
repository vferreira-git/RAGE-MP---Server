using RAGE;
using CEF = RAGE.Ui.HtmlWindow;
using System;
using RAGE.Elements;
using System.Text.RegularExpressions;

namespace gf_login
{
    public class gf_login : Events.Script
    {
        CEF loginwindow = null;
        public gf_login()
        {
            RAGE.Events.Add("login", TryLogin);
            RAGE.Events.Add("register", TryRegister);
            RAGE.Events.Add("OnLoginRegister", OnLoginRegister);
            RAGE.Events.Add("showlogin", ShowLogin);
        }

        private void ShowLogin(object[] args)
        {
            loginwindow = new CEF("package://html/login/index.html");
            loginwindow.Active = true;
        }
        private void TryRegister(object[] args)
        {
            if (args[0] == null || args[1] == null || args[2] == null || string.IsNullOrEmpty(args[0].ToString()) || string.IsNullOrEmpty(args[1].ToString()) || string.IsNullOrEmpty(args[2].ToString()))
                return;

            RAGE.Events.CallRemote("register", args[0], args[1], args[2]);

        }
        private void TryLogin(object[] args)
        {
            if (string.IsNullOrEmpty(args[0].ToString()) || string.IsNullOrEmpty(args[1].ToString()))
                return;

            RAGE.Events.CallRemote("login", args[0], args[1], bool.Parse(args[2].ToString()));
        }
        private void OnLoginRegister(object[] args)
        {
            Events.CallLocal("shownotification", "Logado com sucesso!");
            loginwindow.Active = false;
            loginwindow.Destroy();
            RAGE.Ui.Cursor.Visible = false;
        }
    }
}
