using SFML.System;
using SFML.Window;

namespace Handus;

using SFML.Graphics;
public class Engine
{
    private RenderWindow window;
    private Player player;
    private Level level;
    private Dictionary<string, Texture> textures = new();

    public Engine(RenderWindow window)  // initializes engine, creates player and level
    {
        this.window = window;

        level = new Level1(window.Size);
        
        var texture = new Texture("Files/player.jpg");
        textures.Add("idle1", texture);
        player = new Player(textures, level.GetSpawnPoint());
        
        window.Closed += (sender, e) => window.Close();
        
    }
    
    void Update(float dt)
    {
        window.DispatchEvents();
        if (Keyboard.IsKeyPressed(Keyboard.Key.Escape)) window.Close();

        UpdateCollisions();
        player.Update(dt);
        level.Update(dt);
    }

    void UpdateCollisions()
    {
        var hitboxes = level.GetHitboxes();
        var p = player.GetHitbox();

        foreach (var h in hitboxes)
        {
            // first check if the rectangles overlap at all
            bool horizontalOverlap = p.Left < h.Left + h.Width && p.Left + p.Width > h.Left;
            bool verticalOverlap   = p.Top < h.Top + h.Height && p.Top + p.Height > h.Top;

            if (!horizontalOverlap || !verticalOverlap)
                continue; // no collision

            // calculate penetration depth
            float overlapX = Math.Min(p.Left + p.Width, h.Left + h.Width) - Math.Max(p.Left, h.Left);
            float overlapY = Math.Min(p.Top + p.Height, h.Top + h.Height) - Math.Max(p.Top, h.Top);

            // resolve along the smaller penetration axis
            if (overlapX < overlapY)
            {
                if (p.Left < h.Left)
                    player.Collide(Direction.Right);
                else
                    player.Collide(Direction.Left);
            }
            else
            {
                if (p.Top < h.Top)
                    player.Collide(Direction.Down);
                else
                    player.Collide(Direction.Up);
            }
        }
    }

    void Render()   // draws all sprites
    {
        window.Clear();
        foreach (var s in level.GetSprites()) window.Draw(s);
        window.Draw(player.GetSprite());
        window.Display();
    }
    
    public void Loop()  // launches the game's loop
    {
        var clock = new Clock();
        
        
        while (window.IsOpen)   // TODO: delta time
        {
            float dt = clock.Restart().AsSeconds();
            
            Update(dt);
            Render();
        }
    }
}

public enum Direction
{
    None,
    Up,
    Down,
    Left,
    Right
};
