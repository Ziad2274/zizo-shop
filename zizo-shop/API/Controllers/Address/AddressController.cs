using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zizo_shop.Application.Features.Address.Commands;
using zizo_shop.Application.Features.Address.Queries;

namespace zizo_shop.API.Controllers.Address
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AddressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyAddresses()
            => Ok(await _mediator.Send(new GetMyAddressesQuery()));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAddressCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetMyAddresses), new { id }, new { Id = id });
        }

        [HttpDelete("{addressId:guid}")]
        public async Task<IActionResult> Delete(Guid addressId)
        {
            await _mediator.Send(new DeleteAddressCommand(addressId));
            return Ok("Address deleted successfully.");
        }
    }
}
