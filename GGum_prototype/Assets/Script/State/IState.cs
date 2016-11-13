public interface IState
{
    string GetID { get; }

    void Enter();
    void Excute();
    void Exit();

}
