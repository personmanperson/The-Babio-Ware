using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_Babio_Ware;

public class Lose(GameManager gameManager) : Scene(gameManager)
{
    public override void Load()
    {
        // Load any necessary assets for the Lose scene here
    }

    public override void Activate()
    {
        Active = true;
    }

    public override void Deactivate()
    {
        Active = false;
    }

    public override void Update()
    {
        // Update logic for the Lose scene here
    }

    public override void Draw()
    {
        // Draw logic for the Lose scene here
    }
}