using TexasTaco.Authentication.Core.Entities;

namespace TexasTaco.Api.Gateway.Services
{
    internal interface ICurrentSessionStorage
    {
        void SaveSessionInStorage(Session session);
        Session? GetSavedSessionFromStorage();
    }
}
