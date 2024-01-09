using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;

namespace CommunityToolkit.Mvvm.Navigation.Wpf
{
    public class NavigationService : INavigationService
    {
        public void RegisterForNavigation<TView>(string name = null)
        {
            if (NavigationManager.Manager == null) return;
            if (NavigationManager.Manager.views.Any(o => o.Key == name)) return;
            NavigationManager.Manager.views.TryAdd(name, typeof(TView));
        }
        public void Navigate(string regionName, string source) => Navigate(regionName, source, null);

        public void Navigate(string regionName, string source, NavigationParameters parameters)
        {
            #region 防呆
            if (NavigationManager.Manager == null) return;
            if (!NavigationManager.Manager.navigations.Any(o => o.Key == regionName)) throw new ArgumentNullException($"尚未找到名称：{regionName}区域名称");
            if (!NavigationManager.Manager.views.Any(o => o.Key == source)) throw new ArgumentNullException($"尚未找到名称：{source}视图");
            var navigation = NavigationManager.Manager.navigations.Where(o => o.Key == regionName).FirstOrDefault().Value;
            var view = NavigationManager.Manager.views.Where(o => o.Key == source).FirstOrDefault().Value;
            if (!(navigation is ContentControl nav) || !(Activator.CreateInstance(view) is ContentControl page)) return;
            #endregion
            #region 执行导航 
            if (nav.Content is UserControl control && control.DataContext is IJournalAware journal)
            {
                if (journal.PersistInHistory())
                {
                    #region 添加导航记录
                    if (!NavigationManager.Manager.navigationHistory.ContainsKey(regionName))
                        NavigationManager.Manager.navigationHistory.TryAdd(regionName, new List<object>() { page });
                    else 
                        NavigationManager.Manager.navigationHistory[regionName].Add(page);
                    #endregion
                }
            }
            if (nav.Content is UserControl controlFrom && controlFrom.DataContext is INavigationAware from)
            {
                var isNav = from.IsNavigationTarget(parameters);
                if (!isNav) return;
                from.OnNavigatedFrom(null);
            }
            if (page.DataContext is INavigationAware to) to.OnNavigatedTo(parameters);
            nav.Content = page;
            #endregion
            #region 设置导航索引
            if (NavigationManager.Manager.navigationHistoryIndex.ContainsKey(regionName))
                NavigationManager.Manager.navigationHistoryIndex[regionName] = NavigationManager.Manager.navigationHistory[regionName].Count - 1;
            else 
            {
                if (!NavigationManager.Manager.navigationHistory.ContainsKey(regionName))
                    NavigationManager.Manager.navigationHistory.TryAdd(regionName,new List<object>() { nav.Content });
                NavigationManager.Manager.navigationHistoryIndex.TryAdd(regionName, NavigationManager.Manager.navigationHistory[regionName].Count - 1);
            }
            #endregion
            if (CanGoBack.ContainsKey(regionName))
                CanGoBack.TryAdd(regionName, true);
            if (CanGoForward.ContainsKey(regionName))
                CanGoForward.TryAdd(regionName, false);
        }
        public ConcurrentDictionary<string, bool> _CanGoBack;
        public ConcurrentDictionary<string, bool> CanGoBack
        {
            get
            {
                if (_CanGoBack == null) _CanGoBack = new ConcurrentDictionary<string, bool>();
                return _CanGoBack;
            }
        }
        public ConcurrentDictionary<string, bool> _CanGoForward;
        public ConcurrentDictionary<string, bool> CanGoForward
        {
            get
            {
                if (_CanGoForward == null) _CanGoForward = new ConcurrentDictionary<string, bool>();
                return _CanGoForward;
            }
        }

        public void GoBack(string regionName)
        {
            if (!NavigationManager.Manager.navigationHistoryIndex.ContainsKey(regionName)) return;
            var index = NavigationManager.Manager.navigationHistoryIndex[regionName] - 1;
            CanGoForward[regionName] = !(NavigationManager.Manager.navigationHistoryIndex[regionName] + 1 > NavigationManager.Manager.navigationHistory[regionName].Count - 1);//判断前进调整
            if (index < 0)
            {
                CanGoBack[regionName] = false;
                return;
            }
            NavigationManager.Manager.navigationHistoryIndex[regionName]--;
            CanGoBack[regionName] = true;
            var view = NavigationManager.Manager.navigationHistory[regionName][NavigationManager.Manager.navigationHistoryIndex[regionName]];
            var navigation = NavigationManager.Manager.navigations.Where(o => o.Key == regionName).FirstOrDefault().Value;
            if (!(navigation is ContentControl nav)) return; 
            nav.Content = view;
        }

        public void GoForward(string regionName)
        {
            if (!NavigationManager.Manager.navigationHistoryIndex.ContainsKey(regionName)) return;
            var index = NavigationManager.Manager.navigationHistoryIndex[regionName] + 1;
            CanGoBack[regionName] = !(NavigationManager.Manager.navigationHistoryIndex[regionName] - 1 < 0);//判断后退条件
            if (index > NavigationManager.Manager.navigationHistory[regionName].Count - 1)
            {
                CanGoForward[regionName] = false;
                return;
            }
            NavigationManager.Manager.navigationHistoryIndex[regionName]++;
            CanGoForward[regionName] = true;
            var view = NavigationManager.Manager.navigationHistory[regionName][NavigationManager.Manager.navigationHistoryIndex[regionName]];
            var navigation = NavigationManager.Manager.navigations.Where(o => o.Key == regionName).FirstOrDefault().Value;
            if (!(navigation is ContentControl nav)) return; 
            nav.Content = view;
        }
    }
}
