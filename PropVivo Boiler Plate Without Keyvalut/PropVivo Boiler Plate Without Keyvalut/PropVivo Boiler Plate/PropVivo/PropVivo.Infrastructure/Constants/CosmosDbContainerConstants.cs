using PropVivo.Domain.Entities.FeatureRolePermissionMaster;
using PropVivo.Domain.Entities.ServiceRequest;
using PropVivo.Domain.Entities.Customer;
using PropVivo.Domain.Entities.Call;

namespace PropVivo.Infrastructure.Constants
{
    public class CosmosDbContainerConstants
    {
        public const string CONTAINER_NAME_FEATUREROLEPERMISSIONMASTER = nameof(FeatureRolePermissionMaster);
        public const string CONTAINER_NAME_ServiceRequest = nameof(ServiceRequests);
        // Add these lines to existing file
        public const string CONTAINER_NAME_CUSTOMER = nameof(Customer);
        public const string CONTAINER_NAME_CALL = nameof(Call);

    }
}