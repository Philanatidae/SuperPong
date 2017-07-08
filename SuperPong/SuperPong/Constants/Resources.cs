using System;
using Microsoft.Xna.Framework.Input;

namespace SuperPong.Constants
{
    public class Resources
    {
        public static readonly string TEXTURE_PONG_BALL = "textures/pong/ball";
        public static readonly string TEXTURE_PONG_PADDLE = "textures/pong/paddle";
        public static readonly string TEXTURE_PONG_EDGE = "textures/pong/border";
        public static readonly string TEXTURE_PONG_GOAL = "textures/pong/GoalLine";
        public static readonly string TEXTURE_PONG_FIELD_BACKGROUND = "textures/pong/Field";

        public static readonly string TEXTURE_BUTTON_RELEASED = "textures/gui/button_up_background";
        public static readonly string TEXTURE_BUTTON_HOVER = "textures/gui/button_over_background";
        public static readonly string TEXTURE_BUTTON_PRESSED = "textures/gui/button_down_background";

        public static readonly string TEXTURE_BACKGROUND_BLACK = "textures/gui/black";

        public static readonly string TEXTURE_LOBBY_PANEL_BACKGROUND = "textures/gui/LobbyPanelBackground";

        public static readonly string TEXTURE_PARTICLE_VELOCITY = "textures/particles/VelocityParticle";

        public static readonly string TEXTURE_CONTROLLER_BUTTON_ICON_A = "textures/gui/ControllerButtons/360_A";
        public static readonly string TEXTURE_CONTROLLER_BUTTON_ICON_B = "textures/gui/ControllerButtons/360_B";
        public static readonly string TEXTURE_CONTROLLER_STICK_LEFT = "textures/gui/ControllerButtons/360_Left_Stick";

        public static readonly string FONT_PONG_LIVES = "fonts/Lives";
        public static readonly string FONT_MENU_BUTTON = "fonts/MenuButtonLabel";
        public static readonly string FONT_LOBBY = "fonts/Lobby";
        public static readonly string FONT_PONG_INTRO = "fonts/Intro";

        public static readonly string EFFECT_WARP = "effects/WarpEffect";
        public static readonly string EFFECT_BLUR = "effects/BlurEffect";

        // Keys
        public struct KeyboardIconTexture
        {
            public Keys Key;
            public string Resource;
        }

