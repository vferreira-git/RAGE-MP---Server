using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RAGE;
using RAGE.Elements;
using static RAGE.Ui.Cursor;
using CEF = RAGE.Ui.HtmlWindow;

namespace Main
{
    public class Gf_main : Events.Script
    {
        private CEF mainUI;
        readonly Dictionary<int, bool> KeysDown = new Dictionary<int, bool>();
        bool IsChatTyping = false;
        bool IsRunning;
        int ignore = 2;
        readonly List<int> KeysToCheck = new List<int>();
        readonly Dictionary<string, int> Timers = new Dictionary<string, int>();
        readonly Dictionary<string, DateTime> TimersTime = new Dictionary<string, DateTime>();
        bool hungerStarted = false;
        int lights = 0;
        float hunger = 100;
        float thirst = 100;

        public Gf_main()
        {
            RAGE.Events.Add("callserverfunc", CallServerFunc);
            RAGE.Events.Add("changeChatState", ChatFocus);
            RAGE.Events.Add("keydown", KeyDown);
            RAGE.Events.Add("initialhungerthirst", InitialValuesHunger);
            RAGE.Events.Add("requeststats", RequestStats);
            RAGE.Events.Add("test", Test);
            RAGE.Events.Add("test2", Test2);

            RAGE.Events.Add("hungertimertick", HungerTimer_Elapsed);

            RAGE.Events.Add("gf:addtimer", AddTimer);

            RAGE.Chat.Colors = true;
            RAGE.Events.Tick += OnTick;
            RAGE.Chat.SafeMode = false;

            RAGE.Events.OnPlayerEnterVehicle += OnPlayerEnterVehicle;

            KeysDown.Add(0x4D, false);
            KeysDown.Add(0x45, false);
            KeysDown.Add(0x49, false);
            KeysDown.Add(0x4C, false);

            IsChatTyping = false;
            IsRunning = false;
            hungerStarted = false;

            KeysToCheck.Add(0x4D);
            KeysToCheck.Add(0x45);
            KeysToCheck.Add(0x49);
            KeysToCheck.Add(0x4C);



            ignore = KeysToCheck.Count;
        }

        private void Test2(object[] args)
        {
            Player.LocalPlayer.Vehicle.SetLightsMode(int.Parse(args[0].ToString()));
        }

        private void Test(object[] args)
        {
            ScriptControls
            Player.LocalPlayer.Vehicle.SetLights(int.Parse(args[0].ToString()));
        }

        private void OnPlayerEnterVehicle(Vehicle vehicle, int seatId)
        {
            vehicle.SetLightsMode(0);
            lights = 0;
        }

        private void AddTimer(object[] args)
        {
            if (string.IsNullOrEmpty(args[0].ToString()) && int.TryParse(args[1].ToString(), out int i))
            {
                Timers.Add(args[0].ToString(), i);
                TimersTime.Add(args[0].ToString(), DateTime.Now);
            }
        }

        private void UpdateHungerThirst()
        {

        }

        private void InitialValuesHunger(object[] args)
        {

            if (float.TryParse(args[0].ToString(), out float i) && float.TryParse(args[1].ToString(), out float j))
            {
                mainUI = new CEF("package://html/main/index.html")
                {
                    Active = true
                };

                hunger = i;
                thirst = j;
                hungerStarted = true;
                mainUI.ExecuteJs("$('#hungerPB').css('background','linear-gradient(90deg,#ed7e10 " + ((int)hunger).ToString() + "%,white " + ((int)hunger).ToString() + "%)');");
                mainUI.ExecuteJs("$('#thirstPB').css('background','linear-gradient(90deg,#11a4e6 " + ((int)thirst).ToString() + "%,white " + ((int)thirst).ToString() + "%)');");
                AddTimer("hungertimertick", 10000);
            }
        }

        private void AddTimer(string v1, int v2)
        {
            if (!string.IsNullOrEmpty(v1))
            {
                Timers.Add(v1, v2);
                TimersTime.Add(v1, DateTime.Now);
            }
        }

        private void RequestStats(object state)
        {
            if (hungerStarted)
            {
                RAGE.Events.CallRemote("updatehungerthirst", hunger, thirst);
                mainUI.ExecuteJs("$('#hungerPB').css('background','linear-gradient(90deg,#ed7e10 " + ((int)hunger).ToString() + "%,white " + ((int)hunger).ToString() + "%)');");
                mainUI.ExecuteJs("$('#thirstPB').css('background','linear-gradient(90deg,#11a4e6 " + ((int)thirst).ToString() + "%,white " + ((int)thirst).ToString() + "%)');");
            }
        }

