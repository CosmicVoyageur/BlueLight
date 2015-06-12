using System;
using XamlingCore.Portable.Model.Contract;

namespace SoloFi.Entity
{
    public class ApplicationSettingsEntity : IEntity
    {
        public ApplicationSettingsEntity()
        {
            Id = Guid.NewGuid();
            //ServerBaseUrl = @"http://fakenavigatorapi.azurewebsites.net/api/";
            ServerBaseUrl = @"http://legacynavapi.azurewebsites.net/api/";
            //ServerBaseUrl = @"http://http://navigatorapi.azurewebsites.net/api/";
        }
        public Guid Id { get; set; }

        public Tokens StoredToken { get; set; }

        public string ServerBaseUrl { get; set; }

        public string CurrentUserName { get; set; }
    }
}
