using System.Numerics;
using SFML.Window;

namespace Handus;

using SFML.System;
using SFML.Graphics;
public class Player
{
    private Sprite Sprite;
    private Dictionary<string, Texture> Textures;
    private IntRect Hitbox;
    private const float Scale = 0.2f;
    private const float Speed = 300.0f;
    private const float Dumping = 16.0f;
    private const float MaxSpeed = 300.0f;
    private const float Gravity = 1000.0f;
    private const float JumpSpeed = 700.0f;
    private const float JumpSpeedMultiplier = 1.5f;
    
    private Vector2f Velocity;
    private bool IsOnGround = true;

    public void Update(float dt)
    {
        float move = 0f;
        
        if (IsOnGround)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
            {
                Velocity.Y = -JumpSpeed;
                Velocity.X *= JumpSpeedMultiplier;
                IsOnGround = false;
            }
            else
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.A)) move -= 1;
                if (Keyboard.IsKeyPressed(Keyboard.Key.D)) move += 1;
                
                if (move != 0)
                    Velocity.X += move * Speed * dt;
                if (move == 0 || (move < 0 != Velocity.X < 0)) Velocity.X -= Velocity.X * Dumping * dt;
                
                if (Velocity.X > MaxSpeed) Velocity.X = MaxSpeed;
                if (Velocity.X < -MaxSpeed) Velocity.X = -MaxSpeed;
            }
        }
        else
        {
            Velocity.Y += Gravity * dt;
        }
        
        
        Sprite.Position += Velocity * dt;
        Hitbox.Position = (Vector2i)Sprite.Position;
    }
    
    public Player(Dictionary<string, Texture> textures, Vector2f spawnPoint)    // initializes player with a map of textures
    {
        this.Textures = textures;
        Sprite = new Sprite(textures["idle1"]);
        Sprite.Scale = new Vector2f(Scale, Scale);
        Hitbox.Size = new Vector2i((int)(Sprite.TextureRect.Size.X * Scale), (int)(Sprite.TextureRect.Size.Y * Scale));
        
        SetPosition(spawnPoint);
    }

    public void Collide(Direction direction)
    {
        if (direction == Direction.None) return;

        if (direction == Direction.Down)
        {
            Velocity.Y = 0;
            IsOnGround = true;
            return;
        }

        switch (direction)
        {
            case Direction.Up:
                Velocity.Y = 0;
                break;
            case Direction.Left:
                Velocity.X = 0;
                break;
            case Direction.Right:
                Velocity.X = 0;
                break;
        }
    }
    void SetPosition(Vector2f position) // sets player's middle bottom to passed coords
    {
        var size = Sprite.TextureRect.Size;
        size = new Vector2i((int)(size.X * Scale), (int)(size.Y * Scale));
        Sprite.Position = new Vector2f(position.X - size.X / 2.0f, position.Y - size.Y);
        Hitbox.Position = (Vector2i)Sprite.Position;
    }
    public Sprite GetSprite() => Sprite;
    public IntRect GetHitbox() => Hitbox;
}