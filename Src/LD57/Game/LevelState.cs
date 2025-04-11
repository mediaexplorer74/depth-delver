
// Type: LD57.LevelState
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using LD57.Breakables;
using LD57.Camera;
using LD57.Combat;
using LD57.Enemies;
using LD57.Objects;
using LD57.Physics;
using LD57.Pickups;
using LD57.Spawn;
using LD57.State;
using LD57.Tiled;
using Microsoft.Xna.Framework;
using SharpDX.MediaFoundation;

//using MonoGame.Extended;
//using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using Windows.Foundation;

#nullable disable
namespace LD57
{
  public class LevelState : GameState, StateMachineImplementor
  {
    private TiledMapComponent m_map;
    private PhysicsManager m_physicsManager;
    private CombatManager m_combatManager;
    private GameCamera m_camera;
    private RoomManager m_roomManager;
    private HUDComponent m_hud;
    private List<SpawnPoint> m_spawnPoints;
    private List<HookComponent> m_hooks;
    public PlayerComponent m_player;
    public Vector2 m_playerSpawn = Vector2.Zero;
    public int m_coins;
    public int m_coinsHUD;
    public bool m_key;
    public bool m_keyHUD;
    public bool m_canGrapple;
    public float m_tutorialTimer;
    private const float kTutorialTimer = 0.0833333358f;
    private StateMachine m_stateMachine;
    private float m_transitionPerc;
    private const float kTransitionInTime = 1f;
    private const float kTransitionOutTime = 1f;
    private const float kTransitionHoldTime = 0.25f;
    private bool m_music;
    private bool m_holdMusic = true;

    public LevelState(Game1 b)
      : base(b)
    {
      this.m_stateMachine = new StateMachine((StateMachineImplementor) this);
      this.m_map = new TiledMapComponent(this.m_GameStateRootEntity, "BasicMap");
      this.m_physicsManager = new PhysicsManager(this.m_map);
      this.m_combatManager = new CombatManager();
      this.m_camera = new GameCamera();
      SpriteManager.s_camera = this.m_camera;
      this.m_roomManager = new RoomManager(this);
      this.m_roomManager.LoadRooms(this.m_map.GetTiledMap());
      this.m_spawnPoints = new List<SpawnPoint>();
      this.m_hooks = new List<HookComponent>();
      this.LoadMapEntities(true);
      this.m_roomManager.Update();
      this.m_camera.SnapToTarget();
      this.m_map.SetCamera(this.m_camera);
      this.m_map.SetLayerAlpha(1, 0.0f);
      this.m_hud = new HUDComponent(this.m_GameStateRootEntity, this);
      this.m_stateMachine.SetNextState(Game1.s_debug ? 0 : 1);
    }

        private void LoadMapEntities(bool init = false)
        {
            foreach (var objectLayer in this.m_map.GetTiledMap().ObjectLayers)
            {
                if (!objectLayer.Name.Equals("CAMERA"))
                {
                    foreach (var tiledObject in objectLayer.Objects)
                    {
                        this.SpawnObject(tiledObject, init: init);
                    }
                }
            }
        }

        private void SpawnObject(Camera.TiledMapObject tiledObject, bool init)
        {
            throw new NotImplementedException();
        }

