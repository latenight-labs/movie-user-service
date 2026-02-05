namespace Movie.User.Service.Domain.Configuration;

public sealed class LengthRule
{
    private int _min;
    private int _max;

    public int Min
    {
        get => _min;
        set
        {
            if (value < 0)
                throw new ArgumentException("Min cannot be negative", nameof(Min));
            // If Max is 0 (initial state), only allow Min = 0
            // Otherwise, Min must be < Max
            if (_max == 0 && value != 0)
                throw new ArgumentException("Min must be less than Max", nameof(Min));
            if (_max != 0 && value >= _max)
                throw new ArgumentException("Min must be less than Max", nameof(Min));
            _min = value;
        }
    }

    public int Max
    {
        get => _max;
        set
        {
            if (value < 0)
                throw new ArgumentException("Max cannot be negative", nameof(Max));
            if (value <= _min)
                throw new ArgumentException("Max must be greater than Min", nameof(Max));
            _max = value;
        }
    }

    public LengthRule()
    {
    }

    public LengthRule(int min, int max)
    {
        if (min < 0)
            throw new ArgumentException("Min cannot be negative");
        if (max < 0)
            throw new ArgumentException("Max cannot be negative");
        if (min >= max)
            throw new ArgumentException("Min must be less than Max");

        _min = min;
        _max = max;
    }
}