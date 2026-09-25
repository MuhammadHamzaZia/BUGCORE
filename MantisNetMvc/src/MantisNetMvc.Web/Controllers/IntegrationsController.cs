using Microsoft.AspNetCore.Mvc;
using MantisNetMvc.Core.Enums;
using MantisNetMvc.Core.Interfaces;
using MantisNetMvc.Web.Filters;

namespace MantisNetMvc.Web.Controllers;

[MantisAuthorize(AccessLevel.Viewer, globalOnly: true)]
public class IntegrationsController : Controller
{
    private readonly ISlackService _slackService;
    private readonly IApiTokenService _apiTokenService;

    public IntegrationsController(ISlackService slackService, IApiTokenService apiTokenService)
    {
        _slackService = slackService;
        _apiTokenService = apiTokenService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var settings = await _slackService.GetSlackSettingsAsync();
        var channels = await _slackService.GetAvailableChannelsAsync();

        ViewBag.SlackSettings = settings;
        ViewBag.SlackChannelCount = channels.Count();
        ViewBag.SlackStatus = !string.IsNullOrEmpty(settings["BotToken"]) ? "Active" : "Not Configured";

        return View();
    }
}
