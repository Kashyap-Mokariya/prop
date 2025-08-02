using System.Net;
using System.Security.Authentication;

namespace PropVivo.API.Extensions.Middleware
{
    public class IpAddressMiddleware
    {
        private readonly List<string> _ipAddresses;
        private readonly RequestDelegate _next;

        public IpAddressMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _ipAddresses = configuration.GetValue<string>("IpAddresses")?.Split(',')?.ToList() ?? new List<string>();
        }

        public async Task Invoke(HttpContext context)
        {
            using (var httpClient = new HttpClient())
            {
                var currentIp = await httpClient.GetStringAsync("https://api.ipify.org");
                var remoteIp = context.Connection.RemoteIpAddress;

                if (!IsValidIpAddress(currentIp))
                {
                    throw new AuthenticationException($"User is not authorized. Illegal access detected from IP {currentIp}");
                    //context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    //return;
                }

                await _next.Invoke(context);
            }
        }

        //private bool IsValidIpAddress(string ipAddress) => _ipAddresses.Contains(ipAddress);
        private bool IsValidIpAddress(string currentIp)
        {
            var splitIp = false;

            var currentIpSubList = new List<string>();
            currentIpSubList = currentIp.Split('.').ToList();

            var index4 = currentIpSubList[3];
            var index3 = currentIpSubList[2];
            var index2 = currentIpSubList[1];
            var index1 = currentIpSubList[0];

            var tempAddress = $"{index1}.{index2}.{index3}.*";
            splitIp = _ipAddresses.Contains(tempAddress);

            if (!splitIp)
            {
                tempAddress = $"{index1}.{index2}.*.*";
                splitIp = _ipAddresses.Contains(tempAddress);
            }

            if (!splitIp)
            {
                tempAddress = $"{index1}.*.*.*";
                splitIp = _ipAddresses.Contains(tempAddress);
            }

            if (!splitIp)
            {
                tempAddress = $"*.*.*.*";
                splitIp = _ipAddresses.Contains(tempAddress);
            }

            if (_ipAddresses.Contains(currentIp) || string.IsNullOrEmpty(currentIp) || splitIp)
                splitIp = true;

            return splitIp;
        }
    }
}