
using System.Linq;
using UnityEngine;

namespace Vee.UnityExtend.Binding
{
    public static class BindHelper
    {
        /// <summary>
        /// 用名字查找BindElement
        /// </summary>
        /// <param name="go"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static BindElement GetBindElement(this GameObject go, string name)
        {
            var eles = go.GetComponentsInChildren<BindElement>(true);
            return eles.FirstOrDefault(t => t.BindName.Equals(name));
        }
    }

}
