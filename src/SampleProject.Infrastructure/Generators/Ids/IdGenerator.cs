using SampleProject.Application.Configurations.Generators;

namespace SampleProject.Infrastructure.Generators.Ids;

public class IdGenerator : IIdGenerator
{
    // Custom Epoch to have a smaller timestamp value and more room for sequence bits.
    private static readonly long CustomEpoch = new DateTimeOffset(2026, 5, 1, 0, 0, 0, TimeSpan.Zero).ToUnixTimeMilliseconds();

    private const int NodeIdBits = 10;
    private const int SequenceBits = 12;

    private const long SequenceMask = -1L ^ (-1L << SequenceBits);

    private const int NodeIdShift = SequenceBits;
    private const int TimestampLeftShift = SequenceBits + NodeIdBits;

    private const long NodeId = 1;
    private long _lastTimestamp = -1L;
    private long _sequence = 0L;
    private readonly Lock _lock = new();

    public long Generate()
    {
        lock (_lock)
        {
            return GenerateInternal();
        }
    }

    public IReadOnlyList<long> GenerateBatch(int size)
    {
        if (size <= 0 || size > SequenceMask + 1)
        {
            throw new ArgumentOutOfRangeException(nameof(size), $"Batch size must be between 1 and {SequenceMask + 1}.");
        }

        lock (_lock)
        {
            var ids = new long[size];
            for (var i = 0; i < size; i++)
            {
                ids[i] = GenerateInternal();
            }

            return ids;
        }
    }

    public DateTimeOffset ExtractTimestamp(long id)
    {
        var timestamp = (id >> TimestampLeftShift) + CustomEpoch;
        return DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
    }

    private long GenerateInternal()
    {
        var currentTimestamp = GetCurrentTimestamp();

        if (currentTimestamp < _lastTimestamp)
        {
            throw new InvalidOperationException("Clock moved backwards, refusing to generate ID.");
        }

        if (currentTimestamp == _lastTimestamp)
        {
            // If generated in the same millisecond, increment the sequence.
            // If the sequence overflows, wait for the next millisecond.
            _sequence = (_sequence + 1) & SequenceMask;
            if (_sequence == 0)
            {
                currentTimestamp = WaitNextMillis(_lastTimestamp);
            }
        }
        else
        {
            _sequence = 0L;
        }

        _lastTimestamp = currentTimestamp;

        // Pack the bits together using bitwise OR.
        return ((currentTimestamp - CustomEpoch) << TimestampLeftShift) | (NodeId << NodeIdShift) | _sequence;
    }

    private static long GetCurrentTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    private static long WaitNextMillis(long lastTimestamp)
    {
        var timestamp = GetCurrentTimestamp();
        while (timestamp <= lastTimestamp)
        {
            timestamp = GetCurrentTimestamp();
        }

        return timestamp;
    }
}
