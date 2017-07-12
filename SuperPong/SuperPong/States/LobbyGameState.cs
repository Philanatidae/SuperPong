using System;
using System.Collections.Generic;
using Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;
using SuperPong.Events;
using SuperPong.Input;
using SuperPong.UI;
using SuperPong.UI.Widgets;

namespace SuperPong.States
{
    public class LobbyGameState : GameState, IEventListener
    {
        List<InputMethod> _inputMethods = new List<InputMethod>();
        InputMethod _player1InputMethod = null;
        InputMethod _player2InputMethod = null;

        BitmapFont _lobbyFont;
        BitmapFont _mainFont;

        NinePatchRegion2D _panelBackground;
        NinePatchRegion2D _buttonReleased;
        NinePatchRegion2D _buttonHover;
        NinePatchRegion2D _buttonPressed;
        Texture2D _primaryKey1;
        Texture2D _primaryKey2;
        Texture2D _secondaryKey1;
        Texture2D _secondaryKey2;
        Texture2D _controllerA;
        Texture2D _controllerStick;

        SpriteBatch _spriteBatch;
        Root _root;

        Button _backButton;
        Button _startButton;

        Panel _player1Panel;
        Panel _player2Panel;

        Label _player2Label;

        Panel _player1InputPanel;
        Label _player1JoinLabel;
        Image _player1PrimaryKey1;
        Image _player1PrimaryKey2;
        Image _player1SecondaryKey1;
        Image _player1SecondaryKey2;
        Image _player1ControllerA;

        Panel _player2InputPanel;
        Label _player2JoinLabel;
        Image _player2PrimaryKey1;
        Image _player2PrimaryKey2;
        Image _player2SecondaryKey1;
        Image _player2SecondaryKey2;
        Image _player2ControllerA;

        Panel _player1JoinedPanel;
        Image _player1JoinedPrimaryKey1;
        Image _player1JoinedPrimaryKey2;
        Image _player1JoinedSecondaryKey1;
        Image _player1JoinedSecondaryKey2;
        Image _player1JoinedControllerStick;

        Panel _player2JoinedPanel;
        Image _player2JoinedPrimaryKey1;
        Image _player2JoinedPrimaryKey2;
        Image _player2JoinedSecondaryKey1;
        Image _player2JoinedSecondaryKey2;
        Image _player2JoinedControllerStick;

        public LobbyGameState(GameManager gameManager)
            : base(gameManager)
        {
            _spriteBatch = new SpriteBatch(gameManager.GraphicsDevice);
            _root = new Root(gameManager.GraphicsDevice.Viewport.Width,
                             gameManager.GraphicsDevice.Viewport.Height);
        }

        public LobbyGameState(GameManager gameManager,
                              InputMethod player1InputMethod,
                              InputMethod player2InputMethod)
            : this(gameManager)
        {
            _player1InputMethod = player1InputMethod;
            _player2InputMethod = player2InputMethod;
        }

        public override void Initialize()
        {
            // Add the two keyboard input methods
            PrimaryKeyboardInputMethod primaryKeyboardInputMethod = new PrimaryKeyboardInputMethod();
            _inputMethods.Add(primaryKeyboardInputMethod);
            SecondaryKeyboardInputMethod secondaryKeyboardInputMethod = new SecondaryKeyboardInputMethod();
            _inputMethods.Add(secondaryKeyboardInputMethod);

            if (_player1InputMethod is PrimaryKeyboardInputMethod)
            {
                _player1InputMethod = primaryKeyboardInputMethod;
            }
            if (_player1InputMethod is SecondaryKeyboardInputMethod)
            {
                _player1InputMethod = secondaryKeyboardInputMethod;
            }
            if (_player2InputMethod is PrimaryKeyboardInputMethod)
            {
                _player2InputMethod = primaryKeyboardInputMethod;
            }
            if (_player2InputMethod is SecondaryKeyboardInputMethod)
            {
                _player2InputMethod = secondaryKeyboardInputMethod;
            }

            // Add gamepads
            for (int i = 0; i < 4; ++i)
            {
                GamePadState gamePadState = GamePad.GetState((PlayerIndex)i);

                if (gamePadState.IsConnected)
                {
                    ControllerInputMethod controllerInputMethod = new ControllerInputMethod((PlayerIndex)i);
                    _inputMethods.Add(controllerInputMethod);

                    if (_player1InputMethod is ControllerInputMethod)
                    {
                        if (((ControllerInputMethod)_player1InputMethod).PlayerIndex == (PlayerIndex)i)
                        {
                            _player1InputMethod = controllerInputMethod;
                        }
                    }
                    if (_player2InputMethod is ControllerInputMethod)
                    {
                        if (((ControllerInputMethod)_player2InputMethod).PlayerIndex == (PlayerIndex)i)
                        {
                            _player2InputMethod = controllerInputMethod;
                        }
                    }
                }
            }

            // Register events
            _root.RegisterListeners();
            EventManager.Instance.RegisterListener<GamePadConnectedEvent>(this);
            EventManager.Instance.RegisterListener<GamePadDisconnectedEvent>(this);
        }

