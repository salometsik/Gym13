using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace Gym13.Extensions
{
    public static class LocalizationExtension
    {
        public static IServiceCollection AddCustomLocalization(this IServiceCollection services)
        {
            services.AddRequestLocalization(x =>
            {
                x.RequestCultureProviders = x.RequestCultureProviders.Where(a => a.GetType() == typeof(AcceptLanguageHeaderRequestCultureProvider)).ToList();
                x.SetDefaultCulture("ka");
                x.DefaultRequestCulture = new RequestCulture("ka");
                x.DefaultRequestCulture.Culture.NumberFormat.NumberDecimalSeparator = ".";
                x.DefaultRequestCulture.Culture.NumberFormat.CurrencyDecimalSeparator = ".";
                x.DefaultRequestCulture.Culture.DateTimeFormat.DateSeparator = "/";
                x.DefaultRequestCulture.Culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                x.ApplyCurrentCultureToResponseHeaders = true;
                x.SupportedCultures = new List<CultureInfo> { AddSupportedCulture("ka"), AddSupportedCulture("en") };
                x.SupportedUICultures = new List<CultureInfo> { AddSupportedCulture("ka"), AddSupportedCulture("en") };
            });

            return services;
        }
        static CultureInfo AddSupportedCulture(string culture)
        {
            var cultureInfo = new CultureInfo(culture);
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            cultureInfo.DateTimeFormat.DateSeparator = "/";
            cultureInfo.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            return cultureInfo;
        }
    }
}
