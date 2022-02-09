using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tests.Templates;

public class AssertionException : Exception
{
    public AssertionException(string message, Exception exception) : base(message, exception)
    {
    }
}
    
public class BaseTest
{
    protected BaseTest() {}
    
    protected static void AssertContext(Action assertion, string message)
    {
        try
        {
            assertion.Invoke();
        }
        catch (Exception exception)
        {
            throw new AssertionException(message, exception);
        }
    }
    
    protected static void AssertContext(Action assertion, HttpResponseMessage responseMessage)
    {
        try
        {
            assertion.Invoke();
        }
        catch (Exception exception)
        {
            Task<string> content = responseMessage.Content.ReadAsStringAsync();
            content.Wait();
            throw new AssertionException(content.Result, exception);
        }
    }
}
