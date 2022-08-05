using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManager.Data;

namespace UserManager.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController : 
        ControllerBase
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets all users 
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllAsync()
        {
            return Ok(await _repository.GetAllAsync());
        }

        /// <summary>
        /// Adds new user to database
        /// </summary>
        /// <param name="user">Instance of user to add</param>
        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] User user)
        {
            await _repository.CreateAsync(user);
            return Ok();
        }

        /// <summary>
        /// Gets user by id
        /// </summary>
        /// <param name="id">User's id</param>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<User>> GetAsync(int id)
        {
            return Ok(await _repository.GetAsync(id));
        }

        /// <summary>
        /// Updates existing user
        /// </summary>
        /// <param name="user">Instance of user to update</param>
        [HttpPut]
        public async Task<ActionResult> UpdateAsync([FromBody] User user)
        {
            await _repository.UpdateAsync(user);
            return Ok();
        }

        /// <summary>
        /// Deletes user by id
        /// </summary>
        /// <param name="id">Id of user to delete</param>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            return Ok();
        }
    }
}