        public override void LoadContent()
        {
            _lobbyFont = Content.Load<BitmapFont>(Constants.Resources.FONT_LOBBY);
            _mainFont = Content.Load<BitmapFont>(Constants.Resources.FONT_MENU_BUTTON);

            _panelBackground = new NinePatchRegion2D(new TextureRegion2D(Content.Load<Texture2D>(Constants.Resources.TEXTURE_LOBBY_PANEL_BACKGROUND)),
                                                     4, 4, 4, 4);
            _buttonReleased = new NinePatchRegion2D(new TextureRegion2D(Content.Load<Texture2D>(Constants.Resources.TEXTURE_BUTTON_RELEASED)),
                                                    4, 4, 4, 4);
            _buttonHover = new NinePatchRegion2D(new TextureRegion2D(Content.Load<Texture2D>(Constants.Resources.TEXTURE_BUTTON_HOVER)),
                                                    4, 4, 4, 4);
            _buttonPressed = new NinePatchRegion2D(new TextureRegion2D(Content.Load<Texture2D>(Constants.Resources.TEXTURE_BUTTON_PRESSED)),
                                                    4, 4, 4, 4);

            for (int i = 0; i < Constants.Resources.TEXTURES_KEYBOARD_ICON.Length; ++i)
            {
                if (Constants.Resources.TEXTURES_KEYBOARD_ICON[i].Key
                   == Settings.Instance.Data.PrimaryKey1)
                {
                    _primaryKey1 = Content.Load<Texture2D>(Constants.Resources.TEXTURES_KEYBOARD_ICON[i].Resource);
                }
                if (Constants.Resources.TEXTURES_KEYBOARD_ICON[i].Key
                   == Settings.Instance.Data.PrimaryKey2)
                {
                    _primaryKey2 = Content.Load<Texture2D>(Constants.Resources.TEXTURES_KEYBOARD_ICON[i].Resource);
                }
                if (Constants.Resources.TEXTURES_KEYBOARD_ICON[i].Key
                    == Settings.Instance.Data.SecondaryKey1)
                {
                    _secondaryKey1 = Content.Load<Texture2D>(Constants.Resources.TEXTURES_KEYBOARD_ICON[i].Resource);
                }
                if (Constants.Resources.TEXTURES_KEYBOARD_ICON[i].Key
                    == Settings.Instance.Data.SecondaryKey2)
                {
                    _secondaryKey2 = Content.Load<Texture2D>(Constants.Resources.TEXTURES_KEYBOARD_ICON[i].Resource);
                }
            }

            _controllerA = Content.Load<Texture2D>(Constants.Resources.TEXTURE_CONTROLLER_BUTTON_ICON_A);
            _controllerStick = Content.Load<Texture2D>(Constants.Resources.TEXTURE_CONTROLLER_STICK_LEFT);
        }

        public override void Show()
        {
            BuildUI();
        }

        void BuildUI()
        {
            BuildPlayer1Panel();
            BuildPlayer2Panel();

            _backButton = new Button(_buttonReleased,
                                     _buttonHover,
                                     _buttonPressed,
                                     Origin.BottomLeft,
                                     Constants.Lobby.BACK_BUTTON_PERCENT_X,
                                    0,
                                     Constants.Lobby.BACK_BUTTON_PERCENT_Y,
                                    0,
                                     Constants.Lobby.BACK_BUTTON_ASPECT_PERCENT,
                                    0,
                                     Constants.Lobby.BACK_BUTTON_ASPECT_RATIO,
                                     Constants.Lobby.BACK_BUTTON_ASPECT_TYPE);
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
                                        Constants.Lobby.BACK_BUTTON_LABEL_ASPECT_PERCENT,
                                        0,
                                        Constants.Lobby.BACK_BUTTON_LABEL_ASPECT_TYPE);
            backLabel.Content = Constants.Lobby.BACK_BUTTON_LABEL;
            _backButton.SubPanel.Add(backLabel);

            _startButton = new Button(_buttonReleased,
                                     _buttonHover,
                                     _buttonPressed,
                                      Origin.BottomRight,
                                     Constants.Lobby.START_BUTTON_PERCENT_X,
                                    0,
                                     Constants.Lobby.START_BUTTON_PERCENT_Y,
                                    0,
                                     Constants.Lobby.START_BUTTON_ASPECT_PERCENT,
                                    0,
                                     Constants.Lobby.START_BUTTON_ASPECT_RATIO,
                                     Constants.Lobby.START_BUTTON_ASPECT_TYPE);
            _startButton.Action = () =>
            {
                StartGame();
            };
            _startButton.Hidden = true;
            _root.Add(_startButton);
            Label startLabel = new Label(_mainFont,
                                        Origin.Center,
                                        0,
                                        0,
                                        0,
                                        0,
                                        Constants.Lobby.START_BUTTON_LABEL_ASPECT_PERCENT,
                                        0,
                                        Constants.Lobby.START_BUTTON_LABEL_ASPECT_TYPE);
            startLabel.Content = Constants.Lobby.START_BUTTON_LABEL;
            _startButton.SubPanel.Add(startLabel);
        }

        void BuildPlayer1Panel()
        {
            _player1Panel = new Panel(Origin.Center,
                                      Constants.Lobby.PANEL_LEFT_X_PERCENT,
                                     0,
                                      Constants.Lobby.PANEL_Y_PERCENT,
                                     0,
                                      Constants.Lobby.PANEL_WIDTH_PERCENT,
                                     0,
                                      Constants.Lobby.PANEL_HEIGHT_PERCENT,
                                      (float)0);
            _root.Add(_player1Panel);

            NinePatchImage background = new NinePatchImage(_panelBackground,
                                                           Origin.Center,
                                                          0,
                                                          0,
                                                          0,
                                                          0,
                                                          1,
                                                           0,
                                                          1,
                                                           (float)0);
            _player1Panel.Add(background);

            Label playerLabel = new Label(_lobbyFont,
                                          Origin.Center,
                                         0,
                                         0,
                                          Constants.Lobby.PLAYER_LABEL_Y_PERCENT,
                                         0,
                                          Constants.Lobby.PLAYER_LABEL_1_ASPECT_PERCENT,
                                         0,
                                          Constants.Lobby.PLAYER_LABEL_ASPECT_TYPE);
            playerLabel.Content = Constants.Lobby.PLAYER_LABEL_1;
            _player1Panel.Add(playerLabel);

            BuildPlayer1InputPanel();
            BuildPlayer1JoinedPanel();
        }