        public Entity SpawnObject(TiledMapObject obj, SpawnPoint spawnPoint = null, bool init = false)
    {
      SpawnData data = new SpawnData(obj);
      if (spawnPoint == null && !data.HasProperty("Persistent"))
      {
        if (init || !data.HasProperty("Perm"))
          this.m_spawnPoints.Add(new SpawnPoint(data, this));
        return (Entity) null;
      }
      string type = data.GetType();
      Vector2 vector2 = obj.Position + new Vector2(8f, -8f);
      Entity entity = (Entity) null;
      if (type != null)
      {
        switch (type.Length)
        {
          case 3:
            switch (type[0])
            {
              case 'B':
                if (type == "Bat")
                {
                  entity = new Entity(this.m_GameStateRootEntity);
                  entity.m_position = vector2 + new Vector2(0.0f, 0.0f);
                  BatEnemyComponent batEnemyComponent = new BatEnemyComponent(entity, this, spawnPoint);
                  break;
                }
                break;
              case 'K':
                if (type == "Key")
                {
                  entity = new Entity(this.m_GameStateRootEntity);
                  entity.m_position = vector2;
                  KeyComponent keyComponent = new KeyComponent(entity, this, spawnPoint);
                  break;
                }
                break;
            }
            break;
          case 4:
            switch (type[3])
            {
              case 'k':
                if (type == "Hook")
                {
                  entity = new Entity(this.m_GameStateRootEntity);
                  entity.m_position = vector2;
                  this.m_hooks.Add(new HookComponent(entity, this, spawnPoint));
                  break;
                }
                break;
              case 'n':
                if (type == "Coin")
                {
                  entity = new Entity(this.m_GameStateRootEntity);
                  entity.m_position = vector2;
                  CoinComponent coinComponent = new CoinComponent(entity, this, spawnPoint);
                  break;
                }
                break;
              case 'r':
                if (type == "Door")
                {
                  entity = new Entity(this.m_GameStateRootEntity);
                  entity.m_position = vector2;
                  DoorComponent doorComponent = new DoorComponent(entity, this, spawnPoint);
                  break;
                }
                break;
              case 't':
                if (type == "Dirt")
                {
                  entity = new Entity(this.m_GameStateRootEntity);
                  entity.m_position = vector2;
                  DirtComponent dirtComponent = new DirtComponent(entity, this, spawnPoint, (double) data.GetObject().Size.Width > 16.0);
                  break;
                }
                break;
            }
            break;
          case 5:
            switch (type[0])
            {
              case 'H':
                if (type == "Heart")
                {
                  entity = new Entity(this.m_GameStateRootEntity);
                  entity.m_position = vector2;
                  HeartComponent heartComponent = new HeartComponent(entity, this, spawnPoint);
                  break;
                }
                break;
              case 'S':
                if (type == "Snake")
                {
                  entity = new Entity(this.m_GameStateRootEntity);
                  entity.m_position = vector2 + new Vector2(0.0f, -2f);
                  SnakeEnemyComponent snakeEnemyComponent = new SnakeEnemyComponent(entity, this, spawnPoint);
                  break;
                }
                break;
            }
            break;
          case 6:
            switch (type[0])
            {
              case 'P':
                if (type == "Player")
                {
                  entity = new Entity(this.m_GameStateRootEntity);
                  entity.m_position = !(this.m_playerSpawn == Vector2.Zero) ? this.m_playerSpawn : vector2 + new Vector2(0.0f, -2f);
                  this.m_player = new PlayerComponent(entity, this);
                  this.m_holdMusic = (double) this.m_player.GetPos().Y < 100.0;
                  break;
                }
                break;
              case 'S':
                if (type == "Spikes")
                {
                  entity = new Entity(this.m_GameStateRootEntity);
                  entity.m_position = vector2;
                  SpikesComponent spikesComponent = new SpikesComponent(entity, this, spawnPoint);
                  break;
                }
                break;
              case 'T':
                if (type == "Turret")
                {
                  entity = new Entity(this.m_GameStateRootEntity);
                  entity.m_position = vector2;
                  TurretComponent turretComponent = new TurretComponent(entity, this, spawnPoint);
                  break;
                }
                break;
            }
            break;
          case 10:
            if (type == "Checkpoint")
            {
              entity = new Entity(this.m_GameStateRootEntity);
              entity.m_position = vector2 + new Vector2(0.0f, -2f);
              CheckpointComponent checkpointComponent = new CheckpointComponent(entity, this, spawnPoint);
              break;
            }
            break;
          case 13:
            if (type == "PlatformSolid")
            {
              entity = new Entity(this.m_GameStateRootEntity);
              entity.m_position = vector2;
              MovingPlatformComponent platformComponent = new MovingPlatformComponent(entity, this, spawnPoint, true);
              break;
            }
            break;
          case 14:
            if (type == "GrappleUpgrade")
            {
              entity = new Entity(this.m_GameStateRootEntity);
              entity.m_position = vector2;
              GrappleUpgradeComponent upgradeComponent = new GrappleUpgradeComponent(entity, this, spawnPoint);
              break;
            }
            break;
          case 17:
            if (type == "PlatformSemiSolid")
            {
              entity = new Entity(this.m_GameStateRootEntity);
              entity.m_position = vector2;
              MovingPlatformComponent platformComponent = new MovingPlatformComponent(entity, this, spawnPoint, false);
              break;
            }
            break;
        }
      }
      if (entity != null)
      {
        if (obj.Properties.ContainsKey("Flip"))
          entity.m_facing.X = -1f;
        this.AddEntity(entity);
      }
      return entity;
    }

    public void AddEntity(Entity entity) => this.m_entities.Add(entity);

    private void Clear()
    {
      this.m_player = (PlayerComponent) null;
      for (int index = this.m_entities.Count<Entity>() - 1; index >= 0; --index)
      {
        Entity entity = this.m_entities[index];
        if (entity != this.m_map.GetParent())
        {
          entity.Destroy();
          this.m_entities.RemoveAt(index);
        }
      }
      for (int index = this.m_spawnPoints.Count<SpawnPoint>() - 1; index >= 0; --index)
      {
        if (!this.m_spawnPoints[index].GetData().HasProperty("Perm"))
          this.m_spawnPoints.RemoveAt(index);
      }
      this.m_hooks.Clear();
    }

    public void SetActiveCheckpoint(CheckpointComponent checkpoint)
    {
      this.m_playerSpawn = checkpoint.GetParent().m_position;
    }

    public PhysicsManager GetPhysicsManager() => this.m_physicsManager;

    public CombatManager GetCombatManager() => this.m_combatManager;

    public GameCamera GetCamera() => this.m_camera;

