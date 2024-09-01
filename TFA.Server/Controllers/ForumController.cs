using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TFA.Domain.UseCases.CreateForum;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Domain.UseCases.GetForums;
using TFA.Domain.UseCases.GetTopics;
using TFA.Server.Models;

namespace TFA.Server.Controllers
{
    [ApiController]
    [Route("api/forums")]
    public class ForumController : ControllerBase
    {
        private readonly IMapper mapper;

        public ForumController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        [HttpPost()]
        [ProducesResponseType(201, Type = typeof(Forum))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateForum([FromServices] ICreateForumUseCase useCase,
            [FromBody] CreateForum createForum,
            CancellationToken cancellationTokens)
        {
            var cmd = new CreateForumCommand(createForum.Title);
            var forum = await useCase.Execute(cmd, cancellationTokens);
            return CreatedAtAction(nameof(CreateForum), mapper.Map<Forum>(forum));
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Forum))]
        public async Task<IActionResult> GetForums([FromServices] IGetForumsUseCase useCase, 
            CancellationToken cancellationToken)
        {
            var forums = await useCase.Execute(cancellationToken);
            return Ok(forums.Select(mapper.Map<Forum>));
        }

        [HttpPost("{forumId}/topics")]
        [ProducesResponseType(201, Type = typeof(Topic))]
        [ProducesResponseType(400)]
        [ProducesResponseType(410)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateTopic([FromServices] ICreateTopicUseCase useCase,
            Guid forumId, [FromBody] CreateTopic createTopic, CancellationToken cancellationToken)
        {
            var cmd = new CreateTopicCommand(forumId, createTopic.Title);
            var topic = await useCase.Execute(cmd, cancellationToken);

            return CreatedAtAction(nameof(CreateTopic), mapper.Map<Topic>(topic));
        }

        [HttpGet("{forumId}/topics")]
        [ProducesResponseType(200, Type = typeof(Topic))]
        [ProducesResponseType(410)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetTopics([FromServices] IGetTopicsUseCase useCase,
            [FromRoute] Guid forumId, [FromQuery] int skip, [FromQuery] int take, 
            CancellationToken cancellationToken)
        {
            var (resources, count) = await useCase.Execute(new GetTopicsQuery(forumId, skip, take), cancellationToken);
            return Ok(new { resources = resources.Select(mapper.Map<Topic>), count });
        }
    }
}
