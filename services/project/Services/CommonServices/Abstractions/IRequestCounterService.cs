using System.Collections.Generic;
using Models.Misc;

namespace Services.CommonServices.Abstractions
{
    public interface IRequestCounterService
    {
        void Notice(string path);
        void NoticeIndexNotFound();

        (IDictionary<string, RequestData> stats, int totalPhp, int totalPhpMyAdmin, int totalIndexNotFound) Get();
    }
}