using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WhatToRead.API.AdoNet.DB.Repositories.Interfaces;
using WhatToRead.API.Core.Models;
using WhatToRead.Core.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace WhatToRead.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BookController> _logger;
        public BookController(IUnitOfWork unitOfWork, ILogger<BookController> logger)
        {

            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: api/book
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            try
            {
                var results = await _unitOfWork.Books.GetAllAsync();
                _unitOfWork.Commit();
                _logger.LogInformation($"Отримали всі книги з бази даних!");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetAllAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        // GET: api/book/Id
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var result = await _unitOfWork.Books.GetByIdAsync(id);
                _unitOfWork.Commit();
                if (result == null)
                {
                    _logger.LogInformation($"Книга із Id: {id}, не була знайдена у базі даних");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Отримали книгу з бази даних!");
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetByIdAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        //GET: api/book/id
        [Route("GetBookByAuthorId/{id}")]
        [HttpGet]


        public async Task<ActionResult> GetBookByAuthorId(int id)
        {
            try
            {
                var result = await _unitOfWork.Books.GetBookByAuthorId(id);
                _unitOfWork.Commit();
                if (result == null)
                {
                    _logger.LogInformation($"Книга із Author_Id: {id}, не була знайдена у базі даних");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Отримали книгу з бази даних!");
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetBookByAuthorId() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        // POST: api/book
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Book entity)
        {
            try
            {
                if (entity == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Об'єкт книга є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Об'єкт книга є некоректним");
                }
                var created_id = await _unitOfWork.Books.AddAsync(entity);
                _unitOfWork.Commit();
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі AddAsync - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        //GET: api/book/Id
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var entity = await _unitOfWork.Books.GetByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogInformation($"Книга із Id: {id}, не була знайдена у базі даних");
                    return NotFound();
                }

                await _unitOfWork.Books.DeleteAsync(id);
                _unitOfWork.Commit();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі DeleteAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }
        //POST: api/book/id
        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] Book book)
        {
            try
            {
                if (book == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Об'єкт книга є null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Об'єкт книга є некоректним");
                }

                var entity = await _unitOfWork.Books.GetByIdAsync(id);
                if (entity == null)
                {
                    _logger.LogInformation($"Книга із Id: {id}, не була знайдена у базі даних");
                    return NotFound();
                }

                await _unitOfWork.Books.UpdateAsync(book);
                _unitOfWork.Commit();
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі PostEventAsync - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }
    }
}

