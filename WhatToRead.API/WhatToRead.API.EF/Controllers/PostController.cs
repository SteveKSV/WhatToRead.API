using AutoMapper;
using EFTopics.DAL.Data;
using EFTopics.DAL.Dtos;
using EFTopics.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFWhatToRead_DAL.Repositories.Interfaces;
using EFWhatToRead_BBL.Managers.Interfaces;
using EFWhatToRead_BBL.Managers;

namespace WhatToRead.API.EF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ILogger<TopicController> _logger;
        private readonly ApplicationContext _dbContext;
        private IPostManager PostManager { get; }
        public PostController(IPostManager postManager, ILogger<TopicController> logger, IMapper mapper, ApplicationContext context)
        {
            _logger = logger;
            _dbContext = context;
            PostManager = postManager;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Post>))]
        public async Task<IActionResult> GetAllPosts()
        {
            try
            {
                var results = await PostManager.GetAllPosts();

                _logger.LogInformation($"Отримали всі дані з бази даних!");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetAllPosts() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

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

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Insert([FromBody] PostDto postCreate)
        {
            try
            {
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
                    return BadRequest("Обєкт post є некоректним");
                }

                return Ok("Успішно доданий новий post!");
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

        [HttpPut]
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
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі UpdateAsync - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }
    }
}