        void BuildPlayer1InputPanel()
        {
            _player1InputPanel = new Panel(Origin.Center,
                                      0,
                                     0,
                                     0,
                                     0,
                                      1,
                                     0,
                                      1,
                                     (float)0);
            _player1Panel.Add(_player1InputPanel);

            _player1JoinLabel = new Label(_lobbyFont,
                                          Origin.Center,
                                         0,
                                         0,
                                         0,
                                         0,
                                          Constants.Lobby.JOIN_LABEL_ASPECT_PERCENT,
                                         0,
                                          Constants.Lobby.JOIN_LABEL_ASPECT_TYPE);
            _player1JoinLabel.Content = Constants.Lobby.JOIN_LABEL;
            _player1InputPanel.Add(_player1JoinLabel);

            _player1PrimaryKey1 = new Image(_primaryKey1,
                                                  Origin.Center,
                                                  Constants.Lobby.KEY_PRIMARY1_X_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_PRIMARY1_Y_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_RATIO,
                                                  Constants.Lobby.KEY_ASPECT_TYPE);
            _player1InputPanel.Add(_player1PrimaryKey1);

            _player1PrimaryKey2 = new Image(_primaryKey2,
                                                  Origin.Center,
                                                  Constants.Lobby.KEY_PRIMARY2_X_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_PRIMARY2_Y_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_RATIO,
                                                  Constants.Lobby.KEY_ASPECT_TYPE);
            _player1InputPanel.Add(_player1PrimaryKey2);

            _player1SecondaryKey1 = new Image(_secondaryKey1,
                                              Origin.Center,
                                              Constants.Lobby.KEY_SECONDARY1_X_PERCENT,
                                             0,
                                              Constants.Lobby.KEY_SECONDARY1_Y_PERCENT,
                                             0,
                                              Constants.Lobby.KEY_ASPECT_PERCENT,
                                             0,
                                              Constants.Lobby.KEY_ASPECT_RATIO,
                                              Constants.Lobby.KEY_ASPECT_TYPE);
            _player1InputPanel.Add(_player1SecondaryKey1);

            _player1SecondaryKey2 = new Image(_secondaryKey2,
                                              Origin.Center,
                                              Constants.Lobby.KEY_SECONDARY2_X_PERCENT,
                                             0,
                                              Constants.Lobby.KEY_SECONDARY2_Y_PERCENT,
                                             0,
                                              Constants.Lobby.KEY_ASPECT_PERCENT,
                                             0,
                                              Constants.Lobby.KEY_ASPECT_RATIO,
                                              Constants.Lobby.KEY_ASPECT_TYPE);
            _player1InputPanel.Add(_player1SecondaryKey2);

            _player1ControllerA = new Image(_controllerA,
                                            Origin.Center,
                                            Constants.Lobby.KEY_CONTROLLER_A_X_PERCENT,
                                           0,
                                            Constants.Lobby.KEY_CONTROLLER_A_Y_PERCENT,
                                           0,
                                            Constants.Lobby.KEY_ASPECT_PERCENT,
                                           0,
                                            Constants.Lobby.KEY_ASPECT_RATIO,
                                            Constants.Lobby.KEY_ASPECT_TYPE);
            _player1InputPanel.Add(_player1ControllerA);
        }

