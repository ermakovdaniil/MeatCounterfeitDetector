using Mapster;
using MeatAPI.Features.User.DTO;
using MeatAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppWithReact.Controllers;

namespace MeatAPI.Features.User
{
    public class UserController : BaseAuthorizedController
    {
        private readonly IGenericRepository<DataAccess.Models.User> _userRepository;
        private readonly UserService _userService;

        public UserController(IGenericRepository<DataAccess.Models.User> userRepository,
                               UserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }

        /// <summary>
        ///     Получение всех объектов
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<GetUserDTO>>> GetAll()
        {
            var users = await _userRepository.Get();
            var usersDTO = users.Adapt<List<GetUserDTO>>();

            return Ok(usersDTO);
        }

        /// <summary>
        ///     Получение объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetUserDTO>> Get(Guid id)
        {
            var u = await _userService.Get(id);

            return Ok(u);
        }

        /// <summary>
        ///     Создание объекта
        /// </summary>
        /// <param name="dto">DTO с информацией об объекте</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateUserDTO dto)
        {
            var id = await _userService.Create(dto);

            return Ok(id);
        }

        /// <summary>
        ///     Обновление объекта
        /// </summary>
        /// <param name="dto">DTO с информацией об объекте</param>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Update([FromBody] UpdateUserDTO dto)
        {
            await _userService.Update(dto);

            return Ok();
        }

        /// <summary>
        ///     Удаление объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _userService.Delete(id);

            return Ok();
        }
    }
}
