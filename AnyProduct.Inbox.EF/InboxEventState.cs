namespace AnyProduct.Inbox.EF;

public enum InboxEventState
{
    NotConsumed = 0,
    InProgress = 1,
    Consumed = 2,
    ConsumeFailed = 3
}