        void BuildPlayer1JoinedPanel()
        {
            _player1JoinedPanel = new Panel(Origin.Center,
                                           0,
                                            0,
                                           0,
                                           0,
                                           1,
                                           0,
                                           1,
                                            (float)0);
            _player1JoinedPanel.Hidden = true;
            _player1Panel.Add(_player1JoinedPanel);

            Label movementInstructions = new Label(_lobbyFont,
                                                   Origin.Center,
                                                  0,
                                                  0,
                                                  0,
                                                   0,
                                                   Constants.Lobby.JOINED_LABEL_ASPECT_PERCENT,
                                                  0,
                                                   Constants.Lobby.JOINED_LABEL_ASPECT_TYPE);
            movementInstructions.Content = Constants.Lobby.JOINED_LABEL_MOVEMENT_INSTRUCTIONS;
            _player1JoinedPanel.Add(movementInstructions);

            Button leaveButton = new Button(_buttonReleased,
                                            _buttonHover,
                                            _buttonPressed,
                                            Origin.TopLeft,
                                            Constants.Lobby.JOINED_BUTTON_PERCENT_X,
                                           0,
                                           Constants.Lobby.JOINED_BUTTON_PERCENT_Y,
                                           0,
                                            Constants.Lobby.JOINED_BUTTON_ASPECT_PERCENT,
                                           0,
                                            Constants.Lobby.JOINED_BUTTON_ASPECT_RATIO,
                                            Constants.Lobby.JOINED_BUTTON_ASPECT_TYPE);
            leaveButton.Action = () =>
            {
                _player1InputMethod = null;
            };
            _player1JoinedPanel.Add(leaveButton);
            Label leaveLabel = new Label(_lobbyFont,
                                         Origin.Center,
                                        0,
                                        -0.5f,
                                        0,
                                        0,
                                         Constants.Lobby.JOINED_BUTTON_LABEL_ASPECT_PERCENT,
                                        0,
                                         AspectRatioType.HeightMaster);
            leaveLabel.Content = Constants.Lobby.JOINED_BUTTON_LABEL;
            leaveButton.SubPanel.Add(leaveLabel);

            _player1JoinedPrimaryKey1 = new Image(_primaryKey1,
                                                  Origin.Center,
                                                  Constants.Lobby.KEY_JOINED_1_X_PERCENT,
                                                 0,
                                                 Constants.Lobby.KEY_JOINED_1_Y_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_RATIO,
                                                  Constants.Lobby.KEY_ASPECT_TYPE);
            _player1JoinedPanel.Add(_player1JoinedPrimaryKey1);

            _player1JoinedPrimaryKey2 = new Image(_primaryKey2,
                                                  Origin.Center,
                                                 Constants.Lobby.KEY_JOINED_2_X_PERCENT,
                                                 0,
                                                 Constants.Lobby.KEY_JOINED_2_Y_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_RATIO,
                                                  Constants.Lobby.KEY_ASPECT_TYPE);
            _player1JoinedPanel.Add(_player1JoinedPrimaryKey2);

            _player1JoinedSecondaryKey1 = new Image(_secondaryKey1,
                                                  Origin.Center,
                                                  Constants.Lobby.KEY_JOINED_1_X_PERCENT,
                                                 0,
                                                 Constants.Lobby.KEY_JOINED_1_Y_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_RATIO,
                                                  Constants.Lobby.KEY_ASPECT_TYPE);
            _player1JoinedPanel.Add(_player1JoinedSecondaryKey1);

            _player1JoinedSecondaryKey2 = new Image(_secondaryKey2,
                                                  Origin.Center,
                                                 Constants.Lobby.KEY_JOINED_2_X_PERCENT,
                                                 0,
                                                 Constants.Lobby.KEY_JOINED_2_Y_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_RATIO,
                                                  Constants.Lobby.KEY_ASPECT_TYPE);
            _player1JoinedPanel.Add(_player1JoinedSecondaryKey2);

            _player1JoinedControllerStick = new Image(_controllerStick,
                                                      Origin.Center,
                                                      Constants.Lobby.KEY_JOINED_CONTROLLER_X_PERCENT,
                                                      0,
                                                      Constants.Lobby.KEY_JOINED_CONTROLLER_Y_PERCENT,
                                                      0,
                                                      Constants.Lobby.KEY_ASPECT_PERCENT,
                                                      0,
                                                      Constants.Lobby.KEY_ASPECT_RATIO,
                                                      Constants.Lobby.KEY_ASPECT_TYPE);
            _player1JoinedPanel.Add(_player1JoinedControllerStick);
        }

        void BuildPlayer2Panel()
        {
            _player2Panel = new Panel(Origin.Center,
                                      Constants.Lobby.PANEL_RIGHT_X_PERCENT,
                                     0,
                                     Constants.Lobby.PANEL_Y_PERCENT,
                                     0,
                                      Constants.Lobby.PANEL_WIDTH_PERCENT,
                                     0,
                                      Constants.Lobby.PANEL_HEIGHT_PERCENT,
                                      (float)0);
            _root.Add(_player2Panel);

            NinePatchImage background = new NinePatchImage(_panelBackground,
                                                           Origin.Center,
                                                          0,
                                                          0,
                                                          0,
                                                          0,
                                                          1,
                                                           0,
                                                          1,
                                                           (float)0);
            _player2Panel.Add(background);

            _player2Label = new Label(_lobbyFont,
                                          Origin.Center,
                                         0,
                                         0,
                                          Constants.Lobby.PLAYER_LABEL_Y_PERCENT,
                                         0,
                                          Constants.Lobby.PLAYER_LABEL_2_ASPECT_PERCENT,
                                         0,
                                          Constants.Lobby.PLAYER_LABEL_ASPECT_TYPE);
            _player2Label.Content = Constants.Lobby.PLAYER_LABEL_2;
            _player2Panel.Add(_player2Label);

            BuildPlayer2InputPanel();
            BuildPlayer2JoinedPanel();
        }

