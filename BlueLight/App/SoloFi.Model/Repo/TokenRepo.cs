using SoloFi.Contract.Repo;
using SoloFi.Entity;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Contract.Serialise;
using XamlingCore.Portable.Data.Repos.Base;

namespace SoloFi.Model.Repo
{
    class TokenRepo : WebRepo<AuthorizationTokenDto>, ITokenRepo
    {
        public TokenRepo(IHttpTransferrer downloader, IEntitySerialiser entitySerialiser)
            : base(downloader, entitySerialiser, "auth/")
        {
        }


    }
}
