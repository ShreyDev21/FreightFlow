namespace FreightFlow.Shipment.Domain.ValueObjects;

public sealed class TrackingCode
{
    public string Value { get; }

    private TrackingCode(string value)
    {
        Value = value;
    }

    // Factory method — only way to create a TrackingCode
    public static TrackingCode Generate()
    {
        // Format: FF-YYYYMMDD-XXXXXXXX (FF = FreightFlow prefix)
        var datePart = DateTime.UtcNow.ToString("yyyyMMdd");
        var randomPart = Guid.NewGuid().ToString("N")[..8].ToUpper();
        return new TrackingCode($"FF-{datePart}-{randomPart}");
    }

    public static TrackingCode From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Tracking code cannot be empty.", nameof(value));

        return new TrackingCode(value.ToUpper());
    }

    // Value objects are equal if their values are equal
    public override bool Equals(object? obj) =>
        obj is TrackingCode other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;
}