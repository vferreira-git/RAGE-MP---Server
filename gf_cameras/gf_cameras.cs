using RAGE;
using RAGE.Elements;
using System;
using System.Collections.Generic;
using Cam = RAGE.Game.Cam;

namespace gf_cameras
{
    public class gf_cameras : Events.Script
    {

        int CCCamera = -1;
        int CCCameraPos = -1;
        int loginCamera = -1;
        public gf_cameras()
        {
            RAGE.Events.Tick += OnUpdate;
            RAGE.Events.Add("logincamera", LoginCamera);
            RAGE.Events.Add("cccamera", CharacterCustomizationCamera);
            RAGE.Events.Add("clearcameras", ClearCameras);
            RAGE.Events.Add("adjustcccamera", AdjustCCCamera);
        }

        private void AdjustCCCamera(object[] args)
        {
            int cameraPos = int.Parse(args[0].ToString());
            if (CCCameraPos != cameraPos)
            {
                bool isMale = bool.Parse(args[1].ToString());
                Vector3 forward = Player.LocalPlayer.Position + Player.LocalPlayer.GetForwardVector();
                switch (cameraPos)
                {
                    case 1:
                        Cam.SetCamFov(CCCamera, 50f);
                        Cam.PointCamAtEntity(CCCamera, Player.LocalPlayer.Handle, 0, 0, 0.3f, true);
                        break;
                    case 2:
                        Cam.SetCamFov(CCCamera, 20f);
                        Cam.PointCamAtEntity(CCCamera, Player.LocalPlayer.Handle, 0, 0, isMale ? 0.65f : 0.8f, true);
                        break;
                    case 3:
                        Cam.SetCamFov(CCCamera, 70f);
                        Cam.PointCamAtEntity(CCCamera, Player.LocalPlayer.Handle, 0, 0, -0.5f, true);
                        break;
                    case 4:
                        Cam.SetCamFov(CCCamera, 40f);
                        Cam.PointCamAtEntity(CCCamera, Player.LocalPlayer.Handle, 0, 0, -0.8f, true);
                        break;
                    case 5:
                        Cam.SetCamFov(CCCamera, 60f);
                        Cam.PointCamAtEntity(CCCamera, Player.LocalPlayer.Handle, 0, 0, 0.3f, true);
                        break;
                }
                CCCameraPos = cameraPos;
            }
        }

        private void OnUpdate(List<Events.TickNametagData> nametags)
        {
            if (Player.LocalPlayer.GetData<bool>("CharacterCustomization"))
            {
                if (RAGE.Input.IsDown(0x64))
                {
                    Player.LocalPlayer.SetHeading(Player.LocalPlayer.GetHeading() - 2);
                }
                else if (RAGE.Input.IsDown(0x66))
                {
                    Player.LocalPlayer.SetHeading(Player.LocalPlayer.GetHeading() + 2f);
                }
            }
        }

        public void LoginCamera(object[] args)
        {
            RAGE.Game.Streaming.LoadScene(-219, 6174, 31);
            RAGE.Game.Cam.DestroyAllCams(true);
            loginCamera = RAGE.Game.Cam.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -536, 6301, 147, 0, 0, 290f, 90f, false, 2);
            RAGE.Game.Cam.SetCamActive(loginCamera, true);
            RAGE.Game.Cam.RenderScriptCams(true, false, 0, false, false, 0);
        }

        public void CharacterCustomizationCamera(object[] args)
        {
            Cam.DestroyAllCams(true);
            Vector3 forward = Player.LocalPlayer.Position + Player.LocalPlayer.GetForwardVector();
            CCCamera = Cam.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", forward.X, forward.Y, forward.Z + 0.5f, 0, 0, 0, 90f, false, 2);
            Cam.PointCamAtEntity(CCCamera, Player.LocalPlayer.Handle, 0, 0, 0.5f, true);
            Cam.SetCamActive(CCCamera, true);
            Cam.RenderScriptCams(true, false, 0, false, false, 0);
        }

        public void ClearCameras(object[] args)
        {
            Cam.DestroyAllCams(true);
            Player.LocalPlayer.SetData("CharacterCustomization", false);
            Cam.RenderScriptCams(false, false, 0, false, false, 0);
        }
    }
}
