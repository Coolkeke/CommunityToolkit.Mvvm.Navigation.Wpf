using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityToolkit.Mvvm.Navigation.Wpf
{
    public interface INavigationAware
    { 
        /// <summary>
        /// 是否允许进行导航
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        bool IsNavigationTarget(NavigationParameters parameters);
        /// <summary>
        /// 导航后参数接收方法
        /// </summary>
        /// <param name="parameters">参数</param>
        void OnNavigatedTo(NavigationParameters parameters);
        /// <summary>
        /// 导航前参数传递方法
        /// </summary>
        /// <param name="parameters">参数</param>
        void OnNavigatedFrom(NavigationParameters parameters);
    }
}
