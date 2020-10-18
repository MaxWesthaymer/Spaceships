using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

using Object = UnityEngine.Object;

namespace Extensions
{
    ///<summary>
    ///Toolset for comfortable xml parsing.
    ///</summary>
    public static class XmlExtension
    {
        public static string Child(this XmlElement root, string tagName, string attrName = "Value")
        {
            var element = root[tagName];

            return element?.GetAttribute(attrName) ?? "";
        }
    }
    
    public static class GameObjectExtension
    {
        ///<summary>
        ///<para>GameObject must have the SpriteRenderer component.</para>
        ///<returns>Returns true if point hit in a object borders.</returns>
        ///</summary>
        public static bool Contains(this GameObject self, Vector3 point)
        {
            var sprite = self.GetComponent<SpriteRenderer>();

            if (sprite == null)
            {
                return false;
            }

            return point.x < self.transform.position.x + sprite.sprite.bounds.extents.x &&
                   point.x > self.transform.position.x - sprite.sprite.bounds.extents.x &&
                   point.y < self.transform.position.y + sprite.sprite.bounds.extents.y &&
                   point.y > self.transform.position.y - sprite.sprite.bounds.extents.y;
        }

        ///<summary>
        ///<para>Set color to buttons inner image.</para>
        ///<para>Button must have child image with name "InnerImage".</para>
        ///</summary>
        public static void SetColorWithInner(this Button self, Color32 btnColor, Color32 txtColor)
        {
            self.transform.Find("InnerImage").GetComponent<Image>().color = btnColor;
            self.GetComponentInChildren<Text>().color = txtColor;
        }

        /// <summary>
        /// Gets the first child GameObject with the specified name.
        /// If there is no GameObject with the speficided name, returns null.
        /// </summary>
        public static GameObject Child(this GameObject origin, string name)
        {
            if (origin == null)
            {
                return null;
            }

            var child = origin.transform.Find(name); // transform.find can get inactive object

            return child == null ? null : child.gameObject;
        }
    }

    public static class TransformExtension
    {
        ///<summary>
        ///<returns>Returns true if point hit in a object borders.</returns>
        ///</summary>
        public static bool Contains(this RectTransform self, Vector3 point)
        {
            var v = new Vector3[4];
            self.GetWorldCorners(v);

            return point.x < v[2].x && point.x > v[0].x &&
                   point.y < v[2].y && point.y > v[0].y;
        }
        
        ///<summary>
        ///Removes all child elements.
        ///</summary>
        public static void Clear(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }

        public static void TurnOffAndClear(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
                Object.Destroy(child.gameObject);
            }
        }

        ///<summary>
        ///Set active to all child elements.
        ///</summary>
        public static void SetActiveAll(this Transform self, bool isActive)
        {
            foreach (Transform child in self)
            {
                child.gameObject.SetActive(isActive);
            }
        }

        ///<summary>
        ///Removes all child elements with type &lt;T&gt; component.
        ///</summary>
        public static void Clear<T>(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<T>() != null)
                {
                    Object.Destroy(child.gameObject);
                }
            }
        }

        public static int GetActiveChildCount(this Transform transform)
        {
            return transform.Cast<Transform>().Count(child => child.gameObject.activeSelf);
        }
        
        public static Transform FirstChild(this Transform origin)
        {
            if (origin == null || origin.childCount == 0)
            {
                return null;
            }

            return origin.GetChild(0);
        }

        public static Transform LastChild(this Transform origin)
        {
            if (origin == null || origin.childCount == 0)
            {
                return null;
            }

            return origin.GetChild(origin.childCount - 1);
        }
    }
}