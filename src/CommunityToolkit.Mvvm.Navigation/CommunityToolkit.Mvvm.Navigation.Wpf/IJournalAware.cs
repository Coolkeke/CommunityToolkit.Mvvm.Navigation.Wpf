using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityToolkit.Mvvm.Navigation.Wpf
{
    public interface IJournalAware
    {
        /// <summary>
        /// 导航是否被记录
        /// </summary>
        /// <returns></returns>
        bool PersistInHistory();
    }
}
