using ApiClientPrzelewy24.Objects;
using System.Threading.Tasks;

namespace ApiClientPrzelewy24.Clients
{
    public interface IPrzelewy24SignatureProvider
    {
        Task<string> CreateRegisterSignatureAsync(RegisterRequestDto request, string crcKey);
    }
}
