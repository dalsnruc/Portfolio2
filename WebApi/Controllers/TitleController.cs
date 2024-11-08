﻿using DataLayer;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.NameModels;
using WebApi.Models.TitleModels;

namespace WebApi.Controllers;

[ApiController]
[Route("api/titles")]

public class TitleController : BaseController
{
    ITitleDataService _titledataservice;
    IUserDataService _userdataservice;
    private readonly LinkGenerator _linkGenerator;

    public TitleController(
        ITitleDataService titleDataService,
        IUserDataService userDataService,
        LinkGenerator linkGenerator)
        : base(linkGenerator)
    {
        _titledataservice = titleDataService;
        _userdataservice = userDataService;
        _linkGenerator = linkGenerator;

    }

    [HttpGet("paged", Name = nameof(GetTitlesPaged))]
    public IActionResult GetTitlesPaged(int page = 0, int pageSize = 2)
    {
        try
        {
            var username = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var user = _userdataservice.GetUser(username);

            var titles = _titledataservice
                .GetTitles(user.Id, page, pageSize)
                .Select(CreateAllTitlesModel);
            var numberOfItems = _titledataservice.NumberOfTitles();
            object result = CreatePaging(
                nameof(GetTitles),
                page,
                pageSize,
                numberOfItems,
                titles);
            return Ok(result);
        }
        catch
        {
            return Unauthorized();
        }
    }

    [HttpGet]
    public IActionResult GetTitles(int userid)
    {
        var titles = _titledataservice
            .GetTitles(userid)
            .Select(CreateTitleModel);
        return Ok(titles);
    }

    [HttpGet("{id}", Name = nameof(GetTitle))]
    public IActionResult GetTitle(string id)
    {
        try
        {
            var username = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var user = _userdataservice.GetUser(username);
            var title = _titledataservice.GetTitle(user.Id, id);

            if (title == null)
            {
                return NotFound();
            }
            var model = CreateTitleModel(title);

            return Ok(model);
        }
        catch
        {
            return Unauthorized();
        }
    }

    [HttpPost]
    public IActionResult CreateTitle(CreateTitleModel model)
    {
        try
        {
            var username = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var user = _userdataservice.GetUser(username);
            var title = _titledataservice.CreateTitle(user.Id, model.PrimaryTitle, model.OriginalTitle, model.IsAdult, model.StartYear);
            return CreatedAtRoute(nameof(GetTitle), new { id = title.Id }, CreateTitleModel(title));
        }
        catch
        { 
            return Unauthorized();
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateTitle(string id, CreateTitleModel model)
    {

        try
        {
            var username = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var user = _userdataservice.GetUser(username);
            var title = _titledataservice.GetTitle(user.Id, id);

            if (title == null)
            {
                return NotFound();
            }

            title.PrimaryTitle = model.PrimaryTitle;
            title.OriginalTitle = model.OriginalTitle;
            title.IsAdult = model.IsAdult;
            title.StartYear = model.StartYear;

            _titledataservice.UpdateTitle(user.Id, title);

            return Ok(CreateTitleModel(title));
        }
        catch
        {
            return Unauthorized();
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTitle(string id)
    {
        try
        {
            var username = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var user = _userdataservice.GetUser(username);

            var result = _titledataservice.DeleteTitle(user.Id, id);

            if (result)
            {
                return Ok();
            }

            return NotFound();
        }
        catch 
        { 
            return Unauthorized(); 
        }
    }

    private TitleModel? CreateTitleModel(Title? title)
    {
        if (title == null)
        {
            return null;
        }

        var model = title.Adapt<TitleModel>();
        model.Url = GetUrl(title.Id);

        return model;
    }

    private AllTitlesModel? CreateAllTitlesModel(Title? title)
    {
        if (title == null)
        {
            return null;
        }

        var model = title.Adapt<AllTitlesModel>();
        model.Url = GetUrl(title.Id);

        return model;
    }



    private string? GetUrl(string id)
    {
        return _linkGenerator.GetUriByName(
            HttpContext,
            nameof(GetTitle), new { id });
    }

    private string? GetLink(string linkName, int page, int pageSize)
    {
        return _linkGenerator.GetUriByName(
                    HttpContext,
                    linkName,
                    new { page, pageSize }
                    );
    }
}
