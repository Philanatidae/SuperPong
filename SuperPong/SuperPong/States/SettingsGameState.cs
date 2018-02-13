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

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;
using SuperPong.Content;
using SuperPong.UI;
using SuperPong.UI.Widgets;

namespace SuperPong.States
{
    public class SettingsGameState : GameState
    {
        SpriteBatch _spriteBatch;

        BitmapFont _mainFont;
        NinePatchRegion2D _buttonReleased;
        NinePatchRegion2D _buttonHover;
        NinePatchRegion2D _buttonPressed;

        Texture2D _primaryKey1Icon;
        Texture2D _primaryKey2Icon;
        Texture2D _secondaryKey1Icon;
        Texture2D _secondaryKey2Icon;

        Root _root;

        Button _backButton;

        Image _icon1;
        Image _icon2;
        Image _icon3;
        Image _icon4;

        Label _changeKeyTut1;
        Label _changeKeyTut2;
        Label _changeKeyTut3;
        Label _changeKeyTut4;

        Button _changeKeyButton1;
        Button _changeKeyButton2;
        Button _changeKeyButton3;
        Button _changeKeyButton4;

        enum ChangeState
        {
            None,
            PrimaryKey1,
            PrimaryKey2,
            SecondaryKey1,
            SecondaryKey2,
        }
        ChangeState _changeState = ChangeState.None;

        public SettingsGameState(GameManager gameManager) : base(gameManager)
        {
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);
        }

        public override void Initialize()
        {
            _root = new Root(GameManager.GraphicsDevice.Viewport.Width,
                             GameManager.GraphicsDevice.Viewport.Height);
        }

        public override void LoadContent()
        {
            _mainFont = Content.Load<BitmapFont>(Constants.Resources.FONT_MENU_BUTTON);
            _buttonReleased = new NinePatchRegion2D(new TextureRegion2D(Content.Load<Texture2D>(Constants.Resources.TEXTURE_BUTTON_RELEASED)),
                                                    4, 4, 4, 4);
            _buttonHover = new NinePatchRegion2D(new TextureRegion2D(Content.Load<Texture2D>(Constants.Resources.TEXTURE_BUTTON_HOVER)),
                                                    4, 4, 4, 4);
            _buttonPressed = new NinePatchRegion2D(new TextureRegion2D(Content.Load<Texture2D>(Constants.Resources.TEXTURE_BUTTON_PRESSED)),
                                                    4, 4, 4, 4);

            ReloadKeyIcons();
        }

        void ReloadKeyIcons()
        {
            LockingContentManager content = Content as LockingContentManager;
            bool oLocked = content.Locked;
            content.Locked = false;

            for (int i = 0; i < Constants.Resources.TEXTURES_KEYBOARD_ICON.Length; ++i)
            {
                if (Constants.Resources.TEXTURES_KEYBOARD_ICON[i].Key
                   == Settings.Instance.Data.PrimaryKey1)
                {
                    _primaryKey1Icon = Content.Load<Texture2D>(Constants.Resources.TEXTURES_KEYBOARD_ICON[i].Resource);
                }
                if (Constants.Resources.TEXTURES_KEYBOARD_ICON[i].Key
                   == Settings.Instance.Data.PrimaryKey2)
                {
                    _primaryKey2Icon = Content.Load<Texture2D>(Constants.Resources.TEXTURES_KEYBOARD_ICON[i].Resource);
                }
                if (Constants.Resources.TEXTURES_KEYBOARD_ICON[i].Key
                    == Settings.Instance.Data.SecondaryKey1)
                {
                    _secondaryKey1Icon = Content.Load<Texture2D>(Constants.Resources.TEXTURES_KEYBOARD_ICON[i].Resource);
                }
                if (Constants.Resources.TEXTURES_KEYBOARD_ICON[i].Key
                    == Settings.Instance.Data.SecondaryKey2)
                {
                    _secondaryKey2Icon = Content.Load<Texture2D>(Constants.Resources.TEXTURES_KEYBOARD_ICON[i].Resource);
                }
            }

            content.Locked = oLocked;
        }

        public override void Show()
        {
            _root.RegisterListeners();

            BuildUI();
        }

