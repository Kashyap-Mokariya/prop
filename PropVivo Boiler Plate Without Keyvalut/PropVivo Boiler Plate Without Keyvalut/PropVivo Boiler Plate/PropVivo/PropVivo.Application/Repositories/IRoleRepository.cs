using PropVivo.Application.Dto.RoleFeature.GetAllRole;
using PropVivo.Domain.Entities.FeatureRolePermissionMaster.SupportingTypes;

namespace PropVivo.Application.Repositories
{
    public interface IRoleRepository : ICosmosRepository<Role>
    {
        Task<Role> GetRoleAsync(GetAllRoleRequest request, bool? isRoleExist = null);

        Task<IEnumerable<Role>> GetRolesAsync(GetAllRoleRequest request);

        Task<(IEnumerable<Role> result, int count)> GetRolesWithCountAsync(GetAllRoleRequest request);
    }
}