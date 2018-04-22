using Microsoft.Xna.Framework;

namespace WandererWorld.WandererContent
{
    public interface IWanderer
    {
        void DrawLimb(GameTime gameTime);
        void UpdateLimbMovement(GameTime gameTime);
    }
}
