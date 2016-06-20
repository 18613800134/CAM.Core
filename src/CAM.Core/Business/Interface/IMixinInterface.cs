using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CAM.Common.QueryMaker;
using CAM.Core.Model.Filter;

namespace CAM.Core.Business.Interface
{
    public interface IMixinInterface
    {
        TMixin readMixin<TMixin>(QueryMakerObjectQueue qm);
        TMixin readMixinBySql<TMixin>(string sql);

        List<TMixin> readMixinList<TMixin, TFilter>(QueryMakerObjectQueue qm, TFilter filter) where TFilter : BaseFilter;
        List<TMixin> readMixinListBySql<TMixin, TFilter>(string sql, TFilter filter) where TFilter : BaseFilter;
    }
}