        void BuildPlayer2InputPanel()
        {
            _player2InputPanel = new Panel(Origin.Center,
                                      0,
                                     0,
                                     0,
                                     0,
                                      1,
                                     0,
                                      1,
                                     (float)0);
            _player2Panel.Add(_player2InputPanel);

            _player2JoinLabel = new Label(_lobbyFont,
                                          Origin.Center,
                                         0,
                                         0,
                                         0,
                                         0,
                                          Constants.Lobby.JOIN_LABEL_ASPECT_PERCENT,
                                         0,
                                          Constants.Lobby.JOIN_LABEL_ASPECT_TYPE);
            _player2JoinLabel.Content = Constants.Lobby.JOIN_LABEL;
            _player2InputPanel.Add(_player2JoinLabel);

            _player2PrimaryKey1 = new Image(_primaryKey1,
                                                  Origin.Center,
                                                  Constants.Lobby.KEY_PRIMARY1_X_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_PRIMARY1_Y_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_RATIO,
                                                  Constants.Lobby.KEY_ASPECT_TYPE);
            _player2InputPanel.Add(_player2PrimaryKey1);

            _player2PrimaryKey2 = new Image(_primaryKey2,
                                                  Origin.Center,
                                                  Constants.Lobby.KEY_PRIMARY2_X_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_PRIMARY2_Y_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_RATIO,
                                                  Constants.Lobby.KEY_ASPECT_TYPE);
            _player2InputPanel.Add(_player2PrimaryKey2);

            _player2SecondaryKey1 = new Image(_secondaryKey1,
                                              Origin.Center,
                                              Constants.Lobby.KEY_SECONDARY1_X_PERCENT,
                                             0,
                                              Constants.Lobby.KEY_SECONDARY1_Y_PERCENT,
                                             0,
                                              Constants.Lobby.KEY_ASPECT_PERCENT,
                                             0,
                                              Constants.Lobby.KEY_ASPECT_RATIO,
                                              Constants.Lobby.KEY_ASPECT_TYPE);
            _player2InputPanel.Add(_player2SecondaryKey1);

            _player2SecondaryKey2 = new Image(_secondaryKey2,
                                              Origin.Center,
                                              Constants.Lobby.KEY_SECONDARY2_X_PERCENT,
                                             0,
                                              Constants.Lobby.KEY_SECONDARY2_Y_PERCENT,
                                             0,
                                              Constants.Lobby.KEY_ASPECT_PERCENT,
                                             0,
                                              Constants.Lobby.KEY_ASPECT_RATIO,
                                              Constants.Lobby.KEY_ASPECT_TYPE);
            _player2InputPanel.Add(_player2SecondaryKey2);

            _player2ControllerA = new Image(_controllerA,
                                            Origin.Center,
                                            Constants.Lobby.KEY_CONTROLLER_A_X_PERCENT,
                                           0,
                                            Constants.Lobby.KEY_CONTROLLER_A_Y_PERCENT,
                                           0,
                                            Constants.Lobby.KEY_ASPECT_PERCENT,
                                           0,
                                            Constants.Lobby.KEY_ASPECT_RATIO,
                                            Constants.Lobby.KEY_ASPECT_TYPE);
            _player2InputPanel.Add(_player2ControllerA);
        }

        void BuildPlayer2JoinedPanel()
        {
            _player2JoinedPanel = new Panel(Origin.Center,
                                           0,
                                            0,
                                           0,
                                           0,
                                           1,
                                           0,
                                           1,
                                            (float)0);
            _player2JoinedPanel.Hidden = true;
            _player2Panel.Add(_player2JoinedPanel);

            Label movementInstructions = new Label(_lobbyFont,
                                                   Origin.Center,
                                                  0,
                                                  0,
                                                  0,
                                                   0,
                                                   Constants.Lobby.JOINED_LABEL_ASPECT_PERCENT,
                                                  0,
                                                   Constants.Lobby.JOINED_LABEL_ASPECT_TYPE);
            movementInstructions.Content = Constants.Lobby.JOINED_LABEL_MOVEMENT_INSTRUCTIONS;
            _player2JoinedPanel.Add(movementInstructions);

            Button leaveButton = new Button(_buttonReleased,
                                            _buttonHover,
                                            _buttonPressed,
                                            Origin.TopLeft,
                                            Constants.Lobby.JOINED_BUTTON_PERCENT_X,
                                           0,
                                           Constants.Lobby.JOINED_BUTTON_PERCENT_Y,
                                           0,
                                            Constants.Lobby.JOINED_BUTTON_ASPECT_PERCENT,
                                           0,
                                            Constants.Lobby.JOINED_BUTTON_ASPECT_RATIO,
                                            Constants.Lobby.JOINED_BUTTON_ASPECT_TYPE);
            leaveButton.Action = () =>
            {
                _player2InputMethod = null;
            };
            _player2JoinedPanel.Add(leaveButton);
            Label leaveLabel = new Label(_lobbyFont,
                                         Origin.Center,
                                        0,
                                        -0.5f,
                                        0,
                                        0,
                                         Constants.Lobby.JOINED_BUTTON_LABEL_ASPECT_PERCENT,
                                        0,
                                         AspectRatioType.HeightMaster);
            leaveLabel.Content = Constants.Lobby.JOINED_BUTTON_LABEL;
            leaveButton.SubPanel.Add(leaveLabel);

            _player2JoinedPrimaryKey1 = new Image(_primaryKey1,
                                                  Origin.Center,
                                                  Constants.Lobby.KEY_JOINED_1_X_PERCENT,
                                                 0,
                                                 Constants.Lobby.KEY_JOINED_1_Y_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_RATIO,
                                                  Constants.Lobby.KEY_ASPECT_TYPE);
            _player2JoinedPanel.Add(_player2JoinedPrimaryKey1);

            _player2JoinedPrimaryKey2 = new Image(_primaryKey2,
                                                  Origin.Center,
                                                 Constants.Lobby.KEY_JOINED_2_X_PERCENT,
                                                 0,
                                                 Constants.Lobby.KEY_JOINED_2_Y_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_RATIO,
                                                  Constants.Lobby.KEY_ASPECT_TYPE);
            _player2JoinedPanel.Add(_player2JoinedPrimaryKey2);

            _player2JoinedSecondaryKey1 = new Image(_secondaryKey1,
                                                  Origin.Center,
                                                  Constants.Lobby.KEY_JOINED_1_X_PERCENT,
                                                 0,
                                                 Constants.Lobby.KEY_JOINED_1_Y_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_RATIO,
                                                  Constants.Lobby.KEY_ASPECT_TYPE);
            _player2JoinedPanel.Add(_player2JoinedSecondaryKey1);

            _player2JoinedSecondaryKey2 = new Image(_secondaryKey2,
                                                  Origin.Center,
                                                 Constants.Lobby.KEY_JOINED_2_X_PERCENT,
                                                 0,
                                                 Constants.Lobby.KEY_JOINED_2_Y_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_PERCENT,
                                                 0,
                                                  Constants.Lobby.KEY_ASPECT_RATIO,
                                                  Constants.Lobby.KEY_ASPECT_TYPE);
            _player2JoinedPanel.Add(_player2JoinedSecondaryKey2);

            _player2JoinedControllerStick = new Image(_controllerStick,
                                                      Origin.Center,
                                                      Constants.Lobby.KEY_JOINED_CONTROLLER_X_PERCENT,
                                                      0,
                                                      Constants.Lobby.KEY_JOINED_CONTROLLER_Y_PERCENT,
                                                      0,
                                                      Constants.Lobby.KEY_ASPECT_PERCENT,
                                                      0,
                                                      Constants.Lobby.KEY_ASPECT_RATIO,
                                                      Constants.Lobby.KEY_ASPECT_TYPE);
            _player2JoinedPanel.Add(_player2JoinedControllerStick);
        }

