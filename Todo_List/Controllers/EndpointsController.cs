using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Todo_List.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EndpointsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EndpointsController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
