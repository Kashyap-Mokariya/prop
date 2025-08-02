using MediatR;
using PropVivo.Application.Common.Base;

namespace PropVivo.Application.Dto.RoleFeature.DeleteRole
{
    public class DeleteRoleRequest : ExecutionRequest, IRequest<BaseResponse<DeleteRoleResponse>>
    {
        public string? RoleId { get; set; }
    }
}