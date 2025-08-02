using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PropVivo.Application.Common.Base;
using PropVivo.Application.Dto.RoleFeature.CreateRole;
using PropVivo.Application.Dto.RoleFeature.DeleteRole;
using PropVivo.Application.Dto.RoleFeature.GetAllRole;
using PropVivo.Application.Dto.RoleFeature.UpdateRole;

namespace PropVivo.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class RoleController : BaseController
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [ProducesDefaultResponseType(typeof(BaseResponse<CreateRoleResponse>))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateRoleResponse>> CreateRoleAsync([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesDefaultResponseType(typeof(BaseResponse<DeleteRoleResponse>))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRoleAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var response = await _mediator.Send(new DeleteRoleRequest { RoleId = id });
            return Ok(response);
        }

        [HttpGet()]
        [ProducesDefaultResponseType(typeof(BaseResponsePagination<RoleItem>))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRoleAsync([FromQuery] GetAllRoleRequest roleRequest)
        {
            var response = await _mediator.Send(roleRequest);
            return Ok(response);
        }

        [HttpPut("{id}")]
        [ProducesDefaultResponseType(typeof(BaseResponse<UpdateRoleResponse>))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRoleAsync([FromBody] UpdateRoleRequest roleEditRequest, string id)
        {
            roleEditRequest.RoleId = string.IsNullOrEmpty(roleEditRequest.RoleId) ? id : roleEditRequest.RoleId;
            var response = await _mediator.Send(roleEditRequest);
            return Ok(response);
        }
    }
}