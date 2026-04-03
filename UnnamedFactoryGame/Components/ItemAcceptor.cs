using Frent;
using Frent.Components;

namespace UnnamedFactoryGame.Components;

internal struct ItemAcceptor : IComponentBase
{
    public Entity Item { get => CurrentItem; set => CurrentItem = value; }
    public Entity CurrentItem;
    public bool HasItem => CurrentItem.IsAlive;
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
