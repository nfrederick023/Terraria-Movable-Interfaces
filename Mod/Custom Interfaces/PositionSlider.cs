using DragableUI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;
using static BetterUltrawide.BetterUltrawide;

namespace PositionSlider
{
    public class PositionSliderUI : UIState
    {
        public UIScrollbar scrollbar;
        public UIText label;
        public UIPanel panel;
        public UIColoredSlider slider2;
        public UIColoredSlider slider3;
        public UIColoredSlider slider;

        private float sliderPos = 0.0f;
        private float slider3Pos = 0.0f;
        private float slider2Pos = 0.0f;

        public override void OnInitialize()
        {
            panel = new();

            panel.Left.Set(300f, 0f);
            panel.Top.Set(300f, 0f);
            panel.Width.Set(500f, 0f);
            panel.Height.Set(300f, 0f);

            // scrollbar = new UIScrollbar();
            // scrollbar.SetView(100f, 1000f);
            // scrollbar.Top.Pixels = 32f;
            // scrollbar.Height.Set(-50f, 1f);
            // scrollbar.HAlign = 1f;
            // panel.Append(scrollbar);

            label = new UIText("Hotbar PosX");
            label.Left.Set(8f, 0f);
            label.Top.Set(0f, 0f);
            panel.Append(label);

            label = new UIText("Hotbard PosY");
            label.Left.Set(8f, 0f);
            label.Top.Set(32f, 0f);
            panel.Append(label);

            label = new UIText("MiniMap PosX");
            label.Left.Set(8f, 0f);
            label.Top.Set(64f, 0f);
            panel.Append(label);



            LocalizedText test = LocalizedText.Empty;

            // the first function gets the position
            // the 2nd function is the value when it's done
            // the 3rd function is on hover
            slider = new(test, () => { return sliderPos; }, (float t) => { leftInterfacePostion = (int)(t * Main.screenWidth); sliderPos = t; }, () => { }, (floatValue) => { return new Color(73, 94, 171); }, new Color(73, 94, 171));
            slider.Width.Set(400f, 1f);

            slider2 = new(test, () => { return slider2Pos; }, (float t) => { rightInterfacePostion = (int)(t * Main.screenWidth); slider2Pos = t; }, () => { }, (floatValue) => { return new Color(73, 94, 171); }, new Color(73, 94, 171));
            slider2.Width.Set(400f, 1f);
            slider2.Top.Set(64f, 0f);

            slider3 = new(test, () => { return slider3Pos; }, (float t) => { leftInterfaceY = (int)(t * Main.screenHeight); slider3Pos = t; }, () => { }, (floatValue) => { return new Color(73, 94, 171); }, new Color(73, 94, 171));
            slider3.Width.Set(400f, 1f);
            slider3.Top.Set(32f, 0f);
            panel.Append(slider);
            panel.Append(slider2);
            panel.Append(slider3);

            Append(panel);
        }
    }
}