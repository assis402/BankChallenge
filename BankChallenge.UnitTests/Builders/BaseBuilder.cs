namespace BankChallenge.UnitTests.Builders;

public abstract class BaseBuilder<T> where T : class
{
    protected T _instance;

    public virtual BaseBuilder<T> Default() => this;

    public T Build() => _instance;

    public List<T> BuildList() => new() { _instance };

    public virtual List<T> BuildList(int length)
    {
        var result = new List<T>();

        for (int i = 0; i < length; i++)
            result.Add(Default().Build());

        return result;
    }
}