/*
This file is part of Super Pong.

Super Pong is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Super Pong is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Super Pong.  If not, see <http://www.gnu.org/licenses/>.
*/

using SuperPong.UI;

namespace SuperPong.Constants
{
    public class Lobby
    {
        public static readonly float BACK_BUTTON_PERCENT_X = 0.05f;
        public static readonly float BACK_BUTTON_PERCENT_Y = 0.05f;
        public static readonly float BACK_BUTTON_ASPECT_PERCENT = 0.15f;
        public static readonly float BACK_BUTTON_ASPECT_RATIO = 2.15f;
        public static readonly AspectRatioType BACK_BUTTON_ASPECT_TYPE = AspectRatioType.WidthMaster;
        public static readonly string BACK_BUTTON_LABEL = "Back";
        public static readonly float BACK_BUTTON_LABEL_ASPECT_PERCENT = 0.4f;
        public static readonly AspectRatioType BACK_BUTTON_LABEL_ASPECT_TYPE = AspectRatioType.HeightMaster;

        public static readonly float START_BUTTON_PERCENT_X = 0.05f;
        public static readonly float START_BUTTON_PERCENT_Y = 0.05f;
        public static readonly float START_BUTTON_ASPECT_PERCENT = 0.15f;
        public static readonly float START_BUTTON_ASPECT_RATIO = 2.15f;
        public static readonly AspectRatioType START_BUTTON_ASPECT_TYPE = AspectRatioType.WidthMaster;
        public static readonly string START_BUTTON_LABEL = "Start";
        public static readonly float START_BUTTON_LABEL_ASPECT_PERCENT = 0.4f;
        public static readonly AspectRatioType START_BUTTON_LABEL_ASPECT_TYPE = AspectRatioType.HeightMaster;

        public static readonly AspectRatioType JOIN_LABEL_ASPECT_TYPE = AspectRatioType.WidthMaster;
        public static readonly float JOIN_LABEL_ASPECT_PERCENT = 0.3f;
        public static readonly string JOIN_LABEL = "Press\n\n\n\nto join";

        public static readonly float PANEL_RIGHT_X_PERCENT = 0.25f;
        public static readonly float PANEL_LEFT_X_PERCENT = -PANEL_RIGHT_X_PERCENT;
        public static readonly float PANEL_Y_PERCENT = -0.1f;
        public static readonly float PANEL_WIDTH_PERCENT = 0.4f;
        public static readonly float PANEL_HEIGHT_PERCENT = 0.7f;

        public static readonly float PLAYER_LABEL_Y_PERCENT = -0.35f;
        public static readonly float PLAYER_LABEL_1_ASPECT_PERCENT = 0.45f;
        public static readonly float PLAYER_LABEL_2_ASPECT_PERCENT = 0.4f;
        public static readonly float PLAYER_LABEL_2_ASPECT_PERCENT_ALT = PLAYER_LABEL_1_ASPECT_PERCENT;
        public static readonly AspectRatioType PLAYER_LABEL_ASPECT_TYPE = AspectRatioType.WidthMaster;
        public static readonly string PLAYER_LABEL_1 = "Player 1";
        public static readonly string PLAYER_LABEL_2 = "AI, Or...";
        public static readonly string PLAYER_LABEL_2_ALT = "Player 2";

        public static readonly float JOINED_LABEL_ASPECT_PERCENT = 0.7f;
        public static readonly AspectRatioType JOINED_LABEL_ASPECT_TYPE = AspectRatioType.WidthMaster;
        public static readonly string JOINED_LABEL_MOVEMENT_INSTRUCTIONS = "Use      to move";

        public static readonly float JOINED_BUTTON_PERCENT_X = 0;
        public static readonly float JOINED_BUTTON_PERCENT_Y = 0;
        public static readonly float JOINED_BUTTON_ASPECT_PERCENT = 0.1f;
        public static readonly float JOINED_BUTTON_ASPECT_RATIO = 1;
        public static readonly AspectRatioType JOINED_BUTTON_ASPECT_TYPE = AspectRatioType.WidthMaster;
        public static readonly string JOINED_BUTTON_LABEL = "x";
        public static readonly float JOINED_BUTTON_LABEL_ASPECT_PERCENT = 0.75f;

        public static readonly float KEY_ASPECT_PERCENT = 0.1f;
        public static readonly AspectRatioType KEY_ASPECT_TYPE = AspectRatioType.WidthMaster;
        public static readonly float KEY_ASPECT_RATIO = 1.0f;

        public static readonly float KEY_PRIMARY1_X_PERCENT = -0.12f;
        public static readonly float KEY_PRIMARY1_Y_PERCENT = -0.04f;

        public static readonly float KEY_PRIMARY2_X_PERCENT = KEY_PRIMARY1_X_PERCENT;
        public static readonly float KEY_PRIMARY2_Y_PERCENT = -KEY_PRIMARY1_Y_PERCENT;

        public static readonly float KEY_SECONDARY1_X_PERCENT = 0;
        public static readonly float KEY_SECONDARY1_Y_PERCENT = KEY_PRIMARY1_Y_PERCENT;

        public static readonly float KEY_SECONDARY2_X_PERCENT = KEY_SECONDARY1_X_PERCENT;
        public static readonly float KEY_SECONDARY2_Y_PERCENT = -KEY_SECONDARY1_Y_PERCENT;

        public static readonly float KEY_CONTROLLER_A_X_PERCENT = -KEY_PRIMARY1_X_PERCENT;
        public static readonly float KEY_CONTROLLER_A_Y_PERCENT = 0;

        public static readonly float KEY_1_TWO_METHODS_X_PERCENT = -0.06f;
        public static readonly float KEY_1_TWO_METHODS_Y_PERCENT = KEY_PRIMARY1_Y_PERCENT;

        public static readonly float KEY_2_TWO_METHODS_X_PERCENT = KEY_1_TWO_METHODS_X_PERCENT;
        public static readonly float KEY_2_TWO_METHODS_Y_PERCENT = -KEY_1_TWO_METHODS_Y_PERCENT;

        public static readonly float KEY_CONTROLLER_A_TWO_METHODS_X_PERCENT = -KEY_1_TWO_METHODS_X_PERCENT;
        public static readonly float KEY_CONTROLLER_A_TWO_METHODS_Y_PERCENT = 0;

        public static readonly float KEY_JOINED_1_X_PERCENT = -0.095f;
        public static readonly float KEY_JOINED_1_Y_PERCENT = KEY_PRIMARY1_Y_PERCENT;

        public static readonly float KEY_JOINED_2_X_PERCENT = KEY_JOINED_1_X_PERCENT;
        public static readonly float KEY_JOINED_2_Y_PERCENT = -KEY_PRIMARY1_Y_PERCENT;

        public static readonly float KEY_JOINED_CONTROLLER_X_PERCENT = KEY_JOINED_1_X_PERCENT;
        public static readonly float KEY_JOINED_CONTROLLER_Y_PERCENT = 0;
    }
}
