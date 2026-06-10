using ConectaEleitor.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ConectaEleitor.API.Hubs;

public class HubDemand : Hub
{
    private readonly IUserContext _userContext;
}