        void BuildUI()
        {
            _backButton = new Button(_buttonReleased,
                                     _buttonHover,
                                     _buttonPressed,
                                     Origin.BottomLeft,
                                     Constants.Options.BACK_BUTTON_PERCENT_X,
                                    0,
                                     Constants.Options.BACK_BUTTON_PERCENT_Y,
                                    0,
                                     Constants.Options.BACK_BUTTON_ASPECT_PERCENT,
                                    0,
                                     Constants.Options.BACK_BUTTON_ASPECT_RATIO,
                                     Constants.Options.BACK_BUTTON_ASPECT_TYPE);
            _backButton.Action = () =>
            {
                GameManager.ChangeState(new MenuGameState(GameManager));
            };
            _root.Add(_backButton);
            Label backLabel = new Label(_mainFont,
                                        Origin.Center,
                                        0,
                                        0,
                                        0,
                                        0,
                                        Constants.Options.BACK_BUTTON_LABEL_ASPECT_PERCENT,
                                        0,
                                        Constants.Options.BACK_BUTTON_LABEL_ASPECT_TYPE);
            backLabel.Content = Constants.Options.BACK_BUTTON_LABEL;
            _backButton.SubPanel.Add(backLabel);

            ///

            Label primaryKey1Label = new Label(_mainFont,
                                               Origin.TopLeft,
                                               Constants.Options.PRIMARY_UP_KEY_LABEL_PERCENT_X,
                                              0,
                                              Constants.Options.PRIMARY_UP_KEY_LABEL_PERCENT_Y,
                                              0,
                                               Constants.Options.PRIMARY_UP_KEY_ASPECT_PERCENT,
                                              0,
                                               Constants.Options.PRIMARY_UP_KEY_ASPECT_TYPE);
            primaryKey1Label.Content = Constants.Options.PRIMARY_UP_KEY_CONTENT;
            _root.Add(primaryKey1Label);

            _icon1 = new Image(_primaryKey1Icon,
                                              Origin.TopLeft,
                                              Constants.Options.PRIMARY_UP_KEY_ICON_PERCENT_X,
                                             0,
                                             Constants.Options.PRIMARY_UP_KEY_ICON_PERCENT_Y,
                                             0,
                                              Constants.Options.PRIMARY_UP_KEY_ICON_ASPECT_PERCENT,
                                             0,
                                              Constants.Options.PRIMARY_UP_KEY_ICON_ASPECT_RATIO,
                                              Constants.Options.PRIMARY_UP_KEY_ICON_ASPECT_TYPE);
            _root.Add(_icon1);

            Label primaryKey2Label = new Label(_mainFont,
                                               Origin.TopLeft,
                                               Constants.Options.PRIMARY_DOWN_KEY_LABEL_PERCENT_X,
                                              0,
                                              Constants.Options.PRIMARY_DOWN_KEY_LABEL_PERCENT_Y,
                                              0,
                                               Constants.Options.PRIMARY_DOWN_KEY_ASPECT_PERCENT,
                                              0,
                                               Constants.Options.PRIMARY_DOWN_KEY_ASPECT_TYPE);
            primaryKey2Label.Content = Constants.Options.PRIMARY_DOWN_KEY_CONTENT;
            _root.Add(primaryKey2Label);

            _icon2 = new Image(_primaryKey2Icon,
                                              Origin.TopLeft,
                                              Constants.Options.PRIMARY_DOWN_KEY_ICON_PERCENT_X,
                                             0,
                                             Constants.Options.PRIMARY_DOWN_KEY_ICON_PERCENT_Y,
                                             0,
                                              Constants.Options.PRIMARY_DOWN_KEY_ICON_ASPECT_PERCENT,
                                             0,
                                              Constants.Options.PRIMARY_DOWN_KEY_ICON_ASPECT_RATIO,
                                              Constants.Options.PRIMARY_DOWN_KEY_ICON_ASPECT_TYPE);
            _root.Add(_icon2);

            Label secondaryKey1Label = new Label(_mainFont,
                                               Origin.TopLeft,
                                               Constants.Options.SECONDARY_UP_KEY_LABEL_PERCENT_X,
                                              0,
                                              Constants.Options.SECONDARY_UP_KEY_LABEL_PERCENT_Y,
                                              0,
                                               Constants.Options.SECONDARY_UP_KEY_ASPECT_PERCENT,
                                              0,
                                               Constants.Options.SECONDARY_UP_KEY_ASPECT_TYPE);
            secondaryKey1Label.Content = Constants.Options.SECONDARY_UP_KEY_CONTENT;
            _root.Add(secondaryKey1Label);

            _icon3 = new Image(_secondaryKey1Icon,
                                              Origin.TopLeft,
                                              Constants.Options.SECONDARY_UP_KEY_ICON_PERCENT_X,
                                             0,
                                             Constants.Options.SECONDARY_UP_KEY_ICON_PERCENT_Y,
                                             0,
                                              Constants.Options.SECONDARY_UP_KEY_ICON_ASPECT_PERCENT,
                                             0,
                                              Constants.Options.SECONDARY_UP_KEY_ICON_ASPECT_RATIO,
                                              Constants.Options.SECONDARY_UP_KEY_ICON_ASPECT_TYPE);
            _root.Add(_icon3);

            Label secondaryKey2Label = new Label(_mainFont,
                                               Origin.TopLeft,
                                               Constants.Options.SECONDARY_DOWN_KEY_LABEL_PERCENT_X,
                                              0,
                                              Constants.Options.SECONDARY_DOWN_KEY_LABEL_PERCENT_Y,
                                              0,
                                               Constants.Options.SECONDARY_DOWN_KEY_ASPECT_PERCENT,
                                              0,
                                               Constants.Options.SECONDARY_DOWN_KEY_ASPECT_TYPE);
            secondaryKey2Label.Content = Constants.Options.SECONDARY_DOWN_KEY_CONTENT;
            _root.Add(secondaryKey2Label);

            _icon4 = new Image(_secondaryKey2Icon,
                                              Origin.TopLeft,
                                              Constants.Options.SECONDARY_DOWN_KEY_ICON_PERCENT_X,
                                             0,
                                             Constants.Options.SECONDARY_DOWN_KEY_ICON_PERCENT_Y,
                                             0,
                                              Constants.Options.SECONDARY_DOWN_KEY_ICON_ASPECT_PERCENT,
                                             0,
                                              Constants.Options.SECONDARY_DOWN_KEY_ICON_ASPECT_RATIO,
                                              Constants.Options.SECONDARY_DOWN_KEY_ICON_ASPECT_TYPE);
            _root.Add(_icon4);

            ///
            _changeKeyTut1 = new Label(_mainFont,
                                       Origin.TopLeft,
                                       Constants.Options.CHANGE_KEY_TUT_1_PERCENT_X,
                                      0,
                                      Constants.Options.CHANGE_KEY_TUT_1_PERCENT_Y,
                                      0,
                                       Constants.Options.CHANGE_KEY_TUT_ASPECT_PERCENT,
                                      0,
                                       Constants.Options.CHANGE_KEY_TUT_ASPECT_TYPE);
            _changeKeyTut1.Content = Constants.Options.CHANGE_KEY_TUT_CONTENT;
            _changeKeyTut1.Hidden = true;
            _root.Add(_changeKeyTut1);

            _changeKeyTut2 = new Label(_mainFont,
                                       Origin.TopLeft,
                                       Constants.Options.CHANGE_KEY_TUT_2_PERCENT_X,
                                      0,
                                      Constants.Options.CHANGE_KEY_TUT_2_PERCENT_Y,
                                      0,
                                       Constants.Options.CHANGE_KEY_TUT_ASPECT_PERCENT,
                                      0,
                                       Constants.Options.CHANGE_KEY_TUT_ASPECT_TYPE);
            _changeKeyTut2.Content = Constants.Options.CHANGE_KEY_TUT_CONTENT;
            _changeKeyTut2.Hidden = true;
            _root.Add(_changeKeyTut2);

            _changeKeyTut3 = new Label(_mainFont,
                                       Origin.TopLeft,
                                       Constants.Options.CHANGE_KEY_TUT_3_PERCENT_X,
                                      0,
                                      Constants.Options.CHANGE_KEY_TUT_3_PERCENT_Y,
                                      0,
                                       Constants.Options.CHANGE_KEY_TUT_ASPECT_PERCENT,
                                      0,
                                       Constants.Options.CHANGE_KEY_TUT_ASPECT_TYPE);
            _changeKeyTut3.Content = Constants.Options.CHANGE_KEY_TUT_CONTENT;
            _changeKeyTut3.Hidden = true;
            _root.Add(_changeKeyTut3);

            _changeKeyTut4 = new Label(_mainFont,
                                       Origin.TopLeft,
                                       Constants.Options.CHANGE_KEY_TUT_4_PERCENT_X,
                                      0,
                                      Constants.Options.CHANGE_KEY_TUT_4_PERCENT_Y,
                                      0,
                                       Constants.Options.CHANGE_KEY_TUT_ASPECT_PERCENT,
                                      0,
                                       Constants.Options.CHANGE_KEY_TUT_ASPECT_TYPE);
            _changeKeyTut4.Content = Constants.Options.CHANGE_KEY_TUT_CONTENT;
            _changeKeyTut4.Hidden = true;
            _root.Add(_changeKeyTut4);

            ///

            _changeKeyButton1 = new Button(_buttonReleased,
                                     _buttonHover,
                                     _buttonPressed,
                                           Origin.TopLeft,
                                     Constants.Options.CHANGE_KEY_BUTTON_1_PERCENT_X,
                                    0,
                                           Constants.Options.CHANGE_KEY_BUTTON_1_PERCENT_Y,
                                    0,
                                           Constants.Options.CHANGE_KEY_BUTTON_ASPECT_PERCENT,
                                    0,
                                           Constants.Options.CHANGE_KEY_BUTTON_ASPECT_RATIO,
                                           Constants.Options.CHANGE_KEY_BUTTON_ASPECT_TYPE);
            _changeKeyButton1.Action = () =>
            {
                _changeState = ChangeState.PrimaryKey1;
            };
            _root.Add(_changeKeyButton1);
            Label changeKeyButton1Label = new Label(_mainFont,
                                        Origin.Center,
                                        0,
                                        0,
                                        0,
                                        0,
                                                    Constants.Options.CHANGE_KEY_BUTTON_LABEL_ASPECT_PERCENT,
                                        0,
                                                    Constants.Options.CHANGE_KEY_BUTTON_LABEL_ASPECT_TYPE);
            changeKeyButton1Label.Content = Constants.Options.CHANGE_KEY_BUTTON_LABEL_CONTENT;
            _changeKeyButton1.SubPanel.Add(changeKeyButton1Label);

            _changeKeyButton2 = new Button(_buttonReleased,
                                     _buttonHover,
                                     _buttonPressed,
                                           Origin.TopLeft,
                                     Constants.Options.CHANGE_KEY_BUTTON_2_PERCENT_X,
                                    0,
                                           Constants.Options.CHANGE_KEY_BUTTON_2_PERCENT_Y,
                                    0,
                                           Constants.Options.CHANGE_KEY_BUTTON_ASPECT_PERCENT,
                                    0,
                                           Constants.Options.CHANGE_KEY_BUTTON_ASPECT_RATIO,
                                           Constants.Options.CHANGE_KEY_BUTTON_ASPECT_TYPE);
            _changeKeyButton2.Action = () =>
            {
                _changeState = ChangeState.PrimaryKey2;
            };
            _root.Add(_changeKeyButton2);
            Label changeKeyButton2Label = new Label(_mainFont,
                                        Origin.Center,
                                        0,
                                        0,
                                        0,
                                        0,
                                                    Constants.Options.CHANGE_KEY_BUTTON_LABEL_ASPECT_PERCENT,
                                        0,
                                                    Constants.Options.CHANGE_KEY_BUTTON_LABEL_ASPECT_TYPE);
            changeKeyButton2Label.Content = Constants.Options.CHANGE_KEY_BUTTON_LABEL_CONTENT;
            _changeKeyButton2.SubPanel.Add(changeKeyButton2Label);

            _changeKeyButton3 = new Button(_buttonReleased,
                                     _buttonHover,
                                     _buttonPressed,
                                           Origin.TopLeft,
                                     Constants.Options.CHANGE_KEY_BUTTON_3_PERCENT_X,
                                    0,
                                           Constants.Options.CHANGE_KEY_BUTTON_3_PERCENT_Y,
                                    0,
                                           Constants.Options.CHANGE_KEY_BUTTON_ASPECT_PERCENT,
                                    0,
                                           Constants.Options.CHANGE_KEY_BUTTON_ASPECT_RATIO,
                                           Constants.Options.CHANGE_KEY_BUTTON_ASPECT_TYPE);
            _changeKeyButton3.Action = () =>
            {
                _changeState = ChangeState.SecondaryKey1;
            };
            _root.Add(_changeKeyButton3);
            Label changeKeyButton3Label = new Label(_mainFont,
                                        Origin.Center,
                                        0,
                                        0,
                                        0,
                                        0,
                                                    Constants.Options.CHANGE_KEY_BUTTON_LABEL_ASPECT_PERCENT,
                                        0,
                                                    Constants.Options.CHANGE_KEY_BUTTON_LABEL_ASPECT_TYPE);
            changeKeyButton3Label.Content = Constants.Options.CHANGE_KEY_BUTTON_LABEL_CONTENT;
            _changeKeyButton3.SubPanel.Add(changeKeyButton3Label);

            _changeKeyButton4 = new Button(_buttonReleased,
                                     _buttonHover,
                                     _buttonPressed,
                                           Origin.TopLeft,
                                     Constants.Options.CHANGE_KEY_BUTTON_4_PERCENT_X,
                                    0,
                                           Constants.Options.CHANGE_KEY_BUTTON_4_PERCENT_Y,
                                    0,
                                           Constants.Options.CHANGE_KEY_BUTTON_ASPECT_PERCENT,
                                    0,
                                           Constants.Options.CHANGE_KEY_BUTTON_ASPECT_RATIO,
                                           Constants.Options.CHANGE_KEY_BUTTON_ASPECT_TYPE);
            _changeKeyButton4.Action = () =>
            {
                _changeState = ChangeState.SecondaryKey2;
            };
            _root.Add(_changeKeyButton4);
            Label changeKeyButton4Label = new Label(_mainFont,
                                        Origin.Center,
                                        0,
                                        0,
                                        0,
                                        0,
                                                    Constants.Options.CHANGE_KEY_BUTTON_LABEL_ASPECT_PERCENT,
                                        0,
                                                    Constants.Options.CHANGE_KEY_BUTTON_LABEL_ASPECT_TYPE);
            changeKeyButton4Label.Content = Constants.Options.CHANGE_KEY_BUTTON_LABEL_CONTENT;
            _changeKeyButton4.SubPanel.Add(changeKeyButton4Label);
        }

