using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Constants;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.FeatureRolePermissionMaster;
using PropVivo.Domain.Entities.FeatureRolePermissionMaster.SupportingTypes;
using PropVivo.Domain.Enums;

namespace PropVivo.Application.Dto.RoleFeature.DeleteRole
{
    public sealed class DeleteRoleHandler : IRequestHandler<DeleteRoleRequest, BaseResponse<DeleteRoleResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        private readonly ClaimsPrincipalExtensions _userInfo;
        private readonly string type = nameof(Role);

        public DeleteRoleHandler(IRoleRepository roleRepository, IMapper mapper, ClaimsPrincipalExtensions userInfo)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _userInfo = userInfo;
        }

        public async Task<BaseResponse<DeleteRoleResponse>> Handle(DeleteRoleRequest deleteRoleRequest, CancellationToken cancellationToken)
        {
            if (deleteRoleRequest == null || string.IsNullOrEmpty(deleteRoleRequest.RoleId))
                throw new BadRequestException(string.Format(Messaging.InvalidRequest));

            var roleId = deleteRoleRequest.RoleId;

            var response = new BaseResponse<DeleteRoleResponse>();
            var role = await _roleRepository.GetItemAsync(roleId, type);
            if (role == null)
                throw new NotFoundException(string.Format(Messaging.NotFound, nameof(Role)));

            if (role.IsMapped == true)
                throw new BadRequestException(string.Format(Messaging.Mapped));

            role.Status = Status.Deleted;
            role.UserContext = role.UserContext == null ? _userInfo.GetUserBaseContext(DateTime.UtcNow) : _userInfo.GetUserBaseContext(role.UserContext, DateTime.UtcNow);
            role.SetCustomDocumentType(nameof(FeatureRolePermissionMaster));

            role = await _roleRepository.UpdateItemAsync(roleId, role, type);
            if (role != null)
            {
                response.StatusCode = StatusCodes.Status200OK;
                response.Message = string.Format(Messaging.Delete, nameof(Role));
            }

            return response;
        }

        //var featureRolePermission = await _repo1.GetFeatureRolePermissionAsync(new FeatureRolePermissionRequestV1 { RoleId = id });
        //if (featureRolePermission != null)
        //    throw new APIResponce(string.Format(MessageDisplay.Mapped));
    }
}