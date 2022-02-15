using System.Net.Mime;
using BasisTheory.LuceneSearchingExample.Models.Requests;
using BasisTheory.LuceneSearchingExample.Services;
using Lucene.Net.QueryParsers.Classic;
using Microsoft.AspNetCore.Mvc;

namespace BasisTheory.LuceneSearchingExample.Controllers;

[ApiController]
[Route("search")]
public class PersonsController : ControllerBase
{
    private readonly IPersonsService _personsService;


    public PersonsController(IPersonsService personsService)
    {
        _personsService = personsService;
    }

    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search(
        [FromBody]
        SearchPersonsRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return Ok(await _personsService.SearchPersons(request, cancellationToken));
        }
        catch (Exception e) when (e is InvalidOperationException or ParseException)
        {
            return BadRequest();
        }
    }
}