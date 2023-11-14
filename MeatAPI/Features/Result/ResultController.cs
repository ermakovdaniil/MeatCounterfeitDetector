using ClientAPI.DTO.Result;
using DataAccess.Data;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebAppWithReact.Controllers;

namespace MeatAPI.Features.Result
{
    public class ResultController : BaseAuthorizedController
    {
        private readonly ResultService _resultService;

        public ResultController(ResultService resultService)
        {
            _resultService = resultService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<GetResultDTO>>> GetAll()
        {
            var results = await _resultService.GetAll();
            var resultsDTO = new List<GetResultDTO>();
            foreach (var r in results)
            {
                var dto = new GetResultDTO();
                dto.Id = r.Id;
                dto.Date = r.Date;
                dto.UserId = r.UserId;
                dto.UserName = r.User.Name;
                dto.AnalysisResult = r.AnalysisResult;
                dto.Time = r.Time;
                dto.PercentOfSimilarity = r.PercentOfSimilarity;
                dto.OriginalImageId = r.OriginalImageId;
                dto.OriginalImagePath = r.OriginalImage.ImagePath;
                dto.ResultImageId = r.ResultImageId;
                if(r.ResultImage is not null)
                {
                    dto.ResultImagePath = r.ResultImage.ImagePath;
                }
                resultsDTO.Add(dto);
            }
            return Ok(resultsDTO);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetResultDTO>> Get(Guid id)
        {
            var r = await _resultService.Get(id);
            var rDto = r.Adapt<GetResultDTO>();
            return Ok(rDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateResultDTO dto)
        {
            var r = dto.Adapt<DataAccess.Models.Result>();
            var id = await _resultService.Create(r);
            return Ok(id);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _resultService.Delete(id);
            return Ok();
        }
    }
}