    public RoomManager GetRoomManager() => this.m_roomManager;

    public HUDComponent GetHUD() => this.m_hud;

    public List<HookComponent> GetHooks() => this.m_hooks;

    public void OnDeath() => this.m_stateMachine.SetNextState(2);

    public void OnRoomTransition()
    {
      this.m_camera.SetRoomBound(this.m_roomManager.GetCurRoom().GetArea());
      foreach (SpawnPoint spawnPoint in this.m_spawnPoints)
        spawnPoint.CheckSpawn();
    }

    public override void Update(GameTime gameTime)
    {
      this.m_physicsManager.Update(gameTime);
      this.m_combatManager.Update(gameTime);
      base.Update(gameTime);
      this.m_roomManager.Update();
      this.m_camera.Update(gameTime);
      foreach (SpawnPoint spawnPoint in this.m_spawnPoints)
        spawnPoint.Update();
      this.m_hud.Update(gameTime);
      this.m_stateMachine.Update(gameTime);
      if (this.m_canGrapple && (double) this.m_tutorialTimer < 0.0)
        this.m_tutorialTimer = 0.0833333358f;
      if ((double) this.m_tutorialTimer > 0.0)
      {
        this.m_tutorialTimer = Math.Max(0.0f, this.m_tutorialTimer - gameTime.GetElapsedSeconds());
        this.m_map.SetLayerAlpha(1, (float) (1.0 - (double) this.m_tutorialTimer / 0.0833333358168602));
      }
      if (!this.m_holdMusic || !this.m_player.GetPhysics().IsCollideGround() || (double) this.m_player.GetPos().Y <= 100.0)
        return;
      this.PlayMusic();
    }

    public override void Draw(GameTime gameTime)
    {
      this.m_physicsManager.Draw();
      this.m_combatManager.Draw();
      base.Draw(gameTime);
      this.m_hud.Draw(gameTime);
      if (this.m_stateMachine.GetState() < 1)
        return;
      Vector2 vector2 = new Vector2(128f, 72f);
      float num1 = this.m_transitionPerc * 400f;
      float num2 = num1 * num1;
      for (int index1 = 0; index1 < 256; ++index1)
      {
        for (int index2 = 0; index2 < 144; ++index2)
        {
          AABB aabb = new AABB(new Vector2((float) index1 + 0.5f, (float) index2 + 0.5f), 0.5f, 0.5f);
          if ((double) (vector2 - aabb.m_center).LengthSquared() > (double) num2)
            SpriteManager.DrawAABB(aabb, Microsoft.Xna.Framework.Color.Black, (GameCamera) null, 1f);
        }
      }
    }

    void StateMachineImplementor.InitState()
    {
      switch (this.m_stateMachine.GetState())
      {
        case 1:
          this.m_transitionPerc = 0.0f;
          break;
        case 2:
          this.m_transitionPerc = 1f;
          break;
        case 3:
          this.m_transitionPerc = 0.0f;
          break;
      }
    }

    void StateMachineImplementor.UpdateState(GameTime gameTime)
    {
      switch (this.m_stateMachine.GetState())
      {
        case 1:
          if (this.m_stateMachine.CrossedTime(0.5f) && !this.m_music && !this.m_holdMusic)
            this.PlayMusic();
          if ((double) this.m_stateMachine.GetStateTime() >= 1.0)
          {
            this.m_stateMachine.SetNextState(0);
            break;
          }
          this.m_transitionPerc = this.m_stateMachine.GetStateTime() / 1f;
          break;
        case 2:
          if ((double) this.m_stateMachine.GetStateTime() >= 1.0)
          {
            this.m_stateMachine.SetNextState(3);
            break;
          }
          this.m_transitionPerc = (float) (1.0 - (double) this.m_stateMachine.GetStateTime() / 1.0);
          break;
        case 3:
          if ((double) this.m_stateMachine.GetStateTime() < 0.25)
            break;
          this.Clear();
          this.LoadMapEntities();
          this.m_roomManager.Reset();
          this.m_camera.SnapToTarget();
          this.m_hud.Snap();
          this.m_tutorialTimer = 0.0f;
          if (this.m_canGrapple)
            this.m_map.SetLayerAlpha(1, 1f);
          this.m_stateMachine.SetNextState(1);
          break;
      }
    }

    void StateMachineImplementor.ExitState()
    {
      switch (this.m_stateMachine.GetLastState())
      {
      }
    }

    public void PlayMusic()
    {
      this.m_music = true;
      this.m_holdMusic = false;
      AudioManager.PlaySong("DepthDelver", true);
    }

    public void StopMusic()
    {
      this.m_music = false;
      AudioManager.StopSong();
    }

    private enum State
    {
      Idle,
      TransitionIn,
      TransitionOut,
      TransitionHold,
    }
  }
    public class TiledMapObject
    {
        public string Name;
        public int Id;
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public string Type;
        public Dictionary<string, object> Properties;
        internal Vector2 Position;
        internal Size Size;
    }
}
