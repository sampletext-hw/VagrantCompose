namespace Models.Db.ProcessStates
{
    public enum OrderState
    {
        Created,
        Cooking,
        Packing,
        Delivering,
        Canceled,
        Finished
    }
}