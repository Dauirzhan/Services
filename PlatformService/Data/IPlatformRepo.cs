using PlatformService.Models;

namespace PlatformService.Data{
    public interface IPlatformRepo{
        bool SaveChanges();

        IEnumerable<Platform> GetAllPlatforms();

        Platform GetPlatformId(int id);

        void CreatePlatform(Platform plat);
    }
}