using EFTopics.BBL.Data;
using EFTopics.BBL.Dtos;
using EFTopics.BBL.Entities;
using Microsoft.AspNetCore.Mvc;
using EFWhatToRead_BBL.Managers.Interfaces;
using EFWhatToRead_DAL.Params;
using FluentValidation;

namespace WhatToRead.API.EF.Controllers
{
    /// <summary>
    /// This api handles all logic for Post
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private readonly ApplicationContext _dbContext;
        private readonly IValidator<PostDto> _validator;
        private IPostManager PostManager { get; }

        /// <param name="logger"></param>
        /// <param name="postManager"></param>
        /// <param name="validator"></param>
        /// <param name="context"></param>
        public PostController(IPostManager postManager, ILogger<PostController> logger, ApplicationContext context, IValidator<PostDto> validator)
        {
            _logger = logger;
            _dbContext = context;
            PostManager = postManager;
            _validator = validator;
        }

        /// <summary>
        /// Returns a total posts count.
        /// </summary>
        /// <returns>Returns a total posts count</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /api/post
        ///
        /// </remarks>
        /// <response code="200">Returns a total posts count</response>
        /// <response code="400"></response>

        [HttpGet("GetTotalPostCount")]
        public async Task<IActionResult> GetTotalPostCount()
        {
            var count = await PostManager.GetTotalPostCountAsync();
            return Ok(count);
        }
        /// <summary>
        /// Returns all posts async.
        /// </summary>
        /// <param name="pagination">Page number and page size to view</param>
        /// <param name="dateModel">Filtering by start date and end date</param>
        /// <param name="title">Search by title</param>
        /// <param name="sortByTitle">Sort by title (asc or desc)</param>
        /// <returns>Posts with all their information</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /api/post
        ///
        /// </remarks>
        /// <response code="200">Returns all posts with all their information</response>
        /// <response code="400"></response>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Post>))]
        public async Task<IActionResult> GetAllPosts([FromQuery] PageModel pagination, [FromQuery] DateModel? dateModel = null, [FromQuery] string? title = null, [FromQuery] string? sortByTitle = null)
        {

            try
            {
                // Pagination
                var posts = await PostManager.GetAllPosts(pagination);

                // Filtering
                if (dateModel?.StartDate != null && dateModel?.EndDate != null)
                {
                    var startDate = dateModel.StartDate;
                    var endDate = dateModel.EndDate;

                    if (startDate.ToString() != "01.01.0001 0:00:00")
                        posts = posts.Where(p => p.Created_At >= startDate);

                    if (endDate.ToString() != "01.01.0001 0:00:00")
                    {
                        posts = posts.Where(p => p.Created_At <= endDate);
                    }
                }

                // Searching
                if (!string.IsNullOrEmpty(title))
                {
                    posts = posts.Where(p => p.Title.Contains(title));
                }

                // Sorting
                if (!string.IsNullOrEmpty(sortByTitle))
                {
                    if (sortByTitle.ToLower() == "asc")
                    {
                        posts = posts.OrderBy(p => p.Title);
                    }
                    else if (sortByTitle.ToLower() == "desc")
                    {
                        posts = posts.OrderByDescending(p => p.Title);
                    }
                }

                _logger.LogInformation($"Отримали всі дані з бази даних!");
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetAllPosts() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        /// <summary>
        /// Returns topic by id async.
        /// </summary>
        /// <param name="id">The id of post</param>
        /// <returns>Post by id with all it's information</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /api/post/5
        ///
        /// </remarks>
        /// <response code="200">Returns post by id with all it's information</response>
        /// <response code="400">This post doesn't exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Post))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPostById(int id)
        {
            try
            {
                var result = await PostManager.GetPostById(id);

                _logger.LogInformation($"Отримали всі дані з бази даних!");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetPostById() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        /// <summary>
        /// Creates a new post.
        /// </summary>
        /// <param name="postCreate">Post to add</param>
        /// <returns>StatusCode</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     post /api/post/
        ///
        /// </remarks>
        /// <response code="200">Post is created successfully</response>
        /// <response code="400">There is some problem in method or invalid input</response>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Insert([FromBody] PostDto postCreate)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(postCreate);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                if (postCreate == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт post null");
                }

                var post = await PostManager.CreatePost(postCreate);

                if (post == null)
                {
                    ModelState.AddModelError("", "Post вже існує");
                    return StatusCode(422, ModelState);
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest(ModelState);
                }

                return Ok("Успішно доданий новий post!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі Insert - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        /// <summary>
        /// Deletes an topic by id.
        /// </summary>
        /// <param name="postId">The id of post</param>
        /// <returns>StatusCode</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     delete /api/post/5
        ///
        /// </remarks>
        /// <response code="200">Post is deleted successfully</response>
        /// <response code="400">There is some problem in method</response>
        [HttpDelete("{postId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int postId)
        {
            try
            {
                if (!_dbContext.Posts.Any(c => c.PostId == postId))
                {
                    return NotFound();
                }

                var postToDelete = await PostManager.DeletePostById(postId);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!postToDelete)
                {
                    ModelState.AddModelError("", "Щось пішло не так під час видалення post!");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі DeleteAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        /// <summary>
        /// Updates an post by id.
        /// </summary>
        /// <param name="postId">The id of post</param>
        /// <param name="updatedPost">Updated post</param>
        /// <returns>StatusCode</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     put /api/post/5
        ///
        /// </remarks>
        /// <response code="200">Post is updated successfully</response>
        /// <response code="400">There is some problem in method</response>
        [HttpPut("{postId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int postId, [FromBody] PostDto updatedPost)
        {
            try
            {
                if (updatedPost == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт topic є null");
                }

                if (postId != updatedPost.PostId)
                {
                    return BadRequest(ModelState);
                }

                if (!_dbContext.Posts.Any(c => c.PostId == postId))
                    return NotFound();

                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Об'єкт topic є некоректним");
                }

                if (!await PostManager.UpdatePostById(updatedPost))
                {
                    ModelState.AddModelError("", "Щось пішло не так під час оновлення post!");
                    return StatusCode(500, ModelState);
                }
                return Ok($"Post is updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі UpdateAsync - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        /// <summary>
        /// Get all posts with topics
        /// </summary>
        /// <returns>List of posts</returns>
        [HttpGet("GetAllPostsWithTopics")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetAllPostsWithTopics()
        {
            try
            {
                var topics = await PostManager.GetAllPostsWithTopics();

                if (topics == null)
                    return BadRequest("Немає!");

                _logger.LogInformation($"Отримали всі дані з бази даних!");
                return Ok(topics);
            }

            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetAllPostsWithTopics() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        /// <summary>
        /// Get post with topic by id
        /// </summary>
        /// <param name="id">Post id</param>
        /// <returns>Post with topic</returns>
        [HttpGet("GetPostWithTopics{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTopicByIdWithPosts(int id)
        {
            try
            {
                var topic = await PostManager.GetPostByIdWithTopics(id);

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
