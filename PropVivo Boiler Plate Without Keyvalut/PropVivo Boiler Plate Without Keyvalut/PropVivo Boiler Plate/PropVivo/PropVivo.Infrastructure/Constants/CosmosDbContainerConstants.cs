using PropVivo.Domain.Entities.FeatureRolePermissionMaster;
using PropVivo.Domain.Entities.ServiceRequest;

namespace PropVivo.Infrastructure.Constants
{
    public class CosmosDbContainerConstants
    {
        public const string CONTAINER_NAME_FEATUREROLEPERMISSIONMASTER = nameof(FeatureRolePermissionMaster);
        public const string CONTAINER_NAME_ServiceRequest = nameof(ServiceRequests);
    }
}