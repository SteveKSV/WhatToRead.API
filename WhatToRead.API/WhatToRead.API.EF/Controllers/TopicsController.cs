using EFTopics.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using EFTopics.DAL.Data;
using EFWhatToRead_DAL.Repositories.Interfaces;
using EFWhatToRead_BBL.Managers.Interfaces;
using EFTopics.DAL.Dtos;
using EFWhatToRead_DAL.Params;

namespace WhatToRead.API.EF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly ILogger<TopicController> _logger;
        private readonly ApplicationContext _dbContext;
        private ITopicManager TopicManager { get; }
        public TopicController(ITopicManager topicManager, ILogger<TopicController> logger, IMapper mapper, ApplicationContext context)
        {
            _logger = logger;
            _dbContext = context;
            TopicManager = topicManager;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Topic>))]
        public async Task<IActionResult> GetAllTopics([FromQuery] PageModel pagination)
        {
            try
            {
                var results = await TopicManager.GetAllTopics(pagination);

                _logger.LogInformation($"Отримали всі дані з бази даних!");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetAllTopics() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Topic))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetTopicById(int id)
        {
            try
            {
                var result = await TopicManager.GetTopicById(id);

                _logger.LogInformation($"Отримали всі дані з бази даних!");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetTopicById() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Insert([FromBody] TopicDto topicCreate)
        {
            try
            {
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

                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Обєкт Topic є некоректним");
                }

                return Ok("Успішно доданий новий Topic!");
            } catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі Insert - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

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
    }
}
