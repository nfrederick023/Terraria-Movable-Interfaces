using System;
using System.Collections.Generic;
using System.Linq;
using AccessMain;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.Achievements;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.UI.Gamepad;
using static Terraria.Main;

namespace Inventory
{
    public class InventoryClass
    {
        public static void DrawInterface_27_Inventory()
        {
            AccessMainClass.callMainMethod("HackForGamepadInputHell");
            if (playerInventory)
            {
                if (player[myPlayer].chest != -1)
                    CreativeMenu.CloseMenu();

                if (ignoreErrors)
                {
                    try
                    {
                        DrawInventory();
                        return;
                    }
                    catch (Exception e)
                    {
                        TimeLogger.DrawException(e);
                        return;
                    }
                }

                DrawInventory();
            }
            else
            {
                CreativeMenu.CloseMenu();
                recFastScroll = true;
                AccessMainClass.callMainMethod("SetMouseNPC", -1, -1);
                EquipPage = 0;
            }
        }


        private static void DrawInventory()
        {
            Recipe.GetThroughDelayedFindRecipes();
            if (ShouldPVPDraw)
                DrawPVPIcons();

            int num = 0;
            int num2 = 0;
            int num3 = screenWidth;
            int num4 = 0;
            int num5 = screenWidth;
            int num6 = 0;
            Vector2 vector = new Vector2(num, num2);
            new Vector2(num3, num4);
            new Vector2(num5, num6);
            DrawBestiaryIcon(num, num2);
            DrawEmoteBubblesButton(num, num2);
            DrawTrashItemSlot(num, num2);
            spriteBatch.DrawString(FontAssets.MouseText.Value, Lang.inter[4].Value, new Vector2(40f, 0f) + vector, new Microsoft.Xna.Framework.Color(mouseTextColor, mouseTextColor, mouseTextColor, mouseTextColor), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
            inventoryScale = 0.85f;
            if (mouseX > 20 && mouseX < (int)(20f + 560f * inventoryScale) && mouseY > 20 && mouseY < (int)(20f + 280f * inventoryScale) && !PlayerInput.IgnoreMouseInterface)
                player[myPlayer].mouseInterface = true;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int num7 = (int)(20f + (float)(i * 56) * inventoryScale) + num;
                    int num8 = (int)(20f + (float)(j * 56) * inventoryScale) + num2;
                    int num9 = i + j * 10;
                    new Microsoft.Xna.Framework.Color(100, 100, 100, 100);
                    if (mouseX >= num7 && (float)mouseX <= (float)num7 + (float)TextureAssets.InventoryBack.Width() * inventoryScale && mouseY >= num8 && (float)mouseY <= (float)num8 + (float)TextureAssets.InventoryBack.Height() * inventoryScale && !PlayerInput.IgnoreMouseInterface)
                    {
                        player[myPlayer].mouseInterface = true;
                        ItemSlot.OverrideHover(player[myPlayer].inventory, 0, num9);
                        if (player[myPlayer].inventoryChestStack[num9] && (player[myPlayer].inventory[num9].type == ItemID.None || player[myPlayer].inventory[num9].stack == 0))
                            player[myPlayer].inventoryChestStack[num9] = false;

                        if (!player[myPlayer].inventoryChestStack[num9])
                        {
                            ItemSlot.LeftClick(player[myPlayer].inventory, 0, num9);
                            ItemSlot.RightClick(player[myPlayer].inventory, 0, num9);
                            if (mouseLeftRelease && mouseLeft)
                                Recipe.FindRecipes();
                        }

                        ItemSlot.MouseHover(player[myPlayer].inventory, 0, num9);
                    }

                    ItemSlot.Draw(spriteBatch, player[myPlayer].inventory, 0, num9, new Vector2(num7, num8));
                }
            }

            /*
            GetBuilderAccsCountToShow(LocalPlayer, out var _, out var _, out var totalDrawnIcons);
            bool pushSideToolsUp = totalDrawnIcons >= 10;
            */
            int activeToggles = BuilderToggleLoader.ActiveBuilderToggles();
            bool pushSideToolsUp = activeToggles / 12 != BuilderToggleLoader.BuilderTogglePage || activeToggles % 12 >= 10;
            if (!PlayerInput.UsingGamepad)
                DrawHotbarLockIcon(num, num2, pushSideToolsUp);

            ItemSlot.DrawRadialDpad(spriteBatch, new Vector2(20f) + new Vector2(56f * inventoryScale * 10f, 56f * inventoryScale * 5f) + new Vector2(26f, 70f) + vector);
            if (((AchievementAdvisor)AccessMainClass.getMainField("_achievementAdvisor")).CanDrawAboveCoins)
            {
                int num10 = (int)(20f + 560f * inventoryScale) + num;
                int num11 = (int)(20f + 0f * inventoryScale) + num2;
                ((AchievementAdvisor)AccessMainClass.getMainField("_achievementAdvisor")).DrawOneAchievement(spriteBatch, new Vector2(num10, num11) + new Vector2(5f), large: true);
            }

