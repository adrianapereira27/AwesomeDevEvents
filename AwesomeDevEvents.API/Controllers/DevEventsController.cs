using AwesomeDevEvents.API.Entities;
using AwesomeDevEvents.API.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace AwesomeDevEvents.API.Controllers
{
    [Route("api/dev-events")]
    [ApiController]
    public class DevEventsController : ControllerBase
    {
        private readonly DevEventsDbContext _context;  // injeção de dependência

        public DevEventsController(DevEventsDbContext context)  // construtor
        {
            _context = context;
        }

        // rota = api/dev-events/   GET
        [HttpGet]
        public IActionResult GetAll()
        {
            var devEvents = _context.DevEvents.Where(d => !d.IsDeleted).ToList(); // trazer todos os dados menos os cancelados

            return Ok(devEvents);  // returna status 200
        }
        // rota = api/dev-events/245454514   GET
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(d => d.Id == id); // retorna todos os dados de um id

            if (devEvent == null)
            {
                return NotFound(); // se não encontrar o registro retorna status 404 
            }

            return Ok(); // se encontrar retorna status 200 (sucesso)
        }
        // rota = api/dev-events/   POST 
        [HttpPost]
        public IActionResult Post(DevEvent devEvent) 
        {
            _context.DevEvents.Add(devEvent); // adiciona o objeto no contexto (em memória)

            return CreatedAtAction(nameof(GetById), new { id = devEvent.Id }, devEvent); // return status 201
                                // api que vai recuperar o objeto, parâmetro, objeto
        }
        // rota = api/dev-events/245454514   PUT 
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, DevEvent input)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(d => d.Id == id); // retorna todos os dados de um id

            if (devEvent == null)
            {
                return NotFound(); // se não encontrar o registro retorna status 404 
            }

            devEvent.Update(input.Title, input.Description, input.StartDate, input.EndDate); // atualiza os dados

            return NoContent(); // retorna status 204 (sucesso)
        }
        // rota = api/dev-events/245454514   DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(d => d.Id == id); // retorna todos os dados de um id

            if (devEvent == null)
            {
                return NotFound(); // se não encontrar o registro retorna status 404 
            }

            devEvent.Delete(); // deleta o registro

            return NoContent(); // retorna status 204 (sucesso)
        }
        // rota = api/dev-events/245454514/speakers    POST
        [HttpPost("{id}/speakers")]
        public IActionResult PostSpeaker(Guid id, DevEventSpeaker speaker)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(d => d.Id == id); // retorna todos os dados de um id

            if (devEvent == null)
            {
                return NotFound(); // se não encontrar o registro retorna status 404 
            }

            devEvent.Speakers.Add(speaker); // adiciona o speaker na lista

            return NoContent(); // retorna status 204 (sucesso)
        }
    }
}
