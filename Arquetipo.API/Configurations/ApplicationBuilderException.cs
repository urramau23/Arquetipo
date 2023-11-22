namespace Arquetipo.API.Filters
{
    public static class ApplicationBuilderException
    {
        public static IApplicationBuilder AddGlobalErrorHandler(this IApplicationBuilder applicationBuilder)
            => applicationBuilder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    }
}
