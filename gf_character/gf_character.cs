using gf_character.Classes;
using Newtonsoft.Json;
using RAGE;
using RAGE.Elements;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using CEF = RAGE.Ui.HtmlWindow;

namespace gf_character
{
    public class gf_character : Events.Script
    {
        Skin skin = new Skin();
        int[] CCcolor1 = new int[13];
        int[] CCcolor2 = new int[13];
        private int hairColor1;
        private int hairColor2;
        bool isMale = true;
        CEF CC = null;
        float[] faceFeatures = new float[20];
        // 1 - torso   2 - head   3 - legs   4 - feet   5 - accessories

        private List<int> tshirtBlackListM = new List<int> { 58, 59, 61, 62, 78, 97, 108, 122, 123, 124, 125, 126, 127, 128, 129, 130, 137, 143, 145, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168 };
        private List<int> casacosBlackListM = new List<int> { 2, 5, 15, 17, 36, 48, 49, 50, 53, 54, 55, 66, 91, 178, 186, 201, 228, 231, 243, 246, 252, 254, 274, 275, 276, 277, 278, 283, 284, 285, 286, 287, 289, 291 };
        private List<int> acessoriosBlackListM = new List<int> { 40, 126, 125, 127, 128, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150 };
        private List<int> calçasBlacklistM = new List<int> { 11, 30, 34, 41, 44, 66, 67, 71, 72, 74, 77, 84, 85, 91, 92, 93, 94, 95, 99, 101, 106, 107, 108, 109, 110, 111, 113, 114, 115 };
        private List<int> oculosBlackListM = new List<int> { 24, 25, 26, 27, 14, 11, 6 };
        private List<int> chapeusBlackListM = new List<int> { 0, 1, 15, 16, 17, 18, 19, 37, 38, 39, 46, 47, 48, 49, 50, 51, 52, 53, 57, 59, 62, 67, 68, 69, 70, 71, 72, 73, 74, 75, 78, 79, 80, 81, 82, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 111, 112, 113, 115, 116, 117, 118, 119, 123, 124, 125, 126, 127, 128, 129, 133, 134 };

        private List<int> tshirtBlackListF = new List<int> { 105, 148, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 174, 184, 186, 187, 188, 189, 190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205, 206, 207, 208, 209 };
        private List<int> casacosBlackListF = new List<int> { 41, 42, 43, 46, 47, 48, 82, 74, 100, 101, 188, 203, 238, 241, 254, 256, 287, 288, 289, 290, 291, 296, 297, 298, 299, 300, 302, 304 };
        private List<int> acessoriosBlackListF = new List<int> { 16, 24, 95, 96, 97, 98, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180, 181, 182, 183, 184, 185, 186 };
        private List<int> calçasBlacklistF = new List<int> { 13, 29, 32, 33, 35, 39, 40, 42, 46, 48, 69, 79, 86, 88, 95, 96, 98, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123 };
        private List<int> oculosBlackListF = new List<int> { 26, 27, 28, 29, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159 };
        private List<int> chapeusBlackListF = new List<int> { 0, 1, 15, 16, 17, 18, 19, 36, 37, 38, 45, 46, 47, 48, 49, 50, 51, 52, 59, 62, 66, 67, 68, 69, 70, 71, 72, 73, 74, 77, 78, 79, 80, 81, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 110, 111, 114, 115, 116, 117, 118, 122, 123, 124, 125, 126, 127, 128, 132, 133 };



        public gf_character()
        {
            RAGE.Events.Add("cc", CharacterCustomization);
            RAGE.Events.Add("headoverlay", SetHeadOverlay);
            RAGE.Events.Add("headoverlaycolor", SetHeadOverlayColor);
            RAGE.Events.Add("componentvariation", SetComponentVariation);
            RAGE.Events.Add("facefeature", SetFaceFeature);
            RAGE.Events.Add("propindex", SetPropIndex);
            RAGE.Events.Add("haircolor", SetHairColor);
            RAGE.Events.Add("setmaxvaluestorso", SetMaxValuesTorso);
            RAGE.Events.Add("setmaxvalueslegs", SetMaxValuesLegs);
            RAGE.Events.Add("setmaxvalueshats", SetMaxValuesHats);
            RAGE.Events.Add("setmaxvaluesglasses", SetMaxValuesGlasses);
            RAGE.Events.Add("choosechar", SelectChar);
            RAGE.Events.Add("setmaxvaluesshoes", SetMaxValuesShoes);
            RAGE.Events.Add("finishcc", FinishCC);
            RAGE.Events.Add("choosesexo", ChooseSexo);
            RAGE.Events.Add("newchar", NewChar);
            RAGE.Events.Add("showcharselect", ShowCharSelect);
            RAGE.Events.Add("closecc", CloseCC);
            RAGE.Events.Add("characterselected", CharacterSelected);
        }

