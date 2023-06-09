using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WhatToRead.API.AdoNet.BBL.Dtos;
using WhatToRead.API.AdoNet.BBL.Managers.Interfaces;
using WhatToRead.API.AdoNet.DB.Repositories.Interfaces;
using WhatToRead.API.Core.Models;
using WhatToRead.Core.Models;

namespace WhatToRead.API.Controllers
{
    /// <summary>
    /// This api handles all logic for authors. 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuthorController> _logger;
        private readonly IAuthorManager _authorManager;
        /// <summary>
        ///
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="authorManager"></param>
        public AuthorController(IUnitOfWork unitOfWork, ILogger<AuthorController> logger, IAuthorManager authorManager)
        {

            _unitOfWork = unitOfWork;
            _logger = logger;
            _authorManager = authorManager;
        }

        /// <summary>
        /// Returns all authors async.
        /// </summary>
        /// <returns>Authors with all their information</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /api/author
        ///
        /// </remarks>
        /// <response code="200">Returns all authors with all their information</response>
        /// <response code="400"></response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetAllAsync()
        {
            try
            {
                var results = await _authorManager.GetAllEntities();
                _logger.LogInformation($"Отримали всі дані з бази даних!");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetAllAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        /// <summary>
        /// Returns author by id async.
        /// </summary>
        /// <param name="id">The id of author</param>
        /// <returns>Author by id with all his/her information</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /api/author/5
        ///
        /// </remarks>
        /// <response code="200">Returns author by id with all his/her information</response>
        /// <response code="400">This author doesn't exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var result = await _authorManager.GetEntityById(id);
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

        /// <summary>
        /// Creates a new author.
        /// </summary>
        /// <param name="author">Author to add</param>
        /// <returns>StatusCode</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     post /api/author/
        ///
        /// </remarks>
        /// <response code="200">Author is created successfully</response>
        /// <response code="400">There is some problem in method or invalid input</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] AuthorDTO author)
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
                var created_id = await _authorManager.Create(author);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі AddAsync - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        /// <summary>
        /// Deletes an author by id.
        /// </summary>
        /// <param name="id">The id of author</param>
        /// <returns>StatusCode</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     delete /api/author/5
        ///
        /// </remarks>
        /// <response code="200">Author is deleted successfully</response>
        /// <response code="400">There is some problem in method</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entity = await _authorManager.GetEntityById(id);
                if (entity == null)
                {
                    _logger.LogInformation($"Автор із Id: {id}, не був знайдейний у базі даних");
                    return NotFound();
                }

                await _authorManager.DeleteEntityById(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі DeleteAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        /// <summary>
        /// Updates an author by id.
        /// </summary>
        /// <param name="id">The id of author</param>
        /// <param name="author">Updated author</param>
        /// <returns>StatusCode</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     put /api/author/5
        ///
        /// </remarks>
        /// <response code="200">Author is updated successfully</response>
        /// <response code="400">There is some problem in method</response>
        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorDTO author)
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

                var entity = await _authorManager.GetEntityById(id);
                if (entity == null)
                {
                    _logger.LogInformation($"Автор із Id: {id}, не був знайдейний у базі даних");
                    return NotFound();
                }

                await _authorManager.UpdateEntityById(author);
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
