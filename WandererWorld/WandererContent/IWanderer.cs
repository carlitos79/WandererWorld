using Microsoft.Xna.Framework;

namespace WandererWorld.WandererContent
{
    public interface IWanderer
    {
        void DrawLimb(GameTime gameTime, Matrix world);
        void UpdateLimbMovement(GameTime gameTime);
    }
}
