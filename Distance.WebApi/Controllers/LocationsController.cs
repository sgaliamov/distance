using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Distance.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        //private readonly ISearchService _searchService;

        //public LocationsController(ISearchService searchService)
        //{
        //    _searchService = searchService;
        //}

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new[] { "value1", "value2" };
        }
    }
}
