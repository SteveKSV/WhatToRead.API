using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WhatToRead.API.AdoNet.DB.Repositories.Interfaces;
using WhatToRead.API.Core.Models;
using WhatToRead.Core.Models;

namespace WhatToRead.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuthorController> _logger;
        public AuthorController(IUnitOfWork unitOfWork, ILogger<AuthorController> logger)
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
                var results = await _unitOfWork.Authors.GetAllAsync();
                _unitOfWork.Commit();
                _logger.LogInformation($"Отримали всі дані з бази даних!");
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
                var result = await _unitOfWork.Authors.GetByIdAsync(id);
                _unitOfWork.Commit();
                if (result == null)
                {
                    _logger.LogInformation($"Автор із Id: {id}, не був знайдейний у базі даних");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Отримали автора з бази даних!");
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
        public async Task<IActionResult> Add([FromBody] Author author)
        {
            try
            {
                if (author == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт автор є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Обєкт автор є некоректним");
                }
                var created_id = await _unitOfWork.Authors.AddAsync(author);
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
                var entity = await _unitOfWork.Authors.GetByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogInformation($"Автор із Id: {id}, не був знайдейний у базі даних");
                    return NotFound();
                }

                await _unitOfWork.Authors.DeleteAsync(id);
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
        public async Task<IActionResult> Update(int id, [FromBody] Author author)
        {
            try
            {
                if (author == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт автор є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Об'єкт автор є некоректним");
                }

                var entity = await _unitOfWork.Authors.GetByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogInformation($"Автор із Id: {id}, не був знайдейний у базі даних");
                    return NotFound();
                }

                await _unitOfWork.Authors.UpdateAsync(author);
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
