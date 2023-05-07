using Microsoft.AspNetCore.Mvc;
using WhatToRead.API.AdoNet.BBL.Dtos;
using WhatToRead.API.AdoNet.BBL.Managers.Interfaces;
using WhatToRead.API.AdoNet.DB.Repositories.Interfaces;
using WhatToRead.API.Core.Models;
using WhatToRead.Core.Models;

namespace WhatToRead.API.Controllers
{
    /// <summary>
    /// This api handles all logic for languages 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LanguageController> _logger;
        private readonly ILanguageManager _languageManager;

        /// <summary>
        ///
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="languageManager"></param>
        public LanguageController(IUnitOfWork unitOfWork, ILogger<LanguageController> logger, ILanguageManager languageManager)
        {

            _unitOfWork = unitOfWork;
            _logger = logger;
            _languageManager = languageManager;
        }

        /// <summary>
        /// Returns all languages async.
        /// </summary>
        /// <returns>Languages information</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /api/language
        ///
        /// </remarks>
        /// <response code="200">Returns all languages information</response>
        /// <response code="400"></response>
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            try
            {
                var results = await _languageManager.GetAllEntities();
                _logger.LogInformation($"Отримали всі мови з бази даних!");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetAllAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        /// <summary>
        /// Returns language by id async.
        /// </summary>
        /// <param name="id">The id of language</param>
        /// <returns>Language by id</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /api/language/5
        ///
        /// </remarks>
        /// <response code="200">Returns language by id</response>
        /// <response code="400">This language doesn't exist</response>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var result = await _languageManager.GetEntityById(id);
                if (result == null)
                {
                    _logger.LogInformation($"Мова із Id: {id}, не була знайдена у базі даних");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Отримали мова з бази даних!");
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetByIdAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        /// <summary>
        /// Creates a new language.
        /// </summary>
        /// <param name="entity">Language to add</param>
        /// <returns>StatusCode</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     post /api/language/
        ///
        /// </remarks>
        /// <response code="200">Language is created successfully</response>
        /// <response code="400">There is some problem in method or invalid input</response>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Book_LanguageDTO entity)
        {
            try
            {
                if (entity == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Об'єкт мова є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Об'єкт мова є некоректним");
                }
                var created_id = await _languageManager.Create(entity);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі AddAsync - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        /// <summary>
        /// Deletes an language by id.
        /// </summary>
        /// <param name="id">The id of language</param>
        /// <returns>StatusCode</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     delete /api/language/5
        ///
        /// </remarks>
        /// <response code="200">Language is deleted successfully</response>
        /// <response code="400">There is some problem in method</response>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entity = await _languageManager.GetEntityById(id);
                if (entity == null)
                {
                    _logger.LogInformation($"Мова із Id: {id}, не була знайдена у базі даних");
                    return NotFound();
                }

                await _languageManager.DeleteEntityById(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі DeleteAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        /// <summary>
        /// Updates an language by id.
        /// </summary>
        /// <param name="id">The id of language</param>
        /// <param name="language">Updated language</param>
        /// <returns>StatusCode</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     put /api/language/5
        ///
        /// </remarks>
        /// <response code="200">Language is updated successfully</response>
        /// <response code="400">There is some problem in method</response>
        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] Book_LanguageDTO language)
        {
            try
            {
                if (language == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Об'єкт мова є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Об'єкт мова є некоректним");
                }

                var entity = await _languageManager.GetEntityById(id);
                if (entity == null)
                {
                    _logger.LogInformation($"Мова із Id: {id}, не був знайдена у базі даних");
                    return NotFound();
                }

                await _languageManager.UpdateEntityById(language);
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