        private void CharacterSelected(object[] args)
        {
            RAGE.Events.CallLocal("clearcameras");
            RAGE.Events.CallRemote("characterselected");
        }

        private void CloseCC(object[] args)
        {
            CC.Destroy();
            RAGE.Ui.Cursor.Visible = false;
            CC = null;
        }

        private void SelectChar(object[] args)
        {
            RAGE.Events.CallRemote("selectchar", args[0].ToString());
        }

        private void ShowCharSelect(object[] args)
        {
            RAGE.Events.CallLocal("logincamera");
            Newtonsoft.Json.Linq.JArray chars = (Newtonsoft.Json.Linq.JArray)args[0];
            if (CC == null)
            {
                CC = new CEF("package://html/cc/index.html");
                CC.Active = true;
                RAGE.Ui.Cursor.Visible = true;
            }
            switch (chars.Count)
            {
                case 0:
                    CC.ExecuteJs("$('#btnChar0').addClass('d-none');$('#btnChar3').addClass('d-none');$('#btnChar2').addClass('d-none');$('#btnChar1').addClass('d-none');$('#btnNewChar').prop('disabled',false)");
                    break;
                case 1:
                    CC.ExecuteJs("$('#btnChar0').removeClass('d-none');$('#btnChar0 h3').text('" + chars[0] + "');$('#btnChar1').addClass('d-none');$('#btnChar2').addClass('d-none');$('#btnChar3').addClass('d-none');$('#btnNewChar').prop('disabled',false)");
                    break;
                case 2:
                    CC.ExecuteJs("$('#btnChar0').removeClass('d-none');$('#btnChar0 h3').text('" + chars[0] + "');$('#btnChar1').removeClass('d-none');$('#btnChar1 h3').text('" + chars[1] + "');$('#btnChar2').addClass('d-none');$('#btnChar3').addClass('d-none');$('#btnNewChar').prop('disabled',false)");
                    break;
                case 3:
                    CC.ExecuteJs("$('#btnChar0').removeClass('d-none');$('#btnChar0 h3').text('" + chars[0] + "');$('#btnChar1').removeClass('d-none');$('#btnChar1 h3').text('" + chars[1] + "');$('#btnChar2').removeClass('d-none');$('#btnChar2 h3').text('" + chars[2] + "');$('#btnChar3').addClass('d-none');$('#btnNewChar').prop('disabled',false)");
                    break;
                case 4:
                    CC.ExecuteJs("$('#btnChar0').removeClass('d-none');$('#btnChar0 h3').text('" + chars[0] + "');$('#btnChar1').removeClass('d-none');$('#btnChar1 h3').text('" + chars[1] + "');$('#btnChar2').removeClass('d-none');$('#btnChar2 h3').text('" + chars[2] + "');$('#btnChar3').removeClass('d-none');$('#btnChar3 h3').text('" + chars[3] + "');$('#btnNewChar').prop('disabled',true)");
                    break;
                default:
                    CC.ExecuteJs("$('#mainContainer').hide();$('#charSelect').show()");
                    break;
            }
        }

        private void NewChar(object[] args)
        {
            if (!string.IsNullOrEmpty(args[0].ToString()) && Regex.IsMatch(args[0].ToString(), @"([A-Z]\w+) ([A-Z]\w+)"))
            {
                Player.LocalPlayer.SetData("newCharName", args[0].ToString());
                RAGE.Events.CallRemote("checkcharname", args[0].ToString());
            }
        }

