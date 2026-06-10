using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ConectaEleitor.API.Hubs;
[Authorize]
public class HubCitizen : Hub
{
    
}