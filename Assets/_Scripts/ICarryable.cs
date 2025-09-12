public interface ICarryable
{
    public string Identity { get; set; }
    void OnCarry();
    void OnDrop();
}
