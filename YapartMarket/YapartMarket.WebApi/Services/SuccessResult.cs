using System;
using System.Collections.Generic;
using System.Linq;

namespace YapartMarket.WebApi.Services;

public sealed class SuccessResult : IExecutionResult
{
    public bool Succeeded => Errors.Count == 0;
    public static readonly SuccessResult Success = new(new List<string>());
    public SuccessResult(string? error = null)
    {
        if (error.IsNullOrEmpty())
            Errors = Array.Empty<string>();
        else
            Errors = new List<string>() { error }.AsReadOnly();
    }
    public SuccessResult(IReadOnlyCollection<string>? errors = null)
    { 
        if (errors.IsNullOrEmpty())
            Errors = Array.Empty<string>();
        else
            Errors = new List<string>(errors!).AsReadOnly();
    }
    public SuccessResult(params string[] errorMessages) : this((IReadOnlyCollection<string>)errorMessages) { }

    IReadOnlyList<ErrorMessage> IExecutionResult.Errors
    {
        get
        {
            if (Errors.IsNullOrEmpty())
                return Array.Empty<ErrorMessage>();
            var result = new List<ErrorMessage>(Errors.Count);
            foreach (var error in Errors)
                result.Add(new ErrorMessage(error));
            return result.AsReadOnly();
        }
    }
    public static SuccessResult Combine(params IExecutionResult[] executionResults)
    {
        if (executionResults.IsNullOrEmpty())
            return Success;
        var errors = new List<string>();
        foreach (var executionResult in executionResults.Where(result => result != null))
        foreach (var error in executionResult.Errors)
        {
            var description = error?.Description;
            if (description != null)
                errors.Add(description);
        }
        return new SuccessResult(errors);
    }

    public IReadOnlyCollection<string> Errors { get; }
}

public interface IExecutionResult
{
    IReadOnlyList<ErrorMessage> Errors { get; }
}

public sealed record ErrorMessage
{
    /// <inheritdoc/>
    public ErrorMessage(string description, string errorCode)
    {
        Description = description ?? throw new ArgumentNullException(nameof(description));
        ErrorCode = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
    }
    /// <inheritdoc/>
    public ErrorMessage(string description) : this(description, string.Empty)
    {
    }
    /// <inheritdoc/>
    public string Description { get; }
    /// <inheritdoc/>
    public string ErrorCode { get; }
}