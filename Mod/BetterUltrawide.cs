using System.Collections.Generic;
using System.Reflection;
using HotBar;
using Microsoft.Xna.Framework;
using Minimap;
using PositionSlider;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using AccessMain;
using Inventory;
using DragableUI;

namespace BetterUltrawide
{
    public class BetterUltrawide : ModSystem
    {

        public static int leftInterfacePostion = 0;
        public static int leftInterfaceY = 0;
        public static int rightInterfacePostion = 0;
        public static PositionSliderUI positionSlider;
        public static UserInterface positionSliderInterface;
        public override void Load()
        {
            positionSlider = new PositionSliderUI();
            positionSlider.Activate();
            positionSliderInterface = new UserInterface();
            positionSliderInterface.SetState(positionSlider);

        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {

            // note to self:
            // "Ingame Options" is the options UI
            // "Fancy UI" is the mod config, achievements, and controls in the options

            int captureIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Capture Manager Check"));
            int mapIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Map / Minimap"));
            int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            int statsIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Info Accessories Bar"));
            int optionsButtonIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Settings Button"));
            int resourcesIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            int hotbarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Hotbar"));
            int builderIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Builder Accessories Bar"));

            // minimap
            layers.RemoveAt(mapIndex);
            layers.Insert(mapIndex, new LegacyGameInterfaceLayer("Vanilla: Map / Minimap",
                delegate
                {
                    MinimapClass.DrawInterface_16_MapOrMinimap();
                    return true;
                }, InterfaceScaleType.UI)
            );

            // invetory, accesories, crafting, armor, npc 
            layers.RemoveAt(inventoryIndex);
            layers.Insert(inventoryIndex, new LegacyGameInterfaceLayer("Vanilla: Inventory",
                delegate
                {
                    InventoryClass.DrawInterface_27_Inventory();
                    return true;
                }, InterfaceScaleType.UI)
            );

            // hotbar, hotbar text
            layers.RemoveAt(hotbarIndex);
            layers.Insert(hotbarIndex, new LegacyGameInterfaceLayer("Vanilla: Hotbar",
                delegate
                {
                    HotBarClass.DrawInterface_30_Hotbar();
                    return true;
                }, InterfaceScaleType.UI)
            );

            layers.Insert(builderIndex + 1, new LegacyGameInterfaceLayer("BetterUltrawide: PositionSlider",
                delegate
                {
                    positionSliderInterface.Draw(Main.spriteBatch, new GameTime());
                    return true;
                }, InterfaceScaleType.UI)
            );

            // mana, health, and buffs
            //layers.RemoveAt(resourcesIndex);

            // screen capture
            //layers.RemoveAt(captureIndex);

            // dps meter and other stats
            //layers.RemoveAt(statsIndex);

        }

        public override void UpdateUI(GameTime gameTime)
        {
            positionSliderInterface.Update(gameTime);
        }
    }
}

