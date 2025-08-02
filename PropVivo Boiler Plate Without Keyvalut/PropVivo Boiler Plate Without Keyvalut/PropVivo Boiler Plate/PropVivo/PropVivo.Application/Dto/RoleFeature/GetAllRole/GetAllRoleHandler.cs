using AutoMapper;
using MediatR;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Common.Exceptions;
using PropVivo.Application.Constants;
using PropVivo.Application.Repositories;
using PropVivo.Domain.Entities.FeatureRolePermissionMaster.SupportingTypes;
using System.Net;

namespace PropVivo.Application.Dto.RoleFeature.GetAllRole
{
    public sealed class GetAllRoleHandler : IRequestHandler<GetAllRoleRequest, BaseResponsePagination<GetAllRoleResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        private readonly string type = nameof(Role);

        public GetAllRoleHandler(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponsePagination<GetAllRoleResponse>> Handle(GetAllRoleRequest getAllRoleRequest, CancellationToken cancellationToken)
        {
            if (getAllRoleRequest == null)
                throw new BadRequestException(string.Format(Messaging.InvalidRequest));

            var response = new BaseResponsePagination<GetAllRoleResponse>();

            (var roles, int count) = await _roleRepository.GetRolesWithCountAsync(getAllRoleRequest);
            if (roles != null && roles.Any())
            {
                var data = _mapper.Map<IReadOnlyList<Role>, IReadOnlyList<RoleItem>>(roles.ToList());
                response.Data = new GetAllRoleResponse { Roles = data };
                response.Count = count;
                if (getAllRoleRequest.PageCriteria != null && getAllRoleRequest.PageCriteria.EnablePage)
                {
                    response.Meta = new Meta
                    {
                        Skip = getAllRoleRequest.PageCriteria.Skip,
                        Take = getAllRoleRequest.PageCriteria.PageSize,
                        TotalCount = count
                    };
                }
            }

            response.StatusCode = (int)HttpStatusCode.OK;
            return response;
        }
    }
}