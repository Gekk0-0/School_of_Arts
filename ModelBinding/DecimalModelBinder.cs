using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace School_of_arts.ModelBinding
{
    public sealed class DecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext context)
        {
            var valueResult = context.ValueProvider.GetValue(context.ModelName);
            if (valueResult == ValueProviderResult.None) return Task.CompletedTask;

            context.ModelState.SetModelValue(context.ModelName, valueResult);

            var raw = valueResult.FirstValue?.Trim();
            if (string.IsNullOrWhiteSpace(raw)) return Task.CompletedTask;

            // прибираємо пробіли/nbsp (часто з'являються при форматуванні)
            raw = raw.Replace(" ", "").Replace("\u00A0", "");

            // пробуємо кілька варіантів
            if (decimal.TryParse(raw, NumberStyles.Number, CultureInfo.CurrentCulture, out var d) ||
                decimal.TryParse(raw, NumberStyles.Number, CultureInfo.GetCultureInfo("uk-UA"), out d) ||
                decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out d) ||
                decimal.TryParse(raw.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out d))
            {
                context.Result = ModelBindingResult.Success(d);
                return Task.CompletedTask;
            }

            context.ModelState.TryAddModelError(context.ModelName, "Неправильний формат числа.");
            return Task.CompletedTask;
        }
    }
}