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
        ///     Получение объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetUserDTO>> Get(GetUserDTO dto)
        {
            var u = await _userService.Get(dto);

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
            var id = await _userRepository.Create(dto);

            return Ok(dto);
        }

        /// <summary>
        ///     Обновление объекта
        /// </summary>
        /// <param name="dto">DTO с информацией об объекте</param>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Update([FromBody] UpdateUserDTO dto)
        {
            var obj = await _userRepository.FindById((Guid)dto.Id);

            await _userRepository.Update(dto);

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

        /// <summary>
        ///     Удаление объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete([FromBody] DeleteUserDTO dto)
        {
            await _userService.Delete(dto);

            return Ok();
        }
    }
}
