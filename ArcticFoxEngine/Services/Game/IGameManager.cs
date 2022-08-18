namespace ArcticFoxEngine.Services.Game;

public interface IGameManager
{
    public bool Running { get; set; }
    public void AddUpdateServices(IEnumerable<IUpdateThreadService> updateServices);
    public void Run();
}
