using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.UI;
using static Terraria.Main;
using static BetterUltrawide.BetterUltrawide;

namespace HotBar
{
    public class HotBarClass
    {

        public static void DrawInterface_30_Hotbar()
        {
            try
            {
                GUIHotbarDrawInner();
            }
            catch (Exception e)
            {
                if (ignoreErrors)
                {
                    TimeLogger.DrawException(e);
                    return;
                }

                throw;
            }
        }

        private static void GUIHotbarDrawInner()
        {
            if (playerInventory || player[myPlayer].ghost)
                return;

            string text = Lang.inter[37].Value;
            if (player[myPlayer].inventory[player[myPlayer].selectedItem].Name != null && player[myPlayer].inventory[player[myPlayer].selectedItem].Name != "")
                text = player[myPlayer].inventory[player[myPlayer].selectedItem].AffixName();

            float hotbarText = 236f + leftInterfacePostion;
            DynamicSpriteFontExtensionMethods.DrawString(position: new Vector2(hotbarText - (FontAssets.MouseText.Value.MeasureString(text) / 2f).X, leftInterfaceY), spriteBatch: spriteBatch, spriteFont: FontAssets.MouseText.Value, text: text, color: new Microsoft.Xna.Framework.Color(mouseTextColor, mouseTextColor, mouseTextColor, mouseTextColor), rotation: 0f, origin: default(Vector2), scale: 1f, effects: SpriteEffects.None, layerDepth: 0f);
            // changed from "20"
            int num = leftInterfacePostion;
            for (int i = 0; i < 10; i++)
            {
                if (i == player[myPlayer].selectedItem)
                {
                    if (hotbarScale[i] < 1f)
                        hotbarScale[i] += 0.05f;
                }
                else if ((double)hotbarScale[i] > 0.75)
                {
                    hotbarScale[i] -= 0.05f;
                }

                float num2 = hotbarScale[i];
                int num3 = (int)(20f + 22f * (1f - num2)) + leftInterfaceY;
                int a = (int)(75f + 150f * num2);
                Microsoft.Xna.Framework.Color lightColor = new Microsoft.Xna.Framework.Color(255, 255, 255, a);
                if (!player[myPlayer].hbLocked && !PlayerInput.IgnoreMouseInterface && mouseX >= num && (float)mouseX <= (float)num + (float)TextureAssets.InventoryBack.Width() * hotbarScale[i] && mouseY >= num3 && (float)mouseY <= (float)num3 + (float)TextureAssets.InventoryBack.Height() * hotbarScale[i] && !player[myPlayer].channel)
                {
                    player[myPlayer].mouseInterface = true;
                    player[myPlayer].cursorItemIconEnabled = false;
                    if (mouseLeft && !player[myPlayer].hbLocked && !blockMouse)
                        player[myPlayer].changeItem = i;

                    hoverItemName = player[myPlayer].inventory[i].AffixName();
                    if (player[myPlayer].inventory[i].stack > 1)
                        hoverItemName = hoverItemName + " (" + player[myPlayer].inventory[i].stack + ")";

                    rare = player[myPlayer].inventory[i].rare;
                }

                float num4 = inventoryScale;
                inventoryScale = num2;
                ItemSlot.Draw(spriteBatch, player[myPlayer].inventory, 13, i, new Vector2(num, num3), lightColor);
                inventoryScale = num4;
                num += (int)((float)TextureAssets.InventoryBack.Width() * hotbarScale[i]) + 4;
            }

            int selectedItem = player[myPlayer].selectedItem;
            if (selectedItem >= 10 && (selectedItem != 58 || mouseItem.type > ItemID.None))
            {
                float num5 = 1f;
                int num6 = (int)(20f + 22f * (1f - num5)) + leftInterfaceY;
                int a2 = (int)(75f + 150f * num5);
                Microsoft.Xna.Framework.Color lightColor2 = new Microsoft.Xna.Framework.Color(255, 255, 255, a2);
                float num7 = inventoryScale;
                inventoryScale = num5;
                ItemSlot.Draw(spriteBatch, player[myPlayer].inventory, 13, selectedItem, new Vector2(num, num6), lightColor2);
                inventoryScale = num7;
            }
        }
    }
}