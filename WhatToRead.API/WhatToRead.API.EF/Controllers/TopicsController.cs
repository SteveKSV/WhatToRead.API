using EFTopics.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using EFTopics.DAL.Interfaces;
using AutoMapper;
using EFTopics.DAL.Data;
using EFTopics.DAL.Dtos;

namespace WhatToRead.API.EF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TopicController> _logger;
        private readonly IMapper _mapper;
        private readonly ApplicationContext _dbContext;
        public TopicController(IUnitOfWork unitOfWork, ILogger<TopicController> logger, IMapper mapper, ApplicationContext context)
        {

            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _dbContext = context;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Topic>))]
        public async Task<IActionResult> GetAllTopics()
        {
            try
            {
                var results = _mapper.Map<List<TopicDto>>(await _unitOfWork.TopicsRepository.GetAllEntitiesAsync());

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
                var result = _mapper.Map<TopicDto>(await _unitOfWork.TopicsRepository.GetEntityByIdAsync(id));

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

                var topic = await _unitOfWork.TopicsRepository.GetAllEntitiesAsync();
                topic = topic.Where(p => p.Name.Trim().ToUpper() == topicCreate.Name.Trim().ToUpper());

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

                var topicMap = _mapper.Map<Topic>(topicCreate);

                if (!await _unitOfWork.TopicsRepository.CreateEntityAsync(topicMap))
                {
                    ModelState.AddModelError("", "Щось пішло не так під час зберігання!");
                    return StatusCode(500, ModelState);
                }


                return Ok("Успішно доданий новий Topic!");
            }

            catch (Exception ex)
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

                var topicToDelete = await _unitOfWork.TopicsRepository.GetEntityByIdAsync(topicId);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!await _unitOfWork.TopicsRepository.DeleteEntityAsync(topicToDelete))
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

                var topicMap = _mapper.Map<Topic>(updatedTopic);

                if (!await _unitOfWork.TopicsRepository.UpdateEntityAsync(topicMap))
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
