using Microsoft.AspNetCore.Mvc;
using WhatToRead.API.AdoNet.DB.Repositories.Interfaces;
using WhatToRead.API.Core.Models;

namespace WhatToRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PublisherController> _logger;
        public PublisherController(IUnitOfWork unitOfWork, ILogger<PublisherController> logger)
        {

            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: api/author
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            try
            {
                var results = await _unitOfWork.Publishers.GetAllAsync();
                _unitOfWork.Commit();
                _logger.LogInformation($"Отримали всі publisher з бази даних!");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetAllAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        // GET: api/author/Id
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var result = await _unitOfWork.Publishers.GetByIdAsync(id);
                _unitOfWork.Commit();
                if (result == null)
                {
                    _logger.LogInformation($"Publisher із Id: {id}, не був знайдейний у базі даних");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Отримали publisher з бази даних!");
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetByIdAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        // POST: api/author
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Publisher entity)
        {
            try
            {
                if (entity == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Об'єкт publisher є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Об'єкт publisher є некоректним");
                }
                var created_id = await _unitOfWork.Publishers.AddAsync(entity);
                _unitOfWork.Commit();
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі AddAsync - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        //GET: api/author/Id
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entity = await _unitOfWork.Publishers.GetByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogInformation($"Publisher із Id: {id}, не був знайдейний у базі даних");
                    return NotFound();
                }

                await _unitOfWork.Publishers.DeleteAsync(id);
                _unitOfWork.Commit();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі DeleteAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }
        //POST: api/author/id
        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] Publisher publisher)
        {
            try
            {
                if (publisher == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Об'єкт publisher є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Об'єкт publisher є некоректним");
                }

                var entity = await _unitOfWork.Publishers.GetByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogInformation($"Publisher із Id: {id}, не був знайдейний у базі даних");
                    return NotFound();
                }

                await _unitOfWork.Publishers.UpdateAsync(publisher);
                _unitOfWork.Commit();
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
