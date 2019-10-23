using RAGE;
using RAGE.Elements;
using System;
using CEF = RAGE.Ui.HtmlWindow;

namespace gf_inventory
{
    public class gf_inventory : Events.Script
    {
        CEF inventory;
        bool InventoryShown = false;
        public gf_inventory()
        {
            RAGE.Events.Add("additem", AddItem);
            RAGE.Events.Add("toggleinventory", ToggleInventory);
            RAGE.Events.Add("keydown", KeyDown);
            RAGE.Events.Add("switchitempos", SwitchItemPos);
            RAGE.Events.Add("dropitem", DropItem);
            RAGE.Events.Add("giveitem", GiveItem);
            RAGE.Events.Add("useitem", UseItem);
            RAGE.Events.Add("removeitem", RemoveItem);
            RAGE.Events.Add("clearinventory", ClearInventory);
            inventory = new CEF("package://html/inventory/index.html");
        }

        private void ClearInventory(object[] args)
        {
            inventory.ExecuteJs("$('.slot').remove();");
        }

        private void UseItem(object[] args)
        {
            string itemName = args[1].ToString();
            if (int.TryParse(args[0].ToString(), out int itemIndex) && !string.IsNullOrEmpty(itemName))
            {
                RAGE.Events.CallRemote("useitem", itemIndex, itemName);
            }
        }

        private void RemoveItem(object[] args)
        {
            if (int.TryParse(args[0].ToString(), out int itemIndex))
            {
                inventory.ExecuteJs("$('#mainContainer').children().eq(" + itemIndex + ").remove();");
            }
        }

        private void GiveItem(object[] args)
        {
            string itemName = args[1].ToString();
            if (int.TryParse(args[0].ToString(), out int itemIndex) && !string.IsNullOrEmpty(itemName))
            {
                RAGE.Events.CallRemote("giveitem", itemIndex, args[1].ToString(),itemName);
            }
        }



        private void DropItem(object[] args)
        {
            string itemName = args[1].ToString();
            if (int.TryParse(args[0].ToString(), out int itemIndex) && !string.IsNullOrEmpty(itemName))
            {
                RAGE.Events.CallRemote("dropitem", itemIndex,itemName);
            }
        }

        private void SwitchItemPos(object[] args)
        {
            if (int.TryParse(args[0].ToString(), out _) && int.TryParse(args[1].ToString(), out _) && !string.IsNullOrEmpty(args[2].ToString()) && !string.IsNullOrEmpty(args[3].ToString()))
                Events.CallRemote("switchitempos", args[0].ToString(), args[1].ToString(),args[2].ToString(),args[3].ToString());
        }

        private void ToggleInventory(object[] args)
        {
            InventoryShown = bool.Parse(args[0].ToString());
            inventory.ExecuteJs("Show(" + InventoryShown + ");");
            RAGE.Ui.Cursor.Visible = InventoryShown;
        }

        private void KeyDown(object[] args)
        {
            if (string.Equals(int.Parse(args[0].ToString()).ToString("X"), "49", StringComparison.InvariantCultureIgnoreCase))
                ToggleInventory(!InventoryShown);
            else if (string.Equals(int.Parse(args[0].ToString()).ToString("X"), "45", StringComparison.InvariantCultureIgnoreCase))
                RAGE.Events.CallRemote("trypickupitem");
        }

        private void ToggleInventory(bool args)
        {
            InventoryShown = args;
            inventory.ExecuteJs("Show(" + InventoryShown.ToString().ToLower() + ");");
            RAGE.Ui.Cursor.Visible = InventoryShown;
        }

        private void AddItem(object[] args)
        {
            inventory.ExecuteJs("AddItem('" + args[0].ToString() + "'," + args[1].ToString() + ",'" + args[2].ToString() + "','" + args[3].ToString() + "');");
        }

    }
}
