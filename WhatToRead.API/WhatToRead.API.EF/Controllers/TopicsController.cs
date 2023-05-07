using EFTopics.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using EFTopics.DAL.Data;
using EFWhatToRead_DAL.Repositories.Interfaces;
using EFWhatToRead_BBL.Managers.Interfaces;
using EFTopics.DAL.Dtos;
using EFWhatToRead_DAL.Params;
using Microsoft.Extensions.Hosting;

namespace WhatToRead.API.EF.Controllers
{
    /// <summary>
    /// This api handles all logic for Topic
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly ILogger<TopicController> _logger;
        private readonly ApplicationContext _dbContext;
        private ITopicManager TopicManager { get; }

        /// <param name="logger"></param>
        /// <param name="topicManager"></param>
        /// <param name="mapper"></param>
        /// <param name="context"></param>
        public TopicController(ITopicManager topicManager, ILogger<TopicController> logger, IMapper mapper, ApplicationContext context)
        {
            _logger = logger;
            _dbContext = context;
            TopicManager = topicManager;
        }


        /// <summary>
        /// Returns all topics async.
        /// </summary>
        /// <param name="pagination">Page number and page size to view</param>
        /// <param name="name">Search by name</param>
        /// <param name="sortByName">Sort by name (asc or desc)</param>
        /// <returns>Topics with all their information</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /api/topics
        ///
        /// </remarks>
        /// <response code="200">Returns all topics with all their information</response>
        /// <response code="400"></response>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TopicDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllTopics([FromQuery] PageModel pagination, [FromQuery] string? name = null, [FromQuery] string? sortByName = null)
        {
            try
            {
                // Pagination
                var topics = await TopicManager.GetAllTopics(pagination);

                // Searching
                if (!string.IsNullOrEmpty(name))
                {
                    topics = topics.Where(p => p.Name.Contains(name));
                }

                // Sorting
                if (!string.IsNullOrEmpty(sortByName))
                {
                    if (sortByName.ToLower() == "asc")
                    {
                        topics = topics.OrderBy(p => p.Name);
                    }
                    else if (sortByName.ToLower() == "desc")
                    {
                        topics = topics.OrderByDescending(p => p.Name);
                    }
                }
                _logger.LogInformation($"Отримали всі дані з бази даних!");
                return Ok(topics);
            }

            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetAllTopics() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }


        /// <summary>
        /// Returns topic by id async.
        /// </summary>
        /// <param name="id">The id of topic</param>
        /// <returns>Topic by id with all it's information</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /api/topics/5
        ///
        /// </remarks>
        /// <response code="200">Returns topic by id with all it's information</response>
        /// <response code="400">This topic doesn't exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(TopicDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetTopicById(int id)
        {
            try
            {
                var result = await TopicManager.GetTopicById(id);

                if (result == null)
                    return BadRequest();
                _logger.LogInformation($"Отримали всі дані з бази даних!");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetTopicById() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }


        /// <summary>
        /// Creates a new topic.
        /// </summary>
        /// <param name="topicCreate">Topic to add</param>
        /// <returns>StatusCode</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     post /api/author/
        ///
        /// </remarks>
        /// <response code="200">Topic is created successfully</response>
        /// <response code="400">There is some problem in method or invalid input</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Insert([FromBody] TopicDto topicCreate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest(ModelState);
                }

                if (topicCreate == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт Topic null");
                }

                var topic = await TopicManager.CreateTopic(topicCreate);

                if (topic == null)
                {
                    ModelState.AddModelError("", "Topic вже існує");
                    return StatusCode(422, ModelState);
                }

                

                return Ok("Успішно доданий новий Topic!");
            } catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі Insert - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        /// <summary>
        /// Deletes an topic by id.
        /// </summary>
        /// <param name="topicId">The id of topic</param>
        /// <returns>StatusCode</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     delete /api/topic/5
        ///
        /// </remarks>
        /// <response code="200">Topic is deleted successfully</response>
        /// <response code="400">There is some problem in method</response>
        [HttpDelete]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int topicId)
        {
            try
            {
                if (!_dbContext.Topics.Any(c => c.TopicId == topicId))
                {
                    return NotFound();
                }

                var topicToDelete = await TopicManager.DeleteTopicById(topicId);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!topicToDelete)
                {
                    ModelState.AddModelError("", "Щось пішло не так під час видалення Topic!");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі Delete() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        /// <summary>
        /// Updates an topic by id.
        /// </summary>
        /// <param name="topicId">The id of topic</param>
        /// <param name="updatedTopic">Updated topic</param>
        /// <returns>StatusCode</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     put /api/topic/5
        ///
        /// </remarks>
        /// <response code="200">Topic is updated successfully</response>
        /// <response code="400">There is some problem in method</response>
        [HttpPut]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int topicId, [FromBody] TopicDto updatedTopic)
        {
            try
            {
                if (updatedTopic == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт Topic є null");
                }

                if (topicId != updatedTopic.TopicId)
                {
                    return BadRequest(ModelState);
                }

                if (!_dbContext.Topics.Any(c => c.TopicId == topicId))
                    return NotFound();

                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Об'єкт topic є некоректним");
                }

                if (!await TopicManager.UpdateTopicById(updatedTopic))
                {
                    ModelState.AddModelError("", "Щось пішло не так під час оновлення Topic!");
                    return StatusCode(500, ModelState);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі Update - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }


        /// <summary>
        /// Get all topics that have posts
        /// </summary>
        /// <returns>List of topics</returns>
        [HttpGet("GetTopicsWithPosts")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTopicsWithPosts()
        {
            try
            {
                var topics = await TopicManager.GetAllTopicsWithPosts();

                if (topics == null)
                    return BadRequest("Немає!");

                _logger.LogInformation($"Отримали всі дані з бази даних!");
                return Ok(topics);
            }

            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetTopicsWithPosts() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        /// <summary>
        /// Get topic that have posts by id
        /// </summary>
        /// <param name="id">Topic id</param>
        /// <returns>Topic with posts</returns>
        [HttpGet("GetTopicByIdWithPosts{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTopicByIdWithPosts(int id)
        {
            try
            {
                var topic = await TopicManager.GetTopicByIdWithPosts(id);

                if (topic == null)
                    return BadRequest("Немає!");

                _logger.LogInformation($"Отримали всі дані з бази даних!");
                return Ok(topic);
            }

            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetTopicByIdWithPosts() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }
    }
}
