using System;
using SuperPong.UI;

namespace SuperPong.Constants
{
    public class Options
    {
        public static readonly float BACK_BUTTON_PERCENT_X = 0.05f;
        public static readonly float BACK_BUTTON_PERCENT_Y = 0.05f;
        public static readonly float BACK_BUTTON_ASPECT_PERCENT = 0.15f;
        public static readonly float BACK_BUTTON_ASPECT_RATIO = 2.15f;
        public static readonly AspectRatioType BACK_BUTTON_ASPECT_TYPE = AspectRatioType.WidthMaster;
        public static readonly string BACK_BUTTON_LABEL = "Back";
        public static readonly float BACK_BUTTON_LABEL_ASPECT_PERCENT = 0.4f;
        public static readonly AspectRatioType BACK_BUTTON_LABEL_ASPECT_TYPE = AspectRatioType.HeightMaster;

        public static readonly float PRIMARY_UP_KEY_LABEL_PERCENT_X = 0.05f;
        public static readonly float PRIMARY_UP_KEY_LABEL_PERCENT_Y = 0.1f;
        public static readonly float PRIMARY_UP_KEY_ASPECT_PERCENT = 0.28f;
        public static readonly AspectRatioType PRIMARY_UP_KEY_ASPECT_TYPE = AspectRatioType.WidthMaster;
        public static readonly string PRIMARY_UP_KEY_CONTENT = "Primary Up Key:";

        public static readonly float PRIMARY_UP_KEY_ICON_PERCENT_X = 0.34f;
        public static readonly float PRIMARY_UP_KEY_ICON_PERCENT_Y = 0.085f;
        public static readonly float PRIMARY_UP_KEY_ICON_ASPECT_PERCENT = 0.05f;
        public static readonly float PRIMARY_UP_KEY_ICON_ASPECT_RATIO = 1;
        public static readonly AspectRatioType PRIMARY_UP_KEY_ICON_ASPECT_TYPE = AspectRatioType.WidthMaster;

        public static readonly float PRIMARY_DOWN_KEY_LABEL_PERCENT_X = 0.05f;
        public static readonly float PRIMARY_DOWN_KEY_LABEL_PERCENT_Y = 0.2f;
        public static readonly float PRIMARY_DOWN_KEY_ASPECT_PERCENT = 0.34f;
        public static readonly AspectRatioType PRIMARY_DOWN_KEY_ASPECT_TYPE = AspectRatioType.WidthMaster;
        public static readonly string PRIMARY_DOWN_KEY_CONTENT = "Primary Down Key:";

        public static readonly float PRIMARY_DOWN_KEY_ICON_PERCENT_X = 0.4f;
        public static readonly float PRIMARY_DOWN_KEY_ICON_PERCENT_Y = 0.185f;
        public static readonly float PRIMARY_DOWN_KEY_ICON_ASPECT_PERCENT = PRIMARY_UP_KEY_ICON_ASPECT_PERCENT;
        public static readonly float PRIMARY_DOWN_KEY_ICON_ASPECT_RATIO = 1;
        public static readonly AspectRatioType PRIMARY_DOWN_KEY_ICON_ASPECT_TYPE = AspectRatioType.WidthMaster;

        public static readonly float SECONDARY_UP_KEY_LABEL_PERCENT_X = 0.05f;
        public static readonly float SECONDARY_UP_KEY_LABEL_PERCENT_Y = 0.3f;
        public static readonly float SECONDARY_UP_KEY_ASPECT_PERCENT = 0.34f;
        public static readonly AspectRatioType SECONDARY_UP_KEY_ASPECT_TYPE = AspectRatioType.WidthMaster;
        public static readonly string SECONDARY_UP_KEY_CONTENT = "Secondary Up Key:";

        public static readonly float SECONDARY_UP_KEY_ICON_PERCENT_X = 0.4f;
        public static readonly float SECONDARY_UP_KEY_ICON_PERCENT_Y = 0.285f;
        public static readonly float SECONDARY_UP_KEY_ICON_ASPECT_PERCENT = PRIMARY_UP_KEY_ICON_ASPECT_PERCENT;
        public static readonly float SECONDARY_UP_KEY_ICON_ASPECT_RATIO = 1;
        public static readonly AspectRatioType SECONDARY_UP_KEY_ICON_ASPECT_TYPE = AspectRatioType.WidthMaster;

