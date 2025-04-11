
// Type: LD57.Base
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System;

#nullable disable
namespace LD57
{
  public class Game1 : Game
  {
    private GraphicsDeviceManager graphics;

    public const int kWidth = 256;
    public const int kHeight = 144;

    // *********************************************************************
    Vector2 baseScreenSize = new Vector2
    (
            kWidth,
            kHeight
    );
    private Matrix globalTransformation;
    int backbufferWidth, backbufferHeight;
    public static bool FirstResize = true;
    public static Vector3 screenScale = new Vector3(1,1,1);
    // *********************************************************************

    
    public static int ScreenWidth = kWidth * 3;
    public static int ScreenHeight = kHeight * 3;
    public static Vector2 ScreenSize { get; set; }
    
    private const float kDefaultScale = 3f;
    private float m_scale = 3f;
   
    private SpriteBatch spriteBatch;
    private RenderTarget2D m_renderTarget;
    private GameStateManager m_gameStateManager;
    
    public static bool s_debug = false; // set it *true* for F5 / F6 anim. debug
    
    private bool m_frameStep;

    public Game1()
    {
      this.graphics = new GraphicsDeviceManager((Game) this);
#if WINDOWS_PHONE
            TargetElapsedTime = TimeSpan.FromTicks(333333);
#endif

        Game1.ScreenSize = new Vector2(Game1.ScreenWidth, Game1.ScreenHeight);
        graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft
            | DisplayOrientation.LandscapeRight | DisplayOrientation.Portrait;
            this.Content.RootDirectory = "Content";

      this.IsMouseVisible = true;

      //this.SetWindowSize(this.m_scale);
      
      AudioManager.SetUpLooper();

      InputManager.SetInput("Left", Keys.Left, Buttons.DPadLeft);
      InputManager.SetInput("Right", Keys.Right, Buttons.DPadRight);
      InputManager.SetInput("Jump", Keys.X, Buttons.A);
      InputManager.SetInput("Attack", Keys.C, Buttons.X);

      this.Window.Title = "Depth Delver";
    }

    protected override void Initialize()
    {
      //this.graphics.PreferredBackBufferWidth = Game1.ScreenWidth;
      //this.graphics.PreferredBackBufferHeight = Game1.ScreenHeight;
      this.graphics.IsFullScreen = true; // set *false* only for better debug

      this.graphics.ApplyChanges();

      ScalePresentationArea();

      //this.m_renderTarget = new RenderTarget2D(this.GraphicsDevice, 
      //    (int)(256 * screenScale.X), 
      //    (int)(144 * screenScale.Y));
      base.Initialize();
    }

        // ScalePresentationArea
        public void ScalePresentationArea()
        {
            //Work out how much we need to scale our graphics to fill the screen
            backbufferWidth = GraphicsDevice.PresentationParameters.BackBufferWidth - 0; // 40
            backbufferHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

            float horScaling = (float)(backbufferWidth * 1f / baseScreenSize.X);
            float verScaling =  (float)(backbufferHeight * 1f / baseScreenSize.Y);

            screenScale = new Vector3(horScaling, verScaling, 1);
            //float screenScale = Math.Min(
           //(float)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 256),
           //(float)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 144));

            globalTransformation = Matrix.CreateScale(screenScale);

            //System.Diagnostics.Debug.WriteLine("Screen Size - Width["
            //    + GraphicsDevice.PresentationParameters.BackBufferWidth + "] " +
            //    "Height [" + GraphicsDevice.PresentationParameters.BackBufferHeight + "]");

        }//ScalePresentationArea


        protected override void LoadContent()
    {
      this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

      // **************************************
      ScalePresentationArea();
      // ************************************** 

      SpriteManager.SetContentManager(this.Content);
      SpriteManager.SetSpriteBatch(this.spriteBatch);
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
        // *********************************
        //Check First Resize  & Control time where Screen has not been resized by the user
        if (FirstResize
            ||
            (backbufferHeight != GraphicsDevice.PresentationParameters.BackBufferHeight
            ||
            backbufferWidth != GraphicsDevice.PresentationParameters.BackBufferWidth)
            )
        {
            ScalePresentationArea();
            FirstResize = false;
        }
        // *********************************

    if
    (
        GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed 
        || Keyboard.GetState().IsKeyDown(Keys.Escape)
      )
        this.Exit();

      AudioManager.Update(gameTime);
      InputManager.UpdateInput(Keyboard.GetState(), GamePad.GetState(0), TouchPanel.GetState());
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
     
      /*if (InputManager.IsPressed(Keys.F4))
      {
        if (!this.graphics.IsFullScreen)
          this.SetFullScreen();
        this.graphics.ToggleFullScreen();

        if (!this.graphics.IsFullScreen)
          this.SetWindowSize(3f);
      }*/

      if (((!Game1.s_debug ? 1 : (!this.m_frameStep ? 1 : 0)) | (flag ? 1 : 0)) != 0)
        this.m_gameStateManager.Update(gameTime);

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      this.GraphicsDevice.SetRenderTarget(this.m_renderTarget);
      this.GraphicsDevice.Clear(Color.Black);

      //this.spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
      this.spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend,
          SamplerState.PointClamp, null, null, null, globalTransformation);

      this.m_gameStateManager.Draw(gameTime);
      this.spriteBatch.End();
      
      /*this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);

        this.spriteBatch.Begin(default, default,
            SamplerState.PointClamp, null, null, null, globalTransformation);
        //this.spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        this.spriteBatch.Draw((Texture2D) this.m_renderTarget, new Rectangle(0, 0,
          (int) (256.0 * (double) / * this.m_scale * /screenScale.X), 
          (int) (144.0 * (double) / * this.m_scale * /screenScale.Y)), 
          Color.White);
      this.spriteBatch.End();*/
      base.Draw(gameTime);
    }

    //private void SetWindowSize(float scale)
    //{
    //  this.m_scale = scale;
    //  this.graphics.PreferredBackBufferWidth = (int) (256.0 * (double) scale);
    //  this.graphics.PreferredBackBufferHeight = (int) (144.0 * (double) scale);
    //  //this.graphics.ApplyChanges();
    //}

    //private void SetFullScreen()
    //{
    //    this.SetWindowSize(this.GetFullScreenScale());
    //}

    //private float GetFullScreenScale()
    //{
    //    float screenScale = Math.Min(
    //        (float) (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 256), 
    //        (float) (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 144));
    //
    //    return screenScale;
    //}
  }
}
