using AutoMapper;
using AwesomeDevEvents.API.Entities;
using AwesomeDevEvents.API.Models;
using AwesomeDevEvents.API.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AwesomeDevEvents.API.Controllers
{
    [Route("api/dev-events")]
    [ApiController]
    public class DevEventsController : ControllerBase
    {
        private readonly DevEventsDbContext _context;  // injeção de dependência
        private readonly IMapper _mapper;

        public DevEventsController(DevEventsDbContext context, IMapper mapper)  // construtor
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Obter todos os eventos
        /// </summary>
        /// <returns>Coleção de eventos</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)] // código de retorno
        public IActionResult GetAll()
        {
            var devEvents = _context.DevEvents.Where(d => !d.IsDeleted).ToList(); // trazer todos os dados menos os cancelados

            var viewModel = _mapper.Map<List<DevEventViewModel>>(devEvents); // pegando os dados mapeados no AutoMapper e jogando numa lista
            return Ok(viewModel);  // returna status 200
        }
        /// <summary>
        /// Obter um evento
        /// </summary>
        /// <param name="id">Identificador do evento</param>
        /// <returns>Dados do evento</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // codigo de retorno
        [ProducesResponseType(StatusCodes.Status404NotFound)] // codigo de retorno
        public IActionResult GetById(Guid id)
        {
            var devEvent = _context.DevEvents
                .Include(de => de.Speakers) // inclusão dos speakers
                .SingleOrDefault(d => d.Id == id); // retorna todos os dados de um id

            if (devEvent == null)
            {
                return NotFound(); // se não encontrar o registro retorna status 404 
            }
            
            var viewModel = _mapper.Map<DevEventViewModel>(devEvent);   // pegando os dados mapeados no AutoMapper
            return Ok(viewModel); // se encontrar retorna status 200 (sucesso)
        }
        /// <summary>
        /// Cadastrar um evento
        /// </summary>
        /// <remarks>
        /// {"title":"string","description":"string","startDate":"2023-09-18T02:02:19.096Z","endDate":"2023-09-18T02:02:19.096Z"}
        /// </remarks>
        /// <param name="input">Dados do evento</param>
        /// <returns>Objeto recem-criado</returns>
        /// <response code="201">Sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)] // codigo de retorno
        public IActionResult Post(DevEventInputModel input)
        {
            var devEvent = _mapper.Map<DevEvent>(input);   //passando o modelo de entrada para o domínio 

            _context.DevEvents.Add(devEvent); // adiciona o objeto no contexto (em memória)
            _context.SaveChanges();   //salva os dados no banco 

            return CreatedAtAction(nameof(GetById), new { id = devEvent.Id }, devEvent); // return status 201
                                                                                         // api que vai recuperar o objeto, parâmetro, objeto
        }
        /// <summary>
        /// Atualizar o evento
        /// </summary>
        /// <remarks>
        /// {"title":"string","description":"string","startDate":"2023-09-18T02:02:19.096Z","endDate":"2023-09-18T02:02:19.096Z"}
        /// </remarks>
        /// <param name="id">Identificador do evento</param>
        /// <param name="input">Dados do evento</param>
        /// <returns>Nada.</returns>
        /// <response code="404">Não encontrado.</response>
        /// <response code="204">Sucesso</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // codigo de retorno
        [ProducesResponseType(StatusCodes.Status204NoContent)] // codigo de retorno
        public IActionResult Update(Guid id, DevEventInputModel input)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(d => d.Id == id); // retorna todos os dados de um id

            if (devEvent == null)
            {
                return NotFound(); // se não encontrar o registro retorna status 404 
            }

            devEvent.Update(input.Title, input.Description, input.StartDate, input.EndDate); // move os dados para a entidade DevEvents
            _context.DevEvents.Update(devEvent);  // Este update é metodo do DbSet
            _context.SaveChanges();  // salva no banco de dados

            return NoContent(); // retorna status 204 (sucesso)
        }
        /// <summary>
        /// Deletar um evento
        /// </summary>
        /// <param name="id">Identificador de evento</param>
        /// <returns>Nada</returns>
        /// <response code="404">Não encontrado</response>
        /// <response code="204">Sucesso</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // codigo de retorno
        [ProducesResponseType(StatusCodes.Status204NoContent)] // codigo de retorno
        public IActionResult Delete(Guid id)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(d => d.Id == id); // retorna todos os dados de um id

            if (devEvent == null)
            {
                return NotFound(); // se não encontrar o registro retorna status 404 
            }

            devEvent.Delete(); // deleta o registro

            _context.SaveChanges();  // salva no banco de dados

            return NoContent(); // retorna status 204 (sucesso)
        }
        /// <summary>
        /// Cadastrar palestrante
        /// </summary>        
        /// <remarks>
        /// {"name":"string","talkTitle":"string","talkDescription":"string","linkedInProfile":"string"}
        /// </remarks>
        /// <param name="id">Identificador do evento</param>
        /// <param name="input">Dados do palestrante</param>
        /// <returns>Nada</returns>
        /// <response code="404">Evento não encontrado</response>
        /// <response code="204">Sucesso</response>
        [HttpPost("{id}/speakers")]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // codigo de retorno
        [ProducesResponseType(StatusCodes.Status204NoContent)] // codigo de retorno
        public IActionResult PostSpeaker(Guid id, DevEventSpeakerInputModel input)
        {
            var speaker = _mapper.Map<DevEventSpeaker>(input);  //passando o modelo de entrada para o domínio
            speaker.DevEventId = id;  // pegar o id do DevEvent e mover para o Speaker (chave estrangeira)

            var devEvent = _context.DevEvents.Any(d => d.Id == id); // verifica se existe o DevEvents

            if (!devEvent)
            {
                return NotFound(); // se não encontrar o registro retorna status 404 
            }

            _context.DevEventSpeakers.Add(speaker); // adiciona a entidade speaker na tabela DevEventSpeakers
            _context.SaveChanges();  // salva no banco de dados

            return NoContent();  // retorna status 204 (sucesso)
        }
    }
}
