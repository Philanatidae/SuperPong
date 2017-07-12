using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;
using SuperPong.UI;
using SuperPong.UI.Widgets;

namespace SuperPong.States
{
    public class MenuGameState : GameState
    {
        SpriteBatch _spriteBatch;

        NinePatchRegion2D _buttonReleased;
        NinePatchRegion2D _buttonHover;
        NinePatchRegion2D _buttonPressed;

        Root _root;

        readonly float _logoAspectRatio = 1.51878787879f;
        readonly float _creditsAspectRatio = 4.22093023256f;

        Image _logo;
        Image _credits;
        Button _playButton;
        Button _optionsButton;
        Button _exitButton;

        public MenuGameState(GameManager gameManager) : base(gameManager)
        {

            _spriteBatch = new SpriteBatch(gameManager.GraphicsDevice);

            _root = new Root(gameManager.GraphicsDevice.Viewport.Width,
                             gameManager.GraphicsDevice.Viewport.Height);
        }

        public override void Initialize()
        {
            _root.RegisterListeners();
        }

        public override void LoadContent()
        {
            Content.Load<Texture2D>(Constants.Resources.TEXTURE_LOGO);
            Content.Load<Texture2D>(Constants.Resources.TEXTURE_CREDITS_AND_VERSION);
            Content.Load<BitmapFont>(Constants.Resources.FONT_MENU_BUTTON);

            _buttonReleased = new NinePatchRegion2D(new TextureRegion2D(Content.Load<Texture2D>(Constants.Resources.TEXTURE_BUTTON_RELEASED)),
                                                    4, 4, 4, 4);
            _buttonHover = new NinePatchRegion2D(new TextureRegion2D(Content.Load<Texture2D>(Constants.Resources.TEXTURE_BUTTON_HOVER)),
                                                    4, 4, 4, 4);
            _buttonPressed = new NinePatchRegion2D(new TextureRegion2D(Content.Load<Texture2D>(Constants.Resources.TEXTURE_BUTTON_PRESSED)),
                                                    4, 4, 4, 4);
        }

        public override void Show()
        {
            BuildUI();
        }

        void BuildUI()
        {
            _logo = new Image(Content.Load<Texture2D>(Constants.Resources.TEXTURE_LOGO),
                              Origin.Center,
                             0,
                             0,
                             -0.275f,
                             0,
                             0.412f,
                             0,
                              _logoAspectRatio,
                              AspectRatioType.HeightMaster);
            _root.Add(_logo);

            _credits = new Image(Content.Load<Texture2D>(Constants.Resources.TEXTURE_CREDITS_AND_VERSION),
                                 Origin.BottomLeft,
                                0,
                                5,
                                0,
                                5,
                                0.3f,
                                 0,
                                 _creditsAspectRatio,
                                 AspectRatioType.WidthMaster);
            _root.Add(_credits);

            _playButton = new Button(_buttonReleased,
                                     _buttonHover,
                                     _buttonPressed,
                                     Origin.Center,
                                    0,
                                    0,
                                    0.04f,
                                    0,
                                    0.15f,
                                    0,
                                    2.15f,
                                     AspectRatioType.HeightMaster);
            _playButton.Action = () =>
            {
                GameManager.ChangeState(new LobbyGameState(GameManager));
            };
            Label playLabel = new Label(Content.Load<BitmapFont>(Constants.Resources.FONT_MENU_BUTTON),
                                        Origin.Center,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0.4f,
                                       0,
                                        AspectRatioType.HeightMaster);
            playLabel.Content = "Play";
            _playButton.SubPanel.Add(playLabel);
            _root.Add(_playButton);

            _optionsButton = new Button(_buttonReleased,
                                     _buttonHover,
                                     _buttonPressed,
                                     Origin.Center,
                                    0,
                                    0,
                                    0.22f,
                                    0,
                                    0.15f,
                                    0,
                                    2.15f,
                                     AspectRatioType.HeightMaster);
            _optionsButton.Action = () =>
            {
                GameManager.ChangeState(new SettingsGameState(GameManager));
            };
            Label optionsLabel = new Label(Content.Load<BitmapFont>(Constants.Resources.FONT_MENU_BUTTON),
                                           Origin.Center,
                                           0,
                                           0,
                                           0,
                                           0,
                                           0.4f,
                                          0,
                                           AspectRatioType.HeightMaster);
            optionsLabel.Content = "Options";
            _optionsButton.SubPanel.Add(optionsLabel);
            _root.Add(_optionsButton);

            _exitButton = new Button(_buttonReleased,
                                     _buttonHover,
                                     _buttonPressed,
                                     Origin.Center,
                                    0,
                                    0,
                                    0.4f,
                                    0,
                                    0.15f,
                                    0,
                                    2.15f,
                                     AspectRatioType.HeightMaster);
            _exitButton.Action = () =>
            {
                GameManager.Exit();
            };
            Label exitLabel = new Label(Content.Load<BitmapFont>(Constants.Resources.FONT_MENU_BUTTON),
                                        Origin.Center,
                                        0,
                                        0,
                                        0,
                                        0,
                                        0.4f,
                                       0,
                                        AspectRatioType.HeightMaster);
            exitLabel.Content = "Exit";
            _exitButton.SubPanel.Add(exitLabel);
            _root.Add(_exitButton);
        }

        public override void Hide()
        {

        }

        public override void Update(float dt)
        {
            MouseState mouseState = Mouse.GetState();
        }

        public override void Draw(float dt)
        {
            _spriteBatch.Begin();
            _root.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        public override void Dispose()
        {
            _root.UnregisterListeners();
        }
    }
}
