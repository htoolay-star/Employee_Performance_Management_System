using FluentValidation;
using Mapster;

namespace EPMS.Client.Extensions
{
    public static class ValidationExtensions
    {
        public static Func<object, string, Task<IEnumerable<string>>> ValidateMappedValue<TFormModel, TDto>(
            this IValidator<TDto> validator, TFormModel formModel, Dictionary<string, string>? serverErrors = null)
        {
            return async (_, propertyName) =>
            {
                if (serverErrors != null && serverErrors.TryGetValue(propertyName, out var serverError))
                {
                    serverErrors.Remove(propertyName);

                    return new[] { serverError };
                }

                var dto = formModel.Adapt<TDto>();

                var result = await validator.ValidateAsync(dto);

                if (result.IsValid)
                    return Array.Empty<string>();

                return result.Errors
                    .Where(e => e.PropertyName == propertyName)
                    .Select(e => e.ErrorMessage);
            };
        }
    }
}