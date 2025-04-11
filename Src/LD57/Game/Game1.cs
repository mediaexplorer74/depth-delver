
// Type: LD57.Base
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

#nullable disable
namespace LD57
{
  public class Game1 : Game
  {
    public const int kWidth = 256;
    public const int kHeight = 144;
    private const float kDefaultScale = 3f;
    private float m_scale = 3f;
    private GraphicsDeviceManager m_graphics;
    private SpriteBatch m_spriteBatch;
    private RenderTarget2D m_renderTarget;
    private GameStateManager m_gameStateManager;
    public static bool s_debug;
    private bool m_frameStep;

    public Game1()
    {
      this.m_graphics = new GraphicsDeviceManager((Game) this);
      this.Content.RootDirectory = "Content";
      this.IsMouseVisible = true;
      this.SetWindowSize(this.m_scale);
      AudioManager.SetUpLooper();
      InputManager.SetInput("Left", Keys.Left, Buttons.DPadLeft);
      InputManager.SetInput("Right", Keys.Right, Buttons.DPadRight);
      InputManager.SetInput("Jump", Keys.X, Buttons.A);
      InputManager.SetInput("Attack", Keys.C, Buttons.X);
      this.Window.Title = "Depth Delver";
    }

    protected override void Initialize()
    {
      this.m_renderTarget = new RenderTarget2D(this.GraphicsDevice, 256, 144);
      base.Initialize();
    }

    protected override void LoadContent()
    {
      this.m_spriteBatch = new SpriteBatch(this.GraphicsDevice);
      SpriteManager.SetContentManager(this.Content);
      SpriteManager.SetSpriteBatch(this.m_spriteBatch);
      SpriteManager.LoadTexture("pixel");
      SpriteManager.LoadAnim("Player");
      SpriteManager.LoadAnim("Whip");
      SpriteManager.LoadAnim("Snake");
      SpriteManager.LoadAnim("Checkpoint");
      SpriteManager.LoadAnim("Coin");
      SpriteManager.LoadAnim("CoinHUD");
      SpriteManager.LoadAnim("Heart");
      SpriteManager.LoadAnim("HeartHUD");
      SpriteManager.LoadAnim("Key");
      SpriteManager.LoadAnim("KeyHUD");
      SpriteManager.LoadAnim("Dirt");
      SpriteManager.LoadAnim("DirtBig");
      SpriteManager.LoadAnim("Numbers");
      SpriteManager.LoadAnim("Door");
      SpriteManager.LoadAnim("Spikes");
      SpriteManager.LoadAnim("Bat");
      SpriteManager.LoadAnim("Hook");
      SpriteManager.LoadAnim("Turret");
      SpriteManager.LoadAnim("Arrow");
      SpriteManager.LoadAnim("PlatformSemiSolid");
      SpriteManager.LoadAnim("PlatformSolid");
      this.LoadSFX("Checkpoint");
      this.LoadSFX("Hit");
      this.LoadSFX("Hurt");
      this.LoadSFX("Jump");
      this.LoadSFX("Kill");
      this.LoadSFX("Pickup");
      this.LoadSFX("Shoot");
      this.LoadSFX("Surprise");
      this.LoadSFX("Swoop");
      this.LoadSFX("Whip");
      this.LoadSFX("Latch");
      this.LoadSFX("Upgrade");
      this.LoadSFX("Collect");
      this.LoadSFX("Death");
      this.LoadSong("DepthDelver");
      this.m_gameStateManager = new GameStateManager(this);
    }

    private void LoadSFX(string name)
    {
      AudioManager.AddSFX(name, this.Content.Load<SoundEffect>("sound/" + name));
    }

    public void LoadSong(string name)
    {
      AudioManager.AddSong(name, this.Content.Load<Song>("sound/" + name));
    }

    protected override void Update(GameTime gameTime)
    {
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        this.Exit();
      AudioManager.Update(gameTime);
      InputManager.UpdateInput(Keyboard.GetState(), GamePad.GetState(0));
      bool flag = false;
      if (Game1.s_debug)
      {
        if (InputManager.IsPressed(Keys.F5))
        {
          if (this.m_frameStep)
            flag = true;
          else
            this.m_frameStep = true;
        }
        if (InputManager.IsPressed(Keys.F6))
          this.m_frameStep = !this.m_frameStep;
      }
      if (InputManager.IsPressed(Keys.F4))
      {
        if (!this.m_graphics.IsFullScreen)
          this.SetFullScreen();
        this.m_graphics.ToggleFullScreen();
        if (!this.m_graphics.IsFullScreen)
          this.SetWindowSize(3f);
      }
      if (((!Game1.s_debug ? 1 : (!this.m_frameStep ? 1 : 0)) | (flag ? 1 : 0)) != 0)
        this.m_gameStateManager.Update(gameTime);
      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      this.GraphicsDevice.SetRenderTarget(this.m_renderTarget);
      this.GraphicsDevice.Clear(Color.Black);
      this.m_spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
      this.m_gameStateManager.Draw(gameTime);
      this.m_spriteBatch.End();
      this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
      this.m_spriteBatch.Begin(samplerState: SamplerState.PointClamp);
      this.m_spriteBatch.Draw((Texture2D) this.m_renderTarget, new Rectangle(0, 0, (int) (256.0 * (double) this.m_scale), (int) (144.0 * (double) this.m_scale)), Color.White);
      this.m_spriteBatch.End();
      base.Draw(gameTime);
    }

    private void SetWindowSize(float scale)
    {
      this.m_scale = scale;
      this.m_graphics.PreferredBackBufferWidth = (int) (256.0 * (double) scale);
      this.m_graphics.PreferredBackBufferHeight = (int) (144.0 * (double) scale);
      this.m_graphics.ApplyChanges();
    }

    private void SetFullScreen() => this.SetWindowSize(this.GetFullScreenScale());

    private float GetFullScreenScale()
    {
      return Math.Min((float) (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 256), (float) (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 144));
    }
  }
}