        public static readonly float SECONDARY_DOWN_KEY_LABEL_PERCENT_X = 0.05f;
        public static readonly float SECONDARY_DOWN_KEY_LABEL_PERCENT_Y = 0.4f;
        public static readonly float SECONDARY_DOWN_KEY_ASPECT_PERCENT = 0.4f;
        public static readonly AspectRatioType SECONDARY_DOWN_KEY_ASPECT_TYPE = AspectRatioType.WidthMaster;
        public static readonly string SECONDARY_DOWN_KEY_CONTENT = "Secondary Down Key:";

        public static readonly float SECONDARY_DOWN_KEY_ICON_PERCENT_X = 0.46f;
        public static readonly float SECONDARY_DOWN_KEY_ICON_PERCENT_Y = 0.385f;
        public static readonly float SECONDARY_DOWN_KEY_ICON_ASPECT_PERCENT = PRIMARY_UP_KEY_ICON_ASPECT_PERCENT;
        public static readonly float SECONDARY_DOWN_KEY_ICON_ASPECT_RATIO = 1;
        public static readonly AspectRatioType SECONDARY_DOWN_KEY_ICON_ASPECT_TYPE = AspectRatioType.WidthMaster;

        public static readonly float CHANGE_KEY_TUT_ASPECT_PERCENT = 0.2f;
        public static readonly AspectRatioType CHANGE_KEY_TUT_ASPECT_TYPE = AspectRatioType.WidthMaster;
        public static readonly string CHANGE_KEY_TUT_CONTENT = "<Press New Key>";
        public static readonly float CHANGE_KEY_TUT_1_PERCENT_X = PRIMARY_UP_KEY_ICON_PERCENT_X;
        public static readonly float CHANGE_KEY_TUT_1_PERCENT_Y = 0.11f;
        public static readonly float CHANGE_KEY_TUT_2_PERCENT_X = PRIMARY_DOWN_KEY_ICON_PERCENT_X;
        public static readonly float CHANGE_KEY_TUT_2_PERCENT_Y = 0.21f;
        public static readonly float CHANGE_KEY_TUT_3_PERCENT_X = SECONDARY_UP_KEY_ICON_PERCENT_X;
        public static readonly float CHANGE_KEY_TUT_3_PERCENT_Y = 0.31f;
        public static readonly float CHANGE_KEY_TUT_4_PERCENT_X = SECONDARY_DOWN_KEY_ICON_PERCENT_X;
        public static readonly float CHANGE_KEY_TUT_4_PERCENT_Y = 0.41f;

        public static readonly float CHANGE_KEY_BUTTON_ASPECT_PERCENT = 0.09f;
        public static readonly float CHANGE_KEY_BUTTON_ASPECT_RATIO = 2.1f;
        public static readonly AspectRatioType CHANGE_KEY_BUTTON_ASPECT_TYPE = AspectRatioType.WidthMaster;
        public static readonly float CHANGE_KEY_BUTTON_LABEL_ASPECT_PERCENT = 0.4f;
        public static readonly AspectRatioType CHANGE_KEY_BUTTON_LABEL_ASPECT_TYPE = AspectRatioType.HeightMaster;
        public static readonly string CHANGE_KEY_BUTTON_LABEL_CONTENT = "Change";
        public static readonly float CHANGE_KEY_BUTTON_1_PERCENT_X = 0.4f;
        public static readonly float CHANGE_KEY_BUTTON_1_PERCENT_Y = 0.09f;
        public static readonly float CHANGE_KEY_BUTTON_2_PERCENT_X = 0.46f;
        public static readonly float CHANGE_KEY_BUTTON_2_PERCENT_Y = 0.19f;
        public static readonly float CHANGE_KEY_BUTTON_3_PERCENT_X = 0.46f;
        public static readonly float CHANGE_KEY_BUTTON_3_PERCENT_Y = 0.29f;
        public static readonly float CHANGE_KEY_BUTTON_4_PERCENT_X = 0.52f;
        public static readonly float CHANGE_KEY_BUTTON_4_PERCENT_Y = 0.39f;
    }
}