            if (mapEnabled)
            {
                bool flag = false;
                int num12 = num3 - 440;
                int num13 = 40 + num4;
                if (screenWidth < 940)
                    flag = true;

                if (flag)
                {
                    num12 = num5 - 40;
                    num13 = num6 - 200;
                }

                int num14 = 0;
                for (int k = 0; k < 4; k++)
                {
                    int num15 = 255;
                    int num16 = num12 + k * 32 - num14;
                    int num17 = num13;
                    if (flag)
                    {
                        num16 = num12;
                        num17 = num13 + k * 32 - num14;
                    }

                    int num18 = k;
                    num15 = 120;
                    if (k > 0 && mapStyle == k - 1)
                        num15 = 200;

                    if (mouseX >= num16 && mouseX <= num16 + 32 && mouseY >= num17 && mouseY <= num17 + 30 && !PlayerInput.IgnoreMouseInterface)
                    {
                        num15 = 255;
                        num18 += 4;
                        player[myPlayer].mouseInterface = true;
                        if (mouseLeft && mouseLeftRelease)
                        {
                            if (k == 0)
                            {
                                playerInventory = false;
                                player[myPlayer].SetTalkNPC(-1);
                                npcChatCornerItem = 0;
                                SoundEngine.PlaySound(SoundID.MenuOpen);
                                mapFullscreenScale = 2.5f;
                                mapFullscreen = true;
                                resetMapFull = true;
                            }

                            if (k == 1)
                            {
                                mapStyle = 0;
                                SoundEngine.PlaySound(SoundID.MenuTick);
                            }

                            if (k == 2)
                            {
                                mapStyle = 1;
                                SoundEngine.PlaySound(SoundID.MenuTick);
                            }

                            if (k == 3)
                            {
                                mapStyle = 2;
                                SoundEngine.PlaySound(SoundID.MenuTick);
                            }
                        }
                    }

                    spriteBatch.Draw(TextureAssets.MapIcon[num18].Value, new Vector2(num16, num17), new Microsoft.Xna.Framework.Rectangle(0, 0, TextureAssets.MapIcon[num18].Width(), TextureAssets.MapIcon[num18].Height()), new Microsoft.Xna.Framework.Color(num15, num15, num15, num15), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                }
            }

            if (armorHide)
            {
                armorAlpha -= 0.1f;
                if (armorAlpha < 0f)
                    armorAlpha = 0f;
            }
            else
            {
                armorAlpha += 0.025f;
                if (armorAlpha > 1f)
                    armorAlpha = 1f;
            }

            new Microsoft.Xna.Framework.Color((byte)((float)(int)mouseTextColor * armorAlpha), (byte)((float)(int)mouseTextColor * armorAlpha), (byte)((float)(int)mouseTextColor * armorAlpha), (byte)((float)(int)mouseTextColor * armorAlpha));
            armorHide = false;
            int num19 = 8 + player[myPlayer].GetAmountOfExtraAccessorySlotsToShow();
            int num20 = 174 + (int)AccessMainClass.getMainField("mH");
            int num21 = 950;
            AccessMainClass.setMainField("_cannotDrawAccessoriesHorizontally", false);
            if (screenHeight < num21 && num19 >= 10)
            {
                num20 -= (int)(56f * inventoryScale * (float)(num19 - 9));
                AccessMainClass.setMainField("_cannotDrawAccessoriesHorizontally", true);
            }

            int num22 = DrawPageIcons(num20 - 32);
            if (num22 > -1)
            {
                HoverItem = new Item();
                switch (num22)
                {
                    case 1:
                        hoverItemName = Lang.inter[80].Value;
                        break;
                    case 2:
                        hoverItemName = Lang.inter[79].Value;
                        break;
                    case 3:
                        hoverItemName = (CaptureModeDisabled ? Lang.inter[115].Value : Lang.inter[81].Value);
                        break;
                }
            }

            if (EquipPage == 2)
            {
                Microsoft.Xna.Framework.Point value = new Microsoft.Xna.Framework.Point(mouseX, mouseY);
                Microsoft.Xna.Framework.Rectangle r = new Microsoft.Xna.Framework.Rectangle(0, 0, (int)((float)TextureAssets.InventoryBack.Width() * inventoryScale), (int)((float)TextureAssets.InventoryBack.Height() * inventoryScale));
                Item[] inv = player[myPlayer].miscEquips;
                int num23 = screenWidth - 92;
                int num24 = (int)AccessMainClass.getMainField("mH") + 174;
                for (int l = 0; l < 2; l++)
                {
                    switch (l)
                    {
                        case 0:
                            inv = player[myPlayer].miscEquips;
                            break;
                        case 1:
                            inv = player[myPlayer].miscDyes;
                            break;
                    }

                    r.X = num23 + l * -47;
                    for (int m = 0; m < 5; m++)
                    {
                        int context = 0;
                        int num25 = -1;
                        bool flag2 = false;
                        switch (m)
                        {
                            case 0:
                                context = 19;
                                num25 = 0;
                                break;
                            case 1:
                                context = 20;
                                num25 = 1;
                                break;
                            case 2:
                                context = 18;
                                flag2 = player[myPlayer].unlockedSuperCart;
                                break;
                            case 3:
                                context = 17;
                                break;
                            case 4:
                                context = 16;
                                break;
                        }

                        if (l == 1)
                        {
                            context = 33;
                            num25 = -1;
                            flag2 = false;
                        }

                        r.Y = num24 + m * 47;
                        bool flag3 = false;
                        Texture2D value2 = TextureAssets.InventoryTickOn.Value;
                        Microsoft.Xna.Framework.Rectangle r2 = new Microsoft.Xna.Framework.Rectangle(r.Left + 34, r.Top - 2, value2.Width, value2.Height);
                        int num26 = 0;
                        if (num25 != -1)
                        {
                            if (player[myPlayer].hideMisc[num25])
                                value2 = TextureAssets.InventoryTickOff.Value;

                            if (r2.Contains(value) && !PlayerInput.IgnoreMouseInterface)
                            {
                                player[myPlayer].mouseInterface = true;
                                flag3 = true;
                                if (mouseLeft && mouseLeftRelease)
                                {
                                    if (num25 == 0)
                                        player[myPlayer].TogglePet();

                                    if (num25 == 1)
                                        player[myPlayer].ToggleLight();

                                    mouseLeftRelease = false;
                                    SoundEngine.PlaySound(SoundID.MenuTick);
                                    if (netMode == NetmodeID.MultiplayerClient)
                                        NetMessage.SendData(MessageID.SyncPlayer, -1, -1, null, myPlayer);
                                }

                                num26 = ((!player[myPlayer].hideMisc[num25]) ? 1 : 2);
                            }
                        }

                        if (flag2)
                        {
                            value2 = TextureAssets.Extra[255].Value;
                            if (!player[myPlayer].enabledSuperCart)
                                value2 = TextureAssets.Extra[256].Value;

                            r2 = new Microsoft.Xna.Framework.Rectangle(r2.X + r2.Width / 2, r2.Y + r2.Height / 2, r2.Width, r2.Height);
                            r2.Offset(-r2.Width / 2, -r2.Height / 2);
                            if (r2.Contains(value) && !PlayerInput.IgnoreMouseInterface)
                            {
                                player[myPlayer].mouseInterface = true;
                                flag3 = true;
                                if (mouseLeft && mouseLeftRelease)
                                {
                                    player[myPlayer].enabledSuperCart = !player[myPlayer].enabledSuperCart;
                                    mouseLeftRelease = false;
                                    SoundEngine.PlaySound(SoundID.MenuTick);
                                    if (netMode == NetmodeID.MultiplayerClient)
                                        NetMessage.SendData(MessageID.SyncPlayer, -1, -1, null, myPlayer);
                                }

                                num26 = ((!player[myPlayer].enabledSuperCart) ? 1 : 2);
                            }
                        }

                        if (r.Contains(value) && !flag3 && !PlayerInput.IgnoreMouseInterface)
                        {
                            player[myPlayer].mouseInterface = true;
                            armorHide = true;
                            ItemSlot.Handle(inv, context, m);
                        }

                        ItemSlot.Draw(spriteBatch, inv, context, m, r.TopLeft());
                        if (num25 != -1)
                        {
                            spriteBatch.Draw(value2, r2.TopLeft(), Microsoft.Xna.Framework.Color.White * 0.7f);
                            if (num26 > 0)
                            {
                                HoverItem = new Item();
                                hoverItemName = Lang.inter[58 + num26].Value;
                            }
                        }

                        if (flag2)
                        {
                            spriteBatch.Draw(value2, r2.TopLeft(), Microsoft.Xna.Framework.Color.White);
                            if (num26 > 0)
                            {
                                HoverItem = new Item();
                                hoverItemName = Language.GetTextValue((num26 == 1) ? "GameUI.SuperCartDisabled" : "GameUI.SuperCartEnabled");
                            }
                        }
                    }
                }

                num24 += 247;
                num23 += 8;
                int num27 = -1;
                int num28 = 0;
                int num29 = 3;
                int num30 = 260;
                if (screenHeight > 630 + num30 * (mapStyle == 1).ToInt())
                    num29++;

                if (screenHeight > 680 + num30 * (mapStyle == 1).ToInt())
                    num29++;

                if (screenHeight > 730 + num30 * (mapStyle == 1).ToInt())
                    num29++;

                int num31 = 46;
                for (int n = 0; n < Player.MaxBuffs; n++)
                {
                    if (player[myPlayer].buffType[n] != 0)
                    {
                        int num32 = num28 / num29;
                        int num33 = num28 % num29;
                        Microsoft.Xna.Framework.Point point = new Microsoft.Xna.Framework.Point(num23 + num32 * -num31, num24 + num33 * num31);
                        num27 = DrawBuffIcon(num27, n, point.X, point.Y);
                        UILinkPointNavigator.SetPosition(9000 + num28, new Vector2(point.X + 30, point.Y + 30));
                        num28++;
                        if (buffAlpha[n] < 0.65f)
                            buffAlpha[n] = 0.65f;
                    }
                }

                UILinkPointNavigator.Shortcuts.BUFFS_DRAWN = num28;
                UILinkPointNavigator.Shortcuts.BUFFS_PER_COLUMN = num29;
                if (num27 >= 0)
                {
                    int num34 = player[myPlayer].buffType[num27];
                    if (num34 > 0)
                    {
                        string buffName = Lang.GetBuffName(num34);
                        string buffTooltip = GetBuffTooltip(player[myPlayer], num34);
                        if (num34 == 147)
                            bannerMouseOver = true;

                        /*
                        if (meleeBuff[num34])
                            MouseTextHackZoom(buffName, -10, 0, buffTooltip);
                        else
                            MouseTextHackZoom(buffName, buffTooltip);
                        */
                        int rare = 0;
                        if (meleeBuff[num34])
                            rare = -10;

                        BuffLoader.ModifyBuffText(num34, ref buffName, ref buffTooltip, ref rare);
                        instance.MouseTextHackZoom(buffName, rare, diff: 0, buffTooltip);
                    }
                }
            }
            else if (EquipPage == 1)
            {
                DrawNPCHousesInUI();
            }
            else if (EquipPage == 0)
            { // Added 'if (EquipPage == 0)' to add custom equip pages
                int num35 = 4;
                if (mouseX > screenWidth - 64 - 28 && mouseX < (int)((float)(screenWidth - 64 - 28) + 56f * inventoryScale) && mouseY > num20 && mouseY < (int)((float)num20 + 448f * inventoryScale) && !PlayerInput.IgnoreMouseInterface)
                    player[myPlayer].mouseInterface = true;

                float num36 = inventoryScale;
                bool flag4 = false;
                int num37 = num19 - 1;
                bool flag5 = LocalPlayer.CanDemonHeartAccessoryBeShown();
                bool flag6 = LocalPlayer.CanMasterModeAccessoryBeShown();
                if ((bool)AccessMainClass.getMainField("_settingsButtonIsPushedToSide"))
                    num37--;

                int num38 = num37 - 1;
                Microsoft.Xna.Framework.Color color = inventoryBack;
                Microsoft.Xna.Framework.Color color2 = new Microsoft.Xna.Framework.Color(80, 80, 80, 80);
                DrawLoadoutButtons(num20, flag5, flag6);
                int num39 = -1;

                // Vanilla acc (not armor) slot drawing moved to AccessorySlotLoader.DrawAccSlots
                /*
                for (int num40 = 0; num40 < 10; num40++) {
                */
                for (int num40 = 0; num40 < 3; num40++)
                {
                    if ((num40 == 8 && !flag5) || (num40 == 9 && !flag6))
                        continue;

                    num39++;
                    bool flag7 = LocalPlayer.IsItemSlotUnlockedAndUsable(num40);
                    if (!flag7)
                        flag4 = true;

                    int num41 = screenWidth - 64 - 28;
                    int num42 = (int)((float)num20 + (float)(num39 * 56) * inventoryScale);
                    new Microsoft.Xna.Framework.Color(100, 100, 100, 100);
                    int num43 = screenWidth - 58;
                    int num44 = (int)((float)(num20 - 2) + (float)(num39 * 56) * inventoryScale);
                    int context2 = 8;
                    if (num40 > 2)
                    {
                        num42 += num35;
                        num44 += num35;
                        context2 = 10;
                    }

                    // Moved below, after modded acc slots have drawn
                    /*
                    if (num39 == num38 && !_achievementAdvisor.CanDrawAboveCoins) {
                        _achievementAdvisor.DrawOneAchievement(spriteBatch, new Vector2(num41 - 10 - 47 - 47 - 14 - 14, num42 + 8), large: false);
                        UILinkPointNavigator.SetPosition(1570, new Vector2(num41 - 10 - 47 - 47 - 14 - 14, num42 + 8) + new Vector2(20f) * inventoryScale);
                    }

                    if (num39 == num37)
                        DrawDefenseCounter(num41, num42);
                    */

                    Texture2D value3 = TextureAssets.InventoryTickOn.Value;
                    if (player[myPlayer].hideVisibleAccessory[num40])
                        value3 = TextureAssets.InventoryTickOff.Value;

                    Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(num43, num44, value3.Width, value3.Height);
                    int num45 = 0;
                    if (num40 > 2 && rectangle.Contains(new Microsoft.Xna.Framework.Point(mouseX, mouseY)) && !PlayerInput.IgnoreMouseInterface)
                    {
                        player[myPlayer].mouseInterface = true;
                        if (mouseLeft && mouseLeftRelease)
                        {
                            player[myPlayer].hideVisibleAccessory[num40] = !player[myPlayer].hideVisibleAccessory[num40];
                            SoundEngine.PlaySound(SoundID.MenuTick);
                            if (netMode == NetmodeID.MultiplayerClient)
                                NetMessage.SendData(MessageID.SyncPlayer, -1, -1, null, myPlayer);
                        }

                        num45 = ((!player[myPlayer].hideVisibleAccessory[num40]) ? 1 : 2);
                    }
                    else if (mouseX >= num41 && (float)mouseX <= (float)num41 + (float)TextureAssets.InventoryBack.Width() * inventoryScale && mouseY >= num42 && (float)mouseY <= (float)num42 + (float)TextureAssets.InventoryBack.Height() * inventoryScale && !PlayerInput.IgnoreMouseInterface)
                    {
                        armorHide = true;
                        player[myPlayer].mouseInterface = true;
                        ItemSlot.OverrideHover(player[myPlayer].armor, context2, num40);
                        if (flag7 || mouseItem.IsAir)
                            ItemSlot.LeftClick(player[myPlayer].armor, context2, num40);

                        ItemSlot.MouseHover(player[myPlayer].armor, context2, num40);
                    }

                    if (flag4)
                        inventoryBack = color2;

                    ItemSlot.Draw(spriteBatch, player[myPlayer].armor, context2, num40, new Vector2(num41, num42));
                    if (num40 > 2)
                    {
                        spriteBatch.Draw(value3, new Vector2(num43, num44), Microsoft.Xna.Framework.Color.White * 0.7f);
                        if (num45 > 0)
                        {
                            HoverItem = new Item();
                            hoverItemName = Lang.inter[58 + num45].Value;
                        }
                    }
                }

                inventoryBack = color;
                if (mouseX > screenWidth - 64 - 28 - 47 && mouseX < (int)((float)(screenWidth - 64 - 20 - 47) + 56f * inventoryScale) && mouseY > num20 && mouseY < (int)((float)num20 + 168f * inventoryScale) && !PlayerInput.IgnoreMouseInterface)
                    player[myPlayer].mouseInterface = true;

                num39 = -1;

                // Vanilla acc (not armor) slot drawing moved to AccessorySlotLoader.DrawAccSlots
                /*
                for (int num46 = 10; num46 < 20; num46++) {
                */
                for (int num46 = 10; num46 < 13; num46++)
                {
                    if ((num46 == 18 && !flag5) || (num46 == 19 && !flag6))
                        continue;

                    num39++;
                    bool num47 = LocalPlayer.IsItemSlotUnlockedAndUsable(num46);
                    flag4 = !num47;
                    bool flag8 = !num47 && !mouseItem.IsAir;
                    int num48 = screenWidth - 64 - 28 - 47;
                    int num49 = (int)((float)num20 + (float)(num39 * 56) * inventoryScale);
                    new Microsoft.Xna.Framework.Color(100, 100, 100, 100);
                    if (num46 > 12)
                        num49 += num35;

                    int context3 = 9;
                    if (num46 > 12)
                        context3 = 11;

                    if (mouseX >= num48 && (float)mouseX <= (float)num48 + (float)TextureAssets.InventoryBack.Width() * inventoryScale && mouseY >= num49 && (float)mouseY <= (float)num49 + (float)TextureAssets.InventoryBack.Height() * inventoryScale && !PlayerInput.IgnoreMouseInterface)
                    {
                        player[myPlayer].mouseInterface = true;
                        armorHide = true;
                        ItemSlot.OverrideHover(player[myPlayer].armor, context3, num46);
                        if (!flag8)
                        {
                            ItemSlot.LeftClick(player[myPlayer].armor, context3, num46);
                            ItemSlot.RightClick(player[myPlayer].armor, context3, num46);
                        }

                        ItemSlot.MouseHover(player[myPlayer].armor, context3, num46);
                    }

                    if (flag4)
                        inventoryBack = color2;

                    ItemSlot.Draw(spriteBatch, player[myPlayer].armor, context3, num46, new Vector2(num48, num49));
                }

                inventoryBack = color;
                if (mouseX > screenWidth - 64 - 28 - 47 && mouseX < (int)((float)(screenWidth - 64 - 20 - 47) + 56f * inventoryScale) && mouseY > num20 && mouseY < (int)((float)num20 + 168f * inventoryScale) && !PlayerInput.IgnoreMouseInterface)
                    player[myPlayer].mouseInterface = true;

                num39 = -1;

                // Vanilla acc (not armor) slot drawing moved to AccessorySlotLoader.DrawAccSlots
                /*
                for (int num50 = 0; num50 < 10; num50++) {
                */
                for (int num50 = 0; num50 < 3; num50++)
                {
                    if ((num50 == 8 && !flag5) || (num50 == 9 && !flag6))
                        continue;

                    num39++;
                    bool num51 = LocalPlayer.IsItemSlotUnlockedAndUsable(num50);
                    flag4 = !num51;
                    bool flag9 = !num51 && !mouseItem.IsAir;
                    int num52 = screenWidth - 64 - 28 - 47 - 47;
                    int num53 = (int)((float)num20 + (float)(num39 * 56) * inventoryScale);
                    new Microsoft.Xna.Framework.Color(100, 100, 100, 100);
                    if (num50 > 2)
                        num53 += num35;

                    if (mouseX >= num52 && (float)mouseX <= (float)num52 + (float)TextureAssets.InventoryBack.Width() * inventoryScale && mouseY >= num53 && (float)mouseY <= (float)num53 + (float)TextureAssets.InventoryBack.Height() * inventoryScale && !PlayerInput.IgnoreMouseInterface)
                    {
                        player[myPlayer].mouseInterface = true;
                        armorHide = true;
                        ItemSlot.OverrideHover(player[myPlayer].dye, 12, num50);
                        if (!flag9)
                        {
                            if (mouseRightRelease && mouseRight)
                                ItemSlot.RightClick(player[myPlayer].dye, 12, num50);

                            ItemSlot.LeftClick(player[myPlayer].dye, 12, num50);
                        }

                        ItemSlot.MouseHover(player[myPlayer].dye, 12, num50);
                    }

                    if (flag4)
                        inventoryBack = color2;

                    ItemSlot.Draw(spriteBatch, player[myPlayer].dye, 12, num50, new Vector2(num52, num53));
                }

                inventoryBack = color;

                // Moved from above [[
                var defPos = AccessorySlotLoader.DefenseIconPosition;
                DrawDefenseCounter((int)defPos.X, (int)defPos.Y);

                if (!((AchievementAdvisor)AccessMainClass.getMainField("_achievementAdvisor")).CanDrawAboveCoins)
                {
                    var achievePos = new Vector2(defPos.X - 10 - 47 - 47 - 14 - 14, defPos.Y - 56 * inventoryScale * 0.5f);
                    ((AchievementAdvisor)AccessMainClass.getMainField("_achievementAdvisor")).DrawOneAchievement(spriteBatch, achievePos, large: false);
                    UILinkPointNavigator.SetPosition(1570, achievePos + new Vector2(20f) * inventoryScale);
                }

                inventoryBack = color;
                // ]]

                inventoryScale = num36;
            }

            LoaderManager.Get<AccessorySlotLoader>().DrawAccSlots(num20);

            int num54 = (screenHeight - 600) / 2;
            int num55 = (int)((float)screenHeight / 600f * 250f);
            if (screenHeight < 700)
            {
                num54 = (screenHeight - 508) / 2;
                num55 = (int)((float)screenHeight / 600f * 200f);
            }
            else if (screenHeight < 850)
            {
                num55 = (int)((float)screenHeight / 600f * 225f);
            }

            if (craftingHide)
            {
                craftingAlpha -= 0.1f;
                if (craftingAlpha < 0f)
                    craftingAlpha = 0f;
            }
            else
            {
                craftingAlpha += 0.025f;
                if (craftingAlpha > 1f)
                    craftingAlpha = 1f;
            }

            Microsoft.Xna.Framework.Color color3 = new Microsoft.Xna.Framework.Color((byte)((float)(int)mouseTextColor * craftingAlpha), (byte)((float)(int)mouseTextColor * craftingAlpha), (byte)((float)(int)mouseTextColor * craftingAlpha), (byte)((float)(int)mouseTextColor * craftingAlpha));
            craftingHide = false;
            if (InReforgeMenu)
            {
                if (mouseReforge)
                {
                    if (reforgeScale < 1f)
                        reforgeScale += 0.02f;
                }
                else if (reforgeScale > 1f)
                {
                    reforgeScale -= 0.02f;
                }

                if (player[myPlayer].chest != -1 || npcShop != 0 || player[myPlayer].talkNPC == -1 || InGuideCraftMenu)
                {
                    InReforgeMenu = false;
                    player[myPlayer].dropItemCheck();
                    Recipe.FindRecipes();
                }
                else
                {
                    int num56 = 50;
                    int num57 = 270;
                    string text = Lang.inter[46].Value + ": ";
                    if (reforgeItem.type > ItemID.None)
                    {
                        int num58 = reforgeItem.value;
                        num58 *= reforgeItem.stack; // #StackablePrefixWeapons: scale with current stack size

                        bool canApplyDiscount = true;
                        if (!ItemLoader.ReforgePrice(reforgeItem, ref num58, ref canApplyDiscount))
                            goto skipVanillaPricing;

                        /*
                        if (player[myPlayer].discountAvailable)
                        */
                        if (canApplyDiscount && LocalPlayer.discountAvailable)
                            num58 = (int)((double)num58 * 0.8);

                        num58 = (int)((double)num58 * player[myPlayer].currentShoppingSettings.PriceAdjustment);
                        num58 /= 3;
                    skipVanillaPricing:

                        string text2 = "";
                        int num59 = 0;
                        int num60 = 0;
                        int num61 = 0;
                        int num62 = 0;
                        int num63 = num58;
                        if (num63 < 1)
                            num63 = 1;

                        if (num63 >= 1000000)
                        {
                            num59 = num63 / 1000000;
                            num63 -= num59 * 1000000;
                        }

                        if (num63 >= 10000)
                        {
                            num60 = num63 / 10000;
                            num63 -= num60 * 10000;
                        }

                        if (num63 >= 100)
                        {
                            num61 = num63 / 100;
                            num63 -= num61 * 100;
                        }

                        if (num63 >= 1)
                            num62 = num63;

                        if (num59 > 0)
                            text2 = text2 + "[c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + ":" + num59 + " " + Lang.inter[15].Value + "] ";

                        if (num60 > 0)
                            text2 = text2 + "[c/" + Colors.AlphaDarken(Colors.CoinGold).Hex3() + ":" + num60 + " " + Lang.inter[16].Value + "] ";

                        if (num61 > 0)
                            text2 = text2 + "[c/" + Colors.AlphaDarken(Colors.CoinSilver).Hex3() + ":" + num61 + " " + Lang.inter[17].Value + "] ";

                        if (num62 > 0)
                            text2 = text2 + "[c/" + Colors.AlphaDarken(Colors.CoinCopper).Hex3() + ":" + num62 + " " + Lang.inter[18].Value + "] ";

                        ItemSlot.DrawSavings(spriteBatch, num56 + 130, instance.invBottom, horizontal: true);
                        ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, text2, new Vector2((float)(num56 + 50) + FontAssets.MouseText.Value.MeasureString(text).X, num57), Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, Vector2.One);
                        int num64 = num56 + 70;
                        int num65 = num57 + 40;
                        bool num66 = mouseX > num64 - 15 && mouseX < num64 + 15 && mouseY > num65 - 15 && mouseY < num65 + 15 && !PlayerInput.IgnoreMouseInterface;
                        Texture2D value4 = TextureAssets.Reforge[0].Value;
                        if (num66)
                            value4 = TextureAssets.Reforge[1].Value;

                        spriteBatch.Draw(value4, new Vector2(num64, num65), null, Microsoft.Xna.Framework.Color.White, 0f, value4.Size() / 2f, reforgeScale, SpriteEffects.None, 0f);
                        UILinkPointNavigator.SetPosition(304, new Vector2(num64, num65) + value4.Size() / 4f);
                        if (num66)
                        {
                            hoverItemName = Lang.inter[19].Value;
                            if (!mouseReforge)
                                SoundEngine.PlaySound(SoundID.MenuTick);

                            mouseReforge = true;
                            player[myPlayer].mouseInterface = true;

                            /*
                            if (mouseLeftRelease && mouseLeft && player[myPlayer].BuyItem(num58)) {
                            */
                            if (mouseLeftRelease && mouseLeft && player[myPlayer].CanAfford(num58) && ItemLoader.CanReforge(reforgeItem))
                            {
                                player[myPlayer].BuyItem(num58);
                                ItemLoader.PreReforge(reforgeItem); // After BuyItem just in case

                                reforgeItem.ResetPrefix();
                                reforgeItem.Prefix(-2);
                                reforgeItem.position.X = player[myPlayer].position.X + (float)(player[myPlayer].width / 2) - (float)(reforgeItem.width / 2);
                                reforgeItem.position.Y = player[myPlayer].position.Y + (float)(player[myPlayer].height / 2) - (float)(reforgeItem.height / 2);

                                ItemLoader.PostReforge(reforgeItem);

                                PopupText.NewText(PopupTextContext.ItemReforge, reforgeItem, reforgeItem.stack, noStack: true);
                                SoundEngine.PlaySound(SoundID.Item37);
                            }
                        }
                        else
                        {
                            mouseReforge = false;
                        }
                    }
                    else
                    {
                        text = Lang.inter[20].Value;
                    }

                    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, text, new Vector2(num56 + 50, num57), new Microsoft.Xna.Framework.Color(mouseTextColor, mouseTextColor, mouseTextColor, mouseTextColor), 0f, Vector2.Zero, Vector2.One);
                    if (mouseX >= num56 && (float)mouseX <= (float)num56 + (float)TextureAssets.InventoryBack.Width() * inventoryScale && mouseY >= num57 && (float)mouseY <= (float)num57 + (float)TextureAssets.InventoryBack.Height() * inventoryScale && !PlayerInput.IgnoreMouseInterface)
                    {
                        player[myPlayer].mouseInterface = true;
                        craftingHide = true;
                        ItemSlot.LeftClick(ref reforgeItem, 5);
                        if (mouseLeftRelease && mouseLeft)
                            Recipe.FindRecipes();

                        ItemSlot.RightClick(ref reforgeItem, 5);
                        ItemSlot.MouseHover(ref reforgeItem, 5);
                    }

