using Frent;

namespace UnnamedFactoryGame.Components;

internal struct ItemAcceptor
{
    public Entity CurrentItem;
    public bool CanPlace => !CurrentItem.IsAlive;
    public bool TryPlace(Entity e)
    {
        if(!CurrentItem.IsAlive && e.IsAlive)
        {
            CurrentItem = e;
            return true;
        }
        return false;
    }
}