        private void ChooseSexo(object[] args)
        {
            int sexo = int.Parse(args[0].ToString());
            isMale = sexo == 1 ? true : false;
            Player.LocalPlayer.Model = sexo == 1 ? 0x705E61F2 : 0x9C9EFFD8;
            Player.LocalPlayer.Position = new Vector3(-811.7185f,175.2452f,76.74538f);
            Player.LocalPlayer.SetHeading(120.6735f);
            CC.ExecuteJs("$('#sexoContainer').addClass('d-none');$('#mainContainer').removeClass('d-none');$('#mainContainer').show()");
            if (isMale)
                CC.ExecuteJs("$('#BatomSlide').bootstrapSlider('disable');$('#BatomSlideCor').bootstrapSlider('disable')");
            else
                CC.ExecuteJs("$('#beard').bootstrapSlider('disable');$('#beardCor1').bootstrapSlider('disable')");

            Player.LocalPlayer.SetData("CharacterCustomization", true);
            Events.CallRemote("IdleAnim");
            RAGE.Events.CallLocal("cccamera");

        }
        private void FinishCC(object[] args)
        {
            GetUserSkin();
            string json = JsonConvert.SerializeObject(skin);
            RAGE.Events.CallRemote("finishcc", skin, Player.LocalPlayer.GetData<string>("newCharName"));
        }
        private void GetUserSkin()
        {


            skin.isMale = isMale;
            for (int i = 0; i < 13; i++)
            {
                skin.headOverlay[i].index = Player.LocalPlayer.GetHeadOverlayValue(i);
            }
            for (int i = 0; i < 12; i++)
            {
                skin.componentVariation[i].drawableId = Player.LocalPlayer.GetDrawableVariation(i);
                skin.componentVariation[i].textureId = Player.LocalPlayer.GetTextureVariation(i);
            }
            skin.faceFeature = faceFeatures;
            for (int i = 0; i < 3; i++)
            {
                skin.propIndex[i].drawableId = Player.LocalPlayer.GetPropIndex(i);
                skin.propIndex[i].textureId = Player.LocalPlayer.GetPropTextureIndex(i);
            }
            skin.hairColor = hairColor1;
            skin.hairColor2 = hairColor2;
        }
        private void SetMaxValuesShoes(object[] args)
        {
            CC.ExecuteJs("$('#shoes').bootstrapSlider({max:" + (Player.LocalPlayer.GetNumberOfDrawableVariations(6) - 1).ToString() + "});");
            CC.ExecuteJs("$('#shoes').val(" + Player.LocalPlayer.GetDrawableVariation(6).ToString() + ");");
        }

        private void SetMaxValuesGlasses(object[] args)
        {
            CC.ExecuteJs("$('#glasses').bootstrapSlider({max:" + (Player.LocalPlayer.GetNumberOfPropDrawableVariations(1) - 2 - (isMale ? oculosBlackListM.Count() : oculosBlackListF.Count())).ToString() + "});");
            CC.ExecuteJs("$('#glasses').val(" + Player.LocalPlayer.GetPropIndex(1).ToString() + ");");
        }

        private void SetMaxValuesHats(object[] args)
        {
            CC.ExecuteJs("$('#hats').bootstrapSlider({max:" + (Player.LocalPlayer.GetNumberOfPropDrawableVariations(0) - 2 - (isMale ? chapeusBlackListM.Count() : chapeusBlackListF.Count())).ToString() + "});");
            CC.ExecuteJs("$('#hats').val(" + Player.LocalPlayer.GetPropIndex(0).ToString() + ");");
        }

        private void SetMaxValuesTorso(object[] args)
        {
            CC.ExecuteJs("$('#torso1').bootstrapSlider('setAttribute','max'," + (Player.LocalPlayer.GetNumberOfDrawableVariations(3) - 1 - (isMale ? tshirtBlackListM.Count() : tshirtBlackListF.Count())).ToString() + ");");
            CC.ExecuteJs("$('#torso1').bootstrapSlider('refresh');");
            CC.ExecuteJs("$('#torso1').val(" + Player.LocalPlayer.GetDrawableVariation(3).ToString() + ");");

            CC.ExecuteJs("$('#torso2').bootstrapSlider({max:" + (Player.LocalPlayer.GetNumberOfDrawableVariations(11) - 2 - (isMale ? casacosBlackListM.Count() : casacosBlackListF.Count())).ToString() + "});");
            CC.ExecuteJs("$('#torso2').val(" + Player.LocalPlayer.GetDrawableVariation(11).ToString() + ");");

            CC.ExecuteJs("$('#accessories').bootstrapSlider({max:" + (Player.LocalPlayer.GetNumberOfDrawableVariations(8) - 2 - (isMale ? acessoriosBlackListM.Count() : acessoriosBlackListF.Count())).ToString() + "});");
            CC.ExecuteJs("$('#accessories').val(" + Player.LocalPlayer.GetDrawableVariation(8).ToString() + ");");
        }

