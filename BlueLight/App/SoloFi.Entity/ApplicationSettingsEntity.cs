using System;
using XamlingCore.Portable.Model.Contract;

namespace BlueLight.Entity
{
    public class ApplicationSettingsEntity : IEntity
    {
        public ApplicationSettingsEntity()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        public string ServerBaseUrl { get; set; }

        public string CurrentUserName { get; set; }
    }
}