        public override void Update(float dt)
        {
            switch (_changeState)
            {
                case ChangeState.PrimaryKey1:
                    _changeKeyTut1.Hidden = false;
                    _changeKeyTut2.Hidden = true;
                    _changeKeyTut3.Hidden = true;
                    _changeKeyTut4.Hidden = true;

                    _icon1.Hidden = true;
                    _icon2.Hidden = false;
                    _icon3.Hidden = false;
                    _icon4.Hidden = false;

                    _changeKeyButton1.Hidden = true;
                    _changeKeyButton2.Hidden = true;
                    _changeKeyButton3.Hidden = true;
                    _changeKeyButton4.Hidden = true;

                    {
                        KeyboardState keyboardState = Keyboard.GetState();
                        for (int i = 0; i < Constants.Resources.TEXTURES_KEYBOARD_ICON.Length; i++)
                        {
                            Keys key = Constants.Resources.TEXTURES_KEYBOARD_ICON[i].Key;

                            if (keyboardState.IsKeyDown(key))
                            {
                                Settings.Instance.Data.PrimaryKey1 = key;
                                Settings.Instance.Save();
                                ReloadKeyIcons();
                                _icon1.Texture = _primaryKey1Icon;
                                _changeState = ChangeState.None;
                                goto case ChangeState.None;
                            }
                        }
                    }
                    break;
                case ChangeState.PrimaryKey2:
                    _changeKeyTut1.Hidden = true;
                    _changeKeyTut2.Hidden = false;
                    _changeKeyTut3.Hidden = true;
                    _changeKeyTut4.Hidden = true;

                    _icon1.Hidden = false;
                    _icon2.Hidden = true;
                    _icon3.Hidden = false;
                    _icon4.Hidden = false;

                    _changeKeyButton1.Hidden = true;
                    _changeKeyButton2.Hidden = true;
                    _changeKeyButton3.Hidden = true;
                    _changeKeyButton4.Hidden = true;

                    {
                        KeyboardState keyboardState = Keyboard.GetState();
                        for (int i = 0; i < Constants.Resources.TEXTURES_KEYBOARD_ICON.Length; i++)
                        {
                            Keys key = Constants.Resources.TEXTURES_KEYBOARD_ICON[i].Key;

                            if (keyboardState.IsKeyDown(key))
                            {
                                Settings.Instance.Data.PrimaryKey2 = key;
                                Settings.Instance.Save();
                                ReloadKeyIcons();
                                _icon2.Texture = _primaryKey2Icon;
                                _changeState = ChangeState.None;
                                goto case ChangeState.None;
                            }
                        }
                    }
                    break;
                case ChangeState.SecondaryKey1:
                    _changeKeyTut1.Hidden = true;
                    _changeKeyTut2.Hidden = true;
                    _changeKeyTut3.Hidden = false;
                    _changeKeyTut4.Hidden = true;

                    _icon1.Hidden = false;
                    _icon2.Hidden = false;
                    _icon3.Hidden = true;
                    _icon4.Hidden = false;

                    _changeKeyButton1.Hidden = true;
                    _changeKeyButton2.Hidden = true;
                    _changeKeyButton3.Hidden = true;
                    _changeKeyButton4.Hidden = true;

                    {
                        KeyboardState keyboardState = Keyboard.GetState();
                        for (int i = 0; i < Constants.Resources.TEXTURES_KEYBOARD_ICON.Length; i++)
                        {
                            Keys key = Constants.Resources.TEXTURES_KEYBOARD_ICON[i].Key;

                            if (keyboardState.IsKeyDown(key))
                            {
                                Settings.Instance.Data.SecondaryKey1 = key;
                                Settings.Instance.Save();
                                ReloadKeyIcons();
                                _icon3.Texture = _secondaryKey1Icon;
                                _changeState = ChangeState.None;
                                goto case ChangeState.None;
                            }
                        }
                    }
                    break;
                case ChangeState.SecondaryKey2:
                    _changeKeyTut1.Hidden = true;
                    _changeKeyTut2.Hidden = true;
                    _changeKeyTut3.Hidden = true;
                    _changeKeyTut4.Hidden = false;

                    _icon1.Hidden = false;
                    _icon2.Hidden = false;
                    _icon3.Hidden = false;
                    _icon4.Hidden = true;

                    _changeKeyButton1.Hidden = true;
                    _changeKeyButton2.Hidden = true;
                    _changeKeyButton3.Hidden = true;
                    _changeKeyButton4.Hidden = true;

                    {
                        KeyboardState keyboardState = Keyboard.GetState();
                        for (int i = 0; i < Constants.Resources.TEXTURES_KEYBOARD_ICON.Length; i++)
                        {
                            Keys key = Constants.Resources.TEXTURES_KEYBOARD_ICON[i].Key;

                            if (keyboardState.IsKeyDown(key))
                            {
                                Settings.Instance.Data.SecondaryKey2 = key;
                                Settings.Instance.Save();
                                ReloadKeyIcons();
                                _icon4.Texture = _secondaryKey2Icon;
                                _changeState = ChangeState.None;
                                goto case ChangeState.None;
                            }
                        }
                    }
                    break;
                case ChangeState.None:
                default:
                    _changeKeyTut1.Hidden = true;
                    _changeKeyTut2.Hidden = true;
                    _changeKeyTut3.Hidden = true;
                    _changeKeyTut4.Hidden = true;

                    _icon1.Hidden = false;
                    _icon2.Hidden = false;
                    _icon3.Hidden = false;
                    _icon4.Hidden = false;

                    _changeKeyButton1.Hidden = false;
                    _changeKeyButton2.Hidden = false;
                    _changeKeyButton3.Hidden = false;
                    _changeKeyButton4.Hidden = false;
                    break;
            }
        }

        public override void Draw(float dt)
        {
            _spriteBatch.Begin();
            _root.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        public override void Hide()
        {
            _root.UnregisterListeners();
        }

        public override void Dispose()
        {

        }
    }
}
