namespace CoreApi;

using Microsoft.AspNetCore.Mvc;

public class NotFoundException : Exception
{

}

public class NotFoundProblem : ProblemDetails
{
    public string OtherField { get; set; } = "other";
}
