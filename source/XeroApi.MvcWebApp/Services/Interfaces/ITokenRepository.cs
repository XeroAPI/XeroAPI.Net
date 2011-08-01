using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Storage.Basic;

namespace Xero.ScreencastWeb.Services.Interfaces
{
    public interface ITokenRepository<TToken>
        where TToken : TokenBase
    {
        TToken GetToken(string token);

        void SaveToken(TToken token);
    }
}