        private void SetMaxValuesLegs(object[] args)
        {
            CC.ExecuteJs("$('#legs').bootstrapSlider({max:" + (Player.LocalPlayer.GetNumberOfDrawableVariations(4) - 2 - (isMale ? calçasBlacklistM.Count() : calçasBlacklistF.Count())).ToString() + "});");
            CC.ExecuteJs("$('#legs').val(" + Player.LocalPlayer.GetDrawableVariation(4).ToString() + ");");
        }


        private void SetPropIndex(object[] args)
        {
            int componentId = int.Parse(args[0].ToString());
            int drawableId = int.Parse(args[1].ToString());
            int textureId = int.Parse(args[2].ToString());
            bool isTexture = bool.Parse(args[3].ToString());
            bool attach = true;
            if (componentId == 0)
                drawableId = Enumerable.Range(-1, Player.LocalPlayer.GetNumberOfPropDrawableVariations(componentId) - 1).Except(isMale ? chapeusBlackListM : chapeusBlackListF).ToList()[drawableId];
            else
                drawableId = Enumerable.Range(-1, Player.LocalPlayer.GetNumberOfPropDrawableVariations(componentId) - 1).Except(isMale ? oculosBlackListM : oculosBlackListF).ToList()[drawableId];
            if (!isTexture)
            {
                if (componentId == 0)
                {
                    CC.ExecuteJs("$('#hatsTex').bootstrapSlider({max:" + Player.LocalPlayer.GetNumberOfPropTextureVariations(componentId, drawableId).ToString() + "});");
                    CC.ExecuteJs("$('#hatsTex').val(0);");
                }
                else if (componentId == 1)
                {
                    CC.ExecuteJs("$('#glassesTex').bootstrapSlider({max:" + Player.LocalPlayer.GetNumberOfPropTextureVariations(componentId, drawableId).ToString() + "});");
                    CC.ExecuteJs("$('#glassesTex').val(0);");
                }
            }
            RAGE.Events.CallLocal("adjustcccamera", 2, isMale);

            if (drawableId == -1)
                Player.LocalPlayer.ClearProp(componentId);
            else
                Player.LocalPlayer.SetPropIndex(componentId, drawableId, textureId, attach);
        }

        private void SetFaceFeature(object[] args)
        {
            try
            {
                RAGE.Events.CallLocal("adjustcccamera", 2, isMale);

                int index = int.Parse(args[0].ToString());
                float scale = float.Parse(args[1].ToString(), CultureInfo.InvariantCulture);
                faceFeatures[index] = scale / 100;

                Player.LocalPlayer.SetFaceFeature(index, scale / 100);
            }
            catch (Exception e)
            {
                Chat.Output(e.Message);
            }


        }

