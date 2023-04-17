using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WhatToRead.API.AdoNet.DB.Repositories.Interfaces;
using WhatToRead.API.Core.Models;
using WhatToRead.Core.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace WhatToRead.API.Controllers
{
    /// <summary>
    /// This api handles all logic for books 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BookController> _logger;

        /// <summary>
        ///
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="unitOfWork"></param>
        public BookController(IUnitOfWork unitOfWork, ILogger<BookController> logger)
        {

            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Returns all books async.
        /// </summary>
        /// <returns>Books with all their information</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /api/book
        ///
        /// </remarks>
        /// <response code="200">Returns all books with all their information</response>
        /// <response code="400"></response>
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

        /// <summary>
        /// Returns book by id async.
        /// </summary>
        /// <param name="id">The id of book</param>
        /// <returns>Book by id with all its information</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /api/book/5
        ///
        /// </remarks>
        /// <response code="200">Returns book by id with all its information</response>
        /// <response code="400">This book doesn't exist</response>
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

        /// <summary>
        /// Returns a book by author's id.
        /// </summary>
        /// <param name="id">The id of author</param>
        /// <returns>Book by author's id</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /api/book/GetBookByAuthorId/5
        ///
        /// </remarks>
        /// <response code="200">Returns a book by author's id</response>
        /// <response code="400">This book by author's id doesn't exist</response>
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

        /// <summary>
        /// Returns books with publisher name.
        /// </summary>
        /// <returns>Books with publisher name</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /api/book/GetAllBooksWithPublisher
        ///
        /// </remarks>
        /// <response code="200">Returns books with publisher name</response>
        /// <response code="400">There is a problem in method or books with publishers don't exist</response>
        [Route("GetAllBooksWithPublisher")]
        [HttpGet]
        public async Task<ActionResult> GetAllBooksWithPublisher()
        {
            try
            {
                var result = await _unitOfWork.Books.GetAllBooksWithPublisherName();
                _unitOfWork.Commit();
                _logger.LogInformation($"Отримали книгу з publisher з бази даних!");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetAllBooksWithPublisherName() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        /// <summary>
        /// Returns a book with publisher name by id.
        /// </summary>
        /// <param name="id">The id of bookByPublisher model</param>
        /// <returns>Book with publisher name by id</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /api/book/GetAllBooksWithPublisher/5
        ///
        /// </remarks>
        /// <response code="200">Returns a book with publisher name by id.</response>
        /// <response code="400">There is a problem in method or books with publisher by this id don't exist</response>
        [Route("GetAllBooksWithPublisher/{id}")]
        [HttpGet]
        public async Task<ActionResult> GetBookByPublisher(int id)
        {
            try
            {
                var result = await _unitOfWork.Books.GetBookByPublisherId(id);
                _unitOfWork.Commit();
                if (result == null)
                {
                    _logger.LogInformation($"Книга із Publisher_Id: {id}, не була знайдена у базі даних");
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
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetBookByPublisherId() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        /// <summary>
        /// Returns books which date is higher than date in params.
        /// </summary>
        /// <param name="date">The date that will be used for search of books</param>
        /// <returns>Books by date up</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /api/book/GetBooksByDateUp/date
        ///
        /// </remarks>
        /// <response code="200">Returns books which date is higher than date in params</response>
        /// <response code="400">There is a problem in method or books which date is higher than date in params don't exist</response>
        [Route("GetBooksByDateUp/{date}")]
        [HttpGet]
        public async Task<ActionResult> GetBookByPublisher(DateTime date)
        {
            try
            {
                var result = await _unitOfWork.Books.GetBooksByDateUp(date);
                _unitOfWork.Commit();
                if (result == null)
                {
                    _logger.LogInformation($"Книги з датою вище: {date}, не були знайдені у базі даних");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Отримали книги з бази даних!");
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetBookByDateUp() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        /// <summary>
        /// Creates a new book.
        /// </summary>
        /// <param name="entity">Entity is a book</param>
        /// <returns>StatusCode</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     post /api/book/
        ///
        /// </remarks>
        /// <response code="200">Book is created successfully</response>
        /// <response code="400">There is some problem in method or invalid input</response>
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

        /// <summary>
        /// Deletes a book by id.
        /// </summary>
        /// <param name="id">The id of book</param>
        /// <returns>StatusCode</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     delete /api/book/5
        ///
        /// </remarks>
        /// <response code="200">Book is deleted successfully</response>
        /// <response code="400">There is some problem in method or book by this id doesn't exist</response>
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

        /// <summary>
        /// Updates a book by id.
        /// </summary>
        /// <param name="id">The id of book</param>
        /// <param name="book">Updated book</param>
        /// <returns>StatusCode</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     put /api/book/5
        ///
        /// </remarks>
        /// <response code="200">Book is updated successfully</response>
        /// <response code="400">There is some problem in method book by this id doesn't exist</response>
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