        private void HungerTimer_Elapsed(object a)
        {
            if (hungerStarted)
            {
                if (IsRunning)
                {
                    if (hunger - 0.166666f >= 0)
                        hunger -= 0.166666f;
                    else
                        if (hunger > 0)
                        hunger = 0;

                    if (thirst - 0.333332f >= 0)
                        thirst -= 0.333332f;
                    else
                        if (thirst > 0)
                        thirst = 0;
                }
                else
                {
                    if (hunger - 0.083333f >= 0)
                        hunger -= 0.083333f;
                    else
                        if (hunger > 0)
                        hunger = 0;

                    if (thirst - 0.166666f >= 0)
                        thirst -= 0.166666f;
                    else
                        if (thirst > 0)
                        thirst = 0;
                }
            }
        }

        private void KeyDown(object[] args)
        {
            if (string.Equals(int.Parse(args[0].ToString()).ToString("X"), "4D", StringComparison.InvariantCultureIgnoreCase))
                RAGE.Ui.Cursor.Visible = !RAGE.Ui.Cursor.Visible;
            int isOn = 0;
            int isHigh = 0;
            RAGE.Game.Vehicle.GetVehicleLightsState(Player.LocalPlayer.Vehicle.Handle, ref isOn, ref isHigh);
            Chat.Output(isOn.ToString() + "  " + isHigh.ToString());
            
        }

        private void ChatFocus(object[] args)
        {
            bool SetFocus = bool.Parse(args[0].ToString());

            IsChatTyping = SetFocus;
            ignore = 0;

        }

        public static void Run(Action task, long delayTime = 0)
        {
            Task.Factory.StartNew(async () =>
            {
                await Task.Delay((int)delayTime);
                task();
            });
        }

        private void CallServerFunc(object[] args)
        {
            RAGE.Events.CallRemote(args[0].ToString(), args.Where(x => x.ToString() != args[0].ToString()).ToArray());
        }

        private void OnTick(List<Events.TickNametagData> nametags)
        {
            if (Player.LocalPlayer.GetSharedData("spawned") != null && (bool)Player.LocalPlayer.GetSharedData("spawned"))
            {
                if (Player.LocalPlayer.IsRunning())
                    IsRunning = true;
                else
                    IsRunning = false;

                if (Player.LocalPlayer.IsInAnyVehicle(false))
                {
                    RAGE.Game.Vehicle.SetVehicleLightMultiplier(Player.LocalPlayer.Vehicle.Handle, 2);

                    if (RAGE.Game.Vehicle.GetPedInVehicleSeat(Player.LocalPlayer.Vehicle.Handle, -1, 0) == Player.LocalPlayer.Handle)
                    {
                        mainUI.ExecuteJs("$('#speedo').show()");
                        mainUI.ExecuteJs("$('#speedo').gaugeMeter({text:'" + ((int)(Player.LocalPlayer.Vehicle.GetSpeed() * 3.6)).ToString() + "',min:0,used:" + ((int)(Player.LocalPlayer.Vehicle.GetSpeed() * 3.6)) + ",total:" + (int)(RAGE.Game.Vehicle.GetVehicleModelMaxSpeed(Player.LocalPlayer.Vehicle.Model) * 3.6) + "})");
                    }
                }
                else
                {
                    mainUI.ExecuteJs("$('#speedo').hide()");
                }
                CheckInput();
            }
            else
            {
                RAGE.Game.Ui.HideHudAndRadarThisFrame();
            }
            foreach (KeyValuePair<string, int> timer in Timers)
            {
                TimeSpan timeSpan = DateTime.Now - TimersTime[timer.Key];
                if (timeSpan.TotalMilliseconds >= timer.Value)
                {
                    RAGE.Events.CallLocal(timer.Key);
                    TimersTime[timer.Key] = DateTime.Now;
                }
            }
        }

        // Verifica se o player está a segurar uma tecla ou acabou de a soltar. Dinâmico ( Keys sao adicionadas com evento para serem verificadas )
        private void CheckInput()
        {
            if (!IsChatTyping)
            {
                foreach (int key in KeysToCheck)
                {
                    if (RAGE.Input.IsDown(key))
                    {
                        if (ignore == KeysToCheck.Count)
                        {
                            if (KeysDown[key].ToString() == "False")
                            {
                                Events.CallLocal("keydown", key.ToString());
                                KeysDown[key] = true;
                            }
                        }
                        else
                            ignore++;
                    }
                    else if (RAGE.Input.IsUp(key))
                    {
                        if (ignore == KeysToCheck.Count)
                        {
                            if (KeysDown[key].ToString() == "True")
                            {
                                Events.CallLocal("keyup", key);
                                KeysDown[key] = false;
                            }
                        }
                        else
                            ignore++;
                    }
                }
            }
        }
    }
}
