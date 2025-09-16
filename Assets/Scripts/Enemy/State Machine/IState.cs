public interface IState
{
    void OnEnter();
    void OnExit();
    void Tick();        // por-frame
    void FixedTick();   // opcional si usás física
}
