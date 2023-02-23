using Microsoft.AspNetCore.Mvc;
using TeamworkSystem.DataAccessLayer.Interfaces;

namespace WhatToRead.API.EF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TopicsController> _logger;
        public TopicsController(IUnitOfWork unitOfWork, ILogger<TopicsController> logger)
        {

            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            try
            {
                var results = await _unitOfWork.TopicsRepository.GetAsync();

                _logger.LogInformation($"Отримали всі дані з бази даних!");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Транзакція сфейлилась! Щось пішло не так у методі GetAllAsync() - {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "вот так вот!");
            }
        }
    }
}
