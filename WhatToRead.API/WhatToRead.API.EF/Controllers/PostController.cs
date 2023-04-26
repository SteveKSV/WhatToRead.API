using AutoMapper;
using EFTopics.DAL.Data;
using EFTopics.DAL.Dtos;
using EFTopics.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFWhatToRead_DAL.Repositories.Interfaces;
using EFWhatToRead_BBL.Managers.Interfaces;
using EFWhatToRead_BBL.Managers;
using EFWhatToRead_DAL.Params;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace WhatToRead.API.EF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly ILogger<TopicController> _logger;
        private readonly ApplicationContext _dbContext;
        private readonly IValidator<PostDto> _validator;
        private IPostManager PostManager { get; }
        public PostController(IPostManager postManager, ILogger<TopicController> logger, ApplicationContext context, IValidator<PostDto> validator)
        {
            _logger = logger;
            _dbContext = context;
            PostManager = postManager;
            _validator = validator;
        }

        
        [HttpGet]
        [Authorize]
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
