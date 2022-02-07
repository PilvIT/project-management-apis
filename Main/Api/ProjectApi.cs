using Main.Injectables.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Main.Api;

[Route("projects")]
public class ProjectApi : ApiBase
{
    public ProjectApi(IAuth auth) : base(auth)
    {
    }
    
    [HttpGet]
    public string[] GetProjects()
    {
        Console.WriteLine(User.Id);

        var x = new string[2];
        return x;
    }
}