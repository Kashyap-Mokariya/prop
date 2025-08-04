using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Net.WebSockets;
using System.Threading.Tasks;
using PropVivo.Application.Services;

namespace PropVivo.API.Services
{
    public class VoiceModulationService : IVoiceModulationService
    {
        private readonly IConfiguration _config;

        public VoiceModulationService(IConfiguration config) => _config = config;

        public async Task<byte[]> ModulateVoiceAsync(byte[] audioData, string fromAcc, string toAcc)
        {
            var apiUrl = _config["VoiceModulation:ApiUrl"];
            var apiKey = _config["VoiceModulation:ApiKey"];
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                apiKey
            );

            using var content = new ByteArrayContent(audioData);
            content.Headers.ContentType = new MediaTypeHeaderValue("audio/webm");

            var response = await client.PostAsync(
                $"{apiUrl}modulate?from={fromAcc}&to={toAcc}",
                content
            );
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<Stream> ModulateVoiceStreamAsync(
            Stream audio,
            string fromAcc,
            string toAcc
        )
        {
            var bytes = await new MemoryStream().WriteAsync(audio.ToArray());
            var mod = await ModulateVoiceAsync(bytes, fromAcc, toAcc);
            return new MemoryStream(mod);
        }
    }
}
