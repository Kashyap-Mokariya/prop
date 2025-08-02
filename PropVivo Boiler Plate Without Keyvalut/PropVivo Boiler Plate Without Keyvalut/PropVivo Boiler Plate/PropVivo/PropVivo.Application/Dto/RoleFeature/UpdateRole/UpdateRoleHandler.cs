using AutoMapper;
using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Constants;
using PropVivo.Application.Dto.RoleFeature.GetAllRole;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.FeatureRolePermissionMaster.SupportingTypes;
using System.Net;

namespace PropVivo.Application.Dto.RoleFeature.UpdateRole
{
    public sealed class UpdateRoleHandler : IRequestHandler<UpdateRoleRequest, BaseResponse<UpdateRoleResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        private readonly string type = nameof(Role);

        public UpdateRoleHandler(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<UpdateRoleResponse>> Handle(UpdateRoleRequest updateRoleRequest, CancellationToken cancellationToken)
        {
            if (updateRoleRequest == null || string.IsNullOrEmpty(updateRoleRequest.RoleId))
                throw new BadRequestException(string.Format(Messaging.InvalidRequest));

            var response = new BaseResponse<UpdateRoleResponse>();

            var request = new GetAllRoleRequest
            {
                SubTypeId = updateRoleRequest.LegalEntitySubTypeId,
                CountryId = updateRoleRequest.CountryId,
                BusinessTypeId = updateRoleRequest.BusinessTypeId,
                LegalEntityTypeId = updateRoleRequest.LegalEntityTypeId,
                Name = updateRoleRequest.Name,
                RoleId = updateRoleRequest.RoleId
            };

            var role = await _roleRepository.GetRoleAsync(request, true);
            if (role != null)
                throw new BadRequestException(string.Format(Messaging.AlreadyExist, nameof(Role)));

            role = await _roleRepository.GetItemAsync(request.RoleId, type);
            if (role == null)
                throw new KeyNotFoundException(string.Format(Messaging.NotFound, nameof(Role)));

            var roleDto = _mapper.Map(updateRoleRequest, role);

            if (role.IsMapped == true && (role.Name != updateRoleRequest.Name || role.CountryId != updateRoleRequest.CountryId || role.BusinessTypeId != updateRoleRequest.BusinessTypeId ||
                role.LegalEntityTypeId != updateRoleRequest.LegalEntityTypeId || role.LegalEntitySubTypeId != updateRoleRequest.LegalEntitySubTypeId))
                throw new BadRequestException("Role is already mapped with other item so that you can change it only description and status field.");

            role = await _roleRepository.UpdateItemAsync(request.RoleId, roleDto, type);
            if (role != null)
            {
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = string.Format(Messaging.Update, nameof(Role));
            }

            return response;
        }
    }
}