        public override void Update(float dt)
        {
            UpdateInputMethods(dt);
            CheckForPlayerJoin();
            CheckForPlayerLeave();

            UpdateStartButton();
            CheckStartGame();

            // Update input/joined panels
            UpdateInputPanels();
            UpdateJoinedPanels();

            UpdatePlayerLabels();
        }

        void UpdateInputMethods(float dt)
        {
            for (int i = 0; i < _inputMethods.Count; ++i)
            {
                _inputMethods[i].Update(dt);
            }
        }

        void CheckForPlayerJoin()
        {
            if (_player1InputMethod == null)
            {
                for (int i = 0; i < _inputMethods.Count; ++i)
                {
                    if (_inputMethods[i] == _player2InputMethod)
                    {
                        continue;
                    }

                    if (_inputMethods[i].JoinKeyPressed)
                    {
                        _player1InputMethod = _inputMethods[i];
                        break;
                    }
                }
            }
            if (_player2InputMethod == null)
            {
                for (int i = 0; i < _inputMethods.Count; ++i)
                {
                    if (_inputMethods[i] == _player1InputMethod)
                    {
                        continue;
                    }

                    if (_inputMethods[i].JoinKeyPressed)
                    {
                        _player2InputMethod = _inputMethods[i];
                        break;
                    }
                }
            }
        }

        void CheckForPlayerLeave()
        {
            if (_player1InputMethod != null)
            {
                if (_player1InputMethod.LeaveKeyPressed)
                {
                    _player1InputMethod = null;
                }
            }
            if (_player2InputMethod != null)
            {
                if (_player2InputMethod.LeaveKeyPressed)
                {
                    _player2InputMethod = null;
                }
            }
        }

        void UpdateStartButton()
        {
            _startButton.Hidden = _player1InputMethod == null;
        }

        void CheckStartGame()
        {
            if (_player1InputMethod != null)
            {
                if (_player1InputMethod.StartKeyPressed)
                {
                    StartGame();
                }
            }
            if (_player2InputMethod != null)
            {
                if (_player2InputMethod.StartKeyPressed)
                {
                    StartGame();
                }
            }
        }

