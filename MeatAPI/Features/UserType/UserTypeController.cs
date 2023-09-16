using Mapster;
using MeatAPI.Features.UserType.DTO;
using MeatAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppWithReact.Controllers;

namespace MeatAPI.Features.UserType
{
    public class UserTypeController : BaseAuthorizedController
    {
        private readonly IGenericRepository<DataAccess.Models.UserType> _userTypeRepository;
        private readonly UserTypeService _userTypeService;

        public UserTypeController(IGenericRepository<DataAccess.Models.UserType> userTypeRepository,
                               UserTypeService userTypeService)
        {
            _userTypeRepository = userTypeRepository;
            _userTypeService = userTypeService;
        }

        /// <summary>
        ///     Получение всех объектов
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<GetUserTypeDTO>>> GetAll()
        {
            var userTypes = await _userTypeRepository.Get();
            var userTypesDTO = userTypes.Adapt<List<GetUserTypeDTO>>();

            return Ok(userTypesDTO);
        }

        /// <summary>
        ///     Получение объекта по ID
        /// </summary>
        /// <param name="id">ID объекта</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetUserTypeDTO>> Get(Guid id)
        {
            var ut = await _userTypeService.Get(id);

            return Ok(ut);
        }

        /// <summary>
        ///     Создание объекта
        /// </summary>
        /// <param name="dto">DTO с информацией об объекте</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateUserTypeDTO dto)
        {
            var id = await _userTypeRepository.Create(dto);

            return Ok(id);
        }

        /// <summary>
        ///     Обновление объекта
        /// </summary>
        /// <param name="dto">DTO с информацией об объекте</param>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Update([FromBody] UpdateUserTypeDTO dto)
        {
            var obj = await _userTypeRepository.FindById((Guid)dto.Id);

            await _userTypeRepository.Update(dto);

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
            await _userTypeService.Delete(id);

            return Ok();
        }
    }
}
