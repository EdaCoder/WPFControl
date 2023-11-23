using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;
using System.Linq;

namespace CandyControls
{
    public static class VisualHelper
    {
        /// <summary>
        /// �����ӿؼ�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static List<T> FindChildren<T>(this DependencyObject obj, string name = "") where T : FrameworkElement
        {
            try
            {
                List<T> TList = new List<T> { };
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T)
                    {

                        TList.Add((T)child);
                        List<T> childOfChildren = FindChildren<T>(child);
                        if (childOfChildren != null)
                        {
                            TList.AddRange(childOfChildren);
                        }
                    }
                    else
                    {
                        List<T> childOfChildren = FindChildren<T>(child);
                        if (childOfChildren != null)
                        {
                            TList.AddRange(childOfChildren);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(name))
                    return TList.Where(t => t.Name.Equals(name)).ToList();
                return TList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// ���ҵ����ӿؼ�
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T FindChild<T>(this DependencyObject obj, string name) where T : FrameworkElement
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T && ((T)child).Name.Equals(name))
                    return (T)child;
                else
                {
                    T childOfChild = FindChild<T>(child, name);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return default;
        }
        /// <summary>
        /// ���ָ��Ԫ�صĸ�Ԫ��
        /// </summary>
        /// <typeparam name="T">ָ��ҳ��Ԫ��</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T FindParent<T>(this DependencyObject obj) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);

            while (parent != null)
            {
                if (parent is T t)
                {
                    return t;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }
    }
}