        void UpdateInputPanels()
        {
            _player1InputPanel.Hidden = _player1InputMethod != null;
            _player2InputPanel.Hidden = _player2InputMethod != null;

            // Player 1 Panel
            if (_player2InputMethod == null)
            {
                // All input methods shown
                _player1PrimaryKey1.PercentX = Constants.Lobby.KEY_PRIMARY1_X_PERCENT;
                _player1PrimaryKey1.PercentY = Constants.Lobby.KEY_PRIMARY1_Y_PERCENT;
                _player1PrimaryKey1.Hidden = false;
                _player1PrimaryKey1.ComputeProperties();
                _player1PrimaryKey2.PercentX = Constants.Lobby.KEY_PRIMARY2_X_PERCENT;
                _player1PrimaryKey2.PercentY = Constants.Lobby.KEY_PRIMARY2_Y_PERCENT;
                _player1PrimaryKey2.Hidden = false;
                _player1PrimaryKey2.ComputeProperties();
                _player1SecondaryKey1.PercentX = Constants.Lobby.KEY_SECONDARY1_X_PERCENT;
                _player1SecondaryKey1.PercentY = Constants.Lobby.KEY_SECONDARY1_Y_PERCENT;
                _player1SecondaryKey1.Hidden = false;
                _player1SecondaryKey1.ComputeProperties();
                _player1SecondaryKey2.PercentX = Constants.Lobby.KEY_SECONDARY2_X_PERCENT;
                _player1SecondaryKey2.PercentY = Constants.Lobby.KEY_SECONDARY2_Y_PERCENT;
                _player1SecondaryKey2.Hidden = false;
                _player1SecondaryKey2.ComputeProperties();
                _player1ControllerA.PercentX = Constants.Lobby.KEY_CONTROLLER_A_X_PERCENT;
                _player1ControllerA.PercentY = Constants.Lobby.KEY_CONTROLLER_A_Y_PERCENT;
                _player1ControllerA.ComputeProperties();
            }
            else
            {
                _player1PrimaryKey1.PercentX = Constants.Lobby.KEY_1_TWO_METHODS_X_PERCENT;
                _player1PrimaryKey1.PercentY = Constants.Lobby.KEY_1_TWO_METHODS_Y_PERCENT;
                _player1PrimaryKey1.Hidden = _player1InputMethod is PrimaryKeyboardInputMethod; // Depends on chosen input method
                _player1PrimaryKey1.ComputeProperties();
                _player1PrimaryKey2.PercentX = Constants.Lobby.KEY_2_TWO_METHODS_X_PERCENT;
                _player1PrimaryKey2.PercentY = Constants.Lobby.KEY_2_TWO_METHODS_Y_PERCENT;
                _player1PrimaryKey2.Hidden = _player1InputMethod is PrimaryKeyboardInputMethod; // Depends on chosen input method
                _player1PrimaryKey2.ComputeProperties();
                _player1SecondaryKey1.PercentX = Constants.Lobby.KEY_1_TWO_METHODS_X_PERCENT;
                _player1SecondaryKey1.PercentY = Constants.Lobby.KEY_1_TWO_METHODS_Y_PERCENT;
                _player1SecondaryKey1.Hidden = _player1InputMethod is SecondaryKeyboardInputMethod; // Depends on chosen input method
                _player1SecondaryKey1.ComputeProperties();
                _player1SecondaryKey2.PercentX = Constants.Lobby.KEY_2_TWO_METHODS_X_PERCENT;
                _player1SecondaryKey2.PercentY = Constants.Lobby.KEY_2_TWO_METHODS_Y_PERCENT;
                _player1SecondaryKey2.Hidden = _player1InputMethod is SecondaryKeyboardInputMethod; // Depends on chosen input method
                _player1SecondaryKey2.ComputeProperties();
                _player1ControllerA.PercentX = Constants.Lobby.KEY_CONTROLLER_A_TWO_METHODS_X_PERCENT;
                _player1ControllerA.PercentY = Constants.Lobby.KEY_CONTROLLER_A_TWO_METHODS_Y_PERCENT;
                _player1ControllerA.ComputeProperties();
            }

            // Player 2 Panel
            if (_player1InputMethod == null)
            {
                // All input methods shown
                _player2PrimaryKey1.PercentX = Constants.Lobby.KEY_PRIMARY1_X_PERCENT;
                _player2PrimaryKey1.PercentY = Constants.Lobby.KEY_PRIMARY1_Y_PERCENT;
                _player2PrimaryKey1.Hidden = false;
                _player2PrimaryKey1.ComputeProperties();
                _player2PrimaryKey2.PercentX = Constants.Lobby.KEY_PRIMARY2_X_PERCENT;
                _player2PrimaryKey2.PercentY = Constants.Lobby.KEY_PRIMARY2_Y_PERCENT;
                _player2PrimaryKey2.Hidden = false;
                _player2PrimaryKey2.ComputeProperties();
                _player2SecondaryKey1.PercentX = Constants.Lobby.KEY_SECONDARY1_X_PERCENT;
                _player2SecondaryKey1.PercentY = Constants.Lobby.KEY_SECONDARY1_Y_PERCENT;
                _player2SecondaryKey1.Hidden = false;
                _player2SecondaryKey1.ComputeProperties();
                _player2SecondaryKey2.PercentX = Constants.Lobby.KEY_SECONDARY2_X_PERCENT;
                _player2SecondaryKey2.PercentY = Constants.Lobby.KEY_SECONDARY2_Y_PERCENT;
                _player2SecondaryKey2.Hidden = false;
                _player2SecondaryKey2.ComputeProperties();
                _player2ControllerA.PercentX = Constants.Lobby.KEY_CONTROLLER_A_X_PERCENT;
                _player2ControllerA.PercentY = Constants.Lobby.KEY_CONTROLLER_A_Y_PERCENT;
                _player2ControllerA.ComputeProperties();
            }
            else
            {
                _player2PrimaryKey1.PercentX = Constants.Lobby.KEY_1_TWO_METHODS_X_PERCENT;
                _player2PrimaryKey1.PercentY = Constants.Lobby.KEY_1_TWO_METHODS_Y_PERCENT;
                _player2PrimaryKey1.Hidden = _player1InputMethod is PrimaryKeyboardInputMethod; // Depends on chosen input method
                _player2PrimaryKey1.ComputeProperties();
                _player2PrimaryKey2.PercentX = Constants.Lobby.KEY_2_TWO_METHODS_X_PERCENT;
                _player2PrimaryKey2.PercentY = Constants.Lobby.KEY_2_TWO_METHODS_Y_PERCENT;
                _player2PrimaryKey2.Hidden = _player1InputMethod is PrimaryKeyboardInputMethod; // Depends on chosen input method
                _player2PrimaryKey2.ComputeProperties();
                _player2SecondaryKey1.PercentX = Constants.Lobby.KEY_1_TWO_METHODS_X_PERCENT;
                _player2SecondaryKey1.PercentY = Constants.Lobby.KEY_1_TWO_METHODS_Y_PERCENT;
                _player2SecondaryKey1.Hidden = _player1InputMethod is SecondaryKeyboardInputMethod; // Depends on chosen input method
                _player2SecondaryKey1.ComputeProperties();
                _player2SecondaryKey2.PercentX = Constants.Lobby.KEY_2_TWO_METHODS_X_PERCENT;
                _player2SecondaryKey2.PercentY = Constants.Lobby.KEY_2_TWO_METHODS_Y_PERCENT;
                _player2SecondaryKey2.Hidden = _player1InputMethod is SecondaryKeyboardInputMethod; // Depends on chosen input method
                _player2SecondaryKey2.ComputeProperties();
                _player2ControllerA.PercentX = Constants.Lobby.KEY_CONTROLLER_A_TWO_METHODS_X_PERCENT;
                _player2ControllerA.PercentY = Constants.Lobby.KEY_CONTROLLER_A_TWO_METHODS_Y_PERCENT;
                _player2ControllerA.ComputeProperties();
            }
        }

