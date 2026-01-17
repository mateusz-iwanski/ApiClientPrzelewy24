using ApiClientPrzelewy24.Objects;
using Refit;
using System.Threading.Tasks;

namespace ApiClientPrzelewy24.Clients
{
    public interface IPrzelewy24Api
    {
        [Post("/transaction/register")]
        Task<RegisterResponseDto> RegisterAsync([Body(BodySerializationMethod.UrlEncoded)] RegisterRequestDto request);
    }
}
