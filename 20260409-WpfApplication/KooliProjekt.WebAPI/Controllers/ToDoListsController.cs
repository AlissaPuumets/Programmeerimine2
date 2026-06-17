using System.Threading.Tasks;
using KooliProjekt.Application.Features.ToDoLists;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class ToDoListsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ToDoListsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] ListToDoListsQuery query)
        {
            var response = await _mediator.Send(query);

            return Result(response);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetToDoListQuery { Id = id };
            var response = await _mediator.Send(query);

            return Result(response);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveToDoListCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }

        // 15.11.2025
        // API pöördumispunkt ToDoListi kustutamiseks
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteToDoListCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }
    }
}