        void UpdateJoinedPanels()
        {
            _player1JoinedPanel.Hidden = _player1InputMethod == null;
            _player2JoinedPanel.Hidden = _player2InputMethod == null;

            // Player 1
            if (_player1InputMethod != null)
            {
                _player1JoinedPrimaryKey1.Hidden = !(_player1InputMethod is PrimaryKeyboardInputMethod);
                _player1JoinedPrimaryKey2.Hidden = !(_player1InputMethod is PrimaryKeyboardInputMethod);

                _player1JoinedSecondaryKey1.Hidden = !(_player1InputMethod is SecondaryKeyboardInputMethod);
                _player1JoinedSecondaryKey2.Hidden = !(_player1InputMethod is SecondaryKeyboardInputMethod);

                _player1JoinedControllerStick.Hidden = !(_player1InputMethod is ControllerInputMethod);
            }

            // Player 2
            if (_player2InputMethod != null)
            {
                _player2JoinedPrimaryKey1.Hidden = !(_player2InputMethod is PrimaryKeyboardInputMethod);
                _player2JoinedPrimaryKey2.Hidden = !(_player2InputMethod is PrimaryKeyboardInputMethod);

                _player2JoinedSecondaryKey1.Hidden = !(_player2InputMethod is SecondaryKeyboardInputMethod);
                _player2JoinedSecondaryKey2.Hidden = !(_player2InputMethod is SecondaryKeyboardInputMethod);

                _player2JoinedControllerStick.Hidden = !(_player2InputMethod is ControllerInputMethod);
            }
        }

        void UpdatePlayerLabels()
        {
            _player2Label.PercentWidth = (_player2InputMethod == null) ? Constants.Lobby.PLAYER_LABEL_2_ASPECT_PERCENT : Constants.Lobby.PLAYER_LABEL_2_ASPECT_PERCENT_ALT;
            _player2Label.PercentHeight = (_player2InputMethod == null) ? Constants.Lobby.PLAYER_LABEL_2_ASPECT_PERCENT : Constants.Lobby.PLAYER_LABEL_2_ASPECT_PERCENT_ALT;
            _player2Label.Content = (_player2InputMethod == null) ? Constants.Lobby.PLAYER_LABEL_2 : Constants.Lobby.PLAYER_LABEL_2_ALT;
        }

        public override void Draw(float dt)
        {
            _spriteBatch.Begin();
            _root.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        void StartGame()
        {
            if (_player1InputMethod == null)
            {
                // Can't start game without player1
                return;
            }

            Player player1;
            Player player2;

            if (_player1InputMethod == null)
            {
                player1 = new AIPlayer(0, "Computer");
            }
            else
            {
                player1 = new Player(0, "Player 1", _player1InputMethod);
            }

            if (_player2InputMethod == null)
            {
                player2 = new AIPlayer(1, "Computer");
            }
            else
            {
                player2 = new Player(1, "Player 2", _player2InputMethod);
            }

            // Change to game state
            GameManager.ChangeState(new MainGameState(GameManager,
                                                     player1,
                                                     player2));
        }

        public override void Hide()
        {

        }

        public override void Dispose()
        {
            _root.UnregisterListeners();
            EventManager.Instance.UnregisterListener(this);
        }

        public bool Handle(IEvent evt)
        {
            GamePadConnectedEvent connectedEvent = evt as GamePadConnectedEvent;
            if (connectedEvent != null)
            {
                bool found = false;

                for (int i = 0; i < _inputMethods.Count; ++i)
                {
                    if (_inputMethods[i] is ControllerInputMethod)
                    {
                        ControllerInputMethod controllerInputMethod = (ControllerInputMethod)_inputMethods[i];

                        if (controllerInputMethod.PlayerIndex
                           == connectedEvent.PlayerIndex)
                        {
                            found = true;
                            break;
                        }
                    }
                }

                if (!found)
                {
                    _inputMethods.Add(new ControllerInputMethod(connectedEvent.PlayerIndex));
                }
            }

            GamePadDisconnectedEvent disconnectedEvent = evt as GamePadDisconnectedEvent;
            if (disconnectedEvent != null)
            {
                for (int i = 0; i < _inputMethods.Count; ++i)
                {
                    if (_inputMethods[i] is ControllerInputMethod)
                    {
                        ControllerInputMethod controllerInputMethod = (ControllerInputMethod)_inputMethods[i];

                        if (controllerInputMethod.PlayerIndex
                           == disconnectedEvent.PlayerIndex)
                        {
                            _inputMethods.Remove(controllerInputMethod);

                            if (_player1InputMethod == controllerInputMethod)
                            {
                                _player1InputMethod = null;
                            }
                            if (_player2InputMethod == controllerInputMethod)
                            {
                                _player2InputMethod = null;
                            }

                            break;
                        }
                    }
                }
            }

            return false;
        }
    }
}
