using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace WandererWorld.Systems
{
    public abstract class GenericUpdateSystem
    {
        public List<GenericUpdateSystem> Children { get; set; }

        public GenericUpdateSystem()
        {
            Children = new List<GenericUpdateSystem>();
        }

        public virtual void Update(GameTime gameTime)
        {            
            Children.ForEach(x => x.Update(gameTime));
        }
    }
}
