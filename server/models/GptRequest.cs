namespace GptClient
{
    public record GptRequest(string message, Context Context);

    public enum Context
    {
        CallCenterHelp,
        CustomerInformation,
        Test
    }
}