namespace WandererWorld.Interfaces
{
    public class Renderer
    {
        public void Render(params IRenderSystem[] objects)
        {
            foreach (var obj in objects)
            {
                obj.RenderSystem();
            }
        }
    }
}
