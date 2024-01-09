using System.Collections.Concurrent; 

namespace CommunityToolkit.Mvvm.Navigation.Wpf
{
    /// <summary>
    /// 导航服务
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// 能否后退
        /// </summary>
        ConcurrentDictionary<string, bool> CanGoBack { get; }
        /// <summary>
        /// 能否前进
        /// </summary>
        ConcurrentDictionary<string, bool> CanGoForward { get; }
        /// <summary>
        /// 后退
        /// </summary>
        /// <param name="regionName"></param>
        void GoBack(string regionName);
        /// <summary>
        /// 前进
        /// </summary>
        /// <param name="regionName">注册区域名称</param>
        void GoForward(string regionName);
        /// <summary>
        /// 注册视图
        /// </summary>
        /// <typeparam name="TView">需要注册的视图</typeparam>
        /// <param name="name">视图名称</param>
        void RegisterForNavigation<TView>(string name = null);
        /// <summary>
        /// 导航
        /// </summary>
        /// <param name="regionName">注册区域名称</param>
        /// <param name="source">视图名称</param>
        void Navigate(string regionName, string source);
        /// <summary>
        /// 导航
        /// </summary>
        /// <param name="regionName">注册区域名称</param>
        /// <param name="source">视图名称</param>
        /// <param name="parameters">参数</param>
        void Navigate(string regionName, string source, NavigationParameters parameters); 
    }
}
