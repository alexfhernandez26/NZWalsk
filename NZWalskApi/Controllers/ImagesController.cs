using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalskApi.Models.Domain;
using NZWalskApi.Models.DTO;
using NZWalskApi.Repositories;

namespace NZWalskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IIMageRepository iMageRepository;

        public ImagesController(IIMageRepository iMageRepository)
        {
            this.iMageRepository = iMageRepository;
        }

        [HttpPost]
        [Route("Upload")]

        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            ValidateFileUpload(request);

            if (ModelState.IsValid)
            {
                //Map
                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInByte = request.File.Length,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription,

                };

                //User Repository to upload Image
                await iMageRepository.Upload(imageDomainModel);

                return Ok(imageDomainModel);
            }

            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowExtensions = new string[] { ".jpg", ".jpeg", ".png", ".pdf" };

            if (!allowExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "unsupported File Extension");
            }

            if (request.File.FileName.Length > 10485760)
            {
                ModelState.AddModelError("file", "Size File more than 10MB, please upload a smaller size file");
            }
        }
    }
}
