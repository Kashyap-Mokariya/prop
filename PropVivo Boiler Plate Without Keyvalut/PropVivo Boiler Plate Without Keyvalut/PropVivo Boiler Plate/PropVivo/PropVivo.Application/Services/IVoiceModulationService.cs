namespace PropVivo.Application.Services
{
    public interface IVoiceModulationService
    {
        Task<byte[]> ModulateVoiceAsync(byte[] audioData, string fromAccent, string toAccent);
        Task<Stream> ModulateVoiceStreamAsync(Stream audioStream, string fromAccent, string toAccent);
    }
}
