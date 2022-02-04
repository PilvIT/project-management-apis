﻿using Core.Features.GitHubApp;

namespace Main.Services;

public class GitHubService : IGitHubService
{
    private readonly IConfiguration _conf;
    
    public GitHubAuthorization Authorization { get; }
    
    public GitHubService(IConfiguration conf)
    {
        Authorization = new GitHubAuthorization(conf.GetGitHubClientId(), conf.GetGitHubClientSecret());
        _conf = conf;
    }
    
    public GitHubUserApiClient GetUserApiClient(string accessToken)
    {
        return new GitHubUserApiClient(appName: _conf.GetGitHubAppName(), accessToken: accessToken);
    } 
}