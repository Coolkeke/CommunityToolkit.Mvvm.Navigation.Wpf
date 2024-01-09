using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System.Collections.Concurrent;

namespace CommunityToolkit.Mvvm.Navigation.Wpf
{
    public class NavigationManager
    {
        /// <summary>
        /// 注册视图
        /// </summary>
        internal readonly ConcurrentDictionary<string, Type> views = new ConcurrentDictionary<string, Type>();
        /// <summary>
        /// 导航区域
        /// </summary>
        internal readonly ConcurrentDictionary<string, object> navigations = new ConcurrentDictionary<string, object>();
        /// <summary>
        /// 导航历史记录
        /// </summary>
        internal readonly ConcurrentDictionary<string, List<object>> navigationHistory = new ConcurrentDictionary<string, List<object>>();
        /// <summary>
        /// 导航索引
        /// </summary>
        internal readonly ConcurrentDictionary<string, int> navigationHistoryIndex = new ConcurrentDictionary<string, int>();
        private static NavigationManager _Manager;
        public static NavigationManager Manager
        {
            get
            {
                if (_Manager == null) _Manager = new NavigationManager();
                return _Manager;
            }
        }
        public static string GetRegionName(DependencyObject obj)
        {
            return (string)obj.GetValue(RegionNameProperty);
        }

        public static void SetRegionName(DependencyObject obj, string value)
        {
            obj.SetValue(RegionNameProperty, value);
        }

        // Using a DependencyProperty as the backing store for RegionName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RegionNameProperty =
            DependencyProperty.RegisterAttached("RegionName", typeof(string), typeof(NavigationManager), new PropertyMetadata(OnRegionNameChanged));

        private static void OnRegionNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is ContentControl)) return;
            if (Manager.views.Any(o => o.Key == GetRegionName(d))) throw new Exception($"重复注册导航区域名称：{GetRegionName(d)}");
            Manager.navigations.TryAdd(GetRegionName(d), d);
        }
        /// <summary>
        /// 移除注册区域
        /// </summary>
        /// <param name="regionName"></param>
        public void Remove(string regionName)
        {
            if(navigations.ContainsKey(regionName)) navigations.TryRemove(regionName, out _);
        }
    }
}
