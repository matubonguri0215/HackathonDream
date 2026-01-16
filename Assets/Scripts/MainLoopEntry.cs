using System.Collections.Generic;



public interface IStartable
{
    void OnStart();
}
public interface IUpdatable
{
    void OnUpdate();
}


public interface IMainLoopEntryRegistable
{
    void RegisterStartable(IStartable startable);
    void UnregisterStartable(IStartable startable);
    void RegisterUpdatable(IUpdatable updatable);
    void UnregisterUpdatable(IUpdatable updatable);
}


public class MainLoopEntry : MonoSingleton<MainLoopEntry>, IMainLoopEntryRegistable
{
    private List<IStartable> _startables = new List<IStartable>();
    private List<IUpdatable> _updatables = new List<IUpdatable>();
    private bool _isStarted = false;

    public void RegisterStartable(IStartable startable)
    {
        _startables.Add(startable);
        if (_isStarted)
        {
            startable.OnStart();
        }
    }

    public void RegisterUpdatable(IUpdatable updatable)
    {
        _updatables.Add(updatable);
    }

    public void UnregisterStartable(IStartable startable)
    {
        _startables.Remove(startable);
    }

    public void UnregisterUpdatable(IUpdatable updatable)
    {
        _updatables.Remove(updatable);
    }

    private void Start()
    {
        foreach (IStartable startable in _startables)
        {
            startable.OnStart();
        }
        _isStarted = true;
    }
    private void Update()
    {
        foreach (IUpdatable updatable in _updatables)
        {
            updatable.OnUpdate();
        }
    }
}