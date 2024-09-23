namespace AnyProduct.Inbox.EF;

public class InboxOptions
{
    public const string InboxOptionsKey = "Inbox";
    public int RetryCount { get; set; }
}
