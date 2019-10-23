using RAGE;
using RAGE.Elements;
using System;
using System.Collections.Generic;

namespace gf_buildings
{
    public class gf_buildings : Events.Script
    {
        private bool isEClicked = false;
        public gf_buildings()
        {
            RAGE.Events.Add("loadInterior", LoadInterior);
            RAGE.Events.Add("keydown", KeyDown);

        }

        private void KeyDown(object[] args)
        {
            if (string.Equals(int.Parse(args[0].ToString()).ToString("X"), "45", StringComparison.InvariantCultureIgnoreCase))
                RAGE.Events.CallRemote("checkbuilding");

        }

        private void LoadInterior(object[] args)
        {
            RAGE.Game.Streaming.RequestIpl(args[0].ToString());
        }
    }
}
