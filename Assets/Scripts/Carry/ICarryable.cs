public interface ICarryable
{
    public string Identity { get; set; }
    public bool EnableCarry { get; set; } 
    public bool CarryMoving { get; set; } 
    void OnCarry();
    void OnDrop();
}
