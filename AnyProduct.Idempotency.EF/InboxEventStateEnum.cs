namespace AnyProduct.Inbox.EF;

public enum InboxEventStateEnum
{
    NotConsumed = 0,
    InProgress = 1,
    Consumed = 2,
    ConsumeFailed = 3
}

