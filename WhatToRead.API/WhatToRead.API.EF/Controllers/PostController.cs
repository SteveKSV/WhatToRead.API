using AutoMapper;
using EFTopics.DAL.Data;
using EFTopics.DAL.Dtos;
using EFTopics.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFTopics.DAL.Interfaces;

namespace WhatToRead.API.EF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PostController> _logger;
        private readonly IMapper _mapper;
        private readonly ApplicationContext _dbContext;
        public PostController(IUnitOfWork unitOfWork, ILogger<PostController> logger, IMapper mapper, ApplicationContext context)
        {

            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _dbContext = context;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Post>))]
        public IActionResult GetAllPosts()
        {
            try
            {
                var results = _mapper.Map<List<PostDto>>(_unitOfWork.PostRepository.GetAllEntities());

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
        public IActionResult GetPostById(int id)
        {
            try
            {
                var result = _mapper.Map<PostDto>(_unitOfWork.PostRepository.GetEntityById(id));

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
        public IActionResult Insert([FromBody] PostDto postCreate)
        {
            try
            {
                if (postCreate == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт post null");
                }

                var post = _unitOfWork.PostRepository.GetAllEntities()
                    .Where(p=>p.Title.Trim().ToUpper() == postCreate.Title.Trim().ToUpper());

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

                var postMap = _mapper.Map<Post>(postCreate);

                if (!_unitOfWork.PostRepository.CreateEntity(postMap))
                {
                    ModelState.AddModelError("", "Щось пішло не так під час зберігання!");
                    return StatusCode(500, ModelState);
                }


                return Ok("Успішно доданий новий post!");
            }

            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі Insert - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }

        [HttpDelete]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(int postId)
        {
            try
            {
                if (!_dbContext.Posts.Any(c => c.PostId == postId))
                {
                    return NotFound();
                }

                var postToDelete = _unitOfWork.PostRepository.GetEntityById(postId);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if(!_unitOfWork.PostRepository.DeleteEntity(postToDelete))
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
        public IActionResult Update(int postId, [FromBody] PostDto updatedPost)
        {
            try
            {
                if (updatedPost == null)
                {
                    _logger.LogInformation($"Ми отримали пустий json зі сторони клієнта");
                    return BadRequest("Обєкт topic є null");
                }

                if(postId != updatedPost.PostId)
                {
                    return BadRequest(ModelState);
                }

                if(!_dbContext.Posts.Any(c => c.PostId == postId))
                    return NotFound();


                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"Ми отримали некоректний json зі сторони клієнта");
                    return BadRequest("Об'єкт topic є некоректним");
                }

                var postMap = _mapper.Map<Post>(updatedPost);

                if (!_unitOfWork.PostRepository.UpdateEntity(postMap))
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
