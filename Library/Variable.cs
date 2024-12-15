namespace AdventOfCode.Library;

public class Variable<T>
{
    private T test;
    private T actual;
    public static Variable<T> Create(T test, T actual)
    {
        return new Variable<T>
        {
            test = test,
            actual = actual
        };
    }

    private T Value => DayEngine.isTest ? test : actual;
    public static implicit operator T(Variable<T> t) => t.Value;
}