        public static readonly KeyboardIconTexture[] TEXTURES_KEYBOARD_ICON = {
            new KeyboardIconTexture {
                Key = Keys.D0,
                Resource = "textures/gui/Keys/Keyboard_White_0"
            },
            new KeyboardIconTexture {
                Key = Keys.D1,
                Resource = "textures/gui/Keys/Keyboard_White_1"
            },
            new KeyboardIconTexture {
                Key = Keys.D2,
                Resource = "textures/gui/Keys/Keyboard_White_2"
            },
            new KeyboardIconTexture {
                Key = Keys.D3,
                Resource = "textures/gui/Keys/Keyboard_White_3"
            },
            new KeyboardIconTexture {
                Key = Keys.D4,
                Resource = "textures/gui/Keys/Keyboard_White_4"
            },
            new KeyboardIconTexture {
                Key = Keys.D5,
                Resource = "textures/gui/Keys/Keyboard_White_5"
            },
            new KeyboardIconTexture {
                Key = Keys.D6,
                Resource = "textures/gui/Keys/Keyboard_White_6"
            },
            new KeyboardIconTexture {
                Key = Keys.D7,
                Resource = "textures/gui/Keys/Keyboard_White_7"
            },
            new KeyboardIconTexture {
                Key = Keys.D8,
                Resource = "textures/gui/Keys/Keyboard_White_8"
            },
            new KeyboardIconTexture {
                Key = Keys.D9,
                Resource = "textures/gui/Keys/Keyboard_White_9"
            },
            new KeyboardIconTexture {
                Key = Keys.A,
                Resource = "textures/gui/Keys/Keyboard_White_A"
            },
            new KeyboardIconTexture {
                Key = Keys.Down,
                Resource = "textures/gui/Keys/Keyboard_White_Arrow_Down"
            },
            new KeyboardIconTexture {
                Key = Keys.Left,
                Resource = "textures/gui/Keys/Keyboard_White_Arrow_Left"
            },
            new KeyboardIconTexture {
                Key = Keys.Right,
                Resource = "textures/gui/Keys/Keyboard_White_Arrow_Right"
            },
            new KeyboardIconTexture {
                Key = Keys.Up,
                Resource = "textures/gui/Keys/Keyboard_White_Arrow_Up"
            },
            new KeyboardIconTexture {
                Key = Keys.B,
                Resource = "textures/gui/Keys/Keyboard_White_B"
            },
            new KeyboardIconTexture {
                Key = Keys.C,
                Resource = "textures/gui/Keys/Keyboard_White_C"
            },
            new KeyboardIconTexture {
                Key = Keys.D,
                Resource = "textures/gui/Keys/Keyboard_White_D"
            },
            new KeyboardIconTexture {
                Key = Keys.E,
                Resource = "textures/gui/Keys/Keyboard_White_E"
            },
            new KeyboardIconTexture {
                Key = Keys.F,
                Resource = "textures/gui/Keys/Keyboard_White_F"
            },
            new KeyboardIconTexture {
                Key = Keys.G,
                Resource = "textures/gui/Keys/Keyboard_White_G"
            },
            new KeyboardIconTexture {
                Key = Keys.H,
                Resource = "textures/gui/Keys/Keyboard_White_H"
            },
            new KeyboardIconTexture {
                Key = Keys.I,
                Resource = "textures/gui/Keys/Keyboard_White_I"
            },
            new KeyboardIconTexture {
                Key = Keys.J,
                Resource = "textures/gui/Keys/Keyboard_White_J"
            },
            new KeyboardIconTexture {
                Key = Keys.K,
                Resource = "textures/gui/Keys/Keyboard_White_K"
            },
            new KeyboardIconTexture {
                Key = Keys.L,
                Resource = "textures/gui/Keys/Keyboard_White_L"
            },
            new KeyboardIconTexture {
                Key = Keys.M,
                Resource = "textures/gui/Keys/Keyboard_White_M"
            },
            new KeyboardIconTexture {
                Key = Keys.N,
                Resource = "textures/gui/Keys/Keyboard_White_N"
            },
            new KeyboardIconTexture {
                Key = Keys.O,
                Resource = "textures/gui/Keys/Keyboard_White_O"
            },
            new KeyboardIconTexture {
                Key = Keys.P,
                Resource = "textures/gui/Keys/Keyboard_White_P"
            },
            new KeyboardIconTexture {
                Key = Keys.Q,
                Resource = "textures/gui/Keys/Keyboard_White_Q"
            },
            new KeyboardIconTexture {
                Key = Keys.R,
                Resource = "textures/gui/Keys/Keyboard_White_R"
            },
            new KeyboardIconTexture {
                Key = Keys.S,
                Resource = "textures/gui/Keys/Keyboard_White_S"
            },
            new KeyboardIconTexture {
                Key = Keys.T,
                Resource = "textures/gui/Keys/Keyboard_White_T"
            },
            new KeyboardIconTexture {
                Key = Keys.U,
                Resource = "textures/gui/Keys/Keyboard_White_U"
            },
            new KeyboardIconTexture {
                Key = Keys.V,
                Resource = "textures/gui/Keys/Keyboard_White_V"
            },
            new KeyboardIconTexture {
                Key = Keys.W,
                Resource = "textures/gui/Keys/Keyboard_White_W"
            },
            new KeyboardIconTexture {
                Key = Keys.X,
                Resource = "textures/gui/Keys/Keyboard_White_X"
            },
            new KeyboardIconTexture {
                Key = Keys.Y,
                Resource = "textures/gui/Keys/Keyboard_White_Y"
            },
            new KeyboardIconTexture {
                Key = Keys.Z,
                Resource = "textures/gui/Keys/Keyboard_White_Z"
            }
        };
    }
}
