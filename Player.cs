using SFML.Window;

namespace Handus;

using SFML.System;
using SFML.Graphics;
public class Player
{
    private Sprite Sprite;
    private Dictionary<string, Texture> Textures;
    private FloatRect Hitbox;
    private Vector2f Velocity;
    private const float Speed = 50.0f;
    private const float Dumping = 8.0f;
    private const float Scale = 0.2f;
    private const float MaxSpeed = 20.0f;

    public void Update(float dt)
    {
        float move = 0f;
        if (Keyboard.IsKeyPressed(Keyboard.Key.A)) move -= 1;
        if (Keyboard.IsKeyPressed(Keyboard.Key.D)) move += 1;

        if (move != 0) Velocity.X += move * Speed * dt;
        else Velocity.X -= Velocity.X * Dumping * dt;
        
        Sprite.Position += Speed * Velocity * dt;
        if (Velocity.X > MaxSpeed) Velocity.X = MaxSpeed;
        if (Velocity.X < -MaxSpeed) Velocity.X = -MaxSpeed;
    }
    
    public Player(Dictionary<string, Texture> textures, Vector2f spawnPoint)    // initializes player with a map of textures
    {
        this.Textures = textures;
        Sprite = new Sprite(textures["idle1"]);
        Sprite.Scale = new Vector2f(Scale, Scale);
        
        SetPosition(spawnPoint);
    }
    
    void SetPosition(Vector2f position) // sets player's middle bottom to passed coords
    {
        var size = Sprite.TextureRect.Size;
        size = new Vector2i((int)(size.X * Scale), (int)(size.Y * Scale));
        Sprite.Position = new Vector2f(position.X - size.X / 2.0f, position.Y - size.Y);
    }
    public Sprite GetSprite() => Sprite;
}