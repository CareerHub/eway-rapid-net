namespace eWAY.Rapid.Internals.Enums {
    internal enum TransactionTypes : byte {
        Unknown,
        Purchase = 1,
        Refund = 4,
        PreAuth = 8
    }
}
