using AutoMapper;
using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Constants;
using PropVivo.Application.Dto.RoleFeature.GetAllRole;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.FeatureRolePermissionMaster.SupportingTypes;
using System.Net;

namespace PropVivo.Application.Dto.RoleFeature.CreateRole
{
    public sealed class CreateRoleHandler : IRequestHandler<CreateRoleRequest, BaseResponse<CreateRoleResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        private readonly string type = nameof(Role);

        public CreateRoleHandler(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<CreateRoleResponse>> Handle(CreateRoleRequest createRoleRequest, CancellationToken cancellationToken)
        {
            if (createRoleRequest == null || string.IsNullOrEmpty(createRoleRequest.Name) || createRoleRequest.LegalEntitySubTypes == null)
                throw new BadRequestException(string.Format(Messaging.InvalidRequest));

            var response = new BaseResponse<CreateRoleResponse>();

            var request = new GetAllRoleRequest
            {
                SubTypeId = string.Join(",", createRoleRequest.LegalEntitySubTypes?.Select(x => x.LegalEntitySubTypeId)),
                CountryId = createRoleRequest.CountryId,
                BusinessTypeId = createRoleRequest.BusinessTypeId,
                LegalEntityTypeId = createRoleRequest.LegalEntityTypeId,
                Name = createRoleRequest.Name
            };

            var role = await _roleRepository.GetRoleAsync(request);
            if (role != null)
                throw new BadRequestException(string.Format(Messaging.AlreadyExist, nameof(Role)));

            var roles = new List<Role>();
            foreach (var legalEntitySubType in createRoleRequest.LegalEntitySubTypes)
            {
                var roleDTO = _mapper.Map<Role>(createRoleRequest);
                roleDTO.LegalEntitySubTypeId = legalEntitySubType.LegalEntitySubTypeId;
                roleDTO.LegalEntitySubTypeName = legalEntitySubType.LegalEntitySubTypeName;
                roles.Add(roleDTO);
            }

            var createdRoles = await _roleRepository.AddItemsAsync(roles, type);
            if (createdRoles != null)
            {
                response.Success = true;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = string.Format(Messaging.Insert, nameof(Role));
            }

            return response;
        }
    }
}