        private void SetComponentVariation(object[] args)
        {
            int componentId = int.Parse(args[0].ToString());
            int drawableId = int.Parse(args[1].ToString());
            int textureId = int.Parse(args[2].ToString());
            bool isTexture = bool.Parse(args[3].ToString());

            switch (componentId)
            {
                case 2:
                    RAGE.Events.CallLocal("adjustcccamera", 2, isMale);
                    break;

                case 4:
                    RAGE.Events.CallLocal("adjustcccamera", 3, isMale);

                    drawableId = Enumerable.Range(0, Player.LocalPlayer.GetNumberOfDrawableVariations(componentId) - 1).Except(isMale ? calçasBlacklistM : calçasBlacklistF).ToList()[drawableId];
                    if (!isTexture)
                    {
                        int maxTex = Player.LocalPlayer.GetNumberOfTextureVariations(componentId, drawableId);
                        CC.ExecuteJs("$('#legsTex').bootstrapSlider({max:" + maxTex + "});");
                        CC.ExecuteJs("$('#legsTex').val(0);");
                    }
                    break;
                case 6:
                    RAGE.Events.CallLocal("adjustcccamera", 4, isMale);

                    if (!isTexture)
                    {
                        int maxTex = Player.LocalPlayer.GetNumberOfTextureVariations(componentId, drawableId);
                        CC.ExecuteJs("$('#shoesTex').bootstrapSlider({max:" + maxTex + "});");
                        CC.ExecuteJs("$('#shoesTex').val(0);");
                    }

                    break;
                case 7:
                    RAGE.Events.CallLocal("adjustcccamera", 5, isMale);
                    drawableId = Enumerable.Range(0, Player.LocalPlayer.GetNumberOfDrawableVariations(componentId) - 1).Except(isMale ? acessoriosBlackListM : acessoriosBlackListF).ToList()[drawableId];
                    break;
                case 8:
                    RAGE.Events.CallLocal("adjustcccamera", 1, isMale);
                    drawableId = Enumerable.Range(0, Player.LocalPlayer.GetNumberOfDrawableVariations(componentId) - 1).Except(isMale ? tshirtBlackListM : tshirtBlackListF).ToList()[drawableId];

                    if (!isTexture)
                    {
                        int maxTex = Player.LocalPlayer.GetNumberOfTextureVariations(componentId, drawableId);
                        CC.ExecuteJs("$('#torso1Tex').bootstrapSlider({max:" + maxTex + "});");
                        CC.ExecuteJs("$('#torso1Tex').val(0);");
                    }
                    break;
                case 11:
                    RAGE.Events.CallLocal("adjustcccamera", 1, isMale);

                    drawableId = Enumerable.Range(0, Player.LocalPlayer.GetNumberOfDrawableVariations(componentId) - 1).Except(isMale ? casacosBlackListM : casacosBlackListF).ToList()[drawableId];
                    if (!isTexture)
                    {
                        int maxTex = Player.LocalPlayer.GetNumberOfTextureVariations(componentId, drawableId);
                        CC.ExecuteJs("$('#torso2Tex').bootstrapSlider({max:" + maxTex + "});");
                        CC.ExecuteJs("$('#torso2Tex').val(0);");
                    }
                    break;

            }

            Player.LocalPlayer.SetComponentVariation(componentId, drawableId, textureId, 2);
        }

        private void SetHairColor(object[] args)
        {
            int hairColor1 = int.Parse(args[0].ToString());
            int hairColor2 = int.Parse(args[1].ToString());
            this.hairColor1 = hairColor1;
            this.hairColor2 = hairColor2;
            Player.LocalPlayer.SetHairColor(hairColor1, hairColor2);
        }

        private void SetHeadOverlayColor(object[] args)
        {
            int overlayID = int.Parse(args[0].ToString());
            int colortype = int.Parse(args[1].ToString());
            int color1 = int.Parse(args[2].ToString());
            int color2 = int.Parse(args[3].ToString());

            skin.headOverlay[overlayID].color1 = color1;
            skin.headOverlay[overlayID].color2 = color2;
            Player.LocalPlayer.SetHeadOverlayColor(overlayID, colortype, color1, color2);
        }

        private void SetHeadOverlay(object[] args)
        {
            RAGE.Events.CallLocal("adjustcccamera", 2, isMale);
            int overlayID = int.Parse(args[0].ToString());
            int index = int.Parse(args[1].ToString());
            float opacity = 1f;

            Player.LocalPlayer.SetHeadOverlay(overlayID, index, opacity);
        }

        private void CharacterCustomization(object[] args)
        {
            CC.ExecuteJs("$('#charSelect').hide()");
            CC.ExecuteJs("$('#sexoContainer').show()");
            CCcolor1 = new int[13];
            CCcolor2 = new int[13];
            faceFeatures = new float[20];

            skin = new Skin();

            for (int i = 0; i < 13; i++)
            {
                skin.headOverlay.Add(new HeadOverlay());
            }

            for (int i = 0; i < 12; i++)
            {
                skin.componentVariation.Add(new ComponentVariation());
            }
            for (int i = 0; i < 3; i++)
            {
                skin.propIndex.Add(new PropIndex());
            }

            CC.Active = true;
            CC.ExecuteJs("$('#sexoContainer').show();");
            RAGE.Ui.Cursor.Visible = true;

        }
    }
}
