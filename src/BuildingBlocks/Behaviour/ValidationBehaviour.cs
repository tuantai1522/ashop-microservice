using BuildingBlocks.Validation;
using FluentValidation;
using MediatR;
using ValidationException = BuildingBlocks.Validation.ValidationException;

namespace BuildingBlocks.Behaviour;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    /// <summary>
    /// 1. Validate request
    /// 2. If there is any errors, return validation result
    /// 3. Otherwise, return next();
    /// </summary>
    /// <param name="request"></param>
    /// <param name="next"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next(cancellationToken);
        }

        var validationResults = await Task.WhenAll(_validators.Select(validator => validator.ValidateAsync(request, cancellationToken)));

        var errors = validationResults
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationResult => validationResult is not null)
            .Select(failure => new Error(failure.PropertyName, failure.ErrorMessage))
            .Distinct()
            .ToArray();

        
        if (errors.Length != 0)
        {
            throw new ValidationException(errors);
        }

        return await next(cancellationToken);
    }

}