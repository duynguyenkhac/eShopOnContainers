﻿// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860
namespace Microsoft.eShopOnContainers.Services.Catalog.API.Controllers;

[ApiController]
public class PicController : ControllerBase
{
    private readonly IWebHostEnvironment _env;
    private readonly CatalogContext _catalogContext;

    public PicController(IWebHostEnvironment env,
        CatalogContext catalogContext)
    {
        _env = env;
        _catalogContext = catalogContext;
    }

    [HttpGet]
    [Route("api/v1/catalog/items/{catalogItemId:int}/pic")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    // GET: /<controller>/
    public async Task<ActionResult> GetImageAsync(int catalogItemId)
    {
        if (catalogItemId <= 0)
        {
            return BadRequest();
        }

        var item = await _catalogContext.CatalogItems
            .SingleOrDefaultAsync(ci => ci.Id == catalogItemId);

        if (item != null)
        {
            var path = Path.Combine(_env.ContentRootPath, "Pics", item.PictureFileName);

            string imageFileExtension = Path.GetExtension(item.PictureFileName);
            string mimetype = GetImageMimeTypeFromImageFileExtension(imageFileExtension);

            return PhysicalFile(path, mimetype);
        }

        return NotFound();
    }

    private string GetImageMimeTypeFromImageFileExtension(string extension)
    {
        string mimetype = extension switch
        {
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".bmp" => "image/bmp",
            ".tiff" => "image/tiff",
            ".wmf" => "image/wmf",
            ".jp2" => "image/jp2",
            ".svg" => "image/svg+xml",
            _ => "application/octet-stream",
        };
        return mimetype;
    }
}
