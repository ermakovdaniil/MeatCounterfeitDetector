using DataAccess.Models;
using MeatAPI.Extensions;
using MeatAPI.Features.Counterfeit.DTO;
using MeatAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppWithReact.Controllers;

namespace MeatAPI.Features.Counterfeit
{

    /// <summary>
    ///     Контроллер для работы со слоями
    /// </summary>
    public class CounterfeitController : BaseAuthorizedController
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IGenericRepository<DataAccess.Models.Counterfeit> _counterfeitRepository;
        private readonly CounterfeitService _counterfeitService;

        public CounterfeitController(IGenericRepository<DataAccess.Models.Counterfeit> counterfeitRepository,
                               CounterfeitService counterfeitService, IAuthorizationService authorizationService)
        {
            _counterfeitRepository = counterfeitRepository;
            _counterfeitService = counterfeitService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        ///     Получает все слои
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<GetCounterfeitDTO>>> Get()
        {
            var counterfeits = await _counterfeitRepository.Get(x => x.AppUserId == User.GetLoggedInUserId<Guid>());
            var counterfeitsDTO = counterfeits.Adapt<List<GetCounterfeitDTO>>();

            return Ok(counterfeitsDTO);
        }

        /// <summary>
        ///     Получает слой по ID
        /// </summary>
        /// <param name="id">ID слоя</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetCounterfeitDTO>> Get(Guid id)
        {
            var l = await _counterfeitRepository.FindById(id);

            var authorizeResult = await _authorizationService.AuthorizeAsync(User, l, Policies.IsObjectOwnByUser);

            if (authorizeResult.Succeeded)
            {
                var o = await _counterfeitService.Get(id);

                return Ok(o);
            }

            return Forbid();
        }

        /// <summary>
        ///     Создает слой
        /// </summary>
        /// <param name="dto">DTO с информацией о слое</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateCounterfeitDTO dto)
        {
            var authorizeResult = true;

            foreach (var oId in dto.Objects)
            {
                var obj = await _objectOnMapRepository.FindById(oId);
                authorizeResult = authorizeResult && _authorizationService.AuthorizeAsync(User, obj, Policies.IsObjectOwnByUser).Result.Succeeded;

                if (!authorizeResult)
                {
                    break;
                }
            }

            var id = await _counterfeitService.Create(dto, User.GetLoggedInUserId<Guid>());

            return Ok(id);
        }

        /// <summary>
        ///     Обновляет слой
        /// </summary>
        /// <param name="dto">DTO с информацией о слое</param>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Update([FromBody] UpdateCounterfeitDTO dto)
        {
            var counterfeit = await _counterfeitRepository.FindById((Guid)dto.Id);
            var counterfeitAuthorizeResult = _authorizationService.AuthorizeAsync(User, counterfeit, Policies.IsObjectOwnByUser).Result.Succeeded;
            var objectsAuthorizeResult = true;

            foreach (var oId in dto.Objects)
            {
                var obj = await _objectOnMapRepository.FindById(oId);

                objectsAuthorizeResult = objectsAuthorizeResult &&
                                         _authorizationService.AuthorizeAsync(User, obj, Policies.IsObjectOwnByUser).Result.Succeeded;

                if (!objectsAuthorizeResult)
                {
                    break;
                }
            }

            if (objectsAuthorizeResult && counterfeitAuthorizeResult)
            {
                await _counterfeitService.Update(dto);

                return Ok();
            }

            return Forbid();
        }

        /// <summary>
        ///     Удаляет слой по ID
        /// </summary>
        /// <param name="id">ID слоя</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var counterfeit = await _counterfeitRepository.FindById(id);
            var authorizeResult = await _authorizationService.AuthorizeAsync(User, counterfeit, Policies.IsObjectOwnByUser);

            if (authorizeResult.Succeeded)
            {
                await _counterfeitService.Delete(id);

                return Ok();
            }

            return Forbid();
        }
    }
}