                    ItemSlot.Draw(spriteBatch, ref reforgeItem, 5, new Vector2(num56, num57));
                }
            }
            else if (InGuideCraftMenu)
            {
                if (player[myPlayer].chest != -1 || npcShop != 0 || player[myPlayer].talkNPC == -1 || InReforgeMenu)
                {
                    InGuideCraftMenu = false;
                    player[myPlayer].dropItemCheck();
                    Recipe.FindRecipes();
                }
                else
                {
                    DrawGuideCraftText(num54, color3, out var inventoryX, out var inventoryY);
                    new Microsoft.Xna.Framework.Color(100, 100, 100, 100);
                    if (mouseX >= inventoryX && (float)mouseX <= (float)inventoryX + (float)TextureAssets.InventoryBack.Width() * inventoryScale && mouseY >= inventoryY && (float)mouseY <= (float)inventoryY + (float)TextureAssets.InventoryBack.Height() * inventoryScale && !PlayerInput.IgnoreMouseInterface)
                    {
                        player[myPlayer].mouseInterface = true;
                        craftingHide = true;
                        ItemSlot.OverrideHover(ref guideItem, 7);
                        ItemSlot.LeftClick(ref guideItem, 7);
                        if (mouseLeftRelease && mouseLeft)
                            Recipe.FindRecipes();

                        ItemSlot.RightClick(ref guideItem, 7);
                        ItemSlot.MouseHover(ref guideItem, 7);
                    }

                    ItemSlot.Draw(spriteBatch, ref guideItem, 7, new Vector2(inventoryX, inventoryY));
                }
            }

            CreativeMenu.Draw(spriteBatch);
            bool flag10 = CreativeMenu.Enabled && !CreativeMenu.Blocked;

            // Added by TML.
            flag10 |= hidePlayerCraftingMenu;

            if (!InReforgeMenu && !LocalPlayer.tileEntityAnchor.InUse && !flag10)
            {
                UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig = -1;
                UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall = -1;
                if (numAvailableRecipes > 0)
                    spriteBatch.DrawString(FontAssets.MouseText.Value, Lang.inter[25].Value, new Vector2(76f, 414 + num54), color3, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);

                for (int num67 = 0; num67 < Recipe.maxRecipes; num67++)
                {
                    inventoryScale = 100f / (Math.Abs(availableRecipeY[num67]) + 100f);
                    if ((double)inventoryScale < 0.75)
                        inventoryScale = 0.75f;

                    if (recFastScroll)
                        inventoryScale = 0.75f;

                    if (availableRecipeY[num67] < (float)((num67 - focusRecipe) * 65))
                    {
                        if (availableRecipeY[num67] == 0f && !recFastScroll)
                            SoundEngine.PlaySound(SoundID.MenuTick);

                        availableRecipeY[num67] += 6.5f;
                        if (recFastScroll)
                            availableRecipeY[num67] += 130000f;

                        if (availableRecipeY[num67] > (float)((num67 - focusRecipe) * 65))
                            availableRecipeY[num67] = (num67 - focusRecipe) * 65;
                    }
                    else if (availableRecipeY[num67] > (float)((num67 - focusRecipe) * 65))
                    {
                        if (availableRecipeY[num67] == 0f && !recFastScroll)
                            SoundEngine.PlaySound(SoundID.MenuTick);

                        availableRecipeY[num67] -= 6.5f;
                        if (recFastScroll)
                            availableRecipeY[num67] -= 130000f;

                        if (availableRecipeY[num67] < (float)((num67 - focusRecipe) * 65))
                            availableRecipeY[num67] = (num67 - focusRecipe) * 65;
                    }
                    else
                    {
                        recFastScroll = false;
                    }

                    if (num67 >= numAvailableRecipes || Math.Abs(availableRecipeY[num67]) > (float)num55)
                        continue;

                    int num68 = (int)(46f - 26f * inventoryScale);
                    int num69 = (int)(410f + availableRecipeY[num67] * inventoryScale - 30f * inventoryScale + (float)num54);
                    double num70 = inventoryBack.A + 50;
                    double num71 = 255.0;
                    if (Math.Abs(availableRecipeY[num67]) > (float)num55 - 100f)
                    {
                        num70 = (double)(150f * (100f - (Math.Abs(availableRecipeY[num67]) - ((float)num55 - 100f)))) * 0.01;
                        num71 = (double)(255f * (100f - (Math.Abs(availableRecipeY[num67]) - ((float)num55 - 100f)))) * 0.01;
                    }

                    new Microsoft.Xna.Framework.Color((byte)num70, (byte)num70, (byte)num70, (byte)num70);
                    Microsoft.Xna.Framework.Color lightColor = new Microsoft.Xna.Framework.Color((byte)num71, (byte)num71, (byte)num71, (byte)num71);
                    if (!LocalPlayer.creativeInterface && mouseX >= num68 && (float)mouseX <= (float)num68 + (float)TextureAssets.InventoryBack.Width() * inventoryScale && mouseY >= num69 && (float)mouseY <= (float)num69 + (float)TextureAssets.InventoryBack.Height() * inventoryScale && !PlayerInput.IgnoreMouseInterface)
                        AccessMainClass.callMainMethod("HoverOverCraftingItemButton", num67);

                    if (numAvailableRecipes <= 0)
                        continue;

                    num70 -= 50.0;
                    if (num70 < 0.0)
                        num70 = 0.0;

                    if (num67 == focusRecipe)
                    {
                        UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall = 0;
                        if (PlayerInput.SettingsForUI.HighlightThingsForMouse)
                            ItemSlot.DrawGoldBGForCraftingMaterial = true;
                    }
                    else
                    {
                        UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall = -1;
                    }

                    Microsoft.Xna.Framework.Color color4 = inventoryBack;
                    inventoryBack = new Microsoft.Xna.Framework.Color((byte)num70, (byte)num70, (byte)num70, (byte)num70);
                    ItemSlot.Draw(spriteBatch, ref recipe[availableRecipe[num67]].createItem, 22, new Vector2(num68, num69), lightColor);
                    inventoryBack = color4;
                }

                if (numAvailableRecipes > 0)
                {
                    UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig = -1;
                    UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall = -1;
                    for (int num72 = 0; num72 < recipe[availableRecipe[focusRecipe]].requiredItem.Count; num72++)
                    {
                        if (recipe[availableRecipe[focusRecipe]].requiredItem[num72].type == ItemID.None)
                        {
                            UILinkPointNavigator.Shortcuts.CRAFT_CurrentIngredientsCount = num72 + 1;
                            break;
                        }

                        int num73 = 80 + num72 * 40;
                        int num74 = 380 + num54;
                        double num75 = inventoryBack.A + 50;
                        double num76 = 255.0;
                        Microsoft.Xna.Framework.Color white = Microsoft.Xna.Framework.Color.White;
                        Microsoft.Xna.Framework.Color white2 = Microsoft.Xna.Framework.Color.White;
                        num75 = (float)(inventoryBack.A + 50) - Math.Abs(availableRecipeY[focusRecipe]) * 2f;
                        num76 = 255f - Math.Abs(availableRecipeY[focusRecipe]) * 2f;
                        if (num75 < 0.0)
                            num75 = 0.0;

                        if (num76 < 0.0)
                            num76 = 0.0;

                        white.R = (byte)num75;
                        white.G = (byte)num75;
                        white.B = (byte)num75;
                        white.A = (byte)num75;
                        white2.R = (byte)num76;
                        white2.G = (byte)num76;
                        white2.B = (byte)num76;
                        white2.A = (byte)num76;
                        inventoryScale = 0.6f;
                        if (num75 == 0.0)
                            break;

                        if (mouseX >= num73 && (float)mouseX <= (float)num73 + (float)TextureAssets.InventoryBack.Width() * inventoryScale && mouseY >= num74 && (float)mouseY <= (float)num74 + (float)TextureAssets.InventoryBack.Height() * inventoryScale && !PlayerInput.IgnoreMouseInterface)
                        {
                            craftingHide = true;
                            player[myPlayer].mouseInterface = true;
                            AccessMainClass.callMainMethod("SetRecipeMaterialDisplayName", num72);
                        }

                        num75 -= 50.0;
                        if (num75 < 0.0)
                            num75 = 0.0;

                        UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall = 1 + num72;
                        Microsoft.Xna.Framework.Color color5 = inventoryBack;
                        inventoryBack = new Microsoft.Xna.Framework.Color((byte)num75, (byte)num75, (byte)num75, (byte)num75);

                        //TML: Ref a local copy variable instead of the data in the array.
                        /*
                        ItemSlot.Draw(spriteBatch, ref recipe[availableRecipe[focusRecipe]].requiredItem[num72], 22, new Vector2(num73, num74));
                        */
                        Item tempItem = recipe[availableRecipe[focusRecipe]].requiredItem[num72];
                        ItemSlot.Draw(spriteBatch, ref tempItem, 22, new Vector2(num73, num74));
                        inventoryBack = color5;
                    }
                }

                if (numAvailableRecipes == 0)
                {
                    recBigList = false;
                }
                else
                {
                    int num77 = 94;
                    int num78 = 450 + num54;
                    if (InGuideCraftMenu)
                        num78 -= 150;

                    bool flag11 = mouseX > num77 - 15 && mouseX < num77 + 15 && mouseY > num78 - 15 && mouseY < num78 + 15 && !PlayerInput.IgnoreMouseInterface;
                    int num79 = recBigList.ToInt() * 2 + flag11.ToInt();
                    spriteBatch.Draw(TextureAssets.CraftToggle[num79].Value, new Vector2(num77, num78), null, Microsoft.Xna.Framework.Color.White, 0f, TextureAssets.CraftToggle[num79].Value.Size() / 2f, 1f, SpriteEffects.None, 0f);
                    if (flag11)
                    {
                        instance.MouseText(Language.GetTextValue("GameUI.CraftingWindow"), 0, 0);
                        player[myPlayer].mouseInterface = true;
                        if (mouseLeft && mouseLeftRelease)
                        {
                            if (!recBigList)
                            {
                                recBigList = true;
                                SoundEngine.PlaySound(SoundID.MenuTick);
                            }
                            else
                            {
                                recBigList = false;
                                SoundEngine.PlaySound(SoundID.MenuTick);
                            }
                        }
                    }
                }
            }

            // Added by TML
            hidePlayerCraftingMenu = false;

            if (recBigList && !flag10)
            {
                UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig = -1;
                UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall = -1;
                int num80 = 42;
                if ((double)inventoryScale < 0.75)
                    inventoryScale = 0.75f;

                int num81 = 340;
                int num82 = 310;
                int num83 = (screenWidth - num82 - 280) / num80;
                int num84 = (screenHeight - num81 - 20) / num80;
                UILinkPointNavigator.Shortcuts.CRAFT_IconsPerRow = num83;
                UILinkPointNavigator.Shortcuts.CRAFT_IconsPerColumn = num84;
                int num85 = 0;
                int num86 = 0;
                int num87 = num82;
                int num88 = num81;
                int num89 = num82 - 20;
                int num90 = num81 + 2;
                if (recStart > numAvailableRecipes - num83 * num84)
                {
                    recStart = numAvailableRecipes - num83 * num84;
                    if (recStart < 0)
                        recStart = 0;
                }

                if (recStart > 0)
                {
                    if (mouseX >= num89 && mouseX <= num89 + TextureAssets.CraftUpButton.Width() && mouseY >= num90 && mouseY <= num90 + TextureAssets.CraftUpButton.Height() && !PlayerInput.IgnoreMouseInterface)
                    {
                        player[myPlayer].mouseInterface = true;
                        if (mouseLeftRelease && mouseLeft)
                        {
                            recStart -= num83;
                            if (recStart < 0)
                                recStart = 0;

                            SoundEngine.PlaySound(SoundID.MenuTick);
                            mouseLeftRelease = false;
                        }
                    }

                    spriteBatch.Draw(TextureAssets.CraftUpButton.Value, new Vector2(num89, num90), new Microsoft.Xna.Framework.Rectangle(0, 0, TextureAssets.CraftUpButton.Width(), TextureAssets.CraftUpButton.Height()), new Microsoft.Xna.Framework.Color(200, 200, 200, 200), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                }

                if (recStart < numAvailableRecipes - num83 * num84)
                {
                    num90 += 20;
                    if (mouseX >= num89 && mouseX <= num89 + TextureAssets.CraftUpButton.Width() && mouseY >= num90 && mouseY <= num90 + TextureAssets.CraftUpButton.Height() && !PlayerInput.IgnoreMouseInterface)
                    {
                        player[myPlayer].mouseInterface = true;
                        if (mouseLeftRelease && mouseLeft)
                        {
                            recStart += num83;
                            SoundEngine.PlaySound(SoundID.MenuTick);
                            if (recStart > numAvailableRecipes - num83)
                                recStart = numAvailableRecipes - num83;

                            mouseLeftRelease = false;
                        }
                    }

                    spriteBatch.Draw(TextureAssets.CraftDownButton.Value, new Vector2(num89, num90), new Microsoft.Xna.Framework.Rectangle(0, 0, TextureAssets.CraftUpButton.Width(), TextureAssets.CraftUpButton.Height()), new Microsoft.Xna.Framework.Color(200, 200, 200, 200), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                }

                for (int num91 = recStart; num91 < Recipe.maxRecipes && num91 < numAvailableRecipes; num91++)
                {
                    int num92 = num87;
                    int num93 = num88;
                    double num94 = inventoryBack.A + 50;
                    double num95 = 255.0;
                    new Microsoft.Xna.Framework.Color((byte)num94, (byte)num94, (byte)num94, (byte)num94);
                    new Microsoft.Xna.Framework.Color((byte)num95, (byte)num95, (byte)num95, (byte)num95);
                    if (mouseX >= num92 && (float)mouseX <= (float)num92 + (float)TextureAssets.InventoryBack.Width() * inventoryScale && mouseY >= num93 && (float)mouseY <= (float)num93 + (float)TextureAssets.InventoryBack.Height() * inventoryScale && !PlayerInput.IgnoreMouseInterface)
                    {
                        player[myPlayer].mouseInterface = true;
                        if (mouseLeftRelease && mouseLeft)
                        {
                            focusRecipe = num91;
                            recFastScroll = true;
                            recBigList = false;
                            SoundEngine.PlaySound(SoundID.MenuTick);
                            mouseLeftRelease = false;
                            if (PlayerInput.UsingGamepadUI)
                            {
                                UILinkPointNavigator.ChangePage(9);
                                LockCraftingForThisCraftClickDuration();
                            }
                        }

                        craftingHide = true;
                        HoverItem = recipe[availableRecipe[num91]].createItem.Clone();
                        ItemSlot.MouseHover(22);
                        hoverItemName = recipe[availableRecipe[num91]].createItem.Name;
                        if (recipe[availableRecipe[num91]].createItem.stack > 1)
                            hoverItemName = hoverItemName + " (" + recipe[availableRecipe[num91]].createItem.stack + ")";
                    }

                    if (numAvailableRecipes > 0)
                    {
                        num94 -= 50.0;
                        if (num94 < 0.0)
                            num94 = 0.0;

                        UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig = num91 - recStart;
                        Microsoft.Xna.Framework.Color color6 = inventoryBack;
                        inventoryBack = new Microsoft.Xna.Framework.Color((byte)num94, (byte)num94, (byte)num94, (byte)num94);
                        ItemSlot.Draw(spriteBatch, ref recipe[availableRecipe[num91]].createItem, 22, new Vector2(num92, num93));
                        inventoryBack = color6;
                    }

                    num87 += num80;
                    num85++;
                    if (num85 >= num83)
                    {
                        num87 = num82;
                        num88 += num80;
                        num85 = 0;
                        num86++;
                        if (num86 >= num84)
                            break;
                    }
                }
            }

            Vector2 vector2 = FontAssets.MouseText.Value.MeasureString("Coins");
            Vector2 vector3 = FontAssets.MouseText.Value.MeasureString(Lang.inter[26].Value);
            float num96 = vector2.X / vector3.X;
            spriteBatch.DrawString(FontAssets.MouseText.Value, Lang.inter[26].Value, new Vector2(496f, 84f + (vector2.Y - vector2.Y * num96) / 2f), new Microsoft.Xna.Framework.Color(mouseTextColor, mouseTextColor, mouseTextColor, mouseTextColor), 0f, default(Vector2), 0.75f * num96, SpriteEffects.None, 0f);
            inventoryScale = 0.6f;
            for (int num97 = 0; num97 < 4; num97++)
            {
                int num98 = 497;
                int num99 = (int)(85f + (float)(num97 * 56) * inventoryScale + 20f);
                int slot = num97 + 50;
                new Microsoft.Xna.Framework.Color(100, 100, 100, 100);
                if (mouseX >= num98 && (float)mouseX <= (float)num98 + (float)TextureAssets.InventoryBack.Width() * inventoryScale && mouseY >= num99 && (float)mouseY <= (float)num99 + (float)TextureAssets.InventoryBack.Height() * inventoryScale && !PlayerInput.IgnoreMouseInterface)
                {
                    player[myPlayer].mouseInterface = true;
                    ItemSlot.OverrideHover(player[myPlayer].inventory, 1, slot);
                    ItemSlot.LeftClick(player[myPlayer].inventory, 1, slot);
                    ItemSlot.RightClick(player[myPlayer].inventory, 1, slot);
                    if (mouseLeftRelease && mouseLeft)
                        Recipe.FindRecipes();

                    ItemSlot.MouseHover(player[myPlayer].inventory, 1, slot);
                }

                ItemSlot.Draw(spriteBatch, player[myPlayer].inventory, 1, slot, new Vector2(num98, num99));
            }

            Vector2 vector4 = FontAssets.MouseText.Value.MeasureString("Ammo");
            Vector2 vector5 = FontAssets.MouseText.Value.MeasureString(Lang.inter[27].Value);
            float num100 = vector4.X / vector5.X;
            spriteBatch.DrawString(FontAssets.MouseText.Value, Lang.inter[27].Value, new Vector2(532f, 84f + (vector4.Y - vector4.Y * num100) / 2f), new Microsoft.Xna.Framework.Color(mouseTextColor, mouseTextColor, mouseTextColor, mouseTextColor), 0f, default(Vector2), 0.75f * num100, SpriteEffects.None, 0f);
            inventoryScale = 0.6f;
            for (int num101 = 0; num101 < 4; num101++)
            {
                int num102 = 534;
                int num103 = (int)(85f + (float)(num101 * 56) * inventoryScale + 20f);
                int slot2 = 54 + num101;
                new Microsoft.Xna.Framework.Color(100, 100, 100, 100);
                if (mouseX >= num102 && (float)mouseX <= (float)num102 + (float)TextureAssets.InventoryBack.Width() * inventoryScale && mouseY >= num103 && (float)mouseY <= (float)num103 + (float)TextureAssets.InventoryBack.Height() * inventoryScale && !PlayerInput.IgnoreMouseInterface)
                {
                    player[myPlayer].mouseInterface = true;
                    ItemSlot.OverrideHover(player[myPlayer].inventory, 2, slot2);
                    ItemSlot.LeftClick(player[myPlayer].inventory, 2, slot2);
                    ItemSlot.RightClick(player[myPlayer].inventory, 2, slot2);
                    if (mouseLeftRelease && mouseLeft)
                        Recipe.FindRecipes();

                    ItemSlot.MouseHover(player[myPlayer].inventory, 2, slot2);
                }

                ItemSlot.Draw(spriteBatch, player[myPlayer].inventory, 2, slot2, new Vector2(num102, num103));
            }

            if (npcShop > 0 && (!playerInventory || player[myPlayer].talkNPC == -1))
                SetNPCShopIndex(0);

            if (npcShop > 0 && !recBigList)
            {
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, Lang.inter[28].Value, 504f, instance.invBottom, Microsoft.Xna.Framework.Color.White * ((float)(int)mouseTextColor / 255f), Microsoft.Xna.Framework.Color.Black, Vector2.Zero);
                ItemSlot.DrawSavings(spriteBatch, 504f, instance.invBottom);
                inventoryScale = 0.755f;
                if (mouseX > 73 && mouseX < (int)(73f + 560f * inventoryScale) && mouseY > instance.invBottom && mouseY < (int)((float)instance.invBottom + 224f * inventoryScale) && !PlayerInput.IgnoreMouseInterface)
                    player[myPlayer].mouseInterface = true;

                for (int num104 = 0; num104 < 10; num104++)
                {
                    for (int num105 = 0; num105 < 4; num105++)
                    {
                        int num106 = (int)(73f + (float)(num104 * 56) * inventoryScale);
                        int num107 = (int)((float)instance.invBottom + (float)(num105 * 56) * inventoryScale);
                        int slot3 = num104 + num105 * 10;
                        new Microsoft.Xna.Framework.Color(100, 100, 100, 100);
                        if (mouseX >= num106 && (float)mouseX <= (float)num106 + (float)TextureAssets.InventoryBack.Width() * inventoryScale && mouseY >= num107 && (float)mouseY <= (float)num107 + (float)TextureAssets.InventoryBack.Height() * inventoryScale && !PlayerInput.IgnoreMouseInterface)
                        {
                            ItemSlot.OverrideHover(instance.shop[npcShop].item, 15, slot3);
                            player[myPlayer].mouseInterface = true;
                            ItemSlot.LeftClick(instance.shop[npcShop].item, 15, slot3);
                            ItemSlot.RightClick(instance.shop[npcShop].item, 15, slot3);
                            ItemSlot.MouseHover(instance.shop[npcShop].item, 15, slot3);
                        }

                        ItemSlot.Draw(spriteBatch, instance.shop[npcShop].item, 15, slot3, new Vector2(num106, num107));
                    }
                }
            }

            if (player[myPlayer].chest > -1 && !tileContainer[tile[player[myPlayer].chestX, player[myPlayer].chestY].TileType])
            {
                player[myPlayer].chest = -1;
                Recipe.FindRecipes();
            }

            int offsetDown = 0;
            UIVirtualKeyboard.ShouldHideText = !PlayerInput.SettingsForUI.ShowGamepadHints;
            if (!PlayerInput.UsingGamepad)
                offsetDown = 9999;

            UIVirtualKeyboard.OffsetDown = offsetDown;
            ChestUI.Draw(spriteBatch);
            LocalPlayer.tileEntityAnchor.GetTileEntity()?.OnInventoryDraw(LocalPlayer, spriteBatch);
            if (player[myPlayer].chest == -1 && npcShop == 0)
            {
                int num108 = 0;
                int num109 = 498;
                int num110 = 244;
                int num111 = TextureAssets.ChestStack[num108].Width();
                int num112 = TextureAssets.ChestStack[num108].Height();
                UILinkPointNavigator.SetPosition(301, new Vector2((float)num109 + (float)num111 * 0.75f, (float)num110 + (float)num112 * 0.75f));
                if (mouseX >= num109 && mouseX <= num109 + num111 && mouseY >= num110 && mouseY <= num110 + num112 && !PlayerInput.IgnoreMouseInterface)
                {
                    num108 = 1;
                    if (!allChestStackHover)
                    {
                        SoundEngine.PlaySound(SoundID.MenuTick);
                        allChestStackHover = true;
                    }

                    if (mouseLeft && mouseLeftRelease)
                    {
                        mouseLeftRelease = false;
                        player[myPlayer].QuickStackAllChests();
                        Recipe.FindRecipes();
                    }

                    player[myPlayer].mouseInterface = true;
                }
                else if (allChestStackHover)
                {
                    SoundEngine.PlaySound(SoundID.MenuTick);
                    allChestStackHover = false;
                }

                spriteBatch.Draw(TextureAssets.ChestStack[num108].Value, new Vector2(num109, num110), new Microsoft.Xna.Framework.Rectangle(0, 0, TextureAssets.ChestStack[num108].Width(), TextureAssets.ChestStack[num108].Height()), Microsoft.Xna.Framework.Color.White, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                if (!mouseText && num108 == 1)
                    instance.MouseText(Language.GetTextValue("GameUI.QuickStackToNearby"), 0, 0);
            }

            if (player[myPlayer].chest != -1 || npcShop != 0)
                return;

            int num113 = 0;
            int num114 = 534;
            int num115 = 244;
            int num116 = 30;
            int num117 = 30;
            UILinkPointNavigator.SetPosition(302, new Vector2((float)num114 + (float)num116 * 0.75f, (float)num115 + (float)num117 * 0.75f));
            bool flag12 = false;
            if (mouseX >= num114 && mouseX <= num114 + num116 && mouseY >= num115 && mouseY <= num115 + num117 && !PlayerInput.IgnoreMouseInterface)
            {
                num113 = 1;
                flag12 = true;
                player[myPlayer].mouseInterface = true;
                if (mouseLeft && mouseLeftRelease)
                {
                    mouseLeftRelease = false;
                    ItemSorting.SortInventory();
                    Recipe.FindRecipes();
                }
            }

            if (flag12 != inventorySortMouseOver)
            {
                SoundEngine.PlaySound(SoundID.MenuTick);
                inventorySortMouseOver = flag12;
            }

            Texture2D value5 = TextureAssets.InventorySort[inventorySortMouseOver ? 1 : 0].Value;
            spriteBatch.Draw(value5, new Vector2(num114, num115), null, Microsoft.Xna.Framework.Color.White, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
            if (!mouseText && num113 == 1)
                instance.MouseText(Language.GetTextValue("GameUI.SortInventory"), 0, 0);
        }

        private static void DrawLoadoutButtons(int inventoryTop, bool demonHeartSlotAvailable, bool masterModeSlotAvailable)
        {
            int num = 10;
            Player player = Main.player[myPlayer];
            if (!demonHeartSlotAvailable)
                num--;

            if (!masterModeSlotAvailable)
                num--;

            int x = screenWidth - 58 + 14;
            int num2 = (int)((float)(inventoryTop - 2) + 0f * inventoryScale);
            int num3 = (int)((float)(inventoryTop - 2) + (float)(num * 56) * inventoryScale);
            Texture2D value = TextureAssets.Extra[259].Value;
            Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(x, num2 + 2, 4, num3 - num2);
            ItemSlot.GetLoadoutColor(player.CurrentLoadoutIndex);
            int num4 = player.Loadouts.Length;
            int num5 = 32;
            int num6 = 4;
            int num7 = -1;
            _ = FontAssets.ItemStack.Value;
            for (int i = 0; i < num4; i++)
            {
                Microsoft.Xna.Framework.Rectangle rectangle2 = new Microsoft.Xna.Framework.Rectangle(rectangle.X + rectangle.Width, rectangle.Y + (num5 + num6) * i, 32, num5);
                Microsoft.Xna.Framework.Color loadoutColor = ItemSlot.GetLoadoutColor(i);
                _ = player.Loadouts[i];
                int frameX = ((i == player.CurrentLoadoutIndex) ? 1 : 0);
                bool flag = false;
                if (rectangle2.Contains(MouseScreen.ToPoint()))
                {
                    flag = true;
                    loadoutColor = Microsoft.Xna.Framework.Color.Lerp(loadoutColor, Microsoft.Xna.Framework.Color.White, 0.8f);
                    player.mouseInterface = true;
                    if (!mouseText)
                    {
                        instance.MouseText(Language.GetTextValue("UI.Loadout" + (i + 1)), 0, 0);
                        mouseText = true;
                    }

                    if (mouseLeft && mouseLeftRelease)
                        player.TrySwitchingLoadout(i);
                }

                Microsoft.Xna.Framework.Rectangle rectangle3 = value.Frame(3, 3, frameX, i);
                spriteBatch.Draw(value, rectangle2.Center.ToVector2(), rectangle3, Microsoft.Xna.Framework.Color.White, 0f, rectangle3.Size() / 2f, 1f, SpriteEffects.None, 0f);
                if (flag)
                {
                    rectangle3 = value.Frame(3, 3, 2, i);
                    spriteBatch.Draw(value, rectangle2.Center.ToVector2(), rectangle3, OurFavoriteColor, 0f, rectangle3.Size() / 2f, 1f, SpriteEffects.None, 0f);
                }

                UILinkPointNavigator.SetPosition(312 + i, rectangle2.Center.ToVector2());
            }

            if ((int)AccessMainClass.getMainField("_lastHoveredLoadoutIndex") != num7)
            {
                AccessMainClass.setMainField("_lastHoveredLoadoutIndex", num7);
                SoundEngine.PlaySound(SoundID.MenuTick);
            }
        }

        private static void DrawPVPIcons()
        {
            if (EquipPage == 1)
            {
                if ((bool)AccessMainClass.getMainField("hidePVPIcons"))
                    return;
            }
            else
            {
                AccessMainClass.setMainField("hidePVPIcons", false);
            }

            inventoryScale = 0.6f;
            int num = (int)(52f * inventoryScale);
            int num2 = 707 - num * 4 + screenWidth - 800;
            int num3 = 114 + (int)AccessMainClass.getMainField("mH") + num * 2 + num / 2 - 12;
            if (EquipPage == 2)
                num2 += num + num / 2;

            int num4 = (player[myPlayer].hostile ? 2 : 0);
            if (mouseX > num2 - 7 && mouseX < num2 + 25 && mouseY > num3 - 2 && mouseY < num3 + 37 && !PlayerInput.IgnoreMouseInterface)
            {
                player[myPlayer].mouseInterface = true;
                if (teamCooldown == 0)
                    num4++;

                if (mouseLeft && mouseLeftRelease && teamCooldown == 0)
                {
                    teamCooldown = teamCooldownLen;
                    SoundEngine.PlaySound(SoundID.MenuTick);
                    player[myPlayer].hostile = !player[myPlayer].hostile;
                    NetMessage.SendData(MessageID.TogglePVP, -1, -1, null, myPlayer);
                }
            }

            Microsoft.Xna.Framework.Rectangle rectangle = TextureAssets.Pvp[0].Frame(4, 6);
            rectangle.Location = new Microsoft.Xna.Framework.Point(rectangle.Width * num4, rectangle.Height * player[myPlayer].team);
            rectangle.Width -= 2;
            rectangle.Height--;
            spriteBatch.Draw(TextureAssets.Pvp[0].Value, new Vector2(num2 - 10, num3), rectangle, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            UILinkPointNavigator.SetPosition(1550, new Vector2(num2 - 10, num3) + rectangle.Size() * 0.75f);
            num3 += 60;
            num2 -= 10;
            rectangle = TextureAssets.Pvp[1].Frame(6);
            Microsoft.Xna.Framework.Rectangle r = rectangle;
            for (int i = 0; i < 6; i++)
            {
                r.Location = new Microsoft.Xna.Framework.Point(num2 + i % 2 * 20, num3 + i / 2 * 20);
                rectangle.X = rectangle.Width * i;
                bool flag = false;
                if (r.Contains(MouseScreen.ToPoint()) && !PlayerInput.IgnoreMouseInterface)
                {
                    player[myPlayer].mouseInterface = true;
                    if (teamCooldown == 0)
                        flag = true;

                    if (mouseLeft && mouseLeftRelease && player[myPlayer].team != i && teamCooldown == 0)
                    {
                        if (!player[myPlayer].TeamChangeAllowed())
                        {
                            NewText(Lang.misc[84].Value, byte.MaxValue, 240, 20);
                        }
                        else
                        {
                            teamCooldown = teamCooldownLen;
                            SoundEngine.PlaySound(SoundID.MenuTick);
                            player[myPlayer].team = i;
                            NetMessage.SendData(MessageID.PlayerTeam, -1, -1, null, myPlayer);
                        }
                    }
                }

                r.Width = rectangle.Width - 2;
                if (flag)
                    spriteBatch.Draw(TextureAssets.Pvp[2].Value, r.Location.ToVector2() + new Vector2(-2f), Microsoft.Xna.Framework.Color.White);

                Microsoft.Xna.Framework.Rectangle value = rectangle;
                value.Width -= 2;
                spriteBatch.Draw(TextureAssets.Pvp[1].Value, r.Location.ToVector2(), value, Microsoft.Xna.Framework.Color.White);
                UILinkPointNavigator.SetPosition(1550 + i + 1, r.Location.ToVector2() + r.Size() * 0.75f);
            }
        }

        private static void DrawBestiaryIcon(int pivotTopLeftX, int pivotTopLeftY)
        {
            inventoryScale = 0.85f;
            int num = (int)((float)(450 + pivotTopLeftX) - 56f * inventoryScale * 2f);
            int num2 = 258 + pivotTopLeftY;
            int num3 = 244;
            int width = 30;
            int num4 = 30;
            num3 = 244;
            num = 498;
            num2 = num3 + num4 + 4;
            if ((player[myPlayer].chest != -1 || npcShop > 0) && !recBigList)
            {
                num2 += 168;
                inventoryScale = 0.755f;
                num += 5;
                num3 += 24;
            }

            if (editChest)
                num2 += 24;

            Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(num, num2, (int)((float)TextureAssets.InventoryBack.Width() * inventoryScale), (int)((float)TextureAssets.InventoryBack.Height() * inventoryScale));
            rectangle = new Microsoft.Xna.Framework.Rectangle(num, num2, width, num4);
            bool flag = false;
            if (rectangle.Contains(new Microsoft.Xna.Framework.Point(mouseX, mouseY)) && !PlayerInput.IgnoreMouseInterface)
            {
                player[myPlayer].mouseInterface = true;
                flag = true;
                if (mouseLeft && mouseLeftRelease)
                {
                    player[myPlayer].SetTalkNPC(-1);
                    npcChatCornerItem = 0;
                    npcChatText = "";
                    mouseLeftRelease = false;
                    SoundEngine.PlaySound(SoundID.MenuTick);
                    IngameFancyUI.OpenUIState(BestiaryUI);
                    BestiaryUI.OnOpenPage();
                }
            }

            Texture2D value = TextureAssets.BestiaryMenuButton.Value;
            Vector2 position = rectangle.Center.ToVector2();
            Microsoft.Xna.Framework.Rectangle rectangle2 = value.Frame(2, 1, flag ? 1 : 0);
            rectangle2.Width -= 2;
            rectangle2.Height -= 2;
            Vector2 origin = rectangle2.Size() / 2f;
            Microsoft.Xna.Framework.Color white = Microsoft.Xna.Framework.Color.White;
            spriteBatch.Draw(value, position, rectangle2, white, 0f, origin, 1f, SpriteEffects.None, 0f);
            UILinkPointNavigator.SetPosition(310, position);
            if (!mouseText && flag)
                instance.MouseText(Language.GetTextValue("GameUI.Bestiary"), 0, 0);
        }

        private static void DrawEmoteBubblesButton(int pivotTopLeftX, int pivotTopLeftY)
        {
            inventoryScale = 0.85f;
            int num = (int)((float)(450 + pivotTopLeftX) - 56f * inventoryScale);
            int num2 = 258 + pivotTopLeftY;
            int num3 = 244;
            int width = 30;
            int num4 = 30;
            num = 534;
            num2 = num3 + num4 + 4;
            if ((player[myPlayer].chest != -1 || npcShop > 0) && !recBigList)
            {
                num2 += 168;
                inventoryScale = 0.755f;
                num += 5;
                num3 += 24;
            }

            if (editChest)
                num2 += 24;

            Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle(num, num2, (int)((float)TextureAssets.InventoryBack.Width() * inventoryScale), (int)((float)TextureAssets.InventoryBack.Height() * inventoryScale));
            rectangle = new Microsoft.Xna.Framework.Rectangle(num, num2, width, num4);
            bool flag = false;
            if (rectangle.Contains(new Microsoft.Xna.Framework.Point(mouseX, mouseY)) && !PlayerInput.IgnoreMouseInterface)
            {
                player[myPlayer].mouseInterface = true;
                flag = true;
                if (mouseLeft && mouseLeftRelease)
                {
                    player[myPlayer].SetTalkNPC(-1);
                    npcChatCornerItem = 0;
                    npcChatText = "";
                    mouseLeftRelease = false;
                    SoundEngine.PlaySound(SoundID.MenuTick);
                    IngameFancyUI.OpenUIState(new UIEmotesMenu());
                }
            }

            Texture2D value = TextureAssets.EmoteMenuButton.Value;
            Vector2 position = rectangle.Center.ToVector2();
            Microsoft.Xna.Framework.Rectangle rectangle2 = value.Frame(2, 1, flag ? 1 : 0);
            rectangle2.Width -= 2;
            rectangle2.Height -= 2;
            Vector2 origin = rectangle2.Size() / 2f;
            Microsoft.Xna.Framework.Color white = Microsoft.Xna.Framework.Color.White;
            spriteBatch.Draw(value, position, rectangle2, white, 0f, origin, 1f, SpriteEffects.None, 0f);
            UILinkPointNavigator.SetPosition(309, position);
            if (!mouseText && flag)
                instance.MouseText(Language.GetTextValue("GameUI.Emote"), 0, 0);
        }

        private static void DrawTrashItemSlot(int pivotTopLeftX, int pivotTopLeftY)
        {
            inventoryScale = 0.85f;
            int num = 448 + pivotTopLeftX;
            int num2 = 258 + pivotTopLeftY;
            if ((player[myPlayer].chest != -1 || npcShop > 0) && !recBigList)
            {
                num2 += 168;
                inventoryScale = 0.755f;
                num += 5;
            }
            else if ((player[myPlayer].chest == -1 || npcShop == -1) && trashSlotOffset != Point16.Zero)
            {
                num += trashSlotOffset.X;
                num2 += trashSlotOffset.Y;
                inventoryScale = 0.755f;
            }

            new Microsoft.Xna.Framework.Color(150, 150, 150, 150);
            if (mouseX >= num && (float)mouseX <= (float)num + (float)TextureAssets.InventoryBack.Width() * inventoryScale && mouseY >= num2 && (float)mouseY <= (float)num2 + (float)TextureAssets.InventoryBack.Height() * inventoryScale && !PlayerInput.IgnoreMouseInterface)
            {
                player[myPlayer].mouseInterface = true;
                ItemSlot.LeftClick(ref player[myPlayer].trashItem, 6);
                if (mouseLeftRelease && mouseLeft)
                    Recipe.FindRecipes();

                ItemSlot.MouseHover(ref player[myPlayer].trashItem, 6);
            }

            ItemSlot.Draw(spriteBatch, ref player[myPlayer].trashItem, 6, new Vector2(num, num2));
        }

        private static void DrawHotbarLockIcon(int pivotTopLeftX, int pivotTopLeftY, bool pushSideToolsUp)
        {
            int num = 21 + pivotTopLeftY;
            _ = player[myPlayer];
            if (pushSideToolsUp)
                num = pivotTopLeftY;

            float num2 = 0.9f;
            Texture2D value = TextureAssets.HbLock[(!player[myPlayer].hbLocked) ? 1 : 0].Value;
            Microsoft.Xna.Framework.Rectangle value2 = value.Frame(2);
            bool flag = false;
            if (mouseX > pivotTopLeftX && (float)mouseX < (float)pivotTopLeftX + (float)value2.Width * num2 && mouseY > num && (float)mouseY < (float)num + (float)value2.Height * num2)
            {
                flag = true;
                player[myPlayer].mouseInterface = true;
                if (!player[myPlayer].hbLocked)
                    instance.MouseText(Lang.inter[5].Value, 0, 0);
                else
                    instance.MouseText(Lang.inter[6].Value, 0, 0);

                mouseText = true;
                if (mouseLeft && mouseLeftRelease)
                {
                    SoundEngine.PlaySound(SoundID.Unlock);
                    player[myPlayer].hbLocked = !player[myPlayer].hbLocked;
                }
            }

            spriteBatch.Draw(value, new Vector2(pivotTopLeftX, num), value2, Microsoft.Xna.Framework.Color.White, 0f, default(Vector2), num2, SpriteEffects.None, 0f);
            if (flag)
                spriteBatch.Draw(value, new Vector2(pivotTopLeftX, num), value.Frame(2, 1, 1), OurFavoriteColor, 0f, default(Vector2), num2, SpriteEffects.None, 0f);
        }

        private static int DrawPageIcons(int yPos)
        {
            int num = -1;
            Vector2 vector = new Vector2(screenWidth - 162, yPos);
            vector.X += 82f;
            Texture2D value = TextureAssets.EquipPage[(EquipPage == 2) ? 3 : 2].Value;
            if (Collision.CheckAABBvAABBCollision(vector, value.Size(), new Vector2(mouseX, mouseY), Vector2.One) && (mouseItem.stack < 1 || mouseItem.dye > 0))
                num = 2;

            if (num == 2)
                spriteBatch.Draw(TextureAssets.EquipPage[6].Value, vector, null, OurFavoriteColor, 0f, new Vector2(2f), 0.9f, SpriteEffects.None, 0f);

            spriteBatch.Draw(value, vector, null, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);
            UILinkPointNavigator.SetPosition(305, vector + value.Size() * 0.75f);
            vector.X -= 48f;
            value = TextureAssets.EquipPage[(EquipPage == 1) ? 5 : 4].Value;
            if (Collision.CheckAABBvAABBCollision(vector, value.Size(), new Vector2(mouseX, mouseY), Vector2.One) && mouseItem.stack < 1)
                num = 1;

            if (num == 1)
                spriteBatch.Draw(TextureAssets.EquipPage[7].Value, vector, null, OurFavoriteColor, 0f, new Vector2(2f), 0.9f, SpriteEffects.None, 0f);

            spriteBatch.Draw(value, vector, null, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);
            UILinkPointNavigator.SetPosition(306, vector + value.Size() * 0.75f);
            vector.X -= 48f;
            value = TextureAssets.EquipPage[(EquipPage == 3) ? 10 : 8].Value;
            if (Collision.CheckAABBvAABBCollision(vector, value.Size(), new Vector2(mouseX, mouseY), Vector2.One) && mouseItem.stack < 1)
                num = 3;

            if (num == 3 && !CaptureModeDisabled)
                spriteBatch.Draw(TextureAssets.EquipPage[9].Value, vector, null, OurFavoriteColor, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);

            spriteBatch.Draw(value, vector, null, CaptureModeDisabled ? Microsoft.Xna.Framework.Color.Red : Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);
            UILinkPointNavigator.SetPosition(307, vector + value.Size() * 0.75f);
            if (num != -1)
            {
                player[myPlayer].mouseInterface = true;
                if (mouseLeft && mouseLeftRelease)
                {
                    bool flag = true;
                    if (num == 3)
                    {
                        if (CaptureModeDisabled)
                        {
                            flag = false;
                        }
                        else if (PlayerInput.UsingGamepad)
                        {
                            CaptureInterface.QuickScreenshot();
                        }
                        else
                        {
                            CaptureManager.Instance.Active = true;
                            blockMouse = true;
                        }
                    }
                    else if (EquipPageSelected != num)
                    {
                        EquipPageSelected = num;
                    }
                    else
                    {
                        EquipPageSelected = 0;
                    }

                    if (flag)
                        SoundEngine.PlaySound(SoundID.MenuOpen);
                }
            }

            ItemSlot.SelectEquipPage(mouseItem);
            if (EquipPage == -1)
                EquipPage = EquipPageSelected;

            return num;
        }

        private static void DrawNPCHousesInUI()
        {
            UILinkPointNavigator.Shortcuts.NPCS_LastHovered = -2;
            if (mouseX > screenWidth - 64 - 28 && mouseX < (int)((float)(screenWidth - 64 - 28) + 56f * inventoryScale) && mouseY > 174 + (int)AccessMainClass.getMainField("mH") && mouseY < (int)((float)(174 + (int)AccessMainClass.getMainField("mH")) + 448f * inventoryScale) && !PlayerInput.IgnoreMouseInterface)
                player[myPlayer].mouseInterface = true;

            int num = 0;
            string text = "";
            int num2 = 0;
            int num3 = 0;
            ((List<int>)AccessMainClass.getMainField("_npcTypesThatAlreadyDrewAHead")).Clear();
            for (int i = 0; i < ((int[])AccessMainClass.getMainField("_npcIndexWhoHoldsHeadIndex")).Length; i++)
            {
                ((int[])AccessMainClass.getMainField("_npcIndexWhoHoldsHeadIndex"))[i] = -1;
            }

            for (int j = 0; j < 200; j++)
            {
                if (npc[j].active && !((List<int>)AccessMainClass.getMainField("_npcTypesThatAlreadyDrewAHead")).Contains(npc[j].type))
                {
                    if (npc[j].ModNPC?.TownNPCStayingHomeless is true)
                        continue;

                    int headIndexSafe = TownNPCProfiles.GetHeadIndexSafe(npc[j]);
                    if (headIndexSafe > 0 && !NPCHeadID.Sets.CannotBeDrawnInHousingUI[headIndexSafe] && ((int[])AccessMainClass.getMainField("_npcIndexWhoHoldsHeadIndex"))[headIndexSafe] == -1)
                    {
                        ((int[])AccessMainClass.getMainField("_npcIndexWhoHoldsHeadIndex"))[headIndexSafe] = j;
                        ((List<int>)AccessMainClass.getMainField("_npcTypesThatAlreadyDrewAHead")).Add(npc[j].type);
                    }
                }
            }

            AccessMainClass.setMainField("hidePVPIcons", false);
            int num4 = 0;
            int num5 = 0;
            UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn = 1;
            for (int k = 0; k < TextureAssets.NpcHead.Length; k++)
            {
                if (k != 0 && ((int[])AccessMainClass.getMainField("_npcIndexWhoHoldsHeadIndex"))[k] == -1)
                    continue;

                int num6 = ((int[])AccessMainClass.getMainField("_npcIndexWhoHoldsHeadIndex"))[k];
                int num7 = screenWidth - 64 - 28 + num3;
                int num8 = (int)((float)(174 + (int)AccessMainClass.getMainField("mH")) + (float)(num * 56) * inventoryScale) + num2;
                Microsoft.Xna.Framework.Color color = new Microsoft.Xna.Framework.Color(100, 100, 100, 100);
                if (num8 > screenHeight - 80)
                {
                    num3 -= 48;
                    num2 -= num8 - (174 + (int)AccessMainClass.getMainField("mH"));
                    num7 = screenWidth - 64 - 28 + num3;
                    num8 = (int)((float)(174 + (int)AccessMainClass.getMainField("mH")) + (float)(num * 56) * inventoryScale) + num2;
                    UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn = num5;
                    if (num3 <= -144)
                        AccessMainClass.setMainField("hidePVPIcons", true);

                    num5 = 0;
                }

                if (mouseX >= num7 && (float)mouseX <= (float)num7 + (float)TextureAssets.InventoryBack.Width() * inventoryScale && mouseY >= num8 && (float)mouseY <= (float)num8 + (float)TextureAssets.InventoryBack.Height() * inventoryScale)
                {
                    UILinkPointNavigator.Shortcuts.NPCS_LastHovered = num6;
                    mouseText = true;
                    text = ((k != 0) ? npc[num6].FullName : Lang.inter[8].Value);
                    if (!PlayerInput.IgnoreMouseInterface)
                    {
                        player[myPlayer].mouseInterface = true;
                        if (mouseLeftRelease && mouseLeft && !PlayerInput.UsingGamepadUI && mouseItem.type == ItemID.None)
                        {
                            SoundEngine.PlaySound(SoundID.MenuTick);
                            if (k == 0)
                                instance.SetMouseNPC_ToHousingQuery();
                            else
                                instance.SetMouseNPC(num6, npc[num6].type);

                            mouseLeftRelease = false;
                        }
                    }
                }

                UILinkPointNavigator.SetPosition(600 + num, new Vector2(num7, num8) + TextureAssets.InventoryBack.Value.Size() * 0.75f);
                Texture2D value = TextureAssets.InventoryBack11.Value;
                Microsoft.Xna.Framework.Color white = inventoryBack;
                if (UILinkPointNavigator.CurrentPoint - 600 == num)
                {
                    value = TextureAssets.InventoryBack14.Value;
                    white = Microsoft.Xna.Framework.Color.White;
                }

                spriteBatch.Draw(value, new Vector2(num7, num8), null, white, 0f, default(Vector2), inventoryScale, SpriteEffects.None, 0f);
                color = Microsoft.Xna.Framework.Color.White;
                int num9 = k;
                float scale = 1f;
                float num10 = 0f;
                num10 = ((TextureAssets.NpcHead[num9].Width() <= TextureAssets.NpcHead[num9].Height()) ? ((float)TextureAssets.NpcHead[num9].Height()) : ((float)TextureAssets.NpcHead[num9].Width()));
                if (num10 > 36f)
                    scale = 36f / num10;

                spriteBatch.Draw(TextureAssets.NpcHead[num9].Value, new Vector2((float)num7 + 26f * inventoryScale, (float)num8 + 26f * inventoryScale), null, color, 0f, new Vector2(TextureAssets.NpcHead[num9].Width() / 2, TextureAssets.NpcHead[num9].Height() / 2), scale, SpriteEffects.None, 0f);
                num++;
                num4++;
                num5++;
            }

            UILinkPointNavigator.Shortcuts.NPCS_IconsTotal = num4;
            if (text != "" && mouseItem.type == ItemID.None)
                instance.MouseText(text, 0, 0);
        }

        private static void DrawDefenseCounter(int inventoryX, int inventoryY)
        {
            Vector2 vector = new Vector2(inventoryX - 10 - 47 - 47 - 14, (float)inventoryY + (float)TextureAssets.InventoryBack.Height() * 0.5f);
            spriteBatch.Draw(TextureAssets.Extra[58].Value, vector, null, Microsoft.Xna.Framework.Color.White, 0f, TextureAssets.Extra[58].Value.Size() / 2f, inventoryScale, SpriteEffects.None, 0f);
            Vector2 vector2 = FontAssets.MouseText.Value.MeasureString(player[myPlayer].statDefense.ToString());
            ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, player[myPlayer].statDefense.ToString(), vector - vector2 * 0.5f * inventoryScale, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, new Vector2(inventoryScale));
            if (Utils.CenteredRectangle(vector, TextureAssets.Extra[58].Value.Size()).Contains(new Microsoft.Xna.Framework.Point(mouseX, mouseY)) && !PlayerInput.IgnoreMouseInterface)
            {
                player[myPlayer].mouseInterface = true;
                string value = player[myPlayer].statDefense + " " + Lang.inter[10].Value;
                if (!string.IsNullOrEmpty(value))
                    hoverItemName = value;
            }

            UILinkPointNavigator.SetPosition(1557, vector + TextureAssets.Extra[58].Value.Size() * inventoryScale / 4f);
        }

        private static void DrawGuideCraftText(int adjY, Microsoft.Xna.Framework.Color craftingTipColor, out int inventoryX, out int inventoryY)
        {
            inventoryX = 73;
            inventoryY = 331;
            inventoryY += adjY;
            string text = null;
            ((List<string>)AccessMainClass.getMainField("_requiredObjecsForCraftingText")).Clear();
            if (guideItem.IsAir)
            {
                text = Lang.inter[24].Value;
            }
            else
            {
                text = Lang.inter[21].Value + " " + guideItem.Name;
                Recipe recipe = Main.recipe[availableRecipe[focusRecipe]];
                for (int i = 0; i < recipe.requiredTile.Count; i++)
                {
                    int num = recipe.requiredTile[i];
                    if (num == -1)
                        break;

                    int requiredTileStyle = Recipe.GetRequiredTileStyle(num);
                    string mapObjectName = Lang.GetMapObjectName(MapHelper.TileToLookup(num, requiredTileStyle));
                    ((List<string>)AccessMainClass.getMainField("_requiredObjecsForCraftingText")).Add(mapObjectName);
                }

                if ((bool)AccessMainClass.getRecipeField(recipe, "needWater"))
                    ((List<string>)AccessMainClass.getMainField("_requiredObjecsForCraftingText")).Add(Lang.inter[53].Value);

                if ((bool)AccessMainClass.getRecipeField(recipe, "needHoney"))
                    ((List<string>)AccessMainClass.getMainField("_requiredObjecsForCraftingText")).Add(Lang.inter[58].Value);

                if ((bool)AccessMainClass.getRecipeField(recipe, "needLava"))
                    ((List<string>)AccessMainClass.getMainField("_requiredObjecsForCraftingText")).Add(Lang.inter[56].Value);

                if ((bool)AccessMainClass.getRecipeField(recipe, "needSnowBiome"))
                    ((List<string>)AccessMainClass.getMainField("_requiredObjecsForCraftingText")).Add(Lang.inter[123].Value);

                if ((bool)AccessMainClass.getRecipeField(recipe, ""))
                    ((List<string>)AccessMainClass.getMainField("_requiredObjecsForCraftingText")).Add(Lang.inter[124].Value);

                //TML: Display recipe conditions' descriptions
                ((List<string>)AccessMainClass.getMainField("_requiredObjecsForCraftingText")).AddRange(recipe.Conditions.Select(x => x.Description.Value));

                if (((List<string>)AccessMainClass.getMainField("_requiredObjecsForCraftingText")).Count == 0)
                {
                    string value = Lang.inter[23].Value;
                    ((List<string>)AccessMainClass.getMainField("_requiredObjecsForCraftingText")).Add(value);
                }
            }

            Microsoft.Xna.Framework.Color color = new Microsoft.Xna.Framework.Color(mouseTextColor, mouseTextColor, mouseTextColor, mouseTextColor);
            Vector2 vector = new Vector2(inventoryX + 50, inventoryY + 12);
            DynamicSpriteFont value2 = FontAssets.MouseText.Value;
            if (((List<string>)AccessMainClass.getMainField("_requiredObjecsForCraftingText")).Count > 0)
            {
                vector.Y -= 14f;
                Vector2 position = vector + new Vector2(0f, 26f);
                Microsoft.Xna.Framework.Color color2 = color;
                string value3 = Lang.inter[22].Value;
                string text2 = string.Join(", ", ((List<string>)AccessMainClass.getMainField("_requiredObjecsForCraftingText")));
                string text3 = value3 + " " + text2;
                spriteBatch.DrawString(value2, text3, position, color2, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            spriteBatch.DrawString(value2, text, vector, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

    }
}