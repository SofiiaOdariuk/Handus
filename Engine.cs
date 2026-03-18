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
        player.Update(dt);
        level.Update(dt);
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