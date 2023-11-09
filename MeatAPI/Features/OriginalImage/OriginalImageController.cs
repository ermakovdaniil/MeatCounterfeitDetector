﻿using ClientAPI.DTO.OriginalImage;
using DataAccess.Data;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebAppWithReact.Controllers;

namespace MeatAPI.Features.OriginalImage
{
    public class OriginalImageController : BaseAuthorizedController
    {
        private readonly EntityAccessServiceBase<ResultDBContext, DataAccess.Models.OriginalImage> _originalImageService;

        public OriginalImageController(EntityAccessServiceBase<ResultDBContext, DataAccess.Models.OriginalImage> originalImageService)
        {
            _originalImageService = originalImageService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<GetOriginalImageDTO>>> GetAll()
        {
            var originalImages = await _originalImageService.GetAll();
            var originalImagesDTO = originalImages.Adapt<List<GetOriginalImageDTO>>();
            return Ok(originalImagesDTO);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetOriginalImageDTO>> Get(Guid id)
        {
            var op = await _originalImageService.Get(id);
            var opDto = op.Adapt<GetOriginalImageDTO>();
            return Ok(opDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateOriginalImageDTO dto)
        {
            var op = dto.Adapt<DataAccess.Models.OriginalImage>();
            var id = await _originalImageService.Create(op);
            return Ok(id);
        }

        //[HttpPut]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult> Update([FromBody] UpdateOriginalImageDTO dto)
        //{
        //    var op = dto.Adapt<DataAccess.Models.OriginalImage>();
        //    await _originalImageService.Update(op);
        //    return Ok();
        //}

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _originalImageService.Delete(id);
            return Ok();
        }
    }
}