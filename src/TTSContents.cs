namespace Aivis;

public record TTSContents(
        byte[] Audio,
        string ContentDisposition,
        string BillingMode,
        uint CharacterCount,
        uint CreditsRemaining = 0,
        uint CreditsUsed = 0,
        uint RateLimitRemaining = 0
        )
{
    public bool IsPayAsYouGo => BillingMode == "PayAsYouGo";
    public bool IsSubscription => BillingMode == "Subscription";
}