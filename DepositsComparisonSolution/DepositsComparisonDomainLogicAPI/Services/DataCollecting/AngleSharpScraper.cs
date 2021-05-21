namespace DepositsComparisonDomainLogicAPI.Services.DataCollecting
{
    using System.Threading.Tasks;
    using AngleSharp;
    using Models.DataCollecting;

    public abstract class AngleSharpScraper
    {
        protected AngleSharpScraper()
        {
            var config = Configuration.Default.WithDefaultLoader();
            Context = BrowsingContext.New(config);
        }
        
        protected IBrowsingContext Context { get; set; }
    }
}