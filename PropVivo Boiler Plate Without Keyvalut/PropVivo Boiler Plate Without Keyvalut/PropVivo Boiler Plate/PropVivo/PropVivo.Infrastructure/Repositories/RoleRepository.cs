using Microsoft.Azure.Cosmos;
using PropVivo.Application.Dto.RoleFeature.GetAllRole;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.FeatureRolePermissionMaster;
using PropVivo.Domain.Entities.FeatureRolePermissionMaster.SupportingTypes;
using PropVivo.Domain.Enums;
using PropVivo.Infrastructure.Constants;
using PropVivo.Infrastructure.Helper;
using PropVivo.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace PropVivo.Infrastructure.Repositories
{
    public class RoleRepository : CosmosDbRepository<Role>, IRoleRepository
    {
        public RoleRepository(ICosmosDbContainerFactory factory) : base(factory)
        { }

        public override string ContainerName { get; } = CosmosDbContainerConstants.CONTAINER_NAME_FEATUREROLEPERMISSIONMASTER;

        public override string GenerateId(Role entity) => $"{Guid.NewGuid()}";

        public async Task<Role> GetRoleAsync(GetAllRoleRequest request, bool? isRoleExist = null)
        {
            var orderBy = OrderBy(request);
            if (orderBy != null)
                return await this.GetItemAsync(GetRoleQuery(request, isRoleExist), nameof(FeatureRolePermissionMaster));
            else
                return await this.GetItemAsync(GetRoleQuery(request, isRoleExist), nameof(FeatureRolePermissionMaster));
        }

        public Expression<Func<Role, bool>> GetRoleQuery(GetAllRoleRequest featureRolePermissionMasterRequest, bool? isRoleExist = null)
        {
            Expression<Func<Role, bool>> filter_role = role => role.Status != Status.Deleted && role.Type == nameof(Role);

            if (isRoleExist.HasValue && isRoleExist == true)
            {
                if (!string.IsNullOrEmpty(featureRolePermissionMasterRequest.RoleId))
                {
                    var roleIds = featureRolePermissionMasterRequest.RoleId.Split(',');
                    filter_role = filter_role.And(x => !roleIds.Contains(x.Id));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(featureRolePermissionMasterRequest.RoleId))
                {
                    var roleIds = featureRolePermissionMasterRequest.RoleId.Split(',');
                    filter_role = filter_role.And(x => roleIds.Contains(x.Id));
                }
            }

            if (!string.IsNullOrEmpty(featureRolePermissionMasterRequest.CountryId))
            {
                var countryIds = featureRolePermissionMasterRequest.CountryId.Split(',');
                filter_role = filter_role.And(x => countryIds.Contains(x.CountryId));
            }

            if (!string.IsNullOrEmpty(featureRolePermissionMasterRequest.BusinessTypeId))
            {
                var businessTypeIds = featureRolePermissionMasterRequest.BusinessTypeId.Split(',');
                filter_role = filter_role.And(x => businessTypeIds.Contains(x.BusinessTypeId));
            }

            if (!string.IsNullOrEmpty(featureRolePermissionMasterRequest.LegalEntityTypeId))
            {
                var legalEntityTypeIds = featureRolePermissionMasterRequest.LegalEntityTypeId.Split(',');
                filter_role = filter_role.And(x => legalEntityTypeIds.Contains(x.LegalEntityTypeId));
            }

            if (!string.IsNullOrEmpty(featureRolePermissionMasterRequest.SubTypeId))
            {
                var subTypeIds = featureRolePermissionMasterRequest.SubTypeId.Split(',');
                filter_role = filter_role.And(x => subTypeIds.Contains(x.LegalEntitySubTypeId));
            }

            if (!string.IsNullOrEmpty(featureRolePermissionMasterRequest.Name))
            {
                var name = featureRolePermissionMasterRequest.Name.ToLower();
                filter_role = filter_role.And(x => x.Name != null && x.Name.ToLower() == name);
            }

            if (featureRolePermissionMasterRequest.IsActive.HasValue)
            {
                var status = featureRolePermissionMasterRequest.IsActive == true ? Status.Active : Status.Inactive;
                filter_role = filter_role.And(x => x.Status == status);
            }

            ////if (!string.IsNullOrEmpty(featureRolePermissionMasterRequest.Keyword))
            ////{
            ////    var filterByKeyWord = featureRolePermissionMasterRequest.Keyword.ToLower();
            ////    Expression<Func<Role1, bool>> fitler_by_Keyword = n => false;

            ////    Expression<Func<Role1, bool>> name = a => (a.Name.IsNull() ? false : a.Name.ToLower().Contains(filterByKeyWord));

            ////    fitler_by_Keyword = fitler_by_Keyword.Or(name);
            ////    filter_role = filter_role.And(fitler_by_Keyword);
            ////}

            return filter_role;
        }

        public async Task<IEnumerable<Role>> GetRolesAsync(GetAllRoleRequest request)
        {
            var orderBy = OrderBy(request);
            if (orderBy != null)
                return await this.GetItemsAsync(GetRoleQuery(request), request, orderBy, nameof(FeatureRolePermissionMaster));
            else
                return await this.GetItemsAsync(GetRoleQuery(request), request, x => x.UserContext.ModifiedOn, nameof(FeatureRolePermissionMaster));
        }

        public async Task<(IEnumerable<Role> result, int count)> GetRolesWithCountAsync(GetAllRoleRequest request)
        {
            var orderBy = OrderBy(request);
            if (orderBy != null)
                return await this.GetItemsWithCountAsync(GetRoleQuery(request), request, orderBy, nameof(FeatureRolePermissionMaster));
            else
                return await this.GetItemsWithCountAsync(GetRoleQuery(request), request, x => x.UserContext.ModifiedOn, nameof(FeatureRolePermissionMaster));
        }

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId);
    }
}