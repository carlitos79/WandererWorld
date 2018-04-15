using Microsoft.Xna.Framework;

namespace WandererWorld.Interfaces
{
    public class Updater
    {
        GameTime gameTime;

        public Updater(GameTime gameTime)
        {
            this.gameTime = gameTime;
        }

        public void Update(params IUpdateSystem[] objects)
        {
            foreach (var obj in objects)
            {
                obj.UpdateSystem(gameTime);
            }
        }
    }
}
