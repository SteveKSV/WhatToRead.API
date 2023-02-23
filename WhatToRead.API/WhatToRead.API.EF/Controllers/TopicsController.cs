using EFTopics.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using TeamworkSystem.DataAccessLayer.Interfaces;

namespace WhatToRead.API.EF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TopicsController> _logger;
        public TopicsController(IUnitOfWork unitOfWork, ILogger<TopicsController> logger)
        {

            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            try
            {
                var results = await _unitOfWork.TopicsRepository.GetAllTopicsAsync();

                _logger.LogInformation($"Отримали всі дані з бази даних!");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetAllTopicsAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTopicByIdAsync(int id)
        {
            try
            {
                var results = await _unitOfWork.TopicsRepository.GetTopicByIdAsync(id);

                _logger.LogInformation($"Отримали всі дані з бази даних!");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetTopicByIdAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        [HttpPost]
        public async Task<ActionResult> InsertAsync([FromBody] Topics topic)
        {
            try
            {
                if (topic == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт topic null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Обєкт topic є некоректним");
                }
                await _unitOfWork.TopicsRepository.InsertAsync(topic);
                await _unitOfWork.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі InsertAsync - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.TopicsRepository.GetTopicByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogInformation($"Topic із Id: {id}, не був знайдейний у базі даних");
                    return NotFound();
                }

                await _unitOfWork.TopicsRepository.DeleteAsync(id);
                 await _unitOfWork.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі DeleteAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Topics topic)
        {
            try
            {
                if (topic == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт topic є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Об'єкт topic є некоректним");
                }

                var entity = await _unitOfWork.TopicsRepository.GetTopicByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogInformation($"Topic із Id: {id}, не був знайдейний у базі даних");
                    return NotFound();
                }

                await _unitOfWork.TopicsRepository.UpdateAsync(id, topic);
                await _unitOfWork.SaveChangesAsync();
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі UpdateAsync - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }
    }
}
