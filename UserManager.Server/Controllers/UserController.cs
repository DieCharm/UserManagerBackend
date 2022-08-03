using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManager.Data;

namespace UserManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : 
        ControllerBase
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllAsync()
        {
            return Ok(await _repository.GetAllAsync());
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] User user)
        {
            await _repository.CreateAsync(user);
            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<User>> GetAsync(int id)
        {
            return Ok(await _repository.GetAsync(id));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] User user)
        {
            await _repository.UpdateAsync(user);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            return Ok();
        }
    }
}