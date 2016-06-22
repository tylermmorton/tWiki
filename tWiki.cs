using Terraria;
using Terraria.ModLoader;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Net;
using System;
using Microsoft.Xna.Framework;
using tWiki.Data;

namespace tWiki
{
    class tWiki : Mod
    {
        static string tWikiSearch = "http://terraria.gamepedia.com/index.php?search=";
        static Dictionary<int, int> itemTile = new Dictionary<int, int>();


        public override void Load()
        {
         
            this.RegisterHotKey("tWiki Search", "L");
            //this.RegisterHotKey("tWiki Item Cursor", "RSHIFT");

            //this.AddGlobalItem("tWikiItemCursor", new ItemCursor());

            //for(int i = 0; i < this.Unload )
        }

        public override void HotKeyPressed(string name)
        {
            
            //if (name == "tWiki Item Cursor")
            //{
            //    // Toggle the item cursor
            //    ItemCursor.showItemCursor = !ItemCursor.showItemCursor;
            //}

            // Check to see if the player is hovering an NPC
            string hover = null;
            if (name == "tWiki Search" && (hover = GetHoveringNPC()) != null)
            {
                // Open the generated link in the default web browser.
                Process.Start(Link.Shorten(new Link(tWikiSearch + hover)).url);
            }

            // Make sure the player has the inventory open
            if (Main.playerInventory)
                // Check to see if the keypressed is correct
                // Also check to see if they are hovering an item
                if (name == "tWiki Search" && Main.hoverItemName.Length != 0)
                {
                    // Declare the item name (could be in any language)
                    string itemName = Main.hoverItemName;

                    // Use ReGex to detect stack information
                    // Example : Copper (300)
                    string pattern = "(\\ \\([^)]+\\))";
                    itemName = Regex.Replace(itemName, pattern, string.Empty).Trim();

                    // Check to see if the item is from a mod
                    if (this.GetItem(itemName) != null)
                    {
                        Main.NewText(itemName + " is a modded item and doesn't exist on the Terraria Wiki!");
                        return;
                    }

                    // Translate the itemName to english
                    if (Lang.lang != 1)
                    {
                        for (int i = 0; i < Main.player[Main.myPlayer].inventory.Length; i++)
                            if (itemName == Main.player[Main.myPlayer].inventory[i].name)
                            {
                                itemName = Lang.itemName(Main.player[Main.myPlayer].inventory[i].type, true);
                                break;
                            }
                    }

                    // Replace all spaces with +
                    // This is for the search queries
                    itemName = itemName.Replace(" ", "+");
                    //Main.NewText(itemName);
                    // Open the generated link in the default web browser.
                    Link.Shorten(new Link(tWikiSearch + itemName));
                    Process.Start(Link.Shorten(new Link(tWikiSearch + itemName)).url);


                }
        }

        public string GetHoveringNPC()
        {
            // Declare our mouse coordinates relative to the screen position
            float ptX = Main.mouseX + Main.screenPosition.X;
            float ptY = Main.mouseY + Main.screenPosition.Y;

            // Loop through every single one of the NPC's
            // This loop will run until it finds a hovered NPC.
            for (int i = 0; i < Main.npc.Length; i++)
            {
                // Check to see if the NPC's hitbox contains our mouse points
                if(Main.npc[i].Hitbox.Contains((int)ptX, (int)ptY))
                {
                    // If it's a town NPC, we don't want their display name
                    // We want a generic name like "Guide", "Merchant", ect                    
                    if (Main.npc[i].townNPC)
                         return Main.npc[i].name;
                    else return Main.npc[i].displayName;
                }
            }
            // If no hovered NPC is found, return null for our != null check
            return null;
        }
    }
}
